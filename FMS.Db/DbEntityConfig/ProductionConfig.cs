using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class ProductionConfig : IEntityTypeConfiguration<Production>
    {
        public void Configure(EntityTypeBuilder<Production> builder)
        {
            builder.ToTable("Productions", "dbo");
            builder.HasKey(e => e.ProductionId);
            builder.Property(e => e.ProductionId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_RawMaterialId).IsRequired(true);
            builder.Property(e => e.Fk_FinishedGoodId).IsRequired(true);
            builder.Property(e => e.Quantity).HasColumnType("decimal(18, 5)").IsRequired(true);
            builder.Property(e => e.Unit).HasMaxLength(100).IsRequired(true);
        }
    }
}
