namespace BookingService.Booking.Domain.Contracts.Models;

public enum BookingStatus
{
    AwaitsConfirmation = 0, // Ожидает подтверждения
    Confirmed = 1, // Подтверждено 
    Cancelled = 2 // Отменено 
}