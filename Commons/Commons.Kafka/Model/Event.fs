namespace Commons.Kafka.Model.Event

open Commons.Kafka.Enum.Status.Main
open System

module Main =
    type Event<'T> =
        { Id: Guid
          Status: Status
          Message: 'T }
