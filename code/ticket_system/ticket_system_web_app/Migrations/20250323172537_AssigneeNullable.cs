using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    /// <inheritdoc />
    public partial class AssigneeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectBoards_p_id",
                table: "ProjectBoards");

            migrationBuilder.AlterColumn<int>(
                name: "assignee_id",
                table: "task",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoards_p_id",
                table: "ProjectBoards",
                column: "p_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectBoards_p_id",
                table: "ProjectBoards");

            migrationBuilder.AlterColumn<int>(
                name: "assignee_id",
                table: "task",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoards_p_id",
                table: "ProjectBoards",
                column: "p_id");
        }
    }
}
