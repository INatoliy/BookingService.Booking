using BookingService.Booking.Domain;
using BookingService.Booking.Domain.Bookings;
using BookingService.Booking.Domain.Contracts.Models;
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

        public async Task<IReadOnlyCollection<BookingAggregate>> GetConfirmationAwaitingBookingsAsync
            (CancellationToken cancellationToken, int limit = 10)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Where(x => x.Status == BookingStatus.AwaitsConfirmation)
                .OrderBy(x => x.Id)
                .Take(limit)
                .ToListAsync();

        }
    }
}
