using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class SubGroupConfig : IEntityTypeConfiguration<ProductSubGroup>
    {
        public void Configure(EntityTypeBuilder<ProductSubGroup> builder)
        {
            builder.ToTable("SubGroups", "dbo");
            builder.HasKey(e => e.ProductSubGroupId);
            builder.Property(e => e.ProductSubGroupId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_ProductGroupId).IsRequired(false);
            builder.Property(e => e.ProductSubGroupName).HasMaxLength(200).IsRequired(true);
            builder.HasOne(s => s.ProductGroup).WithMany(t => t.ProductSubGroups).HasForeignKey(s => s.Fk_ProductGroupId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
