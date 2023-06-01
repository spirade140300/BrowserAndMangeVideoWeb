using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrowseAndManageVideos_WEB.Migrations
{
    /// <inheritdoc />
    public partial class build14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcessTime",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcessTime",
                table: "Movies");
        }
    }
}
