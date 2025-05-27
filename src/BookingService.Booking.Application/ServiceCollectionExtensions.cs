using BookingService.Booking.Application.Contracts.Interfaces;
using BookingService.Booking.Application.Dates;
using BookingService.Booking.Application.Options;
using BookingService.Catalog.Api.Contracts.BookingJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestEase;

namespace BookingService.Booking.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BookingCatalogRestOptions>(configuration.GetSection("BookingCatalogRestOptions"));

        services.AddHttpClient("BookingCatalogRestOptions", (ctx, client) =>
        {
            var options = ctx.GetRequiredService<IOptions
            <BookingCatalogRestOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseAddress);
            client.Timeout = TimeSpan.FromSeconds(90);
        });

        services.AddScoped<IBookingsService, BookingsService>();
        services.AddSingleton<ICurrentDateTimeProvider, DefaultCurrentDateTimeProvider>();
        services.AddScoped<IBookingsBackgroundServiceHandler, BookingsBackgroundServiceHandler>();
        services.AddScoped(ctx => RestClient.For<IBookingJobsController>(ctx
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient("BookingCatalogRestOptions")));

        return services;
    }
}