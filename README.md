
This is a program that uses Pulsar for messaging<br>

The flow<br>

```
FrontEnd -> 
  sends a request to the Orchestrator API -> 
  sends a message to Pulsar -> 
  API Client consumes the message and tries to insert it into the database -> 
  And sends a success or error message to Pulsar -> 
  The Orchestrator API consumes the response and sends it to the frontend via socket (not yet implemented)
End
```

1 - Inside the folder Docker, do on terminal : docker-compose up -d<br>  
2 - Run "Accesses.Users.Migrations" to create schemas and tables\newline <br>
3 - Run "Accesses.Users.Orchestration"  and open Postman (or Insomnia) and\newline<br>  
        do a POST request: "http://localhost:5000/api/accesses/users/" with the<br>   follow body:<br>  
```
        {	
            "id" : uuid,  
            "name" : string,	  
            "active" : boolean,  
            "address" : uuid
        }
```
				
4 - Run "Accesses.Users.Consumer" to consume the message and insert on database.<br>
5 - To see the response, on terminal do :<br>
     - docker-compose exec pulsar bash<br>
     - cd bin/<br>
     - ./pulsar-client consume response-insert-client -s "some-name-here" -n 0<br>

    

Thanks to the entire F# community in particular to:<br>

```
@lanayx 
@vorotato 
@chethusk 
@dave.curylo
@Zaid Ajaj 
@foggyfinder 
@pat 
@Elliott V. Brown 
@Christopher Pritchard 
@sandeepc24 
```

You helped me understand F # better.<br>
Thanks.
