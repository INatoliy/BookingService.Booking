using BookingService.Booking.Application.Contracts.Exceptions;
using BookingService.Booking.Application;
using BookingService.Booking.Persistence;
using BookingService.Booking.Domain.Contracts.Exceptions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

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
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.
        })
        services.AddControllers();
        services.AddApplication(Configuration);

        var connectionString = Configuration.GetConnectionString("BookingsContext");
        services.AddPersistence(connectionString);
        services.AddHostedService<BookingsBackgroundService>();
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