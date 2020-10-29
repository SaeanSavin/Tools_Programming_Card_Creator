using Microsoft.EntityFrameworkCore.Migrations;

namespace Card_Creator.Migrations
{
    public partial class UpdateCardInfoAndreas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardTypes_typeID",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_CardTypes_CardTypes_WeaknessID",
                table: "CardTypes");

            migrationBuilder.DropIndex(
                name: "IX_CardTypes_WeaknessID",
                table: "CardTypes");

            migrationBuilder.DropIndex(
                name: "IX_Cards_typeID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "WeaknessID",
                table: "CardTypes");

            migrationBuilder.DropColumn(
                name: "typeID",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Cards",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "CardTypeID",
                table: "Cards",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardTypeID",
                table: "Cards",
                column: "CardTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards",
                column: "CardTypeID",
                principalTable: "CardTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CardTypeID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardTypeID",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cards",
                newName: "name");

            migrationBuilder.AddColumn<int>(
                name: "WeaknessID",
                table: "CardTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "typeID",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardTypes_WeaknessID",
                table: "CardTypes",
                column: "WeaknessID");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_typeID",
                table: "Cards",
                column: "typeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardTypes_typeID",
                table: "Cards",
                column: "typeID",
                principalTable: "CardTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardTypes_CardTypes_WeaknessID",
                table: "CardTypes",
                column: "WeaknessID",
                principalTable: "CardTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
