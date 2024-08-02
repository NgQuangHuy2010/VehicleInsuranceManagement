using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project3.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleInformationNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "VehicleInformations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Usage",
                table: "VehicleInformations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarrantyType",
                table: "VehicleInformations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolicyType",
                table: "VehicleInformations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "VehicleInformations");

            migrationBuilder.DropColumn(
                name: "Usage",
                table: "VehicleInformations");

            migrationBuilder.DropColumn(
                name: "WarrantyType",
                table: "VehicleInformations");

            migrationBuilder.DropColumn(
                name: "PolicyType",
                table: "VehicleInformations");
        }
    }

}
