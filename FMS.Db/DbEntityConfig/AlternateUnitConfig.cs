using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FMS.Db.DbEntityConfig
{
    public class AlternateUnitConfig : IEntityTypeConfiguration<AlternateUnit>
    {
        public void Configure(EntityTypeBuilder<AlternateUnit> builder)
        {
            builder.ToTable("AlternateUnits", "dbo");
            builder.HasKey(e => e.AlternateUnitId);
            builder.Property(e => e.AlternateUnitId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.FK_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_UnitId).IsRequired(true);
            builder.Property(e => e.AlternateUnitName).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.AlternateQuantity).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.HasOne(p => p.Product).WithMany(po => po.AlternateUnits).HasForeignKey(po => po.FK_ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Unit).WithMany(po => po.AlternateUnits).HasForeignKey(po => po.Fk_UnitId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
