using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    /// <inheritdoc />
    public partial class CreateTempAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                    table: "Employees",
                    columns: new[] { "e_id", "email", "f_name", "l_name", "username", "hashed_password", "is_active", "is_admin", "is_manager" },
                    values: new object[] { 1, "admin@temp.com", "admin", "admin", "tempAdmin", "$2a$11$tqFhRcVPxPe/F7g4i2.9c.tms9AlneY5RDZb1SipsY1FQtMcaaecu", true, true, true }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                     table: "Employees",
                     columns: new[] { "e_id", "email", "f_name", "l_name", "username", "hashed_password", "is_active", "is_admin", "is_manager" },
                     values: new object[] { 1, "admin@temp.com", "admin", "admin", "tempAdmin", "$2a$11$tqFhRcVPxPe/F7g4i2.9c.tms9AlneY5RDZb1SipsY1FQtMcaaecu", true, true, true }
             );
        }
    }
}
