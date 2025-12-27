using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserPermissionSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Read
            migrationBuilder.Sql(@"
        INSERT INTO UserPermissions (UserId, PermissionId)
        VALUES (1, 1);
    ");

            // Create
            migrationBuilder.Sql(@"
        INSERT INTO UserPermissions (UserId, PermissionId)
        VALUES (1, 2);
    ");

            // Update
            migrationBuilder.Sql(@"
        INSERT INTO UserPermissions (UserId, PermissionId)
        VALUES (1, 3);
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM UserPermissions WHERE UserId = 1 AND PermissionId IN (1, 2, 3);
    ");
        }
    }
}
