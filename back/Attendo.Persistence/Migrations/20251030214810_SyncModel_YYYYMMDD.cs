using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Attendo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel_YYYYMMDD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassAttendanceId",
                table: "students",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClassAttendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassAttendance_classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_students_ClassAttendanceId",
                table: "students",
                column: "ClassAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassAttendance_ClassId",
                table: "ClassAttendance",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_students_ClassAttendance_ClassAttendanceId",
                table: "students",
                column: "ClassAttendanceId",
                principalTable: "ClassAttendance",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_ClassAttendance_ClassAttendanceId",
                table: "students");

            migrationBuilder.DropTable(
                name: "ClassAttendance");

            migrationBuilder.DropIndex(
                name: "IX_students_ClassAttendanceId",
                table: "students");

            migrationBuilder.DropColumn(
                name: "ClassAttendanceId",
                table: "students");
        }
    }
}
