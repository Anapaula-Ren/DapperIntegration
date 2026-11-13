using Orders.Api.models;

namespace Orders.Api.repositories;

public interface IOrderRepository
{
    Task<(bool Success, int? OrderId, string? Error)> CreateAsync(Order order);
    Task<Order?> GetByIdAsync(int orderId);
    Task<IReadOnlyList<Order>> GetAllAsync(string? status = null);
    Task<(bool Success, string? Error)> UpdateAsync(Order order);
    Task<(bool Success, string? Error)> DeleteAsync(int orderId);
}