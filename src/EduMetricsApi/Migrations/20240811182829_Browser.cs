using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduMetricsApi.Migrations
{
    /// <inheritdoc />
    public partial class Browser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "UserSession",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Browser",
                table: "UserSession");
        }
    }
}
