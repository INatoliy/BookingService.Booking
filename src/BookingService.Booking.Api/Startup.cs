using BookingService.Booking.Application.Contracts.Exceptions;
using BookingService.Booking.Application;
using BookingService.Booking.Persistence;
using BookingService.Booking.Domain.Contracts.Exceptions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        var jwtSettings = Configuration.GetSection("Jwt"); // конфиг токена
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // схема, которая будет использоваться по умолчанию для проверки токена.
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;   // схема, которая будет использоваться для отправки ответа, если пользователь не авторизован.
        }).AddJwtBearer(options =>
        {      // Настройки для проверки JWT-токена
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,                         //проверять ли, что токен выдан нужным издателем.
                ValidateAudience = true,                       //проверять ли, что токен предназначен для нужного получателя.
                ValidateLifetime = true,                       // проверять ли срок действия токена.
                ValidateIssuerSigningKey = true,               //проверять ли подпись токена.
                ValidIssuer = jwtSettings["Issuer"],           //строка, с которой сравнивается Issuer в токене.
                ValidAudience = jwtSettings["Audience"],       //строка, с которой сравнивается Audience в токене.
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))// ключ, которым подписан токен (секретная строка, которую знаете только вы и ваш сервер).
            };
        });
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