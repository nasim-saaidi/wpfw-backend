using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_portfolio.Migrations
{
    /// <inheritdoc />
    public partial class removeAuthorAddInternalNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Blogs",
                newName: "InternalNotes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InternalNotes",
                table: "Blogs",
                newName: "Author");
        }
    }
}
