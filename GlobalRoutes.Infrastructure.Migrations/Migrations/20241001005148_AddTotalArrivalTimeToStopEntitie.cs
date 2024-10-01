using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalRoutes.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalArrivalTimeToStopEntitie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalArrivalTime",
                table: "stop",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalArrivalTime",
                table: "stop");
        }
    }
}
