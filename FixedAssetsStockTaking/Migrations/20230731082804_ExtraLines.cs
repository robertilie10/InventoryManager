using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedAssetsStockTaking.Migrations
{
    /// <inheritdoc />
    public partial class ExtraLines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExtraLine",
                columns: table => new
                {
                    InventarID = table.Column<int>(type: "int", nullable: false),
                    MFLine = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraLine", x => new { x.InventarID, x.MFLine });
                    table.ForeignKey(
                        name: "FK_ExtraLine_Inventar_InventarID",
                        column: x => x.InventarID,
                        principalTable: "Inventar",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraLine_MijlocFix_MFLine",
                        column: x => x.MFLine,
                        principalTable: "MijlocFix",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtraLine_MFLine",
                table: "ExtraLine",
                column: "MFLine");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtraLine");
        }
    }
}
