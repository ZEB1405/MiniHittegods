using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniHittegods.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "FoundItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FoundItems",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "FoundItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FoundItems");
        }
    }
}
