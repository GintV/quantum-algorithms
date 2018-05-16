using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace QuantumAlgorithms.Data.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinishTime",
                table: "QuantumAlgorithm",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InnerJobId",
                table: "QuantumAlgorithm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobId",
                table: "QuantumAlgorithm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "QuantumAlgorithm",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SubscriberId",
                table: "QuantumAlgorithm",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishTime",
                table: "QuantumAlgorithm");

            migrationBuilder.DropColumn(
                name: "InnerJobId",
                table: "QuantumAlgorithm");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "QuantumAlgorithm");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "QuantumAlgorithm");

            migrationBuilder.DropColumn(
                name: "SubscriberId",
                table: "QuantumAlgorithm");
        }
    }
}
