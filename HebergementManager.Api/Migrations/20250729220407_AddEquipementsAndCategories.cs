using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HebergementManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipementsAndCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategorieEquipement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorieEquipement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    CategorieEquipementId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipements_CategorieEquipement_CategorieEquipementId",
                        column: x => x.CategorieEquipementId,
                        principalTable: "CategorieEquipement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipements_CategorieEquipementId",
                table: "Equipements",
                column: "CategorieEquipementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipements");

            migrationBuilder.DropTable(
                name: "CategorieEquipement");
        }
    }
}
