using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class UnitConfig : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("Units", "dbo");
            builder.HasKey(e => e.UnitId);
            builder.Property(e => e.UnitId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.UnitName).HasMaxLength(500).IsRequired(true);
        }
    }
}
