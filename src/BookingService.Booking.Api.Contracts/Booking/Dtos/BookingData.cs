namespace BookingService.Booking.Api.Contracts.Booking.Dtos;

public class BookingData
{
    public long Id { get; set; }
    public string Status { get; set; }
    public long UserId { get; set; }
    public long ResourceId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}