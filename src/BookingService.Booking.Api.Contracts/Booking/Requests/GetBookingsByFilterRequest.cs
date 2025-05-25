namespace BookingService.Booking.Api.Contracts.Booking.Requests;

public class GetBookingsByFilterRequest
{
    public string? Status { get; set; }
    public long? UserId { get; set; }
    public long? ResourceId { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}