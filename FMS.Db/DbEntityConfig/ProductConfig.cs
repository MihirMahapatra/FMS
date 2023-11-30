using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", "dbo");
            builder.HasKey(e => e.ProductId);
            builder.Property(e => e.ProductId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.ProductName).HasMaxLength(200).IsRequired(true);
            builder.Property(e => e.Price).HasColumnType("decimal(18, 2)").HasDefaultValue(0.00);
            builder.Property(e => e.GST).HasColumnType("decimal(18, 2)").HasDefaultValue(0.00);
            builder.Property(e => e.Fk_GroupId).IsRequired(true);
            builder.Property(e => e.Fk_SubGroupId).IsRequired(false);
            builder.Property(e => e.Fk_ProductTypeId).IsRequired(true);
            builder.Property(e => e.Fk_UnitId).IsRequired(true);
            builder.HasOne(d => d.Group).WithMany(e => e.Products).HasForeignKey(d => d.Fk_GroupId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(d => d.SubGroup).WithMany(e => e.products).HasForeignKey(d => d.Fk_SubGroupId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            builder.HasOne(d => d.ProductType).WithMany(e => e.Products).HasForeignKey(d => d.Fk_ProductTypeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(d => d.Unit).WithMany(e => e.Products).HasForeignKey(d => d.Fk_UnitId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
