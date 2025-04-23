using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projektas.Migrations
{
    /// <inheritdoc />
    public partial class LinkNotesToSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "InsightNotes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InsightNotes_SessionId",
                table: "InsightNotes",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_InsightNotes_SessionsList_SessionId",
                table: "InsightNotes",
                column: "SessionId",
                principalTable: "SessionsList",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsightNotes_SessionsList_SessionId",
                table: "InsightNotes");

            migrationBuilder.DropIndex(
                name: "IX_InsightNotes_SessionId",
                table: "InsightNotes");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "InsightNotes");
        }
    }
}
