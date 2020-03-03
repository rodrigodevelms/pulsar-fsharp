namespace Accesses.Users.Consumer

open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Accesses.Users.Consumer.Worker.Main

module Main =
  let CreateHostBuilder argv: IHostBuilder =
    let builder = Host.CreateDefaultBuilder(argv)
    builder.UseSystemd().ConfigureServices(fun hostContext services -> services.AddHostedService<Worker>() |> ignore<IServiceCollection>)

  [<EntryPoint>]
  let main argv =
    let hostBuilder = CreateHostBuilder argv
    hostBuilder.Build().Run()
    0