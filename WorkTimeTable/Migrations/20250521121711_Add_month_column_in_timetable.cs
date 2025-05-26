using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkTimeTable.Migrations
{
    /// <inheritdoc />
    public partial class Add_month_column_in_timetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "Timetable",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Timetable");
        }
    }
}
