using Microsoft.EntityFrameworkCore.Migrations;

namespace auth_api.Migrations
{
    public partial class changeThumbnailForPreview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                table: "BlogPosts",
                newName: "Preview");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Preview",
                table: "BlogPosts",
                newName: "ThumbnailUrl");
        }
    }
}
