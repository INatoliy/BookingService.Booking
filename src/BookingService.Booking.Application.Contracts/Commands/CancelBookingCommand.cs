namespace BookingService.Booking.Application.Contracts.Commands;

public class CancelBookingCommand
{
    public long BookingId { get; set; }
}