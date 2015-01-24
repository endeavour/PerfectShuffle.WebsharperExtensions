namespace PerfectShuffle.WebSharperExtensions

module JsHelpers =
  open IntelliFactory.WebSharper

  [<JavaScript>]
  let Redirect (url: string) = IntelliFactory.WebSharper.JavaScript.JS.Window.Location.Assign url