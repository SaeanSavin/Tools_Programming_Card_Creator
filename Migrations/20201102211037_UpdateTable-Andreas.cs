using Microsoft.EntityFrameworkCore.Migrations;

namespace Card_Creator.Migrations
{
    public partial class UpdateTableAndreas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardTypeID",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "CardTypeIDID",
                table: "Cards",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardTypeIDID",
                table: "Cards",
                column: "CardTypeIDID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardTypes_CardTypeIDID",
                table: "Cards",
                column: "CardTypeIDID",
                principalTable: "CardTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardTypes_CardTypeIDID",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CardTypeIDID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardTypeIDID",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "CardTypeID",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
