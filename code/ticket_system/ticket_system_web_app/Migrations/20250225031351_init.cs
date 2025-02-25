using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                    p_description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.p_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
                name: "project_group",
                columns: table => new
                {
                    AssignedGroupsGId = table.Column<int>(type: "int", nullable: false),
                    AssignedProjectsPId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_group", x => new { x.AssignedGroupsGId, x.AssignedProjectsPId });
                    table.ForeignKey(
                        name: "FK_project_group_Groups_AssignedGroupsGId",
                        column: x => x.AssignedGroupsGId,
                        principalTable: "Groups",
                        principalColumn: "g_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_group_Projects_AssignedProjectsPId",
                        column: x => x.AssignedProjectsPId,
                        principalTable: "Projects",
                        principalColumn: "p_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "e_id", "email", "f_name", "hashed_password", "is_active", "is_admin", "is_manager", "l_name", "username" },
                values: new object[] { 1, "admin@temp.com", "admin", "$2a$11$tqFhRcVPxPe/F7g4i2.9c.tms9AlneY5RDZb1SipsY1FQtMcaaecu", true, true, true, "admin", "tempAdmin" });

            migrationBuilder.CreateIndex(
                name: "IX_group_member_GroupsExistingInGId",
                table: "group_member",
                column: "GroupsExistingInGId");

            migrationBuilder.CreateIndex(
                name: "IX_project_group_AssignedProjectsPId",
                table: "project_group",
                column: "AssignedProjectsPId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_member");

            migrationBuilder.DropTable(
                name: "project_group");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
