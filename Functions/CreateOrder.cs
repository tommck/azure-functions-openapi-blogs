using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Bmazon.Models;
using System.Net;
using System.Net.Http;
using Bmazon.Services;
using AzureFunctions.Extensions.Swashbuckle.Attribute;

namespace Bmazon.Functions
{
  public class CreateOrder
  {
    private readonly OrderService orderService;

    public CreateOrder(OrderService orderService)
    {
      this.orderService = orderService;
    }

    /// <summary>
    /// Creates an Order that will be shipped to the Warehouse for fulfillment.
    /// </summary>
    /// <param name="req">the HTTP request</param>
    /// <param name="log">the logger</param>
    /// <returns>a success message or a collection of error messages</returns>
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
    [FunctionName("CreateOrder")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "order")]
        [RequestBodyType(typeof(Order), "The Order To Create")]
        HttpRequestMessage req,
        ILogger log)
    {
      var order = await req.Content.ReadAsAsync<Order>();

      log.LogInformation($"Received order {order.OrderId}");

      await this.orderService.SendOrderToWarehouse(order);

      return new OkObjectResult("Order Created Successfully");
    }
  }
}
