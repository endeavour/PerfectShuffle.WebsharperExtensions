namespace PerfectShuffle.WebsharperExtensions
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

module Enhance =
  open IntelliFactory.WebSharper
  open IntelliFactory.WebSharper.Html
  open IntelliFactory.WebSharper.Formlet  
    
  [<JavaScript>]
  let WithDebugOutput (formlet: Formlet<string>) =
    Formlet.Do {
      let! res = 
        formlet
        |> Formlet.LiftResult
      return!
        Formlet.OfElement <| fun _ ->
          let status =
            match res with
            | Result.Success x -> "Success : " + x
            | Result.Failure fs -> "Failure: " + (Seq.fold (+) "" fs)  
          Div
            [Attr.Style "border:2px solid #CCC; padding:10px; margin-top:10px"]  -< [Text status]
    }    
    |> Enhance.WithResetButton
    |> Enhance.WithFormContainer  