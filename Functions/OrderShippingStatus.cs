using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Bmazon.Models;
using Bmazon.Services;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Net;
using Microsoft.OpenApi.Models;

namespace Bmazon.Functions
{
  public class OrderShippingStatus
  {
    private readonly OrderService orderService;

    public OrderShippingStatus(OrderService orderService)
    {
      this.orderService = orderService;
    }

    /// <summary>
    /// Gets the current Shipping Information for an order, if present
    /// </summary>
    /// <param name="req">the request</param>
    /// <param name="id">the ID from the URL</param>
    /// <param name="log">the logger</param>
    /// <returns>the shipping info, or null if it hasn't shipped yet</returns>
    [OpenApiOperation("OrderShippingStatus", tags: new[] { "Shopping", "Warehouse" }, Description = "Gets the current Shipping Information for an order, if present")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "the Shipment ID")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OrderShippingInfo), Description = "Indicates success and returns the order's status")]
    [FunctionName("OrderShippingStatus")]
    public async Task<OrderShippingInfo> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "order/shipment/{id}")] HttpRequest req,
        int id, // comes from the URL
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      var info = await this.orderService.GetOrderStatus(id);

      return info;
    }
  }
}
