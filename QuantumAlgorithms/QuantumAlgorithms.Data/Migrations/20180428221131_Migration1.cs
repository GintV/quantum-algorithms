using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace QuantumAlgorithms.Data.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuantumAlgorithm",
                columns: table => new
                {
                    FactorP = table.Column<int>(type: "int", nullable: true),
                    FactorQ = table.Column<int>(type: "int", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantumAlgorithm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantumAlgorithmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionMessages_QuantumAlgorithm_QuantumAlgorithmId",
                        column: x => x.QuantumAlgorithmId,
                        principalTable: "QuantumAlgorithm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionMessages_QuantumAlgorithmId",
                table: "ExecutionMessages",
                column: "QuantumAlgorithmId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionMessages");

            migrationBuilder.DropTable(
                name: "QuantumAlgorithm");
        }
    }
}
