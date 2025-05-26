using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkTimeTable.Migrations
{
    /// <inheritdoc />
    public partial class Rename_time_column_in_timetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Timetable",
                newName: "Hours");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Hours",
                table: "Timetable",
                newName: "Time");
        }
    }
}
