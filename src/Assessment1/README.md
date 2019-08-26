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
