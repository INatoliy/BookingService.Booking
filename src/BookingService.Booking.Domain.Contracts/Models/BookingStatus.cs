namespace BookingService.Booking.Domain.Contracts.Models;

public enum BookingStatus
{
    AwaitsConfirmation = 0, // Ожидает подтверждения
    Confirmed,  // Подтверждено 
    Cancelled  // Отменено 
}