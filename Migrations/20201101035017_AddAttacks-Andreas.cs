using Microsoft.EntityFrameworkCore.Migrations;

namespace Card_Creator.Migrations
{
    public partial class AddAttacksAndreas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards");

            migrationBuilder.AlterColumn<int>(
                name: "CardTypeID",
                table: "Cards",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Attacks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Damage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attacks", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards",
                column: "CardTypeID",
                principalTable: "CardTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "Attacks");

            migrationBuilder.AlterColumn<int>(
                name: "CardTypeID",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardTypes_CardTypeID",
                table: "Cards",
                column: "CardTypeID",
                principalTable: "CardTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
