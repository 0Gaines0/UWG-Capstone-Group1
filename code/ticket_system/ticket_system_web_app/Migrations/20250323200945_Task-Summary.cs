using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    /// <inheritdoc />
    public partial class TaskSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "summary",
                table: "task",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "summary",
                table: "task");
        }
    }
}
