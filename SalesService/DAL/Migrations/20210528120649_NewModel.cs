﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesService.DAL.Migrations
{
    public partial class NewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "SaleRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SaleRequest");
        }
    }
}
