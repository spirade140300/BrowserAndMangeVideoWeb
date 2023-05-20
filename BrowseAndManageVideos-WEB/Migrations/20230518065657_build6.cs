using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrowseAndManageVideos_WEB.Migrations
{
    /// <inheritdoc />
    public partial class build6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_MovieDetail_MovieDetailId",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "MovieDetail");

            migrationBuilder.DropIndex(
                name: "IX_Movies_MovieDetailId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MovieDetailId",
                table: "Movies");

            migrationBuilder.AddColumn<string>(
                name: "Actor",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EncodingBitrate",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrameHeight",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrameWidth",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalBitrate",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actor",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "EncodingBitrate",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "FrameHeight",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "FrameWidth",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "TotalBitrate",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "MovieDetailId",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MovieDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Actor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EncodingBitrate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrameHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrameWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalBitrate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieDetail", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_MovieDetailId",
                table: "Movies",
                column: "MovieDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_MovieDetail_MovieDetailId",
                table: "Movies",
                column: "MovieDetailId",
                principalTable: "MovieDetail",
                principalColumn: "Id");
        }
    }
}
