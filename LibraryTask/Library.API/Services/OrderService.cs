using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Library.API.DTOs;
using Library.API.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Models;

namespace Library.API.Services;

public class OrderService(IOrderRepository repository, IMapper mapper) : IOrderService
{
    public async Task<Result<Guid>> Create(OrderDTO order, CancellationToken cancellationToken = default)
    {
        var orderModel = mapper.Map<Order>(order);
        return await repository.CreateOrder(orderModel, cancellationToken);
    }
    
    public async Task<Result<List<OrderDTO>>> GetOrders(Expression<Func<Order, bool>> filter, 
        CancellationToken cancellationToken = default)
    {
        var result = await repository.GetOrders(filter, cancellationToken);
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        var ordersDto = mapper.Map<List<OrderDTO>>(result.Value);
        return Result.Ok(ordersDto);
    }
}