using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HebergementManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hebergements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Adresse = table.Column<string>(type: "TEXT", nullable: false),
                    Ville = table.Column<string>(type: "TEXT", nullable: false),
                    CodePostal = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    CapaciteMax = table.Column<int>(type: "INTEGER", nullable: false),
                    PrixParNuit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstActif = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hebergements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HebergementId = table.Column<int>(type: "INTEGER", nullable: false),
                    NomClient = table.Column<string>(type: "TEXT", nullable: false),
                    EmailClient = table.Column<string>(type: "TEXT", nullable: false),
                    TelephoneClient = table.Column<string>(type: "TEXT", nullable: false),
                    DateArrivee = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateDepart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NombrePersonnes = table.Column<int>(type: "INTEGER", nullable: false),
                    PrixTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Statut = table.Column<string>(type: "TEXT", nullable: false),
                    DateReservation = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Hebergements_HebergementId",
                        column: x => x.HebergementId,
                        principalTable: "Hebergements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_HebergementId",
                table: "Reservations",
                column: "HebergementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Hebergements");
        }
    }
}
