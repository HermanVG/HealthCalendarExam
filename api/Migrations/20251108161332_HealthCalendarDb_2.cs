using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class HealthCalendarDb_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availability_User_PatientId",
                table: "Availability");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Assignments_AssignmentId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.RenameColumn(
                name: "AssignmentId",
                table: "Schedules",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_AssignmentId",
                table: "Schedules",
                newName: "IX_Schedules_EventId");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "Availability",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    From = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    To = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PatientId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_User_PatientId",
                        column: x => x.PatientId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_PatientId",
                table: "Events",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Availability_User_PatientId",
                table: "Availability",
                column: "PatientId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Events_EventId",
                table: "Schedules",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availability_User_PatientId",
                table: "Availability");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Events_EventId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Schedules",
                newName: "AssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_EventId",
                table: "Schedules",
                newName: "IX_Schedules_AssignmentId");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "Availability",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    From = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    To = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_Assignments_User_PatientId",
                        column: x => x.PatientId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_PatientId",
                table: "Assignments",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Availability_User_PatientId",
                table: "Availability",
                column: "PatientId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Assignments_AssignmentId",
                table: "Schedules",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "AssignmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
