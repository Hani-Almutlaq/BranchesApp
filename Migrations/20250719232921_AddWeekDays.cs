using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BranchesApp.Migrations
{
    /// <inheritdoc />
    public partial class AddWeekDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Days",
                columns: new[] { "DayId", "DayName" },
                values: new object[,]
                {
                    { 1, "Sunday" },
                    { 2, "Monday" },
                    { 3, "Tuesday" },
                    { 4, "Wednesday" },
                    { 5, "Thursday" },
                    { 6, "Friday" },
                    { 7, "Saturday" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "DayId",
                keyValue: 7);
        }
    }
}
