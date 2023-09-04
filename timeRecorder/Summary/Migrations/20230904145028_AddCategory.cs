using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Summary.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "parentId",
                table: "ToDos",
                newName: "priority");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ToDos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TaskDetails",
                table: "ToDos",
                type: "TEXT",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToDoTaskSettingId",
                table: "ToDos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "taskId",
                table: "MyTime",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "TaskDetails",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "ToDoTaskSettingId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "taskId",
                table: "MyTime");

            migrationBuilder.RenameColumn(
                name: "priority",
                table: "ToDos",
                newName: "parentId");
        }
    }
}
