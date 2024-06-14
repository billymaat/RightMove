using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RightMove.Db.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatePostGre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultsTable",
                columns: table => new
                {
                    ResultsTableId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultsTable", x => x.ResultsTableId);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    RightMovePropertyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RightMoveId = table.Column<int>(type: "integer", nullable: false),
                    HouseInfo = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateReduced = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResultsTableId = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    RightMovePropertyEntityRightMovePropertyId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Properties_RightMovePropertyEntityRightMovePropertyId",
                        column: x => x.RightMovePropertyEntityRightMovePropertyId,
                        principalTable: "Properties",
                        principalColumn: "RightMovePropertyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prices_RightMovePropertyEntityRightMovePropertyId",
                table: "Prices",
                column: "RightMovePropertyEntityRightMovePropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ResultsTableId",
                table: "Properties",
                column: "ResultsTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "ResultsTable");
        }
    }
}
