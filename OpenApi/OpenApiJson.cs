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
    /// <param name="group">the optional document from the URL (default: "Everything"</param>
    /// <returns>the JSON data as an http response</returns>
    [SwaggerIgnore]
    [FunctionName(nameof(OpenApiJson))]
    public static Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "openapi/json/{group?}")]
        HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashbuckleClient,
        string group)
    {
      return Task.FromResult(swashbuckleClient.CreateSwaggerJsonDocumentResponse(req, group ?? "Everything"));
    }
  }
}