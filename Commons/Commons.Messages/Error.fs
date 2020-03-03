namespace Commons.Messages.Error

open System

module Kafka =
  let errorMessages errors =
    errors
    |> Seq.choose (function
         | Error ex -> Some ex
         | Ok _ -> None)
    |> Seq.toList

module Main =
  type Message =
    { Id: Guid
      Date: DateTime
      Error: String }
