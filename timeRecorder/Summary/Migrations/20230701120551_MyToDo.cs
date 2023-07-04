using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Summary.Migrations
{
    /// <inheritdoc />
    public partial class MyToDo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyTime",
                columns: table => new
                {
                    currentIndex = table.Column<int>(type: "int", nullable: false),
                    createDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    startTime = table.Column<TimeSpan>(type: "time", maxLength: 10, nullable: false),
                    endTime = table.Column<TimeSpan>(type: "time", maxLength: 10, nullable: false),
                    lastTime = table.Column<TimeSpan>(type: "time", maxLength: 10, nullable: false),
                    note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    userid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyTime", x => new { x.currentIndex, x.createDate });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyTime");
        }
    }
}
