using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduMetricsApi.Migrations
{
    /// <inheritdoc />
    public partial class ComputerColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Browser",
                table: "UserSession");

            migrationBuilder.DropColumn(
                name: "UserIp",
                table: "UserSession");

            migrationBuilder.AddColumn<string>(
                name: "ComputerBrowser",
                table: "UserSession",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComputerIp",
                table: "UserSession",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComputerBrowser",
                table: "UserSession");

            migrationBuilder.DropColumn(
                name: "ComputerIp",
                table: "UserSession");

            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "UserSession",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserIp",
                table: "UserSession",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
