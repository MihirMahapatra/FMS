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
            builder.HasKey(e => e.ProductGroupId);
            builder.Property(e => e.ProductGroupId).HasDefaultValueSql("(newid())");   
            builder.Property(e => e.ProductGroupName).HasMaxLength(500).IsRequired(true);
            builder.Property(e => e.Fk_ProductTypeId).IsRequired(false);
            builder.HasOne(p => p.ProductType).WithMany(po => po.ProductGroups).HasForeignKey(po => po.Fk_ProductTypeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
