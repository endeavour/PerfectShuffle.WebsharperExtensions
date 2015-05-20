namespace PerfectShuffle.WebsharperExtensions.UI

module Next =
  open WebSharper.UI.Next
  
  type View with
      static member Sequence views =
          views
          |> Seq.fold (View.Map2 (fun a b -> 
              seq { yield! a; yield b })) (View.Const Seq.empty)