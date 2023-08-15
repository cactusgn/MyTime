using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Summary.Migrations
{
    /// <inheritdoc />
    public partial class MyToDoSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "userid",
                table: "MyTime",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "MyTime",
                type: "TEXT",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "startTime",
                table: "MyTime",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "MyTime",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "lastTime",
                table: "MyTime",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "endTime",
                table: "MyTime",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createDate",
                table: "MyTime",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "currentIndex",
                table: "MyTime",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "userid",
                table: "MyTime",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "MyTime",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "startTime",
                table: "MyTime",
                type: "time",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "MyTime",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "lastTime",
                table: "MyTime",
                type: "time",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "endTime",
                table: "MyTime",
                type: "time",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createDate",
                table: "MyTime",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "currentIndex",
                table: "MyTime",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
