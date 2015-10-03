namespace PerfectShuffle.WebSharperExtensions
open WebSharper
open WebSharper.JavaScript.Pervasives

open PerfectShuffle.WebSharperExtensions.Types.PointerEvents

[<JavaScript>]
module Touch =

  [<CustomEquality; CustomComparison>]
  type Touch =
    {
      Identifier : int
      ScreenX : int
      ScreenY : int
      ClientX : int
      ClientY : int
      PageX : int
      PageY : int
      Force : float
      Buttons : Buttons
    }
    with
      override x.Equals(yobj) =
          match yobj with
          | :? Touch as y -> (x.Identifier = y.Identifier)
          | _ -> false
 
      override x.GetHashCode() = hash x.Identifier

      interface System.IComparable with
        member x.CompareTo yobj =
            match yobj with
            | :? Touch as y -> compare x.Identifier y.Identifier
            | _ -> failwith "cannot compare values of different types"

  type TouchEvent =
  | Start of Touch
  | Change of Touch
  | Stop of Touch

  let toTouch (jqe:JQuery.Event) (pe:PerfectShuffle.WebSharperBindings.PointerEvents.PointerEvent) =
    {
      Identifier = int pe.PointerId //TODO: Change to int rather than int64 in the library itself
      ScreenX = pe.ScreenX
      ScreenY = pe.ScreenY
      ClientX = pe.ClientX
      ClientY = pe.ClientY
      PageX = jqe.PageX
      PageY = jqe.PageY
      Force = pe.Pressure
      Buttons = pe.Buttons
    }

  [<Inline("$el.setPointerCapture($pointerId)")>]
  let private setPointerCapture (el:WebSharper.JavaScript.Dom.Element) (pointerId : int) = X<unit>

  [<Inline("$el.releasePointerCapture($pointerId)")>]
  let private releasePointerCapture (el:WebSharper.JavaScript.Dom.Element) (pointerId : int) = X<unit>

  type WebSharper.JavaScript.Dom.Element
    with
      member el.SetPointerCapture(pointerId : int) = setPointerCapture el pointerId
      member el.ReleasePointerCapture(pointerId : int) = releasePointerCapture el pointerId
      member el.Touches =
        let events = Event<TouchEvent>()

        JQuery.JQuery.Of(el).On("pointerover pointermove pointerdown pointerup pointerout", fun el ev ->
          
          let e = ev.OriginalEvent :?> PerfectShuffle.WebSharperBindings.PointerEvents.PointerEvent          
          let touch = toTouch ev e

          let event =
            match ev.Type with
            | name when name = "pointerdown" -> Start touch
            | name when name = "pointerup" -> Stop touch
            | name -> Change touch

          events.Trigger event
          ) |> ignore<JQuery.JQuery>

        let currentTouches =
          events.Publish
          |> Observable.scan(fun (state:Set<Touch>) evt ->
            match evt with
            | Start touch ->
              state |> Set.add touch
            | Change touch ->
              state |> Set.remove touch |> Set.add touch
            | Stop touch ->
              state |> Set.remove touch
            ) Set.empty

        currentTouches