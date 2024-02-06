using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Db.DbEntityConfig
{
    public class SalesConfigConfig : IEntityTypeConfiguration<SalesConfig>
    {
        public void Configure(EntityTypeBuilder<SalesConfig> builder)
        {
            builder.ToTable("SalesConfigs", "dbo");
            builder.HasKey(e => e.SalesConfigId);
            builder.Property(e => e.SalesConfigId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_FinishedGoodId).IsRequired(true);
            builder.Property(e => e.Fk_SubFinishedGoodId).IsRequired(true);
            builder.Property(e => e.Quantity).HasColumnType("decimal(18, 5)").IsRequired(true);
            builder.Property(e => e.Unit).HasMaxLength(100).IsRequired(true);
        }
    }
}

