using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganicFood_MiniProject.Migrations
{
    /// <inheritdoc />
    public partial class addedbackgroundimagecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "SliderImages",
                newName: "BackgroundImage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BackgroundImage",
                table: "SliderImages",
                newName: "Image");
        }
    }
}
