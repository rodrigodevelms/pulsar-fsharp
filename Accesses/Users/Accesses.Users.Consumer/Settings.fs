namespace Accesses.Users.Consumer.Settings

open Commons.Kafka.Api.Producer.Main

module PulsarResponse =
  let startProducer = producer "pulsar://localhost:6650" "response-insert-client" "orchestrationClientProducer"
