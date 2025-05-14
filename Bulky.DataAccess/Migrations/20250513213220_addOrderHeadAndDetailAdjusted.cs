using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addOrderHeadAndDetailAdjusted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersHeaders_AspNetUsers_ApplicationUserId",
                table: "OrdersHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdewrsDetails_OrdersHeaders_OrderHeaderId",
                table: "OrdewrsDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdewrsDetails_Products_ProductId",
                table: "OrdewrsDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdewrsDetails",
                table: "OrdewrsDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersHeaders",
                table: "OrdersHeaders");

            migrationBuilder.RenameTable(
                name: "OrdewrsDetails",
                newName: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "OrdersHeaders",
                newName: "OrderHeaders");

            migrationBuilder.RenameIndex(
                name: "IX_OrdewrsDetails_ProductId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdewrsDetails_OrderHeaderId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_OrderHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdersHeaders_ApplicationUserId",
                table: "OrderHeaders",
                newName: "IX_OrderHeaders_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderHeaders",
                table: "OrderHeaders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                table: "OrderHeaders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                table: "OrderHeaders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderHeaders",
                table: "OrderHeaders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "OrderHeaders",
                newName: "OrdersHeaders");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                newName: "OrdewrsDetails");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeaders_ApplicationUserId",
                table: "OrdersHeaders",
                newName: "IX_OrdersHeaders_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrdewrsDetails",
                newName: "IX_OrdewrsDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_OrderHeaderId",
                table: "OrdewrsDetails",
                newName: "IX_OrdewrsDetails_OrderHeaderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersHeaders",
                table: "OrdersHeaders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdewrsDetails",
                table: "OrdewrsDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersHeaders_AspNetUsers_ApplicationUserId",
                table: "OrdersHeaders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdewrsDetails_OrdersHeaders_OrderHeaderId",
                table: "OrdewrsDetails",
                column: "OrderHeaderId",
                principalTable: "OrdersHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdewrsDetails_Products_ProductId",
                table: "OrdewrsDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
