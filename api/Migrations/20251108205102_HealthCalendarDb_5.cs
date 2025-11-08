using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class HealthCalendarDb_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availability_User_PatientId",
                table: "Availability");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_User_PatientId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_PatientId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Availability_PatientId",
                table: "Availability");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Availability");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "User",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Events",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Availability",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Availability_UserId",
                table: "Availability",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Availability_User_UserId",
                table: "Availability",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_User_UserId",
                table: "Events",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availability_User_UserId",
                table: "Availability");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_User_UserId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UserId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Availability_UserId",
                table: "Availability");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "User",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "Events",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Availability",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "Availability",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Events_PatientId",
                table: "Events",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Availability_PatientId",
                table: "Availability",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Availability_User_PatientId",
                table: "Availability",
                column: "PatientId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_User_PatientId",
                table: "Events",
                column: "PatientId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
