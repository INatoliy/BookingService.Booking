namespace BookingService.Booking.Application.Dates;

internal class DefaultCurrentDateTimeProvider : ICurrentDateTimeProvider
{
    public DateTimeOffset LocalNow => DateTimeOffset.Now.ToLocalTime();
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}