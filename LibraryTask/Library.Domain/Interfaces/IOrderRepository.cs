using System.Linq.Expressions;
using FluentResults;
using Library.Domain.Models;

namespace Library.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Result<Guid>> CreateOrder(Order order, CancellationToken cancellationToken = default);
    Task<Result<List<Order>>> GetOrders(Expression<Func<Order, bool>> filter, CancellationToken cancellationToken = default);
}