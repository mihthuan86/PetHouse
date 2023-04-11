using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHouse.Migrations
{
    /// <inheritdoc />
    public partial class fixv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportDetails_imports_ImportId",
                table: "ImportDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_imports_AspNetUsers_UserId",
                table: "imports");

            migrationBuilder.DropForeignKey(
                name: "FK_imports_Suppliers_SupplierId",
                table: "imports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_imports",
                table: "imports");

            migrationBuilder.RenameTable(
                name: "imports",
                newName: "Imports");

            migrationBuilder.RenameIndex(
                name: "IX_imports_UserId",
                table: "Imports",
                newName: "IX_Imports_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_imports_SupplierId",
                table: "Imports",
                newName: "IX_Imports_SupplierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Imports",
                table: "Imports",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportDetails_Imports_ImportId",
                table: "ImportDetails",
                column: "ImportId",
                principalTable: "Imports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Imports_AspNetUsers_UserId",
                table: "Imports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Imports_Suppliers_SupplierId",
                table: "Imports",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportDetails_Imports_ImportId",
                table: "ImportDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Imports_AspNetUsers_UserId",
                table: "Imports");

            migrationBuilder.DropForeignKey(
                name: "FK_Imports_Suppliers_SupplierId",
                table: "Imports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Imports",
                table: "Imports");

            migrationBuilder.RenameTable(
                name: "Imports",
                newName: "imports");

            migrationBuilder.RenameIndex(
                name: "IX_Imports_UserId",
                table: "imports",
                newName: "IX_imports_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Imports_SupplierId",
                table: "imports",
                newName: "IX_imports_SupplierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_imports",
                table: "imports",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportDetails_imports_ImportId",
                table: "ImportDetails",
                column: "ImportId",
                principalTable: "imports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_imports_AspNetUsers_UserId",
                table: "imports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_imports_Suppliers_SupplierId",
                table: "imports",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
