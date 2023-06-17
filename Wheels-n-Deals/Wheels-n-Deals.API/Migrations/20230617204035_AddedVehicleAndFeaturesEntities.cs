using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheels_n_Deals.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedVehicleAndFeaturesEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CarBody = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FuelType = table.Column<string>(type: "text", nullable: false),
                    EngineSize = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Gearbox = table.Column<string>(type: "text", nullable: false),
                    HorsePower = table.Column<long>(type: "bigint", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VinNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Make = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Year = table.Column<long>(type: "bigint", nullable: false),
                    Mileage = table.Column<long>(type: "bigint", nullable: false),
                    PriceInEuro = table.Column<float>(type: "real", nullable: false),
                    FeatureId = table.Column<Guid>(type: "uuid", nullable: false),
                    TechnicalState = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VinNumber);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "timestamp with time zone",
                maxLength: 50,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
