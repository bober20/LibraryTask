using System.Linq.Expressions;
using FluentResults;
using Library.API.DTOs;
using Library.Domain.Models;

namespace Library.API.Interfaces;

public interface IOrderService
{
    Task<Result<Guid>> Create(OrderDTO order, CancellationToken cancellationToken = default);

    Task<Result<List<OrderDTO>>> GetOrders(Expression<Func<Order, bool>> filter,
        CancellationToken cancellationToken = default);
}