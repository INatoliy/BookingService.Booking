using BookingService.Booking.Domain.Interfaces;
using BookingService.Booking.Persistence.Contexts;
using BookingService.Booking.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookingService.Booking.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

        services.AddScoped<IBookingsRepository, BookingsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBookingsBackgroundQueries, BookingsBackgroundQueries>();

        services.AddDbContext<BookingsContext>((ctx, context) =>
        {
            context.UseNpgsql(connectionString)
                .UseLoggerFactory(ctx.GetRequiredService<ILoggerFactory>());
        });
        return services;
    }
}