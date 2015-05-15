namespace PerfectShuffle.WebsharperExtensions
open WebSharper.Sitelets

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
        UserSession = context.UserSession
      }
    newContext

module Content =
   let map (actionMap:'a -> 'b) (content:Content<'a>) : Content<'b> =
     match content with
     | CustomContent f -> CustomContent (fun context -> f (Context.map actionMap context))
     | PageContent f -> PageContent (fun context -> f (Context.map actionMap context))
     | CustomContentAsync f -> CustomContentAsync (fun context -> f (Context.map actionMap context))
     | PageContentAsync f -> PageContentAsync (fun context -> f (Context.map actionMap context))

   let unasync (content:Async<Content<'action>>) : Content<'action> =
     Content.CustomContentAsync(fun ctx ->
         async {
           let! content = content
           return Content.ToResponse content ctx            
         }
       )
   
module Sitelet =
  let FilterRoute (ok: 'action -> bool) (router:Router<'action>) =
    let route req =
        match router.Route(req) with
        | Some x when ok x -> Some x
        | _ -> None
    let link action =
        if ok action then
            router.Link(action)
        else None
    Router.New route link
  
  let FilterAction (ok: 'action -> bool) (sitelet: Sitelet<'action>) =
    { sitelet with Router = FilterRoute ok sitelet.Router }

  type UserFilter<'Action, 'User> =
    {
      VerifyUser: 'User -> bool
      LoginRedirect: 'Action -> 'Action
    }
    
  /// Constructs a protected sitelet given the filter specification.
  let CustomProtect (getAuthenticatedUser : unit -> Async<'User>) (filter: UserFilter<'Action, 'User>) (sitelet: Sitelet<'Action>)
      : Sitelet<'Action> =
      {
          Router = sitelet.Router
          Controller =
              {
                  Handle =
                    fun action ->   
                      WebSharper.Sitelets.Content.CustomContentAsync <| fun ctx ->
                        async {
                        let! user = getAuthenticatedUser()

                        if filter.VerifyUser user then
                          let resp = WebSharper.Sitelets.Content.ToResponse (sitelet.Controller.Handle action) ctx
                          return resp
                        else
                          // Temporary redirect otherwise browser will cache it
                          let failure = WebSharper.Sitelets.Content.RedirectTemporary (filter.LoginRedirect action)
                          let resp = WebSharper.Sitelets.Content.ToResponse (failure) ctx
                          return resp
                        }
              }
      }
