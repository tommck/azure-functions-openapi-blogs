using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bmazon.Models
{
  public class OrderShippingInfo
  {
    [Required, NotNull]
    public int OrderId { get; set; }
    [Required, NotNull, MinLength(1)]
    public string ShippingCarrier { get; set; }
    [Required, NotNull, MinLength(1)]
    public string TrackingNumber { get; set; }
  }
}