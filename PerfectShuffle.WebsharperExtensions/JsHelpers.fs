namespace PerfectShuffle.WebSharperExtensions

module JsHelpers =
  open WebSharper

  [<JavaScript>]
  let Redirect (url: string) = WebSharper.JavaScript.JS.Window.Location.Assign url

