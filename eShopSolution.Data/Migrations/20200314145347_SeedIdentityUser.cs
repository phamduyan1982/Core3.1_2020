using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class SeedIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 14, 21, 53, 46, 293, DateTimeKind.Local).AddTicks(8023),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 3, 14, 21, 14, 18, 17, DateTimeKind.Local).AddTicks(1299));

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("07fe94ae-f960-442b-badc-3417826f6fd5"), "f7f9acd5-de31-4193-ad36-2bdbcded79af", "Adminstrator role", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("194e1ed3-45b3-42fb-81be-44db403bb56b"), new Guid("07fe94ae-f960-442b-badc-3417826f6fd5") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("194e1ed3-45b3-42fb-81be-44db403bb56b"), 0, "efeede9e-a286-444c-a8a6-5b74b918cbc3", new DateTime(2020, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "an@dhpd.edu.com", true, "AN", "Pham", false, null, "an@dhpd.edu.com", "admin", "AQAAAAEAACcQAAAAEAbsNjzDY68jYua9KBRA1MHNYGkdN9In1AjvaFDWCr+Ch23p5Vp7XrM6Ls4Mm+Oy1w==", null, false, "", false, "admin" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2020, 3, 14, 21, 53, 46, 336, DateTimeKind.Local).AddTicks(6910));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("07fe94ae-f960-442b-badc-3417826f6fd5"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("194e1ed3-45b3-42fb-81be-44db403bb56b"), new Guid("07fe94ae-f960-442b-badc-3417826f6fd5") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("194e1ed3-45b3-42fb-81be-44db403bb56b"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 3, 14, 21, 14, 18, 17, DateTimeKind.Local).AddTicks(1299),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 3, 14, 21, 53, 46, 293, DateTimeKind.Local).AddTicks(8023));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2020, 3, 14, 21, 14, 18, 81, DateTimeKind.Local).AddTicks(1047));
        }
    }
}
