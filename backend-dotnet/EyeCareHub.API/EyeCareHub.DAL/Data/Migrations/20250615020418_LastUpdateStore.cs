using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeCareHub.DAL.Data.Migrations
{
    public partial class LastUpdateStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipToAddress_FirstName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipToAddress_LastName",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress_FirstName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress_LastName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
