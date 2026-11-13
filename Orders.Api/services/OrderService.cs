using Orders.Api.models;
using Orders.Api.repositories;

namespace Orders.Api.services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<(bool Success, int? OrderId, string? Error)> CreateOrderAsync(Order order)
    {
        return await _repository.CreateAsync(order);
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IReadOnlyList<Order>> GetFinishedAsync()
    {
        var all = await _repository.GetAllAsync();
        return all.Where(o => o.HasFinished).ToList();
    }

    public async Task<int> GetTotalAsync()
    {
        var all = await _repository.GetAllAsync();
        return all.Count;
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Order order)
    {
        return await _repository.UpdateAsync(order);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}