namespace Accesses.Users.Infra.Data.Entity.Client

open System

module Main =
  type Client =
    { Id: Guid
      Active: Boolean
      Name: String
      Address: Guid
      }
    
    member this.Error =
      [ (if String.IsNullOrEmpty(this.Name) || this.Name.Length < 3 || this.Name.Length > 120 then
           Some "The field Name is required and must have between 3 and 120 characters"
         else None)
        (if this.Address.Equals(Guid.Empty) then Some "The field Address is required"
         else None) ]
      |> List.choose id
      |> String.concat ""
      |> fun errors ->
        if errors <> "" then Some errors
        else None
   