namespace PerfectShuffle.WebSharperExtensions

module SPA =
  open WebSharper

  [<JavaScript>]
  type MessageBus<'TMsg>() =

    let evt = Event<'TMsg>()
    let post msg = evt.Trigger msg  
    let events = evt.Publish

    member __.Post msg = post msg
    member __.Messages = events
 
  [<JavaScript>]
  module BrowserHistory =
    // See: https://developer.mozilla.org/en-US/docs/Web/Guide/API/DOM/Manipulating_the_browser_history
  
    let history = WebSharper.JavaScript.JS.Window.History
  
    let pushState state url =
      history.PushState(state, "", url)

    let replaceState state url =
      history.ReplaceState(state, "", url)

    let onPopState = WebSharper.JavaScript.JS.Window.Onpopstate
