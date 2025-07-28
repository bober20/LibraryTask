using System.Linq.Expressions;
using FluentResults;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class OrderRepository(LibraryDbContext dbContext) : IOrderRepository
{
    public async Task<Result<Guid>> CreateOrder(Order order, CancellationToken cancellationToken = default)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var bookEntity = await dbContext.AddAsync(order, cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
            
            return Result.Ok(bookEntity.Entity.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<List<Order>>> GetOrders(Expression<Func<Order, bool>> filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var orders = await dbContext.Orders.AsNoTracking().Where(filter).ToListAsync(cancellationToken);
            return Result.Ok(orders);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}