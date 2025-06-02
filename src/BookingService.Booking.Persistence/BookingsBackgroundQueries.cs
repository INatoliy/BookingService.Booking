using BookingService.Booking.Domain;
using BookingService.Booking.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Booking.Persistence
{
    class BookingsBackgroundQueries : IBookingsBackgroundQueries
    {
        private readonly BookingsContext _context;
        public BookingsBackgroundQueries(BookingsContext context)
        {
            _context = context;
        }
        public async Task<BookingAggregate?> GetBookingByRequestIdAsync(Guid requestId, CancellationToken cancellationToken)
        {
            return await _context.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CatalogRequestId == requestId);

        }
    }
}
