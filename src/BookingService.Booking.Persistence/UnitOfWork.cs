using BookingService.Booking.Domain;

namespace BookingService.Booking.Persistence;

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