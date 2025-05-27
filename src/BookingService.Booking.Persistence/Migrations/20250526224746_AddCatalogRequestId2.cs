using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Booking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCatalogRequestId2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "catalog_request_id",
                table: "bookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "catalog_request_id",
                table: "bookings");
        }
    }
}
