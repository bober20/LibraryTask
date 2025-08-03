using Library.API.DTOs;
using Library.API.Interfaces;
using Library.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

public class OrderController(IOrderService orderService): ControllerBase
{
    [HttpGet("api/[controller]")]
    public async Task<IActionResult> GetOrders([FromQuery] Guid? id, [FromQuery] DateTime? orderDate, CancellationToken cancellationToken = default)
    {
        var orders = await orderService.GetOrders(order =>
            (!id.HasValue || order.Id == id) &&
            (!orderDate.HasValue || order.OrderDate.Date == orderDate.Value.Date));
        
        if (orders.IsFailed)
        {
            return NotFound(orders.Errors.FirstOrDefault()?.Message ?? "No orders found.");
        }
        
        return Ok(orders.Value);
    }
    
    [HttpPost("api/[controller]")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDTO order, CancellationToken cancellationToken = default)
    {
        if (order == null)
        {
            return BadRequest("Order data is required.");
        }

        var result = await orderService.Create(order, cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetOrders), new { id = result.Value }, result.Value);
        }
        
        return BadRequest(result.Errors.FirstOrDefault()?.Message ?? "Failed to create order.");
    }
}