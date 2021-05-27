using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OwnershipService.Migrations
{
    public partial class InitialSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShareHolder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareHolder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Share",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchasePrice = table.Column<float>(type: "real", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    ShareHolderId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Share", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Share_ShareHolder_ShareHolderId",
                        column: x => x.ShareHolderId,
                        principalTable: "ShareHolder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Share_Id",
                table: "Share",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Share_ShareHolderId",
                table: "Share",
                column: "ShareHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareHolder_Id",
                table: "ShareHolder",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Share");

            migrationBuilder.DropTable(
                name: "ShareHolder");
        }
    }
}
