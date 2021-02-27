using System.Collections.Generic;

namespace Bmazon.Models
{
  public class Order
  {
    public int OrderId { get; set; }
    public IEnumerable<OrderLineItem> Items { get; set; }
  }
}