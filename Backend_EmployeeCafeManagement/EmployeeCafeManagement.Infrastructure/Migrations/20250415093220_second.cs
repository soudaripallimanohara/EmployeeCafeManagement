using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeCafeManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "EmployeesCafe",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Cafes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Cafes",
                columns: new[] { "Id", "Description", "Location", "Logo", "Name" },
                values: new object[,]
                {
                    { new Guid("0ab5321e-9d86-401f-a21b-16421fffc058"), "A Lush Creamy Lussy drink with Chilled Flavour.", "Mumbai", "https://example.com/logos/sunset-brew.png", "Limca" },
                    { new Guid("4ab1234f-9d86-401f-a21b-16421fffc058"), "A Lush Creamy Lussy drink with Chilled Flavour.", "Bangalore", "https://example.com/logos/sunset-brew.png", "Maaza" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "EmailAddress", "Gender", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { "ECMABC1111", "ramesh@gmail.com", "Male", "Ramesh", "12345678" },
                    { "ECMABC2222", "sures@gmail.com", "Male", "Suresh", "12345678" },
                    { "ECMABC3333", "bavesh@gmail.com", "Male", "Bavesh", "12345678" }
                });

            migrationBuilder.InsertData(
                table: "EmployeesCafe",
                columns: new[] { "EmployeeId", "CafeId", "StartDate" },
                values: new object[,]
                {
                    { "ECMABC1111", new Guid("0ab5321e-9d86-401f-a21b-16421fffc058"), new DateTime(2025, 12, 14, 17, 9, 53, 456, DateTimeKind.Unspecified) },
                    { "ECMABC2222", new Guid("0ab5321e-9d86-401f-a21b-16421fffc058"), new DateTime(2025, 12, 14, 17, 9, 53, 456, DateTimeKind.Unspecified) },
                    { "ECMABC3333", new Guid("0ab5321e-9d86-401f-a21b-16421fffc058"), new DateTime(2025, 12, 14, 17, 9, 53, 456, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cafes",
                keyColumn: "Id",
                keyValue: new Guid("4ab1234f-9d86-401f-a21b-16421fffc058"));

            migrationBuilder.DeleteData(
                table: "EmployeesCafe",
                keyColumn: "EmployeeId",
                keyValue: "ECMABC1111");

            migrationBuilder.DeleteData(
                table: "EmployeesCafe",
                keyColumn: "EmployeeId",
                keyValue: "ECMABC2222");

            migrationBuilder.DeleteData(
                table: "EmployeesCafe",
                keyColumn: "EmployeeId",
                keyValue: "ECMABC3333");

            migrationBuilder.DeleteData(
                table: "Cafes",
                keyColumn: "Id",
                keyValue: new Guid("0ab5321e-9d86-401f-a21b-16421fffc058"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "ECMABC1111");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "ECMABC2222");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "ECMABC3333");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "EmployeesCafe",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Logo",
                table: "Cafes",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
