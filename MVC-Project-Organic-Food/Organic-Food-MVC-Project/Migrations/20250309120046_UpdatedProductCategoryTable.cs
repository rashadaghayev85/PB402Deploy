using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organic_Food_MVC_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedProductCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "ProductCategories");
        }
    }
}
