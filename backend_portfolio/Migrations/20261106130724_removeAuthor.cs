using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_portfolio.Migrations
{
    /// <inheritdoc />
    public partial class removeAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Blogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Blogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
