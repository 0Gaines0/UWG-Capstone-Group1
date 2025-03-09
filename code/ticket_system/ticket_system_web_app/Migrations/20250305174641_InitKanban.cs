using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    /// <inheritdoc />
    public partial class InitKanban : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoards_p_id",
                table: "ProjectBoards",
                column: "p_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBoards_Projects_p_id",
                table: "ProjectBoards",
                column: "p_id",
                principalTable: "Projects",
                principalColumn: "p_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBoards_Projects_p_id",
                table: "ProjectBoards");

            migrationBuilder.DropIndex(
                name: "IX_ProjectBoards_p_id",
                table: "ProjectBoards");
        }
    }
}
