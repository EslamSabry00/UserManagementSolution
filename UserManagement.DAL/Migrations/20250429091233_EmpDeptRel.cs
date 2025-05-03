using Microsoft.EntityFrameworkCore.Migrations;

namespace UserManagement.DAL.Migrations
{
    public partial class EmpDeptRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeptId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DeptId",
                table: "Employees",
                column: "DeptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DeptId",
                table: "Employees",
                column: "DeptId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DeptId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DeptId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DeptId",
                table: "Employees");
        }
    }
}
