using BookingService.Booking.Domain.Bookings;

namespace BookingService.Booking.Domain
{
    public interface IBookingsBackgroundQueries
    {
        Task<BookingAggregate?> GetBookingByRequestIdAsync(Guid requestId, CancellationToken cancellationToken = default);
    }
}
