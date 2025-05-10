using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organic_Food_MVC_Project.Migrations
{
    /// <inheritdoc />
    public partial class CreatedBlogCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogCategoryId",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogCategoryId",
                table: "Blogs",
                column: "BlogCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogCategories_BlogCategoryId",
                table: "Blogs",
                column: "BlogCategoryId",
                principalTable: "BlogCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogCategories_BlogCategoryId",
                table: "Blogs");

            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_BlogCategoryId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "BlogCategoryId",
                table: "Blogs");
        }
    }
}
