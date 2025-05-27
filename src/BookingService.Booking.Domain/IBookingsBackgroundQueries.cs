using BookingService.Booking.Domain.Bookings;

namespace BookingService.Booking.Domain
{
    public interface IBookingsBackgroundQueries
    {
        Task<IReadOnlyCollection<BookingAggregate>> GetConfirmationAwaitingBookingsAsync(CancellationToken cancellationToken, int limit = 10);
    }
}
