namespace PerfectShuffle.WebSharperExtensions
open IntelliFactory.WebSharper

[<JavaScript>]
module Element =
  open IntelliFactory.WebSharper.Html
  let setFocus (el:#IPagelet) =
    JQuery.JQuery.Of(el.Body).Focus().Ignore

[<JavaScript>]
[<AutoOpen>]
module AttrExtensions =
  open IntelliFactory.WebSharper.Html
  let DataToggle = Attr.NewAttr "data-toggle"
  let Role = Attr.NewAttr "role"
  let Placeholder = Attr.NewAttr "placeholder"

  open IntelliFactory.WebSharper.JQuery

  let (-!) (elem: Element) (text: string) =
    JQuery.Of(elem.Body).Html(text).Ignore
    elem

module MarkupExtensions =
  open IntelliFactory.WebSharper.Html
  
  [<JavaScript>]
  let Mixed el (ps : list<IPagelet>) =
    el ps
  
  type Element with
    [<JavaScript>]
    member this.Detach() =
      JQuery.JQuery.Of(this.Body).Detach().Ignore
  
    [<JavaScript>]
    member this.DetachChildren() =
      JQuery.JQuery.Of(this.Body).Children().Detach().Ignore