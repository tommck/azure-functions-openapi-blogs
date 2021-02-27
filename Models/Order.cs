using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bmazon.Models
{
  /// <summary>
  /// An Order sent from the Shipping Division to be sent to the Warehouse
  /// </summary>
  public class Order
  {
    /// <summary>
    /// The unique ID for the order
    /// </summary>
    [Required, NotNull, Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    /// <summary>
    /// The individual line items in the order
    /// </summary>
    [Required, NotNull, MinLength(1)]
    public IEnumerable<OrderLineItem> Items { get; set; }
  }
}