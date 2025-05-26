using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkTimeTable.Migrations
{
    /// <inheritdoc />
    public partial class Add_time_in_timetable_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "Timetable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Timetable");
        }
    }
}
