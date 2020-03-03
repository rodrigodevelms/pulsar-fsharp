module Accsess.Users.Infra.Migration

open System
open System.IO
open Microsoft.Extensions.Configuration
open DbUp

module Main =
  let builder =
    ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("config.json", optional = false, reloadOnChange = true)
      .Build()

  let upgradeDatabase connectionString =
    EnsureDatabase.For.PostgresqlDatabase(connectionString: String)
    let upgradeEngine =
      DeployChanges.To.PostgresqlDatabase(connectionString).WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
                   .LogToConsole().Build()
    match upgradeEngine.TryConnect() with
    | true, _ ->
        match upgradeEngine.IsUpgradeRequired() with
        | true ->
            let res = upgradeEngine.PerformUpgrade()
            match res.Successful with
            | true -> printfn "Database upgraded."
            | false -> printfn "Database deployment error: %O" res.Error
        | false -> printfn "Database up to date."
    | false, err ->
        printfn "Database connection error: %s" err

  [<EntryPoint>]
  let main _ =
    match builder.GetConnectionString("npgsql") with
    | null ->
        match System.Environment.GetEnvironmentVariable("DB_CONN") with
        | null
        | "" -> failwith "Missing connection string.  Add connection string to .config or set DB_CONN environment variable."
        | s -> s
    | connStr -> connStr
    |> upgradeDatabase
    0
