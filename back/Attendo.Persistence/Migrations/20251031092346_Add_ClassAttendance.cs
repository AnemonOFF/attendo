using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ClassAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassAttendance_classes_ClassId",
                table: "ClassAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_students_ClassAttendance_ClassAttendanceId",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_students_ClassAttendanceId",
                table: "students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassAttendance",
                table: "ClassAttendance");

            migrationBuilder.DropIndex(
                name: "IX_ClassAttendance_ClassId",
                table: "ClassAttendance");

            migrationBuilder.DropColumn(
                name: "ClassAttendanceId",
                table: "students");

            migrationBuilder.RenameTable(
                name: "ClassAttendance",
                newName: "class_attendance");

            migrationBuilder.AddPrimaryKey(
                name: "PK_class_attendance",
                table: "class_attendance",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "class_attendance_students",
                columns: table => new
                {
                    ClassAttendanceId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_attendance_students", x => new { x.ClassAttendanceId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_class_attendance_students_class_attendance_ClassAttendanceId",
                        column: x => x.ClassAttendanceId,
                        principalTable: "class_attendance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_attendance_students_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_class_attendance_ClassId_Date",
                table: "class_attendance",
                columns: new[] { "ClassId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_class_attendance_students_StudentId",
                table: "class_attendance_students",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_class_attendance_classes_ClassId",
                table: "class_attendance",
                column: "ClassId",
                principalTable: "classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_class_attendance_classes_ClassId",
                table: "class_attendance");

            migrationBuilder.DropTable(
                name: "class_attendance_students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_class_attendance",
                table: "class_attendance");

            migrationBuilder.DropIndex(
                name: "IX_class_attendance_ClassId_Date",
                table: "class_attendance");

            migrationBuilder.RenameTable(
                name: "class_attendance",
                newName: "ClassAttendance");

            migrationBuilder.AddColumn<int>(
                name: "ClassAttendanceId",
                table: "students",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassAttendance",
                table: "ClassAttendance",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_students_ClassAttendanceId",
                table: "students",
                column: "ClassAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassAttendance_ClassId",
                table: "ClassAttendance",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassAttendance_classes_ClassId",
                table: "ClassAttendance",
                column: "ClassId",
                principalTable: "classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_students_ClassAttendance_ClassAttendanceId",
                table: "students",
                column: "ClassAttendanceId",
                principalTable: "ClassAttendance",
                principalColumn: "Id");
        }
    }
}
