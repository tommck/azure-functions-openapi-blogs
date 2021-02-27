using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Bmazon.Models;
using Bmazon.Services;

namespace Bmazon
{
  public class OrderShipped
  {
    private readonly OrderService orderService;

    public OrderShipped(OrderService orderService)
    {
      this.orderService = orderService;
    }

    [FunctionName("OrderShipped")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "order/shipment")] HttpRequestMessage req,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      var info = await req.Content.ReadAsAsync<OrderShippingInfo>();

      await this.orderService.SaveOrderShippingInfo(info);

      return new OkResult();
    }
  }
}
