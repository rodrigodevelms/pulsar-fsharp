namespace Commons.Kafka.Api.Producer

open System
open System.Runtime.CompilerServices
open Pulsar.Client.Api
open Pulsar.Client.Common
open FSharp.Control.Tasks.V2.ContextInsensitive

module Main =
  let producer (serviceUrl: String) (topic: String) (producerName: String) =
    let producer =
      PulsarClientBuilder()
        .ServiceUrl(serviceUrl)
        .Build()

    task {
      return! ProducerBuilder(producer)
        .Topic(topic)
        .ProducerName(producerName)
        .CreateAsync() }

module Extensions =
  [<Extension>]
  type Utils =
    [<Extension>]
    static member inline ProduceAsync(producer: IProducer, key, value) =
      task {
        try
          let! messageId = producer.SendAsync(MessageBuilder(value, key))
          return Result.Ok()
        with ex ->
          return Result.Error(ex)
      }
