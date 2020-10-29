using Microsoft.EntityFrameworkCore.Migrations;

namespace Card_Creator.Migrations
{
    public partial class UpdateCardInfo_2Andreas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "CardTypes");

            migrationBuilder.AddColumn<string>(
                name: "Cardcolor",
                table: "CardTypes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cardcolor",
                table: "CardTypes");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "CardTypes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
