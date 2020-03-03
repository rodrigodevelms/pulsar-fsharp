namespace Commons.Kafka.Api.Consumer

open Commons.Kafka.Enum.Status.Main
open System
open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2.ContextInsensitive
open Microsoft.Extensions.Logging
open Pulsar.Client.Api
open Pulsar.Client.Common
open Commons.Kafka.Api.Producer.Extensions
open Commons.Kafka.Serialization.Newtonsoft

module Processing =
  let processMessages<'a> (producer: Task<IProducer>, consumer: IConsumer, logger: ILogger, response: Boolean, f: Message -> Task<exn option>,
                           token: CancellationToken) =
    task {
      try
        while not token.IsCancellationRequested do
          let! message = consumer.ReceiveAsync()          
          let! result = f message
          match result with
          | None ->
              if response then
                let event =
                  {|  Id = message.Key
                      Status = Status.Closed
                      Message = "Success" |}
                producer.Result.ProduceAsync(event.Id, Serializer.serializer event) |> ignore
              do! consumer.AcknowledgeAsync(message.MessageId)
          | Some msg ->
              if response then
                let event =
                  {|  Id = message.Key
                      Status = Status.Error
                      Message = msg.Message |}
                producer.Result.ProduceAsync(event.Id, Serializer.serializer event) |> ignore
              do! consumer.AcknowledgeAsync(message.MessageId)
              logger.LogDebug("Logerror")
      with ex ->
        logger.LogError(ex, "ProcessMessages failed for {0}", consumer.Topic)
    }

module Main =
  let consumer (serviceUrl: String) (topic: String) (subscriptionName: String) =
    let consumer =
      PulsarClientBuilder()
        .ServiceUrl(serviceUrl)
        .Build()
    task {
      return! ConsumerBuilder(consumer)
        .Topic(topic)
        .SubscriptionName(subscriptionName)
        .SubscribeAsync() }
