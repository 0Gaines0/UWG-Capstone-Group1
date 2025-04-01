using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    e_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    f_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    l_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    hashed_password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true),
                    is_manager = table.Column<bool>(type: "bit", nullable: true),
                    is_admin = table.Column<bool>(type: "bit", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.e_id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    g_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    manager_id = table.Column<int>(type: "int", nullable: false),
                    g_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    g_description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.g_id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    p_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_lead_id = table.Column<int>(type: "int", nullable: false),
                    p_title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    p_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeEId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.p_id);
                    table.ForeignKey(
                        name: "FK_Projects_Employees_EmployeeEId",
                        column: x => x.EmployeeEId,
                        principalTable: "Employees",
                        principalColumn: "e_id");
                    table.ForeignKey(
                        name: "FK_Projects_Employees_project_lead_id",
                        column: x => x.project_lead_id,
                        principalTable: "Employees",
                        principalColumn: "e_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskChanges",
                columns: table => new
                {
                    change_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    previous_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    new_value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    assignee_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskChanges", x => x.change_id);
                    table.ForeignKey(
                        name: "FK_TaskChanges_Employees_assignee_id",
                        column: x => x.assignee_id,
                        principalTable: "Employees",
                        principalColumn: "e_id");
                });

            migrationBuilder.CreateTable(
                name: "group_member",
                columns: table => new
                {
                    EmployeesEId = table.Column<int>(type: "int", nullable: false),
                    GroupsExistingInGId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_member", x => new { x.EmployeesEId, x.GroupsExistingInGId });
                    table.ForeignKey(
                        name: "FK_group_member_Employees_EmployeesEId",
                        column: x => x.EmployeesEId,
                        principalTable: "Employees",
                        principalColumn: "e_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_group_member_Groups_GroupsExistingInGId",
                        column: x => x.GroupsExistingInGId,
                        principalTable: "Groups",
                        principalColumn: "g_id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    table.ForeignKey(
                        name: "FK_ProjectBoards_Projects_p_id",
                        column: x => x.p_id,
                        principalTable: "Projects",
                        principalColumn: "p_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGroups",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    ProjectPId = table.Column<int>(type: "int", nullable: true),
                    GroupGId = table.Column<int>(type: "int", nullable: true),
                    Accepted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGroups", x => new { x.ProjectId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_ProjectGroups_Groups_GroupGId",
                        column: x => x.GroupGId,
                        principalTable: "Groups",
                        principalColumn: "g_id");
                    table.ForeignKey(
                        name: "FK_ProjectGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "g_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectGroups_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "p_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectGroups_Projects_ProjectPId",
                        column: x => x.ProjectPId,
                        principalTable: "Projects",
                        principalColumn: "p_id");
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

            migrationBuilder.CreateTable(
                name: "task",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    state_id = table.Column<int>(type: "int", nullable: false),
                    assignee_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task", x => x.task_id);
                    table.ForeignKey(
                        name: "FK_task_BoardStates_state_id",
                        column: x => x.state_id,
                        principalTable: "BoardStates",
                        principalColumn: "state_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_task_Employees_assignee_id",
                        column: x => x.assignee_id,
                        principalTable: "Employees",
                        principalColumn: "e_id");
                });

            migrationBuilder.CreateTable(
                name: "TaskChangeLogs",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "int", nullable: false),
                    change_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskChangeLogs", x => new { x.task_id, x.change_id });
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

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "e_id", "email", "f_name", "hashed_password", "is_active", "is_admin", "is_manager", "l_name", "username" },
                values: new object[] { 1, "admin@temp.com", "admin", "$2a$11$tqFhRcVPxPe/F7g4i2.9c.tms9AlneY5RDZb1SipsY1FQtMcaaecu", true, true, true, "admin", "tempAdmin" });

            migrationBuilder.CreateIndex(
                name: "IX_BoardStates_board_id",
                table: "BoardStates",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_member_GroupsExistingInGId",
                table: "group_member",
                column: "GroupsExistingInGId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoards_p_id",
                table: "ProjectBoards",
                column: "p_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGroups_GroupGId",
                table: "ProjectGroups",
                column: "GroupGId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGroups_GroupId",
                table: "ProjectGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGroups_ProjectPId",
                table: "ProjectGroups",
                column: "ProjectPId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_EmployeeEId",
                table: "Projects",
                column: "EmployeeEId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_project_lead_id",
                table: "Projects",
                column: "project_lead_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_assignee_id",
                table: "task",
                column: "assignee_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_state_id",
                table: "task",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_TaskChangeLogs_change_id",
                table: "TaskChangeLogs",
                column: "change_id");

            migrationBuilder.CreateIndex(
                name: "IX_TaskChanges_assignee_id",
                table: "TaskChanges",
                column: "assignee_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_member");

            migrationBuilder.DropTable(
                name: "ProjectGroups");

            migrationBuilder.DropTable(
                name: "TaskChangeLogs");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "TaskChanges");

            migrationBuilder.DropTable(
                name: "task");

            migrationBuilder.DropTable(
                name: "BoardStates");

            migrationBuilder.DropTable(
                name: "ProjectBoards");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
