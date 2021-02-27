using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bmazon.OpenApi
{
  /// <summary>
  /// Function to handle returning the OpenApi document as JSON
  /// </summary>
  public static class OpenApiJson
  {
    /// <summary>
    /// function implementation
    /// </summary>
    /// <param name="req">the http request</param>
    /// <param name="swashbuckleClient">the injected Swashbuckle client</param>
    /// <param name="doc">the optional document from the URL (default: "Everything"</param>
    /// <returns>the JSON data as an http response</returns>
    [SwaggerIgnore]
    [FunctionName(nameof(OpenApiJson))]
    public static Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "openapi/json/{doc?}")]
        HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashbuckleClient,
        string doc)
    {
      return Task.FromResult(swashbuckleClient.CreateSwaggerDocumentResponse(req, doc ?? "Everything"));
    }
  }
}