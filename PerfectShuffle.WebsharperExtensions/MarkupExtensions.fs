﻿namespace PerfectShuffle.WebsharperExtensions
open WebSharper

[<JavaScript>]
module Element =
  open WebSharper.Html.Client
  let setFocus (el:#Pagelet) =
    JQuery.JQuery.Of(el.Body).Focus().Ignore

[<JavaScript>]
[<AutoOpen>]
module AttrExtensions =
  open WebSharper.Html.Client
  let DataToggle = Attr.NewAttr "data-toggle"
  let Role = Attr.NewAttr "role"
  let Placeholder = Attr.NewAttr "placeholder"

  open WebSharper.JQuery

  let (-!) (elem: Element) (text: string) =
    JQuery.Of(elem.Body).Html(text).Ignore
    elem

module MarkupExtensions =
  open WebSharper.Html.Client
  
  [<JavaScript>]
  let Mixed el (ps : list<Pagelet>) =
    el ps
  
  type Element with
    [<JavaScript>]
    member this.Detach() =
      JQuery.JQuery.Of(this.Body).Detach().Ignore
  
    [<JavaScript>]
    member this.DetachChildren() =
      JQuery.JQuery.Of(this.Body).Children().Detach().Ignore