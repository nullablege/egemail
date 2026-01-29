using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project2IdentityEmail.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategoriler",
                columns: table => new
                {
                    KategoriId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoriler", x => x.KategoriId);
                });

            migrationBuilder.CreateTable(
                name: "Mesajlar",
                columns: table => new
                {
                    MesajId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Konu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GonderimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OkunduMu = table.Column<bool>(type: "bit", nullable: false),
                    GonderenId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesajlar", x => x.MesajId);
                    table.ForeignKey(
                        name: "FK_Mesajlar_AspNetUsers_GonderenId",
                        column: x => x.GonderenId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EpostaKutulari",
                columns: table => new
                {
                    EpostaKutusuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SahibiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MesajId = table.Column<int>(type: "int", nullable: false),
                    OkunduMu = table.Column<bool>(type: "bit", nullable: false),
                    YildizliMi = table.Column<bool>(type: "bit", nullable: false),
                    SilindiMi = table.Column<bool>(type: "bit", nullable: false),
                    klasorTipi = table.Column<int>(type: "int", nullable: false),
                    KategoriId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpostaKutulari", x => x.EpostaKutusuId);
                    table.ForeignKey(
                        name: "FK_EpostaKutulari_AspNetUsers_SahibiId",
                        column: x => x.SahibiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EpostaKutulari_Kategoriler_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "KategoriId");
                    table.ForeignKey(
                        name: "FK_EpostaKutulari_Mesajlar_MesajId",
                        column: x => x.MesajId,
                        principalTable: "Mesajlar",
                        principalColumn: "MesajId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EpostaKutulari_KategoriId",
                table: "EpostaKutulari",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_EpostaKutulari_MesajId",
                table: "EpostaKutulari",
                column: "MesajId");

            migrationBuilder.CreateIndex(
                name: "IX_EpostaKutulari_SahibiId",
                table: "EpostaKutulari",
                column: "SahibiId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_GonderenId",
                table: "Mesajlar",
                column: "GonderenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpostaKutulari");

            migrationBuilder.DropTable(
                name: "Kategoriler");

            migrationBuilder.DropTable(
                name: "Mesajlar");
        }
    }
}
