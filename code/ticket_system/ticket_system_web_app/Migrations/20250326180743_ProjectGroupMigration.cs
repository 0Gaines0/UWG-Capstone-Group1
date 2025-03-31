using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    /// <inheritdoc />
    public partial class ProjectGroupMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "project_group");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectGroups");

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

            migrationBuilder.CreateIndex(
                name: "IX_project_group_AssignedProjectsPId",
                table: "project_group",
                column: "AssignedProjectsPId");
        }
    }
}
