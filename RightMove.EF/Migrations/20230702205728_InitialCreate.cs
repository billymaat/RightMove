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
                    ResultsTableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultsTable", x => x.ResultsTableId);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    RightMovePropertyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RightMoveId = table.Column<int>(type: "int", nullable: false),
                    HouseInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateReduced = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Prices = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultsTableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.RightMovePropertyId);
                    table.ForeignKey(
                        name: "FK_Properties_ResultsTable_ResultsTableId",
                        column: x => x.ResultsTableId,
                        principalTable: "ResultsTable",
                        principalColumn: "ResultsTableId",
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
