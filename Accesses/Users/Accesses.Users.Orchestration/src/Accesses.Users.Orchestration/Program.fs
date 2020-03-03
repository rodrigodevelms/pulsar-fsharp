module Accesses.Users.Orchestration.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Accesses.Users.Orchestration.Startup.Main
open Accesses.Users.Orchestration.Settings.Pulsar

[<EntryPoint>]
let main _ =
  let contentRoot = Directory.GetCurrentDirectory()
  let webRoot = Path.Combine(contentRoot, "WebRoot")
  let producer = startProducer.Result
  WebHostBuilder()
    .UseKestrel()
    .UseContentRoot(contentRoot)
    .UseIISIntegration()
    .UseWebRoot(webRoot)
    .Configure(Action<IApplicationBuilder>(configureApp producer))
    .ConfigureServices(configureServices)
    .ConfigureLogging(configureLogging)
    .Build()
    .Run()
  0
