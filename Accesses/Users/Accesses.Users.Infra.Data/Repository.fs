namespace Accesses.Users.Infra.Data.Repository

open System
open Accesses.Users.Infra.Data.Entity.Client.Main
open Accesses.Users.Infra.Data.Query
open Npgsql.FSharp

module Settings =
  let connection =
    Sql.host "localhost"
    |> Sql.port 5432
    |> Sql.username "postgres"
    |> Sql.password "postgres"
    |> Sql.database "accesses_users"

  let getConnection: string =
    connection |> Sql.formatConnectionString

module Main =
  open Settings

  let insert (entity: Client) =
    async {
      let! result = getConnection
                    |> Sql.connect
                    |> Sql.executeTransactionAsync
                         [ Main.insertClient,
                           [ [ ("@id", Sql.uuid entity.Id)
                               ("@active", Sql.bool entity.Active)
                               ("@name", Sql.text entity.Name)
                               ("@address", Sql.uuid entity.Address) ] ] ]
      return match result with
             | Error msg ->
                 Some msg
             | _ ->
                 None
    }
