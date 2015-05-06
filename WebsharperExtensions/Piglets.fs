namespace PerfectShuffle.WebSharperExtensions

open WebSharper
open WebSharper.Piglets
open WebSharper.Html.Client
open WebSharper.JavaScript.Pervasives

[<JavaScript>]
module PigletHelpers =
  
  module Bootstrap =
    let InputWithLabel stream name labelText placeHolderText =
      [
        Label [Attr.Class "control-label"; Attr.For name; Text labelText]
        Controls.Input stream -< [Attr.Class "form-control"; Attr.Id name; Attr.Name name; AttrExtensions.Placeholder placeHolderText]
      ]

    let PasswordWithLabel stream name labelText placeHolderText =
      [
        Label [Attr.Class "control-label"; Attr.For name; Text labelText]
        Controls.Password stream -< [Attr.Class "form-control"; Attr.Id name; Attr.Name name; AttrExtensions.Placeholder placeHolderText]
      ]

    let FormGroupWithErrorHighlight = function Failure _ -> "form-group has-error" | Success _ -> "form-group has-success"

  /// Use to prevent a submit button from submitting a form when it is within <form> tags
  /// This is useful because we still get the benefit of 'enter' key hitting submit
  let preventDefaultFormSubmission (el:#Pagelet) =
    JQuery.JQuery.Of(el.Body).Bind("click", fun _ ev ->
      ev.PreventDefault()
      ).Ignore
    el

  let hasAtLeastOneErrorMessage result =
    match result with
    | Piglets.Success _ -> false
    | Piglets.Failure [] -> false
    | Piglets.Failure _ -> true

[<JavaScript>]
module Piglet =  
  [<JavaScript>]
  let HideWhen (reader: Reader<'a>) (pred: Result<'a> -> bool) (element: Element) =            
    
      element |>! (fun el ->
        let jQuery = JQuery.JQuery.Of(el.Dom)
          
        let setVisibility isVisible =
          if isVisible
            then jQuery.Show() |> ignore
            else jQuery.Hide() |> ignore

        setVisibility <| not (pred (reader.Latest))
        reader.Subscribe <| fun (x:Result<'a>) -> setVisibility (not (pred x))
        |> ignore)

  [<JavaScript>]
  let ShowWhen (reader: Reader<'a>) (pred: Result<'a> -> bool) (element: Element) = HideWhen reader (pred >> not) element