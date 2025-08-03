using System.Linq.Expressions;
using FluentResults;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Repositories;

public class BookRepository(LibraryDbContext dbContext, ILogger<BookRepository> logger) : IBookRepository
{
    public async Task<Result<Guid>> CreateBook(Book book, CancellationToken cancellationToken = default)
    {
        // I decided to use a transaction to ensure that the book is added atomically
        // Result pattern is used to handle exceptions that may occur during request cancellation, other errors
       
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            logger.LogInformation($"Have started creating book.");
            
            var bookEntity = await dbContext.AddAsync(book, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
            
            logger.LogInformation($"Book has been created successfully with ID: {bookEntity.Entity.Id}");
            
            return Result.Ok(bookEntity.Entity.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while creating book. Error: {ex.Message}");
            
            await transaction.RollbackAsync(cancellationToken);
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<Book?>> GetBook(Guid id, CancellationToken cancellationToken = default)
    {
        // FindASync does not throw an exception if the entity is not found,
        // but the cancellation of the operation may still occur
        
        try
        {
            logger.LogInformation($"Have started getting book with id: {id}.");
            
            var book = await dbContext.Books.FindAsync(id, cancellationToken);
            return Result.Ok(book);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while getting book with id: {id}. Error: {ex.Message}");
            
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<List<Book>>> GetBooks(Expression<Func<Book, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Have started getting books.");
            
            IQueryable<Book> query = dbContext.Books.AsNoTracking();
        
            if (predicate is not null)
            {
                query = query.Where(predicate);
            }
        
            var books = await query.ToListAsync(cancellationToken);
            
            logger.LogInformation($"Books found: {books.Count}");
            
            return Result.Ok(books);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while getting books. Error: {ex.Message}");
            
            return Result.Fail(ex.Message);
        }
    }
}