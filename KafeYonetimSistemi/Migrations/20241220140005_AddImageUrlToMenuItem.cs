using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KafeYonetimSistemi.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToMenuItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "MenuItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "MenuItem");
        }
    }
}
