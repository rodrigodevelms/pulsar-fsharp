namespace Commons.Kafka.Serialization.Newtonsoft

open System.Text
open Newtonsoft.Json

module Serializer =
  let serializer (object: obj): byte [] =
    let string = JsonConvert.SerializeObject(object)
    Encoding.Default.GetBytes(string)

module Deserializer =
  let deserializer<'T> (message: byte []): 'T =
    let messageFromTopic = Encoding.UTF8.GetString(message)
    JsonConvert.DeserializeObject<'T>(messageFromTopic)
