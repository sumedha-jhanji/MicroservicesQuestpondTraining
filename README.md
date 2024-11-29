# Microservice 
- architectural style (RESt, SOA, Microservices)

## What
- rather than creating one monolithical project, we divide that into logical projects physically with separate deployments, dbs etc
- physical decoupling

## When
- we have big project(multiple) domains and sub domains
- technical diversity(more technologies ike one in Java, other in .net etc)
- team autonomy(parallel development - one team working on one subdomain and other on another)
- individual scalibility (some domains exapnd to further level and other to small. one fails and other can continue)
- failure isolation
- good budget
- should be driven by business (management should have that)

## When not to 
- small project
- tight budget
- limited team
- single or low tech diversity (means no big or large technologies)
- simple scaling

## Domain Driven Development
- we think in terms of business or domain
- we need to break project into small small domains
    - Ist step - divide business -> identify ur 
                                - core domian(domain by which u are getting ur bread and butter, like in hospital project, core is patient.), 
                                - sub domain (accounting, inventory, infrastructure etc), 
                                - generic domain (domain accross industry - user management system, logging, authorization etc)
    - 2nd step - define bounded context 
                    - once u know ur core core domain, further we seggregate that like in patinet, we divide it to appointment context, clinical context, etc.
        
 - Follow ubiquitous language -> when user is saying patient (context) in terms of billing, u also should consider the patinet in terms of billing context only. i.e context should be same for calling and caller. 

## Context Map - how to map the entity from one context will map to entity in other context.
- Strategic Design - will define how bounded contexts will talk with each other
- Tactical Design - inside the bounded context, what is the approach like in context, we write DB, repository pattern, aggregarte root etc.

## Upstream 
- supplier of things or data

 ## downstream
 - consumer

## Strategic Design  - below are the strategies
- Partnership -> collaboration between teams of different context and they decide/deifne common fields/types. Here more important is co=operation
- Shared Kernel -> define common entities to be used by all the contexts -> patrenship can result into shared kernel many times., teams need to have common code base called kernel
- Customer Supplier -> 2 bounded context whicch need to talk, one will be upstream/supplier (there context needs to handle the data) required by down stream(consumer). Consumer will just consume the data.
-  Conformist -> sometimes there is some request that expected changes by many of the bounded contexts, but one confirms that it will change for that and other needs not worry on that. it wwill pass that to lower contexts as imapct on that context is less in case of change.
- Anti Corruption layer -> translate upstream objects to downstream. Extra layer

## Tactical Design
 - inside bounded context
     - Entity - thing which exists in real world like Patient, Doctor etc
     - Value (value object class) 
         - say there can be one ABC who can be patient and another ABC can be doctor. 
         - We check on basis of value. 
         - Objects are compared by value not by reference. 
         - It should be immutable. i.e. is create a copy and then assign it to other object.
     - Service 
         - anthing(class/logic) that is shared b/w all ur classes(entities and value objects)
         - example - repository class, logging class i.e techincal code/where we are writing logic is service
     - Aggregate root
         - pattern 
         - cluster of domain objects that can be trated as single usint example patient nad his problems
         - any modification to inside object will go through aggregarte root.
         - to maintain integrity.

## Onion Architecture (uses Inversion of Control)
- service should actually use Model.
- Model should not call service. 
- Inside there will be model and outside will be services and outside will have dependency on inside rather than vice versa. 
- This is using inversion of control.
  

## Summary
    Microservice -> follow DDD -> identify core domain, subdomain, generic domain -> in each domain, define bounded context-> each context should follow ubiquitous language.

## CQRS
- create update delete query/read - all are different
- create separate classes & models for each.
- provides clear seggregation
- improve perfomance
- helps to implement event sourcing - source each change as event in autonomous way

- Command Objects
- Handler

## Marker Interface Pattern 
-logically groups an object so that it can be easily traceable at run time

## Commands & Handlers linking
- we can do using either by 
    - using internal DI
    - **OR**
    - using MediatR

## MediatR
- open source framework
- unambitious mediatr implementation
- it will automatically do DI.

## SAGA
- local trasactions of a microservice + command/event for next Microservice

## Event sourcing  - not compulsory to implement.
- we maintain trail of events for audit or replay purpose. example Electronic Medical Record.
- guid along with version is used to identify te records
- basically event data is in form of JSON so we try to use NoSql(it has scalling issues) or event sourcing DBs like EventStoreDB rather than RDBMS
- events regarding one Guid are called entity/aggregates

## Read Model 
- we design separate database for read queries

## Projections
- we can project tabel for dashboard separately amd for reports separately in read DB and can query just that table fo relevant task.

