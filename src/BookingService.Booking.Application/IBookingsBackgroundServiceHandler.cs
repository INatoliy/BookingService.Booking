namespace BookingService.Booking.Application
{
    public interface IBookingsBackgroundServiceHandler
    {
        Task HandleAsync(CancellationToken cancellationToken);
    }
}
