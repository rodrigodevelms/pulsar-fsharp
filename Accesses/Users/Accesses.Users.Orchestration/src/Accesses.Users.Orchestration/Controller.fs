namespace Accesses.Users.Orchestration.Controller

open Giraffe
open Pulsar.Client.Api
open Accesses.Users.Orchestration.Handler.Main

module Main =
  let webApp (producer: IProducer): HttpFunc -> HttpFunc =
    subRoute "/api/accesses/users"
      (choose
        [ GET >=> choose [ route "/" >=> text "world" ]
          POST >=> choose [ route "/" >=> sendToPulsar producer ]
//          POST >=> choose [ route "/test" >=> save ]
          setStatusCode 404 >=> text "Not Found" ])
