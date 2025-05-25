using BookingService.Booking.Domain.Bookings;
using BookingService.Booking.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BookingService.Booking.Persistence;

public class BookingsContext : DbContext
{
    public DbSet<BookingAggregate> Bookings { get; set; }
    public BookingsContext(DbContextOptions<BookingsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookingAggregateConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}