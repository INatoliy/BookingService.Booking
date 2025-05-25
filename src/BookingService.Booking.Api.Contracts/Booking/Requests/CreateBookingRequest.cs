namespace BookingService.Booking.Api.Contracts.Booking.Requests;

public class CreateBookingRequest
{
    public long UserId { get; set; }
    public long ResourceId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}