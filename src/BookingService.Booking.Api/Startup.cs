using BookingService.Booking.Application;
using BookingService.Booking.Application.Contracts.Exceptions;
using BookingService.Booking.Domain.Contracts.Exceptions;
using BookingService.Booking.Persistence;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using ValidationException = BookingService.Booking.Application.Contracts.Exceptions.ValidationException;

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
        services.AddApplication();

        var connectionString = Configuration.GetConnectionString("BookingsContext");
        services.AddPersistence(connectionString);

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