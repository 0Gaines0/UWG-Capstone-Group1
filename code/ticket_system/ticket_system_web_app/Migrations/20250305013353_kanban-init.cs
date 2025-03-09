using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    /// <inheritdoc />
    public partial class kanbaninit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectBoards",
                columns: table => new
                {
                    board_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    p_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBoards", x => x.board_id);
                });

            migrationBuilder.CreateTable(
                name: "BoardStates",
                columns: table => new
                {
                    state_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    board_id = table.Column<int>(type: "int", nullable: false),
                    state_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardStates", x => x.state_id);
                    table.ForeignKey(
                        name: "FK_BoardStates_ProjectBoards_board_id",
                        column: x => x.board_id,
                        principalTable: "ProjectBoards",
                        principalColumn: "board_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_project_lead_id",
                table: "Projects",
                column: "project_lead_id");

            migrationBuilder.CreateIndex(
                name: "IX_BoardStates_board_id",
                table: "BoardStates",
                column: "board_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Employees_project_lead_id",
                table: "Projects",
                column: "project_lead_id",
                principalTable: "Employees",
                principalColumn: "e_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Employees_project_lead_id",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "BoardStates");

            migrationBuilder.DropTable(
                name: "ProjectBoards");

            migrationBuilder.DropIndex(
                name: "IX_Projects_project_lead_id",
                table: "Projects");
        }
    }
}
