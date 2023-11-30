using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;

namespace FMS.Db.DbEntityConfig
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers", "dbo");
            builder.Property(e => e.FkTokenId).HasColumnType("uniqueidentifier");
            builder.Property(e => e.BirthDate).HasColumnType("datetime");
            builder.Property(e => e.Name).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.Photo).HasMaxLength(500).IsUnicode(false);
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValueSql("((1))");
            //One-tO-One Relationship
            builder.HasOne(d => d.Token).WithOne(p => p.User).HasForeignKey<AppUser>(d => d.FkTokenId).HasConstraintName("FK_AppUser_RegisterToken");
        }
    }
}
