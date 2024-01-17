using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class ProductTypeConfig : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.ToTable("ProductTypes", "dbo");
            builder.HasKey(e => e.ProductTypeId);
            builder.Property(e => e.ProductTypeId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Product_Type).HasMaxLength(200).IsRequired(true);
            builder.HasData(
                 new ProductType() { ProductTypeId = Guid.Parse("A4AB180B-ACC7-44CE-AEF7-C588D41EDD5C"), Product_Type = "FINISHED GOODS" },
                 new ProductType() { ProductTypeId = Guid.Parse("B524F4A7-1BB2-4347-84AE-E0DA56EB4A31"), Product_Type = "RAW MATERIALS" },
                 new ProductType() { ProductTypeId = Guid.Parse("B504237F-AF5F-485A-BEC9-0906C50DF3C6"), Product_Type = "SERVICE GOODS" },
                 new ProductType() { ProductTypeId = Guid.Parse("66ADA405-1229-45DF-9598-90B602078933"), Product_Type = "MOULD & MECHINARY" }
                );
        }
    }
}
