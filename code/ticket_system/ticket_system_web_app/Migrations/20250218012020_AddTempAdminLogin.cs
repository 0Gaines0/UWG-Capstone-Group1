using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticket_system_web_app.Migrations
{
    /// <inheritdoc />
    public partial class AddTempAdminLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "e_id",
                keyValue: 1,
                column: "hashed_password",
                value: "$2a$11$tqFhRcVPxPe/F7g4i2.9c.tms9AlneY5RDZb1SipsY1FQtMcaaecu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "e_id",
                keyValue: 1,
                column: "hashed_password",
                value: "$2a$11$cc6lvd/424tR7Y0bbD1u1.zzf/g91HMGMDf/SAEFTyl3EwQQtkv9a");
        }
    }
}
