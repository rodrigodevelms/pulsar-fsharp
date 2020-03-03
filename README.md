
This is a program that the Pulsar messaging system
Follows the following flow

FrontEnd -> 
  sends a request to the Orchestrator API -> 
  sends a message to Pulsar -> 
  API Client consumes the message and tries to insert it into the database -> 
  And sends a success or error message to Pulsar -> 
  The Orchestrator API consumes the response and sends it to the frontend via socket (not yet implemented)
End

1 - Inside the folder Docker, do on terminal : docker-compose up -d\newline  
2 - Run "Accesses.Users.Migrations" to create schemas and tables\newline  
3 - Run "Accesses.Users.Orchestration"  and open Postman (or Insomnia) and\newline  
        do a POST request: "http://localhost:5000/api/accesses/users/" with the\newline   follow body:\newline  
        {\newline   	
            "id" : uuid,\newline  
            "name" : string,\newline	  
            "active" : boolean,\newline  
            "address" : uuid\newline

        }\newline
4 - Run "Accesses.Users.Consumer" to consume the message and insert on database.\newline
5 - To see the response, on terminal do :\newline
     - docker-compose exec pulsar bash\newline
     - cd bin/\newline
     - ./pulsar-client consume response-insert-client -s "some-name-here" -n 0\newline

    

Thanks to the entire F# community in particular to:\newline

@lanayx \newline
@vorotato \newline
@chethusk \newline
@dave.curylo \newline
@Zaid Ajaj \newline
@pat \newline
@Elliott V. Brown \newline
@Christopher Pritchard \newline
@sandeepc24 \newline

You helped me understand F # better.\newline
Thanks.\newline