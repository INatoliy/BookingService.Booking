using BookingService.Booking.Domain.Entities;

namespace BookingService.Booking.Domain.Interfaces
{
    public interface IBookingsBackgroundQueries
    {
        Task<IReadOnlyCollection<BookingAggregate>> GetConfirmationAwaitingBookingsAsync(CancellationToken cancellationToken, int limit = 10);
    }
}
