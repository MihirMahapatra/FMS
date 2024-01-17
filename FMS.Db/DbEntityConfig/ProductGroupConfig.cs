using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class ProductGroupConfig : IEntityTypeConfiguration<ProductGroup>
    {
        public void Configure(EntityTypeBuilder<ProductGroup> builder)
        {
            builder.ToTable("ProductGroups", "dbo");
            builder.HasKey(e => e.GroupId);
            builder.Property(e=>e.Fk_ProductTypeId).IsRequired(false);
            builder.Property(e => e.GroupId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.GroupName).HasMaxLength(500).IsRequired(true);
            builder.HasOne(p => p.ProductType).WithMany(po => po.Groups).HasForeignKey(po => po.Fk_ProductTypeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
