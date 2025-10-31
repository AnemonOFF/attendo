using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_Class_To_OpenApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_groups_classes_ClassId",
                table: "groups");

            migrationBuilder.DropForeignKey(
                name: "FK_students_classes_ClassId",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_students_ClassId",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_groups_ClassId",
                table: "groups");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "students");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "groups");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Start",
                table: "classes",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "End",
                table: "classes",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "classes",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "classes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "classes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "classes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "classes",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_classes_GroupId",
                table: "classes",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_classes_groups_GroupId",
                table: "classes",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_classes_groups_GroupId",
                table: "classes");

            migrationBuilder.DropIndex(
                name: "IX_classes_GroupId",
                table: "classes");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "classes");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "classes");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "classes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "classes");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "classes");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "students",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "groups",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Start",
                table: "classes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "End",
                table: "classes",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateIndex(
                name: "IX_students_ClassId",
                table: "students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_groups_ClassId",
                table: "groups",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_groups_classes_ClassId",
                table: "groups",
                column: "ClassId",
                principalTable: "classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_students_classes_ClassId",
                table: "students",
                column: "ClassId",
                principalTable: "classes",
                principalColumn: "Id");
        }
    }
}
