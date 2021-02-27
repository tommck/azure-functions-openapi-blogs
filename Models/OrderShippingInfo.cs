namespace Bmazon.Models
{
  public class OrderShippingInfo
  {
    public int OrderId { get; set; }
    public string ShippingCarrier { get; set; }
    public string TrackingNumber { get; set; }
  }
}