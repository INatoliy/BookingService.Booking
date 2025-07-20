using BookingService.Booking.Domain.Contracts.Exceptions;
using BookingService.Booking.Domain.Contracts.Models;
using BookingService.Booking.Domain.Entities;

namespace BookingService.Booking.Domain.UnitTests;

public class BookingAggregateTests
{
    [Fact]
    public void Initialize_Create_ValidParameters()
    {
        var userId = 1;
        var resourceId = 1;
        var startDate = new DateOnly(2025, 1, 1);
        var endDate = new DateOnly(2025, 1, 10);
        var now = DateTimeOffset.UtcNow;

        var bookingAggregate = BookingAggregate.Initialize(userId, resourceId, startDate, endDate, now);

        Assert.NotNull(bookingAggregate);
        Assert.Equal(BookingStatus.AwaitsConfirmation, bookingAggregate.Status);
        Assert.Equal(now, bookingAggregate.CreatedAt);
    }

    [Theory]
    [InlineData(0, 1, "2025-01-10", "2025-01-01")]
    [InlineData(1, 0, "2025-01-10", "2025-01-01")]
    [InlineData(1, 1, "2025-01-10", "2025-01-01")]
    public void Initialize_InvalidParameters_ThrowsException(long userId, long resourceId, string start, string end)
    {
        var startDate = DateOnly.Parse(start);
        var endDate = DateOnly.Parse(end);
        var now = DateTimeOffset.UtcNow;

        Assert.Throws<DomainException>(() =>
            BookingAggregate.Initialize(userId, resourceId, startDate, endDate, now));
    }

    [Fact]
    public void Confirm_WhenAwaitsConfirmation()
    {
        var bookingAggregate = BookingAggregate.Initialize(1, 1, new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10),
            DateTimeOffset.UtcNow);

        bookingAggregate.Confirm();

        Assert.Equal(BookingStatus.Confirmed, bookingAggregate.Status);
    }

    [Fact]
    public void Confirm_WhenNotAwaitsConfirmation()
    {
        var bookingAggregate = BookingAggregate.Initialize(1, 1, new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10),
            DateTimeOffset.UtcNow);

        bookingAggregate.Confirm();

        Assert.Throws<DomainException>(bookingAggregate.Confirm);
    }

    [Fact]
    public void Cancel_WhenNotCancelled()
    {
        var bookingAggregate = BookingAggregate.Initialize(1, 1, new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10),
            DateTimeOffset.UtcNow);

        bookingAggregate.Cancel();

        Assert.Equal(BookingStatus.Cancelled, bookingAggregate.Status);
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled()
    {
        var bookingAggregate = BookingAggregate.Initialize(1, 1, new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10),
            DateTimeOffset.UtcNow);

        bookingAggregate.Cancel();

        Assert.Throws<DomainException>(bookingAggregate.Cancel);
    }
}