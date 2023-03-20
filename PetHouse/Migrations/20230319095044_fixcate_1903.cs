using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHouse.Migrations
{
    /// <inheritdoc />
    public partial class fixcate_1903 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDelete",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDelete",
                table: "Categories");
        }
    }
}
