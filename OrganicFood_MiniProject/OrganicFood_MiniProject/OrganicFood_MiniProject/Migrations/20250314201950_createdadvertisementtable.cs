using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganicFood_MiniProject.Migrations
{
    /// <inheritdoc />
    public partial class createdadvertisementtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BackgroundImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThirdImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FourthImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Discounts");
        }
    }
}
