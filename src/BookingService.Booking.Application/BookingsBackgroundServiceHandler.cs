using BookingService.Booking.Domain;
using BookingService.Booking.Domain.Bookings;
using BookingService.Catalog.Api.Contracts.BookingJobs;
using BookingService.Catalog.Api.Contracts.BookingJobs.Queries;
using Microsoft.Extensions.Logging;

namespace BookingService.Booking.Application
{
    class BookingsBackgroundServiceHandler : IBookingsBackgroundServiceHandler
    {
        private readonly IBookingsBackgroundQueries _bookingsBackgroundQueries;
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IBookingJobsController _bookingJobsController;
        private readonly ILogger<BookingsBackgroundServiceHandler> _logger;

        public BookingsBackgroundServiceHandler(
               IBookingsBackgroundQueries bookingsBackgroundQueries,
               IBookingsRepository bookingsRepository,
               IBookingJobsController bookingJobsController,
               ILogger<BookingsBackgroundServiceHandler> logger)
        {
            _bookingsBackgroundQueries = bookingsBackgroundQueries;
            _bookingsRepository = bookingsRepository;
            _bookingJobsController = bookingJobsController;
            _logger = logger;

        }

        public async Task HandleAsync(CancellationToken cancellationToken)
        {
            var bookings = await _bookingsBackgroundQueries.GetConfirmationAwaitingBookingsAsync(cancellationToken);

            foreach (var booking in bookings)
            {
                try
                {
                    var query = new GetBookingJobStatusByRequestIdQuery
                    {
                        RequestId = booking.CatalogRequestId
                    };
                    var status = await _bookingJobsController.GetBookingJobStatusByRequestId(query, cancellationToken);


                    if (status == BookingJobStatus.Confirmed) booking.Confirm();
                    else if (status == BookingJobStatus.Cancelled) booking.Cancel();

                    await _bookingsRepository.UpdateAsync(booking, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Ошибка при обработке бронирования: ID {booking.Id}");
                }
            }
        }
    }
}
