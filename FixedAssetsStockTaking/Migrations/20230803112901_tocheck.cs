using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedAssetsStockTaking.Migrations
{
    /// <inheritdoc />
    public partial class tocheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Locked",
                table: "MijlocFix",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ToCheck",
                table: "MijlocFix",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locked",
                table: "MijlocFix");

            migrationBuilder.DropColumn(
                name: "ToCheck",
                table: "MijlocFix");
        }
    }
}
