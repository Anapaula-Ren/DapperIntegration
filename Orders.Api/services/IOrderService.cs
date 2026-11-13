using Orders.Api.models;

namespace Orders.Api.services;

public interface IOrderService
{
    Task<(bool Success, int? OrderId, string? Error)> CreateOrderAsync(Order order);
    Task<Order?> GetByIdAsync(int id);
    Task<IReadOnlyList<Order>> GetFinishedAsync();
    Task<int> GetTotalAsync();
    Task<(bool Success, string? Error)> UpdateAsync(Order order);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
}