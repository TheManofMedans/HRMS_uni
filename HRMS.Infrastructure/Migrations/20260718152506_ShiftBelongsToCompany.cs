using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ShiftBelongsToCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Users_UserId",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Shifts",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_UserId",
                table: "Shifts",
                newName: "IX_Shifts_CompanyId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Company_CompanyId",
                table: "Shifts",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Company_CompanyId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Shifts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_CompanyId",
                table: "Shifts",
                newName: "IX_Shifts_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Users_UserId",
                table: "Shifts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
