using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organic_Food_MVC_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBlogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Comment",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Like",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Like",
                table: "Blogs");
        }
    }
}
