using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkTimeTable.Migrations
{
    /// <inheritdoc />
    public partial class Rename_Workerid_to_WorkerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetable_Worker_Workerid",
                table: "Timetable");

            migrationBuilder.RenameColumn(
                name: "Workerid",
                table: "Timetable",
                newName: "WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_Timetable_Workerid",
                table: "Timetable",
                newName: "IX_Timetable_WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetable_Worker_WorkerId",
                table: "Timetable",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetable_Worker_WorkerId",
                table: "Timetable");

            migrationBuilder.RenameColumn(
                name: "WorkerId",
                table: "Timetable",
                newName: "Workerid");

            migrationBuilder.RenameIndex(
                name: "IX_Timetable_WorkerId",
                table: "Timetable",
                newName: "IX_Timetable_Workerid");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetable_Worker_Workerid",
                table: "Timetable",
                column: "Workerid",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
