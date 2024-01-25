using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerapiaExam.Migrations
{
    public partial class editEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookLink",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramLink",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedinLink",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterLink",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookLink",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "InstagramLink",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LinkedinLink",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TwitterLink",
                table: "Employees");
        }
    }
}
