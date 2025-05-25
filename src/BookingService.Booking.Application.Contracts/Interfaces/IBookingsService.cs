using BookingService.Booking.Application.Contracts.Commands;
using BookingService.Booking.Application.Contracts.Models;
using BookingService.Booking.Application.Contracts.Queries;
using BookingService.Booking.Domain.Contracts.Models;

namespace BookingService.Booking.Application.Contracts.Interfaces;

public interface IBookingsService
{
    Task<long> CreateBookingAsync(CreateBookingCommand command, CancellationToken cancellationToken = default);
    Task<BookingDto> GetByIdAsync(GetBookingByIdQuery idQuery, CancellationToken cancellationToken = default);
    Task CancelBookingAsync(CancelBookingCommand command, CancellationToken cancellationToken = default);
    Task<BookingDto[]> GetByFilterAsync(GetBookingsByFilterQuery filterQuery, CancellationToken cancellationToken = default);
    Task<BookingStatus> GetStatusByIdAsync(GetBookingStatusByIdQuery idQuery, CancellationToken cancellationToken = default);
}