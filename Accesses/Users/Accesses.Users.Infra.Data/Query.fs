namespace Accesses.Users.Infra.Data.Query

module Main =
  [<Literal>]
  let connectionString = """
    Host=localhost;Username=postgres;Password=postgres;Database=accesses_users
    """
    
  [<Literal>] 
  let insertClient = """
    INSERT INTO users.client
    (id, active, name, address)
    VALUES (@id, @active, @name, @address);
  """