namespace Accesses.Users.Orchestration.Startup

open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Cors.Infrastructure
open Accesses.Users.Orchestration.Handler.Error
open Accesses.Users.Orchestration.Controller.Main
open Microsoft.AspNetCore.Server.Kestrel.Core
open Pulsar.Client.Api

module Cors =
  let configureCors (builder: CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader() |> ignore

module Main =
  let configureApp (producer: IProducer) (app: IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
    (match env.IsDevelopment() with
     | true -> app.UseDeveloperExceptionPage()
     | false -> app.UseGiraffeErrorHandler errorHandler)
      .UseHttpsRedirection()
      .UseCors(Cors.configureCors)
      .UseStaticFiles()
      .UseGiraffe(webApp producer)

  let configureServices (services: IServiceCollection) =
    services.AddCors().AddGiraffe().Configure(fun (options: KestrelServerOptions) -> options.AllowSynchronousIO <- true) |> ignore

  let configureLogging (builder: ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Error).AddConsole().AddDebug() |> ignore
