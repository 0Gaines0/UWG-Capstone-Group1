using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    /// <inheritdoc />
    public partial class TaskChangeLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskChangeLogs",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "int", nullable: false),
                    change_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskChangeLogs", x => x.task_id);
                    table.ForeignKey(
                        name: "FK_TaskChangeLogs_TaskChanges_change_id",
                        column: x => x.change_id,
                        principalTable: "TaskChanges",
                        principalColumn: "change_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskChangeLogs_task_task_id",
                        column: x => x.task_id,
                        principalTable: "task",
                        principalColumn: "task_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskChangeLogs_change_id",
                table: "TaskChangeLogs",
                column: "change_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskChangeLogs");
        }
    }
}
