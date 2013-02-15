namespace PerfectShuffle.WebsharperExtensions
open IntelliFactory.WebSharper.Sitelets

module Context =
  let map (context:Context<'a>) (inverseActionMap:'b -> 'a) : Context<'b> =
    let newContext:Context<'b> =
      {
        ApplicationPath = context.ApplicationPath
        Link = inverseActionMap >> context.Link
        Json = context.Json
        Metadata = context.Metadata
        ResolveUrl = context.ResolveUrl
        ResourceContext = context.ResourceContext
        Request = context.Request
      }
    newContext

module Content =
   let map (actionMap:'a -> 'b) (content:Content<'a>) : Content<'b> =
     match content with
     | CustomContent f -> CustomContent (fun context -> f (Context.map context actionMap))
     | PageContent f -> PageContent (fun context -> f (Context.map context actionMap))

