using BookingService.Booking.Application.Contracts.Commands;
using BookingService.Booking.Application.Contracts.Exceptions;
using BookingService.Booking.Application.Contracts.Interfaces;
using BookingService.Booking.Application.Contracts.Models;
using BookingService.Booking.Application.Contracts.Queries;
using BookingService.Booking.Application.Dates;
using BookingService.Booking.Domain;
using BookingService.Booking.Domain.Bookings;
using BookingService.Booking.Domain.Contracts.Models;
using BookingService.Catalog.Api.Contracts.BookingJobs;
using BookingService.Catalog.Api.Contracts.BookingJobs.Commands;

namespace BookingService.Booking.Application;

internal class BookingsService : IBookingsService
{
    private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IBookingJobsController _bookingJobsController;
    public BookingsService(
        IUnitOfWork unitOfWork,
        ICurrentDateTimeProvider currentDateTimeProvider,
        IBookingsRepository bookingsRepository,
        IBookingJobsController bookingJobsController)
    {
        _unitOfWork = unitOfWork;
        _currentDateTimeProvider = currentDateTimeProvider;
        _bookingsRepository = bookingsRepository;
        _bookingJobsController = bookingJobsController;
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

        var catalogRequestId = Guid.NewGuid();
        var catalogCommand = new CreateBookingJobCommand()
        {
            RequestId = catalogRequestId,
            ResourceId = command.ResourceId,
            StartDate = command.StartDate,
            EndDate = command.EndDate
        };
        // вот тут надо понять куда записывать то что он возвращает
        await _bookingJobsController.CreateBookingJob(catalogCommand, cancellationToken);
        booking.SetCatalogRequestId(catalogRequestId);

        await _bookingsRepository.CreateAsync(booking, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return booking.Id;
    }

    public async Task CancelBookingAsync(CancelBookingCommand command,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingsRepository.GetByIdAsync(command.BookingId, cancellationToken);

        if (booking == null || booking.Id <= 0) throw new ValidationException("Не удалось найти бронирование.");

        var catalogCommand = new CancelBookingJobByRequestIdCommand()
        {
            RequestId = booking.CatalogRequestId
        };
        if (catalogCommand.RequestId != null) _bookingJobsController.CancelBookingJob(catalogCommand, cancellationToken);

        booking.Cancel();
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
        var booking = await _bookingsRepository.GetByIdAsync(idQuery.BookingId, cancellationToken) 
            ?? throw new ValidationException("Бронирование не найдено.");

        return booking.Status;
    }
    public async Task<BookingDto> GetByIdAsync(GetBookingByIdQuery idQuery,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingsRepository.GetByIdAsync(idQuery.BookingId, cancellationToken) 
            ?? throw new ValidationException("Бронирование не найдено.");

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