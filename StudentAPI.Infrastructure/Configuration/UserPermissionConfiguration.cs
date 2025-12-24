

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentAPI.Domain.Entities;

namespace StudentAPI.Infrastructure.Configuration
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.PermissionId)
                .IsRequired();

            builder.ToTable("UserPermissions")
                .HasKey(x => new { x.UserId, x.PermissionId });
        }
    }
}
