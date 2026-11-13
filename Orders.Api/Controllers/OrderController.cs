using Microsoft.AspNetCore.Mvc;
using Orders.Api.models;
using Orders.Api.services;

namespace Orders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        var (success, id, error) = await _service.CreateOrderAsync(order);
        if (!success) return BadRequest(new { error });
        return Ok(new { orderId = id });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _service.GetByIdAsync(id);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpGet("terminadas")]
    public async Task<IActionResult> GetFinished()
    {
        var ordenes = await _service.GetFinishedAsync();
        return Ok(new { ordenesTerminadas = ordenes });
    }

    [HttpGet("total")]
    public async Task<IActionResult> GetTotal()
    {
        var total = await _service.GetTotalAsync();
        return Ok(new { totalOrders = total });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Order order)
    {
        var (success, error) = await _service.UpdateAsync(order);
        if (!success) return BadRequest(new { error });
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _service.DeleteAsync(id);
        if (!success) return BadRequest(new { error });
        return NoContent();
    }
}