using BookingService.Booking.Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Booking.Persistence.Configurations;

public class BookingAggregateConfiguration : IEntityTypeConfiguration<BookingAggregate>
{
    public void Configure(EntityTypeBuilder<BookingAggregate> builder)
    {
        builder.ToTable("bookings");

        builder.HasKey(x => x.Id).HasName("pk_bookings");

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Status).HasColumnName("status");
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.ResourceId).HasColumnName("resource_id");
        builder.Property(x => x.StartDate).HasColumnName("start_date");
        builder.Property(x => x.EndDate).HasColumnName("end_date");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at_date_time");
        builder.Property(x => x.CatalogRequestId).HasColumnName("catalog_request_id");
    }
}