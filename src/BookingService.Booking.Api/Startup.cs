using BookingService.Booking.Application;
using BookingService.Booking.Application.Contracts.Exceptions;
using BookingService.Booking.Domain.Contracts.Exceptions;
using BookingService.Booking.Persistence;
using BookingService.Booking.Application.EventHandlers;
using BookingService.Catalog.Async.Api.Contracts.Events;
using BookingService.Catalog.Async.Api.Contracts.Requests;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Serialization.Json;
using Rebus.Handlers;

namespace BookingService.Booking.Api;

public class Startup
{
    private IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApplication(Configuration);

        var connectionString = Configuration.GetConnectionString("BookingsContext");
        services.AddPersistence(connectionString);

        services.AddRebusHandler<BookingJobConfirmedEventHandler>();
        services.AddRebusHandler<BookingJobDeniedEventHandler>();
        services.AddRebus(configure =>
        configure.Transport(t => t.UseRabbitMq("amqp://admin:admin@localhost:5672/", "domain-service-queue")
        .DefaultQueueOptions(queue => queue.SetDurable(true))
        .ExchangeNames("booking-service-booking-direct", "booking-service-topics"))
        .Serialization(s => s.UseSystemTextJson())
        .Logging(l => l.Serilog())
        .Routing(r => r.TypeBased()));

        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Booking Service API",
                Version = "v1",
                Description = "API для управления бронированиями"
            });
        });
        services.AddProblemDetails(options =>
        {
            options.MapToStatusCode<ValidationException>(StatusCodes.Status400BadRequest);
            options.MapToStatusCode<DomainException>(StatusCodes.Status402PaymentRequired);

            options.Map<ValidationException>(ex => new ProblemDetails
            {
                Status = 400,
                Title = ex.Message
            });
            options.Map<DomainException>(ex => new ProblemDetails
            {
                Status = 402,
                Title = ex.Message,
                Detail = ex.StackTrace
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseExceptionHandler("/error");

        app.UseStatusCodePages();

        app.ApplicationServices
            .GetRequiredService<IBus>()
            .Subscribe<BookingJobConfirmed>();
        app.ApplicationServices
            .GetRequiredService<IBus>()
            .Subscribe<BookingJobDenied>();


        using var scope = app.ApplicationServices.CreateScope();
        var handler1 = scope.ServiceProvider.GetService<IHandleMessages<BookingJobConfirmed>>();
        var handler2 = scope.ServiceProvider.GetService<IHandleMessages<BookingJobDenied>>();

        if (handler1 == null || handler2 == null)
        {
            throw new Exception("Обработчики не зарегистрированы в DI!");
        }


        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
        app.UseProblemDetails();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}