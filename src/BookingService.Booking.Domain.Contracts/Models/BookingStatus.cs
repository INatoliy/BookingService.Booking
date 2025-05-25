namespace BookingService.Booking.Domain.Contracts.Models;

public enum BookingStatus
{
    AwaitsConfirmation, // Ожидает подтверждения
    Confirmed, // Подтверждено 
    Cancelled // Отменено 
}