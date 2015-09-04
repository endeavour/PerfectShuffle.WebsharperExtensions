namespace PerfectShuffle.WebsharperExtensions
open WebSharper
open System

[<JavaScript>]
module Colors =

  /// Decomposes an RGB value into a tuple of its constituents (r,g,b)
  let decompose rgb =
    let r = (rgb &&& 0xFF0000) >>> 16
    let g = (rgb &&& 0x00FF00) >>> 8
    let b = (rgb &&& 0x0000FF)
    r,g,b

  /// Composes a tuple of RGB values into a float
  let compose (r,g,b) =
    (r <<< 16) ||| (g <<< 8) ||| b

  /// Converts RGB to HSL
  /// HSL values are normalised to be in the range [0,1]
  let rgbToHsl (r,g,b) =
    let r' = float r / 255.0
    let g' = float g / 255.0
    let b' = float b / 255.0

    let Min = [r'; g'; b'] |> Seq.min
    let Max = [r'; g'; b'] |> Seq.max
    let deltaMax = Max - Min
    let L = (Max + Min) / 2.0
    
    let S =
      if L < 0.5 then deltaMax / (Max + Min) else deltaMax / (2.0 - Max - Min)

    let H =
      let d = float deltaMax
      match d, Max with
      | 0.0, _ -> 0.0
      | _, m when m = r' -> (g' - b') / d
      | _, m when m = g' -> (((b' - r')/d)) + 2.0
      | _, m when m = b' -> (((r' - g')/d)) + 4.0
      | _ -> failwith "assertion failed"
      |> (*) 60.0
      |> (fun x -> if x < 0.0 then x + 360.0 else x)

    H/360.0, S, L

  /// Converts HSL values in range 0-1 to degrees and percentages
  /// H -> Degrees [0, 360)
  /// S, L -> Percentages [0, 100]
  let hslToHumanUnits (h,s,l) =
    h * 360.0, s * 100.0, l * 100.0

  /// Converts normalised HSL values (in the range [0,1]) to RGB values
  let hslToRgb (h:float,s:float,l:float) =
   
    if s = 0.0
      then
        let l' = Math.Round(l)
        l', l', l' // achromatic
      else
        let hue2rgb = fun (p,q,t) ->
          let mutable t = t
          if t < 0.0 then t <- t + 1.0
          if t > 1.0 then t <- t - 1.0
          match t with
          | n when n < 1.0/6.0 -> p + (q-p) * 6.0 * t
          | n when n < 1.0/2.0 -> q
          | n when n < 2.0/3.0 -> p + (q-p) * ((2.0 / 3.0) - t ) * 6.0
          | _ -> p

        let q = if l < 0.5 then l * (1.0 + s) else  l + s - l * s
        let p = 2.0 * l - q
        
        let r = Math.Round(hue2rgb (p,q,h+1.0/3.0) * 255.0)
        let g = Math.Round(hue2rgb (p,q,h) * 255.0)
        let b = Math.Round(hue2rgb (p,q,h-1.0/3.0) * 255.0)

        r,g,b