using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Request.Module.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CumulativeLeaveRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    TotalHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CumulativeLeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CumulativeLeaveRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestFormNumber = table.Column<string>(type: "nvarchar(max)", nullable: false, computedColumnSql: "('LRF-' + CONVERT(VARCHAR(50), [FormNumber]))", stored: false),
                    LeaveType = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Workflow = table.Column<int>(type: "int", nullable: false),
                    AssignedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Users_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Users_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CumulativeLeaveRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_CumulativeLeaveRequests_CumulativeLeaveRequestId",
                        column: x => x.CumulativeLeaveRequestId,
                        principalTable: "CumulativeLeaveRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "ManagerId", "UserType" },
                values: new object[,]
                {
                    { new Guid("e21cd525-031c-4364-b173-4150a4e18c37"), "munir.ozkul@negzel.net", "Münir", "Özkul", null, 30 },
                    { new Guid("59fb152a-2d59-435d-8fc1-cbc35c0f1d82"), "sener.sen@negzel.net", "Şener", "Şen", new Guid("e21cd525-031c-4364-b173-4150a4e18c37"), 10 },
                    { new Guid("23591451-1cf1-46a5-907a-ee3e52abe394"), "kemal.sunal@negzel.net", "Kemal", "Sunal", new Guid("59fb152a-2d59-435d-8fc1-cbc35c0f1d82"), 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CumulativeLeaveRequests_UserId",
                table: "CumulativeLeaveRequests",
                column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_LeaveRequests_AssignedUserId",
            //    table: "LeaveRequests",
            //    column: "AssignedUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_LeaveRequests_CreatedById",
            //    table: "LeaveRequests",
            //    column: "CreatedById");

            //migrationBuilder.CreateIndex(
            //    name: "IX_LeaveRequests_LastModifiedById",
            //    table: "LeaveRequests",
            //    column: "LastModifiedById");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notifications_CumulativeLeaveRequestId",
            //    table: "Notifications",
            //    column: "CumulativeLeaveRequestId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notifications_UserId",
            //    table: "Notifications",
            //    column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ManagerId",
                table: "Users",
                column: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "CumulativeLeaveRequests");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
