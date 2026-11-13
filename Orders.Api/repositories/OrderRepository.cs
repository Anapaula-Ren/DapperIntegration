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
        p.Add("@OrderNumber", order.Id);
        p.Add("@CustomerId", 1); // Temporal hasta que adaptemos el modelo
        p.Add("@OriginCity", "Monterrey");
        p.Add("@DestinationCity", "CDMX");
        p.Add("@CargoDescription", "Carga General");
        p.Add("@WeightKg", 1000);
        p.Add("@FreightAmount", 5000);
        p.Add("@Status", order.Status);
        p.Add("@PickupDate", DateTime.Today);
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
        
        var response = await _dapper.ExecuteStoredProcedureAsync<Order>(Sp("usp_TransportOrders_GetAll"), p, hasArray: true);
        return response.Success && response.Result is IEnumerable<Order> list ? list.ToList() : Array.Empty<Order>();
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Order order)
    {
        var p = new DynamicParameters();
        p.Add("@OrderId", order.Id);
        p.Add("@OrderNumber", order.Id);
        p.Add("@CustomerId", 1);
        p.Add("@OriginCity", "Monterrey");
        p.Add("@DestinationCity", "CDMX");
        p.Add("@CargoDescription", "Carga General");
        p.Add("@WeightKg", 1000);
        p.Add("@FreightAmount", 5000);
        p.Add("@CurrencyCode", "MXN");
        p.Add("@Status", order.Status);
        p.Add("@PickupDate", DateTime.Today);

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