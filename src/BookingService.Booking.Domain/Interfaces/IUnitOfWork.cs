namespace BookingService.Booking.Domain.Interfaces;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken);
}