using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalRoutes.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddNavegationEntitiesToStopEntitie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoordinateId",
                table: "stop",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_stop_CoordinateId",
                table: "stop",
                column: "CoordinateId");

            migrationBuilder.AddForeignKey(
                name: "FK_stop_coordinate_CoordinateId",
                table: "stop",
                column: "CoordinateId",
                principalTable: "coordinate",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stop_coordinate_CoordinateId",
                table: "stop");

            migrationBuilder.DropIndex(
                name: "IX_stop_CoordinateId",
                table: "stop");

            migrationBuilder.DropColumn(
                name: "CoordinateId",
                table: "stop");
        }
    }
}
