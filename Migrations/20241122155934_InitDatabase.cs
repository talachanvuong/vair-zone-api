using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAirZoneWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizers",
                columns: table => new
                {
                    OrganizerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizerName = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    PageUrl = table.Column<string>(type: "nvarchar(2048)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizers", x => x.OrganizerId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    AdditionalUrl = table.Column<string>(type: "nvarchar(2048)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityName = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", nullable: false),
                    PostUrl = table.Column<string>(type: "nvarchar(2048)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrganizerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_Activities_Organizers_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "Organizers",
                        principalColumn: "OrganizerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ErrorCatchers",
                columns: table => new
                {
                    ErrorCatcherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizerId = table.Column<int>(type: "int", nullable: false),
                    Problem = table.Column<string>(type: "nvarchar(2048)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorCatchers", x => x.ErrorCatcherId);
                    table.ForeignKey(
                        name: "FK_ErrorCatchers_Organizers_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "Organizers",
                        principalColumn: "OrganizerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCodes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    TimeExpired = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCodes", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserCodes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityName",
                table: "Activities",
                column: "ActivityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_OrganizerId",
                table: "Activities",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorCatchers_OrganizerId",
                table: "ErrorCatchers",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizers_OrganizerName",
                table: "Organizers",
                column: "OrganizerName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DisplayName",
                table: "Users",
                column: "DisplayName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "ErrorCatchers");

            migrationBuilder.DropTable(
                name: "UserCodes");

            migrationBuilder.DropTable(
                name: "Organizers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
