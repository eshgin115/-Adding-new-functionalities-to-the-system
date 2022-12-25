using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApplication.Migrations
{
    public partial class AdressNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiverName",
                table: "addresses",
                newName: "Receiver");

            migrationBuilder.RenameColumn(
                name: "NameAdress",
                table: "addresses",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Receiver",
                table: "addresses",
                newName: "ReceiverName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "addresses",
                newName: "NameAdress");
        }
    }
}
