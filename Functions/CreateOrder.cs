using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Bmazon.Models;
using System.Net.Http;
using Bmazon.Services;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;

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
    [OpenApiOperation("CreateOrder", tags: new[] { "Shopping" }, Description = "Creates an Order that will be shipped to the Warehouse for fulfillment.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Order), Description = "The Order To Create")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Indicates success and returns a user-friendly message")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(IEnumerable<string>), Description = "Indicates a data validation issue and will return a list of data validation errors")]
    [FunctionName("CreateOrder")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "order")]
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
