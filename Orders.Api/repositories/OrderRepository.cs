using Dapper;
using Orders.Api.models;
using Orders.Api.Services.Dapper;
using System.Data;

namespace Orders.Api.repositories;

public class OrderRepository : IOrderRepository
{
    private const string ConnectionId = "DefaultConnection";
    private readonly IDapperService _dapper;

    public OrderRepository(IDapperService dapper)
    {
        _dapper = dapper;
    }

    private static StoredProcedureInf Sp(string name) => new()
    {
        IdConnectionString = ConnectionId,
        SchemaName = "dbo",
        Name = name
    };

    public async Task<(bool Success, int? OrderId, string? Error)> CreateAsync(Order order)
    {
        var p = new DynamicParameters();
        p.Add("@OrderNumber", order.OrderNumber);
        p.Add("@CustomerId", order.CustomerId);
        p.Add("@VehicleId", order.VehicleId);
        p.Add("@OriginCity", order.OriginCity);
        p.Add("@DestinationCity", order.DestinationCity);
        p.Add("@CargoDescription", order.CargoDescription);
        p.Add("@WeightKg", order.WeightKg);
        p.Add("@FreightAmount", order.FreightAmount);
        p.Add("@CurrencyCode", order.CurrencyCode);
        p.Add("@Status", order.Status);
        p.Add("@PickupDate", order.PickupDate);
        p.Add("@DeliveryDate", order.DeliveryDate);
        p.Add("@Notes", order.Notes);
        p.Add("@OrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        var response = await _dapper.ExecuteStoredProcedureNonQueryAsync(Sp("usp_TransportOrders_Create"), p);
        if (!response.Success) return (false, null, response.InnerException);
        
        return (true, p.Get<int>("@OrderId"), null);
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        var p = new DynamicParameters();
        p.Add("@OrderId", orderId);
        var response = await _dapper.ExecuteStoredProcedureAsync<Order>(Sp("usp_TransportOrders_GetById"), p, hasArray: false);
        return response.Success ? response.Result as Order : null;
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(string? status = null)
    {
        var p = new DynamicParameters();
        p.Add("@Status", status);
        p.Add("@CustomerId", null);
        p.Add("@OrderNumber", null);
        
        var response = await _dapper.ExecuteStoredProcedureAsync<Order>(Sp("usp_TransportOrders_GetAll"), p, hasArray: true);
        return response.Success && response.Result is IEnumerable<Order> list ? list.ToList() : Array.Empty<Order>();
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Order order)
    {
        var p = new DynamicParameters();
        p.Add("@OrderId", order.OrderId);
        p.Add("@OrderNumber", order.OrderNumber);
        p.Add("@CustomerId", order.CustomerId);
        p.Add("@VehicleId", order.VehicleId);
        p.Add("@OriginCity", order.OriginCity);
        p.Add("@DestinationCity", order.DestinationCity);
        p.Add("@CargoDescription", order.CargoDescription);
        p.Add("@WeightKg", order.WeightKg);
        p.Add("@FreightAmount", order.FreightAmount);
        p.Add("@CurrencyCode", order.CurrencyCode);
        p.Add("@Status", order.Status);
        p.Add("@PickupDate", order.PickupDate);
        p.Add("@DeliveryDate", order.DeliveryDate);
        p.Add("@Notes", order.Notes);

        var response = await _dapper.ExecuteStoredProcedureNonQueryAsync(Sp("usp_TransportOrders_Update"), p);
        return (response.Success, response.InnerException);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int orderId)
    {
        var p = new DynamicParameters();
        p.Add("@OrderId", orderId);
        var response = await _dapper.ExecuteStoredProcedureNonQueryAsync(Sp("usp_TransportOrders_Delete"), p);
        return (response.Success, response.InnerException);
    }
}