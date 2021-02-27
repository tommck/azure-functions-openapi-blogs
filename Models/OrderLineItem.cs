using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bmazon.Models
{
  public class OrderLineItem
  {
    [Required, NotNull, Range(1, 1000)]
    public int Quantity { get; set; }

    [Required, NotNull, MinLength(1)]
    public string SKU { get; set; }
  }
}