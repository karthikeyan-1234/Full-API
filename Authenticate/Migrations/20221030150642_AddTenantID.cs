using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authenticate.Migrations
{
    public partial class AddTenantID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantID",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "AspNetUsers");
        }
    }
}
