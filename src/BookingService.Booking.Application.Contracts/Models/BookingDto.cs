using BookingService.Booking.Domain.Contracts.Models;

namespace BookingService.Booking.Application.Contracts.Models;

public class BookingDto
{
    public long Id { get; set; }
    public BookingStatus Status { get; set; }
    public long UserId { get; set; }
    public long ResourceId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}