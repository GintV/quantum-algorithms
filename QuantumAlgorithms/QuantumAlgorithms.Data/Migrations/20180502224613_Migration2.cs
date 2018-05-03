using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace QuantumAlgorithms.Data.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Exponent",
                table: "QuantumAlgorithm",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Generator",
                table: "QuantumAlgorithm",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Modulus",
                table: "QuantumAlgorithm",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Result",
                table: "QuantumAlgorithm",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exponent",
                table: "QuantumAlgorithm");

            migrationBuilder.DropColumn(
                name: "Generator",
                table: "QuantumAlgorithm");

            migrationBuilder.DropColumn(
                name: "Modulus",
                table: "QuantumAlgorithm");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "QuantumAlgorithm");
        }
    }
}
