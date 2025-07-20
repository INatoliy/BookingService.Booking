using BookingService.Booking.Domain.Contracts.Models;
using BookingService.Booking.Domain.Entities;
using BookingService.Booking.Domain.Interfaces;
using BookingService.Booking.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Booking.Persistence.Repositories
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
