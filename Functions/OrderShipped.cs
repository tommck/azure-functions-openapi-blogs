using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Bmazon.Models;
using Bmazon.Services;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;

namespace Bmazon.Functions
{
  public class OrderShipped
  {
    private readonly OrderService orderService;

    public OrderShipped(OrderService orderService)
    {
      this.orderService = orderService;
    }

    /// <summary>
    /// Called to tell the system that an Order has shipped from the warehouse
    /// </summary>
    /// <param name="req">the request</param>
    /// <param name="log">the logger</param>
    /// <returns>nothing</returns>
    /// <response code="200">
    ///   Indicates success. Returns no payload
    /// </response>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [FunctionName("OrderShipped")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "order/shipment")]
        [RequestBodyType(typeof(OrderShippingInfo), "The Shipping Information for the Order")]
        HttpRequestMessage req,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      var info = await req.Content.ReadAsAsync<OrderShippingInfo>();

      await this.orderService.SaveOrderShippingInfo(info);

      return new OkResult();
    }
  }
}
