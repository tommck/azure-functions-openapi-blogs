# Generating OpenAPI Documents in Azure Functions with C# and Swashbuckle

**(This is part 1 of a X part series... )**

I recently worked on an Azure Functions middleware project. The different clients to the APIs needed to be isolated so that each API client had the least amount of access possible. Our clients code in many different technologies (C#, Python, Java, etc). To document our APIs for these clients, we chose to use [OpenAPI (formerly Swagger)](https://swagger.io).

For the sake of these articles, we will be working with a fictitious shopping site called “Bmazon” and handle sending orders to the warehouse and the warehouse sending shipping information back.

In this series of articles, I will walk you through:

1.  Creation of the Project
1.  Addition of OpenAPI spec generation
1.  Increasing the Quality of the Documentation that is generated
1.  Generation of separate documents based on consumer and the access they should have
1.  Exposing these separate Consumer APIs as separate APIs in Azure API Management (?)

This article covers steps 1 and 2 of these

## Create the Project

To create the project, we will start with the [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local). At the time of this writing, the current version of this library is `3.0.3284`

At the PowerShell prompt, we’ll do following to create our project:

```powershell
C:\dev> func --version
3.0.3284
C:\dev> func init Bmazon --worker-runtime dotnet

Writing C:\dev\Bmazon\.vscode\extensions.json
```

This will create the shell of a project inside the `C:\dev\Bmazon` folder

To Learn more about Azure Functions, visit the [Azure Functions Documentation](https://docs.microsoft.com/en-us/azure/azure-functions/) page.

## Add Functions

NOTE: We're making our functions use Anonymous Authorization because we will eventually be using [Azure API Managment](https://azure.microsoft.com/en-us/services/api-management/) to secure the functions.

### CreateOrder Function (Shopping Division)

The Shopping division needs to call HTTP APIs to make an order to the warehouse, so we will add a `CreateOrder` function that performs this action.

```powershell
C:\dev\Bmazon> func new --template HTTPTrigger --name CreateOrder --authlevel Anonymous
Use the up/down arrow keys to select a template:Function name: CreateOrder

The function "CreateOrder" was created successfully from the "HTTPTrigger" template.
C:\dev\Bmazon>
```

Strangely, it outputs a prompt to select the template even when you have passed in the selection as a parameter. You can ignore this.

### Warehouse Division APIs

At a later date, the Warehouse services need to call an HTTP endpoint to send tracking information back to the Shopping division.

We will follow the pattern above and create an API for them to call.

```powershell
C:\dev\Bmazon> func new --template HTTPTrigger --name OrderShipped --authlevel Anonymous
Use the up/down arrow keys to select a template:Function name: OrderShipped

The function "OrderShipped" was created successfully from the "HTTPTrigger" template.
```

### Shared APIs

Since both the Shopping and Warehouse divisions will need to check on the status of an order, there will be a shared function to check status

```powershell
C:\dev\Bmazon> func new --template HTTPTrigger --name OrderStatus --authlevel Anonymous
Use the up/down arrow keys to select a template:Function name: OrderShipped

The function "OrderStatus" was created successfully from the "HTTPTrigger" template.
```

## Add OpenAPI (single doc)

In order to add OpenAPI to Azure Functions, I chose to the Swashbuckle library. There are a few other libraries out there to work with .Net and OpenAPI, but I chose Swashbuckle because I'm familiar with it.

### Installing the Extension

The core Swashbuckle project doesn't support Azure Functions directly, so I used [AzureExtensions.Swashbuckle](https://github.com/vitalybibikov/AzureExtensions.Swashbuckle), a nice extension written by Vitaly Bibikov

To install it:

```powershell
C:\dev\Bmazon> dotnet add package AzureExtensions.Swashbuckle
  Determining projects to restore...
  Writing C:\Users\XXX\AppData\Local\Temp\tmp69AA.tmp
info : Adding PackageReference for package 'AzureExtensions.Swashbuckle' into project 'C:\dev\Bmazon\Bmazon.csproj'.
info :   GET https://api.nuget.org/v3/registration5-gz-semver2/azureextensions.swashbuckle/index.json
info :   OK https://api.nuget.org/v3/registration5-gz-semver2/azureextensions.swashbuckle/index.json 140ms
info : Restoring packages for C:\dev\Bmazon\Bmazon.csproj...
info : Package 'AzureExtensions.Swashbuckle' is compatible with all the specified frameworks in project 'C:\dev\Bmazon\Bmazon.csproj'.
info : PackageReference for package 'AzureExtensions.Swashbuckle' version '3.2.2' added to file 'C:\dev\Bmazon\Bmazon.csproj'.
info : Committing restore...
info : Generating MSBuild file C:\dev\Bmazon\obj\Bmazon.csproj.nuget.g.props.
info : Writing assets file to disk. Path: C:\dev\Bmazon\obj\project.assets.json
log  : Restored C:\dev\Bmazon\Bmazon.csproj (in 525 ms).
```

### Setting up Swashbuckle

In order to configure Swashbuckle, your Functions App needs a Functions `Startup` class like the following:

```csharp
[assembly: FunctionsStartup(typeof(Bmazon.Startup))]
namespace Bmazon
{
  public class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
      builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
    }
  }
}
```

Additionally, you need to add Functions for generating the JSON and UI:

```csharp
namespace Bmazon.OpenApi
{
  public static class OpenApiFunctions
  {
    [SwaggerIgnore]
    [FunctionName("OpenApiJson")]
    public static Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "openapi/json")]
            HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashbuckleClient)
    {
      return Task.FromResult(swashbuckleClient.CreateSwaggerDocumentResponse(req));
    }

    [SwaggerIgnore]
    [FunctionName("OpenApiUI")]
    public static Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "openapi/ui")]
            HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashbuckleClient)
    {
      // CreateOpenApiUIResponse generates the HTML page from the JSON results
      return Task.FromResult(swashbuckleClient.CreateSwaggerUIResponse(
        req, "openapi/json"));
    }
  }
}
```

NOTE: The `[SwaggerIgnore]` attribute causes Swashbuckle to ignore these API methods

### Generate the Document

NOTE: **You must have the Azure Storage Emulator running locally in order for this to work**

```powershell
C:\dev\Bmazon> func start
Microsoft (R) Build Engine version 16.8.3+39993bd9d for .NET
Copyright (C) Microsoft Corporation. All rights reserved.

  Determining projects to restore...
  Restored C:\dev\Bmazon\Bmazon.csproj (in 840 ms).
  Bmazon -> C:\dev\Bmazon\bin\output\bin\Bmazon.dll

Build succeeded.

Time Elapsed 00:00:05.60

Azure Functions Core Tools
Core Tools Version:       3.0.3284 Commit hash: 98bc25e668274edd175a1647fe5a9bc4ffb6887d
Function Runtime Version: 3.0.15371.0

[2021-02-27T15:05:33.871Z] Found C:\dev\Bmazon\Bmazon.csproj. Using for user secrets file configuration.

Functions:

       CreateOrder: [POST] http://localhost:7071/api/order

        OpenApiJson: [GET] http://localhost:7071/api/openapi/json

        OpenApiUi: [GET] http://localhost:7071/api/openapi/ui

        OrderShipped: [POST] http://localhost:7071/api/order/shipment

        OrderShippingStatus: [GET] http://localhost:7071/api/order/shipment/{id}

For detailed output, run func with --verbose flag.
[2021-02-27T15:05:41.693Z] Host lock lease acquired by instance ID '000000000000000000000000016514FF'.
```

Notice the list of functions shown with the URLs next to them. If you visit the OpenApiUI URL, you will see the

TODO: One more thing here to show the output JSON (?)
