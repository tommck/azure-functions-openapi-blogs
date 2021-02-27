using Bmazon.Models;
using System;
using System.Threading.Tasks;

namespace Bmazon.Services
{
  public class OrderService
  {
    public async Task SendOrderToWarehouse(Order order)
    {
      await Task.Run(() =>
      {
        Console.WriteLine($"Sending Order {order.OrderId}");
      });
    }

    public async Task<OrderShippingInfo> GetOrderStatus(int orderId) {
      return await Task.Run(() =>
      {
        return new OrderShippingInfo()
        {
          OrderId = orderId,
          ShippingCarrier = "FedEx",
          TrackingNumber = "abc123"
        };
      });
    }

    public async Task SaveOrderShippingInfo(OrderShippingInfo info)
    {
      await Task.Run(() =>
      {
        Console.WriteLine($"Saving Shipping Info for Order {info.OrderId}");
      });
    }
  }
}