## Snapshot
- Say we have data of past 10 years, we want to just keep data of last year.
- Rest data we can create snapsshot of by clubbing that in to some format like JSON or XML etc.
  example {guid}, {versions}, {clubbed xml or json for event and event data}
- this will incrrease the performance of read db.

## Eventual Consistency
- since we have separate read amd write models.
- say we created one record in write model. It will add some daly while projection is going ot happen to Read model.
- Eventual Consistency will say that till data gets projected to read model, we will not consider the transaction as complete in Write Model.
- there will be no locking.

## API Gateway
- we have multiple mcrosevices. 
- API gateway will handle requests from a client(react/angular) and pass it ti appropriate microservice.
- provide common authentication and authorization and throttling.
- Azure -> APIM 
- Free -> OCELOT

## OCELOT  
 - add Ocelot nuget package
 - in ocelot.json file define baserurl. upstream (url which client will call) and downstream(backend api url which needs to be actaully called at backend) for other apis
 - Upstreams -> what we want to append in our main API to get redirected to downstream url of API
   - example - Main api -> localhost:9000
             - second api -> localhost:9001 -> for accounting
             - third api -> localhost:9002 -> for inventory
   - we want to run main api and want to pass url as localhost:9000/accounting -> this access accounting api
             - here /accounting is upstream and localhost:9001 is downstream
```csharp
    ocelot.json file sample

    {
        "Reroutes": [
            {
            "DownstreamPathTemplate": "/api/values",
            "DownstreamScheme": "http",
            "DownstreamHostAndPorts": [
                {
                "Host": "localhost",
                "Port": "9001"
                }
            ],
            "UpstreamPathTemplate": "/accounting",
            "UpstreamHttpMethod": [ "Get" ]
            },
            {
            "DownstreamPathTemplate": "/api/values",
            "DownstreamScheme": "http",
            "DownstreamHostAndPorts": [
                {
                "Host": "localhost",
                "Port": "9002"
                }
            ],
            "UpstreamPathTemplate": "/inventory",
            "UpstreamHttpMethod": [ "Get" ]
            }
        ],
        "GlobalConfiguration": {
            "BaseUrl": "http://localhost:9000"
        }
    }
```
 - add ocelot.json to configuration -> builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
 - add ocelot middleware before MVC controller middleware -> builder.Services.AddOcelot(builder.Configuration);
 - use ocelot middleware -> app.UseOcelot().Wait();

## OAUTH 2.0
- protocol of how to exchange a security message
- focuses only on authorization(roles)
- when to use   
    - suppose we have some app already running in background which we already have identified. It needs to access the resource. so here we need to no re-identify the app. We can just check whether the app is authorized to acess the resource.

## Open ID
- protocol of how to exchange a security message
- deals with Authentication (who)
- when to use
    - app already have handled roles for the user. they just want to know who the user is. Like we want to identify user using Active Directory
    
## OpenIDConnect
- protocol of how to exchange a security message
- sits on top of OAUTH 2.0 and uses open identify
- authorization + authentication

## CLAIMS and ROLES and SCOPES
- Role are traditional like user is admin
- claim menas which describes what user claims. Example -> i claim that he is from india.
- Scope - are at level of application/api., generic level.

## Authentication in above protocols
- Authorization code follow 
    - client will send clientId, TenentId, scope(optional) to Authentication server(AS).
    - AS will return secret value and access token (not a JWT token) to client.
    - client will then send secret value and access token to AS, whih in turn will then send the actual token or JWT token.
    - client will then make request to Resource server(resource which client wants to access) with JWT tokenn and will then perfoem further functionality

- Implicit Follow (should not be used more frequentlty as it will require Secret value to be exposed.)
    - Client will send clientId, Tenant Id , sceret value and scope to AS.
    - AS will provide JWT token 
    - Client will use that JWT token and make request to Resource server.
    use case -> we are make call from server which is trusted authority rather than react app, angualr appe etc.

- Steps to suse any of the above protocol using Azure Active Directory as Authentication Server.
  - Azure AD B2C
  - register app
  - note client id and tenant id
  - create client secret and note there values.
  - expose an API -> define scope.
  - Specify authentication -> Authoriztion code flow or implicit flow or access token or Id token(used by client only to get user role)
  - specify the RedirectURl(resource server URL)
  
  - on client and Resource Server apps, we can use clientid, tenant id, domain(redirectURL), instance for authentication purpose. 
  - In client app, we need to write code where we will make 2 calls as specified in Authorization Code flow and can generate JWT token
  - We then need to pass this JWT token as Bearer token to Resource Server and can access the same.

## RESILIENCY
 - ability of application about how it reacts when it receives any exception.

