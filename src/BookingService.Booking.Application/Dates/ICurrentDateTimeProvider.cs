namespace BookingService.Booking.Application.Dates;

public interface ICurrentDateTimeProvider
{
    DateTimeOffset LocalNow { get; }
    DateTimeOffset UtcNow { get; }
}