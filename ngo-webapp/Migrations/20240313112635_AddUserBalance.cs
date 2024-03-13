using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ngo_webapp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appeals",
                columns: table => new
                {
                    AppealsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppealsName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Organization = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    Appeals_Image = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Appeals__5E813B30A8132DFC", x => x.AppealsID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Is_Admin = table.Column<bool>(type: "bit", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Google_Hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    User_Image = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CCAC6A9A6DB9", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppealID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Blogs__54379E50C6BFD400", x => x.BlogID);
                    table.ForeignKey(
                        name: "FK_Blogs_Appeals",
                        column: x => x.AppealID,
                        principalTable: "Appeals",
                        principalColumn: "AppealsID");
                    table.ForeignKey(
                        name: "FK_Blogs_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    DonationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    AppealsID = table.Column<int>(type: "int", nullable: true),
                    DonationDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Donation__C5082EDB745B0277", x => x.DonationID);
                    table.ForeignKey(
                        name: "FK_Donations_Appeals",
                        column: x => x.AppealsID,
                        principalTable: "Appeals",
                        principalColumn: "AppealsID");
                    table.ForeignKey(
                        name: "FK_Donations_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "UC_Appeals_AppealsName",
                table: "Appeals",
                column: "AppealsName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AppealID",
                table: "Blogs",
                column: "AppealID");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserID",
                table: "Blogs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_AppealsID",
                table: "Donations",
                column: "AppealsID");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_UserID",
                table: "Donations",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "UC_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UC_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "Appeals");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
