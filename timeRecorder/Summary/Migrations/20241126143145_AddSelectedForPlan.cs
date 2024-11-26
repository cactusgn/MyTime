using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Summary.Migrations
{
    /// <inheritdoc />
    public partial class AddSelectedForPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SelectedForPlan",
                table: "Categories",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedForPlan",
                table: "Categories");
        }
    }
}
