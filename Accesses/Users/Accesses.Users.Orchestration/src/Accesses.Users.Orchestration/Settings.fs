namespace Accesses.Users.Orchestration.Settings

open Commons.Kafka.Api.Producer.Main

module Pulsar =
  let startProducer = producer "pulsar://localhost:6650" "insert-client" "orchestrationClientProducer"
