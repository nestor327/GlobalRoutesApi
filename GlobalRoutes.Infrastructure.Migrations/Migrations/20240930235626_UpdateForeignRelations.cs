using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalRoutes.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForeignRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_country_CountryId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CountryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "stop",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusId",
                table: "schedule",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_stop_ScheduleId",
                table: "stop",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_BusId",
                table: "schedule",
                column: "BusId");

            migrationBuilder.AddForeignKey(
                name: "FK_schedule_bus_BusId",
                table: "schedule",
                column: "BusId",
                principalTable: "bus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_stop_schedule_ScheduleId",
                table: "stop",
                column: "ScheduleId",
                principalTable: "schedule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_schedule_bus_BusId",
                table: "schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_stop_schedule_ScheduleId",
                table: "stop");

            migrationBuilder.DropIndex(
                name: "IX_stop_ScheduleId",
                table: "stop");

            migrationBuilder.DropIndex(
                name: "IX_schedule_BusId",
                table: "schedule");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "stop");

            migrationBuilder.DropColumn(
                name: "BusId",
                table: "schedule");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CountryId",
                table: "AspNetUsers",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_country_CountryId",
                table: "AspNetUsers",
                column: "CountryId",
                principalTable: "country",
                principalColumn: "Id");
        }
    }
}
