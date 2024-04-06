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
    public class ReferanceConfig : IEntityTypeConfiguration<Referance>
    {
        public void Configure(EntityTypeBuilder<Referance> builder)
        {
            builder.ToTable("Referances", "dbo");
            builder.HasKey(e => e.RefaranceId);
            builder.Property(e => e.RefaranceId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.ReferanceName).IsRequired(true);
        }
    }
}
