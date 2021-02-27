using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bmazon.Models
{
  /// <summary>
  /// A single line item in an order
  /// </summary>
  public class OrderLineItem
  {
    /// <summary>
    /// The number of the SKU desired
    /// </summary>
    [Required, NotNull, Range(1, 1000)]
    public int Quantity { get; set; }

    /// <summary>
    /// The SKU identifying the item to purchase
    /// </summary>
    [Required, NotNull, MinLength(1)]
    public string SKU { get; set; }
  }
}