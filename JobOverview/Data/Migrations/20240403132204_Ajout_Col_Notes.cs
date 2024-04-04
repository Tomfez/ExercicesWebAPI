using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOverview.Data.Migrations
{
    /// <inheritdoc />
    public partial class Ajout_Col_Notes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Versions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Releases",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Versions");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Releases");
        }
    }
}
