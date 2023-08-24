using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedAssetsStockTaking.Migrations
{
    /// <inheritdoc />
    public partial class Obs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observations",
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
                name: "Observations",
                table: "MijlocFix");
        }
    }
}
