using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrowseAndManageVideos_WEB.Migrations
{
    /// <inheritdoc />
    public partial class build5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_MovieDetail_MovieDetailId",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "MovieDetailId",
                table: "Movies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_MovieDetail_MovieDetailId",
                table: "Movies",
                column: "MovieDetailId",
                principalTable: "MovieDetail",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_MovieDetail_MovieDetailId",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "MovieDetailId",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_MovieDetail_MovieDetailId",
                table: "Movies",
                column: "MovieDetailId",
                principalTable: "MovieDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
