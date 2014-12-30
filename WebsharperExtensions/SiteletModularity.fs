namespace PerfectShuffle.WebSharperExtensions
open IntelliFactory.WebSharper.Sitelets

module Context =
  let map (inverseActionMap:'b -> 'a) (context:Context<'a>) : Context<'b> =
    let newContext:Context<'b> =
      {
        ApplicationPath = context.ApplicationPath
        Link = inverseActionMap >> context.Link
        Json = context.Json
        Metadata = context.Metadata
        ResolveUrl = context.ResolveUrl
        ResourceContext = context.ResourceContext
        Request = context.Request
        RootFolder = context.RootFolder
      }
    newContext

module Content =
   let map (actionMap:'a -> 'b) (content:Content<'a>) : Content<'b> =
     match content with
     | CustomContent f -> CustomContent (fun context -> f (Context.map actionMap context))
     | PageContent f -> PageContent (fun context -> f (Context.map actionMap context))
     | CustomContentAsync f -> CustomContentAsync (fun context -> f (Context.map actionMap context))
     | PageContentAsync f -> PageContentAsync (fun context -> f (Context.map actionMap context))

module Sitelet =
  let FilterAction (ok: 'T -> bool) (sitelet: Sitelet<'T>) =
    let route req =
        match sitelet.Router.Route(req) with
        | Some x when ok x -> Some x
        | _ -> None
    let link action =
        if ok action then
            sitelet.Router.Link(action)
        else None
    { sitelet with Router = Router.New route link }

  type UserFilter<'Action, 'User> =
    {
      VerifyUser: 'User -> bool
      LoginRedirect: 'Action -> 'Action
    }
    
  /// Constructs a protected sitelet given the filter specification.
  let CustomProtect (getAuthenticatedUser : unit -> Async<'User>) (filter: UserFilter<'Action, 'User>) (site: Sitelet<'Action>)
      : Sitelet<'Action> =
      {
          Router = site.Router
          Controller =
              {
                  Handle =
                    fun action ->   
                      IntelliFactory.WebSharper.Sitelets.Content.CustomContentAsync <| fun ctx ->
                        async {
                        let! user = getAuthenticatedUser()

                        if filter.VerifyUser user then
                          let resp = IntelliFactory.WebSharper.Sitelets.Content.ToResponse (site.Controller.Handle action) ctx
                          return resp
                        else
                          // Temporary redirect otherwise browser will cache it
                          let failure = IntelliFactory.WebSharper.Sitelets.Content.RedirectTemporary (filter.LoginRedirect action)
                          let resp = IntelliFactory.WebSharper.Sitelets.Content.ToResponse (failure) ctx
                          return resp
                        }
              }
      }
