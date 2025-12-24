using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Password as plain text without hashing just for test
            migrationBuilder.Sql(@"
        INSERT INTO Users (Username, Password, Role)
        VALUES ('admin', 'admin123', 'Admin');
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM Users WHERE Username = 'admin';
    ");
        }
    }
}
