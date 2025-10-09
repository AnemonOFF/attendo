using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Groups_GroupId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_GroupId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Groups",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ClassId",
                table: "Groups",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Classes_ClassId",
                table: "Groups",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Classes_ClassId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ClassId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Groups");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Classes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_GroupId",
                table: "Classes",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Groups_GroupId",
                table: "Classes",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
