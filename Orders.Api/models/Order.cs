namespace Orders.Api.models;

public class Order
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int? VehicleId { get; set; }
    public string OriginCity { get; set; } = string.Empty;
    public string DestinationCity { get; set; } = string.Empty;
    public string CargoDescription { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public decimal FreightAmount { get; set; }
    public string CurrencyCode { get; set; } = "MXN";
    public string Status { get; set; } = string.Empty;
    public DateTime PickupDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? Notes { get; set; }
    
    // Propiedad calculada
    public bool HasFinished => Status is "Delivered" or "Cancelled";
    
    // Para que el controlador actual siga funcionando, mapeamos Id a OrderId
    public string Id { 
        get => OrderNumber; 
        set => OrderNumber = value; 
    }
}