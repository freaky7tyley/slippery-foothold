
using Microsoft.Graph; //nuget package
using Azure.Identity;   //nuget package

Console.WriteLine("Hello, World!");

//https://docs.microsoft.com/de-de/graph/sdks/choose-authentication-providers?tabs=CS

var scopes = new[] { "User.Read" };

// Multi-tenant apps can use "common",
// single-tenant apps must use the tenant ID from the Azure portal
var tenantId = "common";

//https://docs.microsoft.com/de-de/azure/active-directory/develop/quickstart-register-app

// Values from app registration
var clientId = "YOUR_CLIENT_ID";
var clientSecret = "YOUR_CLIENT_SECRET";

// For authorization code flow, the user signs into the Microsoft
// identity platform, and the browser is redirected back to your app
// with an authorization code in the query parameters
var authorizationCode = "AUTH_CODE_FROM_REDIRECT";

// using Azure.Identity;
var options = new TokenCredentialOptions
{
    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
};

// https://docs.microsoft.com/dotnet/api/azure.identity.authorizationcodecredential


var authCodeCredential = new AuthorizationCodeCredential(
    tenantId, clientId, clientSecret, authorizationCode, options);

var graphClient = new GraphServiceClient(authCodeCredential, scopes);


//--------------------------
//https://docs.microsoft.com/de-de/graph/api/calendar-update?view=graph-rest-1.0&tabs=csharp

//GraphServiceClient graphClient = new GraphServiceClient(authProvider);

var calendar = new Calendar
{
    Name = "Social events"
};

await graphClient.Me.Calendar
    .Request()
    .UpdateAsync(calendar);