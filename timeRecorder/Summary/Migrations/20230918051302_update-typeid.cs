using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Summary.Migrations
{
    /// <inheritdoc />
    public partial class updatetypeid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ToDos");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "ToDos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "ToDos");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ToDos",
                type: "TEXT",
                maxLength: 10,
                nullable: true);
        }
    }
}
