using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactorTwinAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsSuperUser = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanCreateReactor = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReactorTwins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Model = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Version = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReactorType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ThermalOutputMW = table.Column<double>(type: "double precision", nullable: false),
                    ElectricalOutputMW = table.Column<double>(type: "double precision", nullable: false),
                    FuelType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CoreTemperature = table.Column<double>(type: "double precision", nullable: false),
                    PressureLevel = table.Column<double>(type: "double precision", nullable: false),
                    CoolingSystemType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CurrentTemperature = table.Column<double>(type: "double precision", nullable: false),
                    CurrentPressure = table.Column<double>(type: "double precision", nullable: false),
                    CurrentPowerOutput = table.Column<double>(type: "double precision", nullable: false),
                    RadiationLevel = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactorTwins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactorTwins_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReactorTwins_OwnerId",
                table: "ReactorTwins",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReactorTwins");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
