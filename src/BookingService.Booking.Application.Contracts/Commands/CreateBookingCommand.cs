namespace BookingService.Booking.Application.Contracts.Commands;

public class CreateBookingCommand
{
    public long UserId { get; set; }
    public long ResourceId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}