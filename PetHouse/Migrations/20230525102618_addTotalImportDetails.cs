using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHouse.Migrations
{
    /// <inheritdoc />
    public partial class addTotalImportDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "ImportDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "ImportDetails");
        }
    }
}
