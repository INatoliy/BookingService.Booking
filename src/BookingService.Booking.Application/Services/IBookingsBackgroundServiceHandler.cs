namespace BookingService.Booking.Application.Services
{
    public interface IBookingsBackgroundServiceHandler
    {
        Task HandleAsync(CancellationToken cancellationToken);
    }
}
