using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Bmazon.Models;
using Bmazon.Services;

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
