using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bmazon.OpenApi
{
  /// <summary>
  /// Function used to render the OpenApi UI
  /// </summary>
  public static class OpenApiUi
  {
    /// <summary>
    /// the function implemntation
    /// </summary>
    /// <param name="req">the http request</param>
    /// <param name="swashbuckleClient">the injected Swashbuckle client</param>
    /// <param name="doc">the optional document from the URL (default: "Everything"</param>
    /// <returns>the HTML page as an http response</returns>
    [SwaggerIgnore]
    [FunctionName(nameof(OpenApiUi))]
    public static Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "openapi/ui/{doc?}")]
            HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashbuckleClient,
        string doc)
    {
      // the CreateOpenApiUIResponse method actually generates the HTTP page from the JSON Function results
      return Task.FromResult(swashbuckleClient.CreateSwaggerUIResponse(
        req, $"openapi/json/{doc ?? "Everything"}"));
    }
  }
}