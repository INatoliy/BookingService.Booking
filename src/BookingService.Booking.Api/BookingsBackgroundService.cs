using BookingService.Booking.Application;

namespace BookingService.Booking.Api
{
    public class BookingsBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BookingsBackgroundService> _logger;

        public BookingsBackgroundService(
               IServiceProvider serviceProvider,
               ILogger<BookingsBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var handler = scope.ServiceProvider
                            .GetRequiredService<IBookingsBackgroundServiceHandler>();
                        await handler.HandleAsync(cancellationToken);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка в фоновой задаче");
                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                }

            }
        }
    }
}
