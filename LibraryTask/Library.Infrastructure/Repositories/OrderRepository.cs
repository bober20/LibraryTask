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
        var bookIds = order.Books.Select(b => b.Id).ToList();

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var availableBooks = await dbContext.Books
                .Where(b => bookIds.Contains(b.Id) && b.OrderId == null)
                .ToListAsync(cancellationToken);

            if (availableBooks.Count != bookIds.Count)
            {
                return Result.Fail("One or more books don't exist or are already ordered");
            }

            order.Books = availableBooks;
            var orderEntry = await dbContext.AddAsync(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        
            await transaction.CommitAsync(cancellationToken);
        
            return Result.Ok(orderEntry.Entity.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<List<Order>>> GetOrders(Expression<Func<Order, bool>>? filter, CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<Order> query = dbContext.Orders
                .Include(o => o.Books)
                .AsNoTracking();

            if (filter is not null)
            {
                query = query.Where(filter);
            }
            
            var orders = await query.ToListAsync(cancellationToken);
            return Result.Ok(orders);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}