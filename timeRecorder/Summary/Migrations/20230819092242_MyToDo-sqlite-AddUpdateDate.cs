using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Summary.Migrations
{
    /// <inheritdoc />
    public partial class MyToDosqliteAddUpdateDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyTime",
                columns: table => new
                {
                    currentIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    createDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    startTime = table.Column<TimeSpan>(type: "TEXT", maxLength: 10, nullable: false),
                    endTime = table.Column<TimeSpan>(type: "TEXT", maxLength: 10, nullable: false),
                    lastTime = table.Column<TimeSpan>(type: "TEXT", maxLength: 10, nullable: false),
                    note = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    type = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    userid = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyTime", x => new { x.currentIndex, x.createDate });
                });

            migrationBuilder.CreateTable(
                name: "ToDos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Finished = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    parentId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyTime");

            migrationBuilder.DropTable(
                name: "ToDos");
        }
    }
}
