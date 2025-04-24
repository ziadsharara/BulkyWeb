using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtGallery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArtworkPriceAndCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuctionEndTime",
                table: "Artworks");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Artworks");

            migrationBuilder.RenameColumn(
                name: "InitialPrice",
                table: "Artworks",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "AuctionStartTime",
                table: "Artworks",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Artworks",
                newName: "InitialPrice");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Artworks",
                newName: "AuctionStartTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "AuctionEndTime",
                table: "Artworks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Artworks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
