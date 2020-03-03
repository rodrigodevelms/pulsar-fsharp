namespace Accesses.Users.Orchestration.Handler

open System
open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open Pulsar.Client.Api
open FSharp.Control.Tasks.V2.ContextInsensitive
open Accesses.Users.Infra.Data.Entity.Client.Main
open Commons.Kafka.Api.Producer.Extensions
open Commons.Kafka.Serialization.Newtonsoft
open Commons.Messages.Error.Kafka
open Commons.Messages.Error.Main
open Commons.Kafka.Enum.Status.Main

module Error =
  let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

module Main =
  let sendToPulsar =
    fun (producer: IProducer) (next: HttpFunc) (ctx: HttpContext) ->
      task {
        let! client = ctx.BindJsonAsync<Client>()
        match client.Error with
        | Some message ->
            return! (setStatusCode 404 >=> (json
                                              { Id = Guid.NewGuid()
                                                Date = DateTime.Now
                                                Error = message })) next ctx
        | None ->
            let event =
              {| Id = client.Id
                 Status = Status.Open
                 Message = client |}
            let! commandClient = producer.ProduceAsync(event.Id.ToString(), Serializer.serializer(event))
            return! match errorMessages [ commandClient ] with
                    | [] -> setStatusCode 200 next ctx
                    | _ -> (setStatusCode 500 >=> json errorMessages) next ctx
      }
//  let save: HttpHandler =
//    fun (next: HttpFunc) (ctx: HttpContext) ->
//      task {
//        let! client = ctx.BindJsonAsync<Client>()
//        match client.Error with
//        | Some message ->
//            return! (setStatusCode 404 >=> (json
//                                              { Id = Guid.NewGuid()
//                                                Date = DateTime.Now
//                                                Error = message })) next ctx
//        | None ->
//            let! result = Main.insert client
//            return! match result with
//                    | Some result -> (setStatusCode 200 >=> json result.ToString) next ctx
//                    | None -> (setStatusCode 500 >=> json "Could not add address. Try again!") next ctx
//      }
