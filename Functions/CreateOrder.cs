using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Bmazon.Models;
using System.Net.Http;
using Bmazon.Services;

namespace Bmazon.Functions
{
  public class CreateOrder
  {
    private readonly OrderService orderService;

    public CreateOrder(OrderService orderService)
    {
      this.orderService = orderService;
    }

    [FunctionName("CreateOrder")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "order")] HttpRequestMessage req,
        ILogger log)
    {
      var order = await req.Content.ReadAsAsync<Order>();

      log.LogInformation($"Received order {order.OrderId}");

      await this.orderService.SendOrderToWarehouse(order);

      return new OkResult();
    }
  }
}