- 3 types of Patterns
     - Retry
         - here we just define policy and execute the policy on method. it will try and execute the methof for given tried automatically
     - Circuit breaker 
         - where we retry for 3 tries and then give a pause and then retry again
         - we need to run the simulated method manually
         - after expected retry calls, it will open the circuit for avoiding any further calls.
     - Fallback
         - as soon as we get any execption, it will go to fallback method.

## POLLY
- framework for RESILIENCY
- it will help to create policy
- Polly categorizes exceptions in to 2 Partnership
    - Response exceoptions
        - when we have say internal server error (500) etc where we can have response.
    - Request execeptions
        - say we have permanent exceptions like connection close, site itself is down (404 error)

## EXPONENTIAL BACK OFF
- when we define pause time or retry time, we specify like TimeSpan.FromSeconds(2). -> we are providing constant time. 
- Rathen than providing this constant time we can use any alogorithm so that this pause time or retry time will be increase value fo last pause time like for first 2 seconds, then 4 seconds, 8 seconds and so on
- example code to do that -> TimeSpan.FromSeconds(Math.Pow(2, x))

## SERVICE DISCOVERY - Open source for this is CONSUL ( we can use Kubernates also rather than consul or Axure service discovery)
- place where we want to dicover a service
- we need to locate the API from centralized location.
    - We should give a name, it will search a service
- we need to know is API up and running
- we can check health status also -  health check will let us know whether service is running and up

- Steps to use CONSUL
  - To register service to consul discovery
      - download exe from https://developer.hashicorp.com/consul/install
      - in cmd,  "run "consul agent -dev"" - it will run it for dev environment. -. it runs on 8500 port (localhost:8500)
      - We need to add nuget - consul in project
      - in appsettins.json, provide consul configuration as below:
      ```csharp
          "Consul": {
              "Host": "localhost",
              "Port":  8500
          }
      ```
      - We need to register the consul and use the same.
      ```csharp
          builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
          {
              var consulHost = builder.Configuration["Consul:Host"];
              var consulPort = Convert.ToInt32(builder.Configuration["Consul:Port"]);
              consulConfig.Address = new Uri($"http://{consulHost}:{consulPort}");
    
          }));
    
          var consulClient = app.Services.GetRequiredService<IConsulClient>();
          var registration = new AgentServiceRegistration()
          {
              ID = "my-service-id",
              Name = "my-service-name",
              Address = "localhost",
              Port = 7062,
              Check = new AgentServiceCheck // to let know that where health check can be done.
              {
                  HTTP = "http://localhost:7062/health",
                  Interval = TimeSpan.FromSeconds(10), // every 10 min interval, it will keep checking the service.
                  Timeout = TimeSpan.FromSeconds(5)
              }
              //2 ways either agent keeps pinging to check if service is up and running or we can dispose the service
          };
    
          await consulClient.Agent.ServiceRegister(registration);
         ```
    
    - Tor health check
    ```csharp
        //for health Check
        [HttpGet("/health")]
        public IActionResult HealthCheck()
        {
            //health chekc logic
            return Ok();
        }
    ```
    - Code to discover the service and query the same via a client app
        - install consul nuget package
        ```csharp
        var consulClient = new ConsulClient();
    
        //specify the service name to discovery
        string serviceName = "my-service-name";
    
        //Query consul for healthy instance of the services
        var services = consulClient.Health.Service(serviceName, tag: null, passingOnly: true).Result.Response;
    
        //Iterate through the discovered services.
        foreach (var service in services)
        {
            var serviceAddress = service.Service.Address;
            var servicePort = service.Service.Port;
    
            Console.WriteLine($"Found service at {serviceAddress}:{servicePort}");
            // we can use the serviceaddress and port to communicate with discovered service.
        }
       ```


## Steps to use APIM where client wil be eithetr postman/react/angukar app
- firstly we need to build the trusted
    - 1) in app registration(microsoft entra id) - register oen app say name -> apim-server, define scope and add url. note its client id, openid toke url, secret value
    - 2) in app registration - register new app say apim-client - add permissions where we allow it to have access of scope defined in above step.. note its client Id
- In API management service -> add jwt xml in inbound processing using above details where client id will of apim-client.
- Now from client app i.e. react/angular/postman -> make call to token URL noted above and get the access token 
- using that access token in header, we cna make calls to our APIs which are behind APIM.

- before below steps we need to build trust using ist and 2nd point above
    client app (react/angular/postman) ------------> APIM (using token url)  ------------> return token to client app (react/angular/postman) 
    client app (react/angular/postman)  ------------> make call to APIs using access token
                    
## Microfrontend
- say we are accessing one angukar smicroservice from say react microservice then routing should work seamlessly. This is taken care by Microfrontend
