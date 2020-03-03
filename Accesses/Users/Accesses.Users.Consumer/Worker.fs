namespace Accesses.Users.Consumer.Worker

open System
open Accesses.Users.Infra.Data.Entity.Client.Main
open Accesses.Users.Infra.Data.Repository
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.Logging
open Commons.Kafka.Api.Consumer.Main
open Commons.Kafka.Api.Consumer.Processing
open Commons.Kafka.Serialization.Newtonsoft
open FSharp.Control.Tasks.V2.ContextSensitive
open Microsoft.Extensions.Hosting
open Commons.Kafka.Model.Event.Main
open Accesses.Users.Consumer.Settings.PulsarResponse
module Settings =
  let startConsumer = consumer "pulsar://localhost:6650" "insert-client" "apiUserConsumer"


module Main =
  open Settings
  type Worker(logger : ILogger<Worker>) =
     inherit BackgroundService()
     let _logger = logger
     override bs.ExecuteAsync stoppingToken =
       let f: Async<unit> =       
        async {
          let cs = new CancellationTokenSource()
          let token = cs.Token       
          while not stoppingToken.IsCancellationRequested do          
          Task.Run(fun () ->          
           task {
             do! processMessages(startProducer, startConsumer.Result, logger, true, (fun (message) ->
                               let event = Deserializer.deserializer<Event<Client>>(message.Data)
                               let client = event.Message
                               Main.insert(client) |> Async.StartAsTask
                               ), token)
           } :> Task) |> ignore
          cs.Dispose()
          _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now)
          do! Async.Sleep(1000)
       }
       Async.StartAsTask f:> Task