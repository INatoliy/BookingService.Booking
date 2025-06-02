using BookingService.Booking.Domain;
using BookingService.Catalog.Async.Api.Contracts.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace BookingService.Booking.Application.EventHandlers
{
    class BookingJobConfirmedEventHandler : IHandleMessages<BookingJobConfirmed>
    {
        private readonly IBookingsBackgroundQueries _bookingsBackgroundQueries;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookingJobConfirmedEventHandler> _logger;
        public BookingJobConfirmedEventHandler(
            IBookingsBackgroundQueries bookingsBackgroundQueries,
            IUnitOfWork unitOfWork,
            ILogger<BookingJobConfirmedEventHandler> logger)
        {
            _bookingsBackgroundQueries = bookingsBackgroundQueries;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task Handle(BookingJobConfirmed message)
        {
            var booking = await _bookingsBackgroundQueries.GetBookingByRequestIdAsync(message.RequestId);

            if (booking != null)
            {
                _logger.LogWarning($"Бронирование не найдено для RequestId: {message.RequestId}");
                return;
            }

            booking.Cancel();
            _logger.LogInformation($"Бронирование ID:{booking.Id} успешно отменено");
            await _unitOfWork.CommitAsync();

        }
    }
}
