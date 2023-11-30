using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class BranchConfig : IEntityTypeConfiguration<Branch>
    {
            public void Configure(EntityTypeBuilder<Branch> builder)
            {
                builder.ToTable("Branches", "dbo");
                builder.HasKey(e => e.BranchId);
                builder.Property(e => e.BranchId).HasDefaultValueSql("(newid())");
                builder.Property(e => e.BranchName).HasMaxLength(100);
                builder.Property(e => e.BranchAddress).HasMaxLength(500);
                builder.Property(e => e.BranchCode).HasMaxLength(50);
                builder.Property(e => e.ContactNumber).HasMaxLength(50);
            }
        }
}
