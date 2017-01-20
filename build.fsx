// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile
open Fake.FileSystemHelper

Target "BuildApp" (fun _ ->
    !! "PerfectShuffle.WebsharperExtensions/*.fsproj"
      |> MSBuildRelease "" "Build"
      |> Log "AppBuild-Output: "
)

Target "CreatePackage" (fun _ ->
  Fake.Paket.Pack id
)

Target "Default" (fun _ ->
    ()
)

// Dependencies
"BuildApp"
  ==> "CreatePackage"
  ==> "Default"

// start build
RunTargetOrDefault "Default"
