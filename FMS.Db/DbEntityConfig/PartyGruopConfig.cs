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
    public class PartyGruopConfig : IEntityTypeConfiguration<PartyGroup>
    {
        public void Configure(EntityTypeBuilder<PartyGroup> builder)
        {
            builder.ToTable("PartyGruops", "dbo");
            builder.HasKey(e => e.PartyGroupId);
            builder.Property(e => e.PartyGroupId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.PartyGroupName).HasMaxLength(100).IsRequired(true);
            builder.HasData(
                    new PartyGroup() { PartyGroupId = Guid.Parse("F38EA62E-D4A8-4D0C-9C3D-9E33F8B9F4E5"), PartyGroupName = "Water Tank" },
                    new PartyGroup() { PartyGroupId = Guid.Parse("6E23176B-AF17-4C7B-B2B2-437B284D80DF"), PartyGroupName = "Door Frame" },
                    new PartyGroup() { PartyGroupId = Guid.Parse("A9873F41-79B2-4EC7-AE94-82BC12D2DBB7"), PartyGroupName = "General" },
                    new PartyGroup() { PartyGroupId = Guid.Parse("C9E8D76F-3D1B-4A97-BAB1-F99E56C672FF"), PartyGroupName = "WH" }
                );
        }
    }
}
