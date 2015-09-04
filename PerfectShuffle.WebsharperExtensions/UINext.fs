namespace PerfectShuffle.WebsharperExtensions.UI.Next
open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Client

[<AutoOpen>]
module Extensions =  
  type View with
      static member Sequence views =
          views
          |> Seq.fold (View.Map2 (fun a b -> 
              seq { yield! a; yield b })) (View.Const Seq.empty)

[<JavaScript>]
module Doc =
  open WebSharper.UI.Next
  let EmbedPageletWithCustomRender (pagelet : WebSharper.Html.Client.Element) (f:unit -> unit)=
      pagelet.Dom
      |> View.Const
      |> View.Map (fun el ->          
          f()
          
          Doc.Static el)
      |> Doc.EmbedView
      
  let EmbedPagelet (pagelet : WebSharper.Html.Client.Element) =
      EmbedPageletWithCustomRender pagelet (fun () -> pagelet.Render())