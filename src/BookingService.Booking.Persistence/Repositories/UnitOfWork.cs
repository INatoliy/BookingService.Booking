using BookingService.Booking.Domain.Interfaces;
using BookingService.Booking.Persistence.Contexts;

namespace BookingService.Booking.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BookingsContext _dbContext;
    public UnitOfWork(BookingsContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}