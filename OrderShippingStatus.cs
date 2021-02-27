using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Bmazon.Models;
using Bmazon.Services;

namespace Bmazon
{
  public class OrderShippingStatus
  {
    private readonly OrderService orderService;

    public OrderShippingStatus(OrderService orderService)
    {
      this.orderService = orderService;
    }

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
