using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BookingService.Booking.Persistence.Contexts;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookingsContext>
{
    public BookingsContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookingsContext>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.Persistence.json", false, true)
            .Build();

        var connectionString = configuration.GetConnectionString(nameof(BookingsContext));
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException($"Строка подключения для '{nameof(BookingsContext)}' не найдена");

        optionsBuilder.UseNpgsql(connectionString);

        return new BookingsContext(optionsBuilder.Options);
    }
}