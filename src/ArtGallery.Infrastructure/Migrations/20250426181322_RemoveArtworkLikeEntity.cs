using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtGallery.Infrastructure.Migrations
{
	public partial class RemoveArtworkLikeEntity : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "ArtworkLikes");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
					name: "ArtworkLikes",
					columns: table => new
					{
						ArtworkId = table.Column<int>(type: "int", nullable: false),
						UserId = table.Column<int>(type: "int", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_ArtworkLikes", x => new { x.ArtworkId, x.UserId });
						table.ForeignKey(
											name: "FK_ArtworkLikes_Artworks_ArtworkId",
											column: x => x.ArtworkId,
											principalTable: "Artworks",
											principalColumn: "Id",
											onDelete: ReferentialAction.Cascade);
						table.ForeignKey(
											name: "FK_ArtworkLikes_Users_UserId",
											column: x => x.UserId,
											principalTable: "Users",
											principalColumn: "Id",
											onDelete: ReferentialAction.Cascade);
					});

			migrationBuilder.CreateIndex(
					name: "IX_ArtworkLikes_UserId",
					table: "ArtworkLikes",
					column: "UserId");
		}
	}
}
