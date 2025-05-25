using BookingService.Booking.Application.Contracts.Commands;
using BookingService.Booking.Application.Contracts.Interfaces;
using BookingService.Booking.Application.Contracts.Models;
using BookingService.Booking.Application.Contracts.Queries;
using BookingService.Booking.Application.Dates;
using BookingService.Booking.Domain;
using BookingService.Booking.Domain.Bookings;
using BookingService.Booking.Domain.Contracts.Models;
using ValidationException = BookingService.Booking.Application.Contracts.Exceptions.ValidationException;

namespace BookingService.Booking.Application;

internal class BookingsService : IBookingsService
{
    private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookingsRepository _bookingsRepository;
    public BookingsService(
        IUnitOfWork unitOfWork,
        ICurrentDateTimeProvider currentDateTimeProvider,
        IBookingsRepository bookingsRepository)
    {
        _unitOfWork = unitOfWork;
        _currentDateTimeProvider = currentDateTimeProvider;
        _bookingsRepository = bookingsRepository;
    }
    public async Task<long> CreateBookingAsync(CreateBookingCommand command,
        CancellationToken cancellationToken)
    {
        var booking = BookingAggregate.Initialize(
            command.UserId,
            command.ResourceId,
            command.StartDate,
            command.EndDate,
            _currentDateTimeProvider.UtcNow);

        await _bookingsRepository.CreateAsync(booking, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return booking.Id;
    }

    public async Task CancelBookingAsync(CancelBookingCommand command,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingsRepository.GetByIdAsync(command.BookingId, cancellationToken);

        if (booking == null || booking.Id <= 0) throw new ValidationException("Не удалось найти бронирование.");

        await _bookingsRepository.UpdateAsync(booking, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task<BookingDto[]> GetByFilterAsync(GetBookingsByFilterQuery filterQuery,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingsRepository.GetByFilterAsync(
            filterQuery.Status,
            filterQuery.UserId,
            filterQuery.ResourceId,
            filterQuery.StartDate,
            filterQuery.EndDate,
            filterQuery.PageSize,
            filterQuery.PageNumber,
            cancellationToken);

        return booking.Select(booking => new BookingDto
        {
            Id = booking.Id,
            Status = booking.Status,
            UserId = booking.UserId,
            ResourceId = booking.ResourceId,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            CreatedAt = booking.CreatedAt
        }).ToArray();
    }

    public async Task<BookingStatus> GetStatusByIdAsync(GetBookingStatusByIdQuery idQuery,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingsRepository.GetByIdAsync(idQuery.BookingId, cancellationToken) ?? throw new ValidationException("Бронирование не найдено.");
        return booking.Status;
    }
    public async Task<BookingDto> GetByIdAsync(GetBookingByIdQuery idQuery,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingsRepository.GetByIdAsync(idQuery.BookingId, cancellationToken) ?? throw new ValidationException("Бронирование не найдено.");

        return new BookingDto
        {
            Id = booking.Id,
            Status = booking.Status,
            UserId = booking.UserId,
            ResourceId = booking.ResourceId,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            CreatedAt = booking.CreatedAt
        };
    }





}