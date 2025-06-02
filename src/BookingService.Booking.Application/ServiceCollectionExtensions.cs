using BookingService.Booking.Application.Contracts.Interfaces;
using BookingService.Booking.Application.Dates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.Booking.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBookingsService, BookingsService>();
        services.AddSingleton<ICurrentDateTimeProvider, DefaultCurrentDateTimeProvider>();

        return services;
    }
}