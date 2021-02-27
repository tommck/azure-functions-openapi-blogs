using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bmazon.Models
{
  public class Order
  {
    // if we get to 2 billion orders, we'll all be retired anyway
    [Required, NotNull, Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Required, NotNull, MinLength(1)]
    public IEnumerable<OrderLineItem> Items { get; set; }
  }
}