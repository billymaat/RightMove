using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RightMove.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultsTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResultsTableId = table.Column<int>(type: "INTEGER", nullable: false),
                    RightMoveId = table.Column<int>(type: "INTEGER", nullable: false),
                    HouseInfo = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateReduced = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Prices = table.Column<string>(type: "TEXT", nullable: false),
                    Dates = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_ResultsTable_ResultsTableId",
                        column: x => x.ResultsTableId,
                        principalTable: "ResultsTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ResultsTableId",
                table: "Properties",
                column: "ResultsTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "ResultsTable");
        }
    }
}
