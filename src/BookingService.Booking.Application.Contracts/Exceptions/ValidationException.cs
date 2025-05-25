namespace BookingService.Booking.Application.Contracts.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}