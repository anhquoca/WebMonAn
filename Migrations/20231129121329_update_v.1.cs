using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMonAn.Migrations
{
    /// <inheritdoc />
    public partial class update_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Products_type_Product_typeId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_review_Product_ProductId",
                table: "Products_review");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_review_User_UserId",
                table: "Products_review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products_type",
                table: "Products_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products_review",
                table: "Products_review");

            migrationBuilder.RenameTable(
                name: "Products_type",
                newName: "Product_type");

            migrationBuilder.RenameTable(
                name: "Products_review",
                newName: "Product_review");

            migrationBuilder.RenameIndex(
                name: "IX_Products_review_UserId",
                table: "Product_review",
                newName: "IX_Product_review_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_review_ProductId",
                table: "Product_review",
                newName: "IX_Product_review_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product_type",
                table: "Product_type",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product_review",
                table: "Product_review",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Product_type_Product_typeId",
                table: "Product",
                column: "Product_typeId",
                principalTable: "Product_type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_review_Product_ProductId",
                table: "Product_review",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_review_User_UserId",
                table: "Product_review",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Product_type_Product_typeId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_review_Product_ProductId",
                table: "Product_review");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_review_User_UserId",
                table: "Product_review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product_type",
                table: "Product_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product_review",
                table: "Product_review");

            migrationBuilder.RenameTable(
                name: "Product_type",
                newName: "Products_type");

            migrationBuilder.RenameTable(
                name: "Product_review",
                newName: "Products_review");

            migrationBuilder.RenameIndex(
                name: "IX_Product_review_UserId",
                table: "Products_review",
                newName: "IX_Products_review_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_review_ProductId",
                table: "Products_review",
                newName: "IX_Products_review_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products_type",
                table: "Products_type",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products_review",
                table: "Products_review",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Products_type_Product_typeId",
                table: "Product",
                column: "Product_typeId",
                principalTable: "Products_type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_review_Product_ProductId",
                table: "Products_review",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_review_User_UserId",
                table: "Products_review",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
