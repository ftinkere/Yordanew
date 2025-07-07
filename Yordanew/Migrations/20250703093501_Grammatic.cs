using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yordanew.Migrations
{
    /// <inheritdoc />
    public partial class Grammatic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartsOfSpeech",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartsOfSpeech", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartsOfSpeech_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GrammaticalCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartOfSpeechId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrammaticalCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrammaticalCategories_PartsOfSpeech_PartOfSpeechId",
                        column: x => x.PartOfSpeechId,
                        principalTable: "PartsOfSpeech",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrammaticalFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrammaticalFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrammaticalFeatures_GrammaticalCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "GrammaticalCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrammaticalCategories_PartOfSpeechId",
                table: "GrammaticalCategories",
                column: "PartOfSpeechId");

            migrationBuilder.CreateIndex(
                name: "IX_GrammaticalFeatures_CategoryId",
                table: "GrammaticalFeatures",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PartsOfSpeech_LanguageId",
                table: "PartsOfSpeech",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrammaticalFeatures");

            migrationBuilder.DropTable(
                name: "GrammaticalCategories");

            migrationBuilder.DropTable(
                name: "PartsOfSpeech");
        }
    }
}
