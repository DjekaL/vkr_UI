using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfApp1.Migrations
{
    public partial class TablesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "Devices");

            migrationBuilder.AddColumn<double>(
                name: "DataSize",
                table: "DataTransferLogs",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataSize",
                table: "DataTransferLogs");

            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
