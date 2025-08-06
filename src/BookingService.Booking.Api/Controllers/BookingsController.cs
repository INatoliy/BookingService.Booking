using BookingService.Booking.Api.Contracts;
using BookingService.Booking.Api.Contracts.Booking.Requests;
using BookingService.Booking.Application.Contracts.Commands;
using BookingService.Booking.Application.Contracts.Interfaces;
using BookingService.Booking.Application.Contracts.Queries;
using BookingService.Booking.Domain.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Booking.Api.Controllers;

[Authorize]
[ApiController]
[Route(WebRoutes.BasePath)]
public class BookingsController : ControllerBase
{
    private readonly IBookingsService _bookingsService;

    public BookingsController(IBookingsService bookingsService)
    {
        _bookingsService = bookingsService;
    }

    [HttpPost(WebRoutes.Create)]
    public async Task<IActionResult> CreateBookingAsync([FromBody] CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        // Маппинг DTO API в команду для севисного слоя
        var command = new CreateBookingCommand
        {
            UserId = request.UserId,
            ResourceId = request.ResourceId,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        var bookingId = await _bookingsService.CreateBookingAsync(command, cancellationToken);
        return Ok(new { BookingId = bookingId });
    }

    [HttpPost(WebRoutes.Cancel)]
    public async Task<IActionResult> CancelBookingAsync(long id, CancellationToken cancellationToken = default)
    {
        var command = new CancelBookingCommand { BookingId = id };
        await _bookingsService.CancelBookingAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpPost(WebRoutes.GetByFilter)]
    public async Task<IActionResult> GetBookingsByFilter([FromBody] GetBookingsByFilterRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetBookingsByFilterQuery
        {
            Status = request.Status != null ? Enum.Parse<BookingStatus>(request.Status) : null,
            UserId = request.UserId,
            ResourceId = request.ResourceId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            PageSize = request.PageSize,
            PageNumber = request.PageNumber
        };

        var bookings = await _bookingsService.GetByFilterAsync(query, cancellationToken);
        return Ok(bookings);
    }

    [HttpGet(WebRoutes.GetById)]
    public async Task<IActionResult> GetBooking(long id, CancellationToken cancellationToken = default)
    {
        var query = new GetBookingByIdQuery { BookingId = id };
        var booking = await _bookingsService.GetByIdAsync(query, cancellationToken);
        return Ok(booking);
    }

    [HttpGet(WebRoutes.GetStatusById)]
    public async Task<IActionResult> GetBookingStatus(long id, CancellationToken cancellationToken = default)
    {
        var query = new GetBookingStatusByIdQuery { BookingId = id };
        var status = await _bookingsService.GetStatusByIdAsync(query, cancellationToken);
        return Ok(status.ToString());
    }
}