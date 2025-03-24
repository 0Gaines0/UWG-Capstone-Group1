using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskChangeLogs",
                table: "TaskChangeLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskChangeLogs",
                table: "TaskChangeLogs",
                columns: new[] { "task_id", "change_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskChangeLogs",
                table: "TaskChangeLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskChangeLogs",
                table: "TaskChangeLogs",
                column: "task_id");
        }
    }
}
