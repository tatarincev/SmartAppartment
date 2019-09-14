# Smart Apartment Data. 1st Technical Assessment

This sample demonstrates how you can configure the standard OAuth2 middleware to authenticate users of an ASP.NET Core MVC application using Auth0.
And implements the following functional requirements:
 - Support secure and unsecure end points through authorization/authentication 
 - Ability to extend the API so it can interact with other APIs  

## Requirements

* .[NET Core 2.2 SDK](https://www.microsoft.com/net/download/core)
## To run this project
1. Ensure that you have replaced the [appsettings.json](appsettings.json) file with the values for your Auth0 account.
```JSON
 "Auth0": {
    "Domain": "https://{TENANT_ID}.auth0.com/",
    "ApiIdentifier": "{API_IDENTIFIER}",
    "ClientId": "{CLIENT_ID}"
```


2. Run the application from the command line:

    ```bash
    dotnet run
    ```

3. Go to `http://localhost:3000` in your web browser to view the website.


# The project implementation flow:

## Incoming functional Requirements that I need to implement in my assessment project:
 - Support secure and unsecure end points through authorization/authentication 
 - Ability to extend the API so it can interact with other APIs  
 - Anything else you wish to incorporate. 

## Incoming  Nonfunctional requirements:
- Use Auth0 service for authorization and authentication. 

For my project I built the  fictional Pet-Shop store with the Rest Web API that contains only two methods secure and insecure  and used  the follow stack of technologies for this:
- I chose the Implicit Grand Flow as main authorization flow because this is better suits for SPA application. I used Swagger UI in my project. 
- ASP.NET Core Web API as main host platform for pet-shop API as Resource server. 
- ASP.NET Core Swagger/UI for work with API through Web interface and for Authorization.
- [NSwag CLI](https://github.com/RicoSuter/NSwag/wiki/CommandLine) for generate C# client for public API https://petstore.swagger.io/ to meet the second functional requirement to interact with another API.

## There are the main steps that I have done for this project. 

### Auth0 prerequisites
- I created the new application “Smart API“
- Then registered the new API “Pets store API” with Audience http://localhost:3000 and authorized it to “Smart API” application
- I added the two new permissions to the “Pets store API” that will be represents scopes resources that can be accessed on behalf of the user with a given access token.
- Registered the two new Users and assigned them the unique permissions from recently defined.

### Create API project
-  Define the two API endpoints. 
- `GET /api/petstore/` - open and not required authorization, that returns information for current store  
- `POST /api/petstore/pets/search` - authorized that will be used for search the pets by given criteria for authorized users. 

The users who have permission `pets:read:any` are allowed to search pets with any statuses instead of users who have `pets:read:sold` are limited to see only pets with status `Sold`.  For this API I used the ASP.NET Core Resource-Based authorization to determine whether the current user can search pets with particular statuses. I made the  `PetsAuthorizationHandler`  type that receives of object of `PetsSearchQuery`  as resource context and changed it according to user effective permissions

###  Connect to external API (https://petstore.swagger.io/)
- As a data source of  my pets store, I decided to use data from public API  https://petstore.swagger.io/ in order to use this API  from my code I generated the strongly typed C# API client  by using [NSwag CLI](https://github.com/RicoSuter/NSwag/wiki/CommandLine) and  the OpenAPI schema for public Petstore API downloaded by this link https://petstore.swagger.io/v2/swagger.json. As result I got the strongly typed client that I can include in my project and can use from the code.

### Configure project to allow to Validate access tokens issued by Authorization Server 
- I set all  Auth section  properties in  appsetting.json with values taken from Auth0  Application and API
```JSON
 "Auth0": {
    "Domain": "https://xxxx.auth0.com/",
    "ApiIdentifier": "xxxxxx",
    "ClientId": "xxxxx"
```

### Run the application and demonstrate the main use cases
- As anonymous user call GET /api/petstore.  Got 200 OK.
- As anonymous user call protected POST /api/petstore/pets/search. Got 401 Unauthorized.
- As authenticated as manager@mail.com call  POST /api/petstore/pets/search with { Statues : [ ‘Available’, ‘Sold’ ] } got response with TotalCount > 8k
- As authenticated as employee@mail.com call POST /api/petstore/pets/search with { Statues : [ ‘Available’, ‘Sold’ ] } got response with TotalCount < 100

