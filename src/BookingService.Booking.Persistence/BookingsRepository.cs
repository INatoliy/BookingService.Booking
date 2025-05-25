using BookingService.Booking.Domain.Bookings;
using BookingService.Booking.Domain.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Booking.Persistence;

public class BookingsRepository : IBookingsRepository
{
    private readonly DbSet<BookingAggregate> _dbSet;

    public BookingsRepository(BookingsContext context)
    {
        _dbSet = context.Bookings;
    }

    public async Task CreateAsync(BookingAggregate aggregate, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(aggregate);
    }
    public async Task<BookingAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }
    public async Task UpdateAsync(BookingAggregate aggregate, CancellationToken cancellationToken)
    {
        _dbSet.Attach(aggregate);
        _dbSet.Entry(aggregate).State = EntityState.Modified;
    }
    public async Task<List<BookingAggregate>> GetByFilterAsync(
         BookingStatus? status,
         long? userId,
         long? resourceId,
         DateOnly? startDate,
         DateOnly? endDate,
         int pageSize,
         int pageNumber,
         CancellationToken cancellationToken)

    {
        var skip = pageNumber;
        var take = pageSize;
        var query = _dbSet.AsQueryable().AsNoTracking();

        if (status.HasValue)
            query = query.Where(q => q.Status == status);
        if (userId.HasValue)
            query = query.Where(q => q.UserId == userId);
        if (resourceId.HasValue)
            query = query.Where(q => q.ResourceId == resourceId);
        if (startDate.HasValue)
            query = query.Where(q => q.StartDate >= startDate);
        if (endDate.HasValue)
            query = query.Where(q => q.EndDate <= endDate);

        return await query
            .OrderBy(q => q.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}
