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

[<Require(typeof<IntelliFactory.WebSharper.JQuery.Resources.JQuery>)>]
type SemanticUIResource() =
  inherit Resources.BaseResource(
    "/javascripts/semantic-ui", //base URI
    "accordion.js",
    "chatroom.js",
    "checkbox.js",
    "dimmer.js",
    "dropdown.js",
    "modal.js",
    "nag.js",
    "popup.js",
    "rating.js",
    "search.js",
    "shape.js",
    "sidebar.js",
    "tab.js",
    "transition.js",
    "video.js",
    "behavior/api.js",
    "behavior/colorize.js",
    "behavior/form.js",
    "behavior/state.js"
    )