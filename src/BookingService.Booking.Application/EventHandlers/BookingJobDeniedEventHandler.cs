using BookingService.Booking.Domain;
using BookingService.Catalog.Async.Api.Contracts.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace BookingService.Booking.Application.EventHandlers
{
    class BookingJobDeniedEventHandler : IHandleMessages<BookingJobDenied>
    {
        private readonly IBookingsBackgroundQueries _bookingsBackgroundQueries;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookingJobDeniedEventHandler> _logger;
        public BookingJobDeniedEventHandler(
            IBookingsBackgroundQueries bookingsBackgroundQueries,
            IUnitOfWork unitOfWork,
            ILogger<BookingJobDeniedEventHandler> logger)
        {
            _bookingsBackgroundQueries = bookingsBackgroundQueries;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(BookingJobDenied message)
        {
            var booking = await _bookingsBackgroundQueries.GetBookingByRequestIdAsync(message.RequestId);

            if (booking != null)
            {
                _logger.LogWarning($"Бронирование не найдено для RequestId: {message.RequestId}");
                return;
            }

            booking.Confirm();
            _logger.LogInformation($"Бронирование ID:{booking.Id} успешно подтверждено");
            await _unitOfWork.CommitAsync();
        }
    }
}
