namespace PerfectShuffle.WebSharperExtensions

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html

[<Require(typeof<IntelliFactory.WebSharper.JQuery.Resources.JQuery>)>]
type BootstrapResource() =
  inherit Resources.BaseResource(
    "/javascripts/bootstrap", //base URI
    "affix.js",
    "alert.js",
    "button.js",
    "carousel.js",
    "collapse.js",
    "dropdown.js",
    "modal.js",
    "tooltip.js",
    "popover.js",
    "scrollspy.js",
    "tab.js",
    "transition.js"
    )