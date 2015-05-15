// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile
open Fake.FileSystemHelper

RestorePackages()

let buildVersion = "0.1.2"

// Properties
let buildDir = "./build/"
let testDir  = "./test/"

let packagingDir = "./build/packages/"
let packagingRoot = "./build/packages/nuget/"
let allPackageFiles =
  [|
    buildDir + "PerfectShuffle.WebsharperExtensions.dll"
    "license.txt"
  |]
  
let projectName = "PerfectShuffle.WebsharperExtensions"
let authors = ["James Freiwirth"]
let projectDescription = "Various extended functionality for websharper"
let projectSummary = "Websharper extensions"
let nuspecFile = "PerfectShuffle.WebsharperExtensions/PerfectShuffle.WebsharperExtensions.nuspec"

// Targets
Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "BuildApp" (fun _ ->
    CreateFSharpAssemblyInfo "./PerfectShuffle.WebsharperExtensions/AssemblyInfo.fs"
        [Attribute.Title "PerfectShuffle.WebsharperExtensions"
         Attribute.Description "Authentication and JWT tools"
         Attribute.Guid "34e4036c-e16c-4cc4-84d3-820207ec5837"
         Attribute.Product "PerfectShuffle.WebsharperExtensions"
         Attribute.Version buildVersion
         Attribute.FileVersion buildVersion]

    !! "PerfectShuffle.WebsharperExtensions/*.fsproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Target "CreatePackage" (fun _ ->
    // Copy all the package files into a package folder
    CopyFiles packagingDir allPackageFiles

    ensureDirExists (System.IO.DirectoryInfo(packagingRoot))
    NuGet (fun p -> 
        {p with
            Authors = authors
            Project = projectName
            Description = projectDescription                               
            OutputPath = packagingRoot
            Summary = projectSummary
            WorkingDir = packagingDir
            Version = buildVersion
            //AccessKey = myAccesskey
            Publish = false
            Files =
              [
                "license.txt", None, None
                "*.dll", Some("lib"), None
              ]
            Dependencies =
              [
                "FSharp.Data", "2.2.2"
                "IntelliFactory.Reactive", "3.0.23.25"
                "PerfectShuffle.Security", "0.1.1"
                "WebSharper", "3.0.59.145"
                "WebSharper.Piglets", "3.0.50.227"
              ]}) 
            nuspecFile
)

Target "Default" (fun _ ->
    ()
)

// Dependencies
"Clean"
  ==> "BuildApp"
  ==> "CreatePackage"
  ==> "Default"

// start build
RunTargetOrDefault "Default"