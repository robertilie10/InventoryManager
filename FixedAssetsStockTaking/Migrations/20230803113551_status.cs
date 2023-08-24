using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedAssetsStockTaking.Migrations
{
    /// <inheritdoc />
    public partial class status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToCheck",
                table: "MijlocFix",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "MijlocFix",
                newName: "ToCheck");
        }
    }
}
