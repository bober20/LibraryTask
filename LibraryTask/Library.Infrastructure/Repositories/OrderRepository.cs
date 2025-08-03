using System.Linq.Expressions;
using FluentResults;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Repositories;

public class OrderRepository(LibraryDbContext dbContext, ILogger<OrderRepository> logger) : IOrderRepository
{
    public async Task<Result<Guid>> CreateOrder(Order order, CancellationToken cancellationToken = default)
    {
        var bookIds = order.Books.Select(b => b.Id).ToList();

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            logger.LogInformation($"Starting to create order with {bookIds.Count} books.");
            
            var availableBooks = await dbContext.Books
                .Where(b => bookIds.Contains(b.Id) && b.OrderId == null)
                .ToListAsync(cancellationToken);

            if (availableBooks.Count != bookIds.Count)
            {
                logger.LogWarning("One or more books do not exist or are already ordered.");
                
                return Result.Fail("One or more books don't exist or are already ordered");
            }

            order.Books = availableBooks;
            var orderEntry = await dbContext.AddAsync(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        
            await transaction.CommitAsync(cancellationToken);
            
            logger.LogInformation($"Order created successfully with ID: {orderEntry.Entity.Id}");
        
            return Result.Ok(orderEntry.Entity.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while creating order. Error: {ex.Message}");
            
            await transaction.RollbackAsync(cancellationToken);
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<List<Order>>> GetOrders(Expression<Func<Order, bool>>? filter, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Starting to retrieve orders.");
            
            IQueryable<Order> query = dbContext.Orders
                .Include(o => o.Books)
                .AsNoTracking();

            if (filter is not null)
            {
                query = query.Where(filter);
            }
            
            var orders = await query.ToListAsync(cancellationToken);
            
            logger.LogInformation($"Retrieved {orders.Count} orders.");
            
            return Result.Ok(orders);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while retrieving orders. Error: {ex.Message}");
            
            return Result.Fail(ex.Message);
        }
    }
}