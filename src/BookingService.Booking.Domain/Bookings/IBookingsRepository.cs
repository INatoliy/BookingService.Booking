using BookingService.Booking.Domain.Contracts.Models;

namespace BookingService.Booking.Domain.Bookings;

public interface IBookingsRepository
{
    Task CreateAsync(BookingAggregate aggregate, CancellationToken cancellationToken);
    Task<BookingAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task UpdateAsync(BookingAggregate aggregate, CancellationToken cancellationToken);
    Task<List<BookingAggregate>> GetByFilterAsync(
        BookingStatus? status,
        long? userId,
        long? resourceId,
        DateOnly? startDate,
        DateOnly? endDate,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken);
}