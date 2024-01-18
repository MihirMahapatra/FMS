using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class PartyConfig : IEntityTypeConfiguration<Party>
    {
        public void Configure(EntityTypeBuilder<Party> builder)
        {
            builder.ToTable("Parties", "dbo");
            builder.HasKey(e => e.PartyId);
            builder.Property(e => e.PartyId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_PartyType).IsRequired(true);
            builder.Property(e => e.Fk_SubledgerId).IsRequired(true);
            builder.Property(e => e.Fk_StateId).IsRequired(true);
            builder.Property(e => e.Fk_CityId).IsRequired(true);
            builder.Property(e => e.PartyName).HasMaxLength(200).IsRequired(true);
            builder.Property(e => e.Address).HasMaxLength(500).IsRequired(true);
            builder.Property(e => e.Phone).HasMaxLength(20).IsRequired(true);
            builder.Property(e => e.Email).HasMaxLength(200).IsRequired(true);
            builder.Property(e => e.GstNo).HasMaxLength(200).IsRequired(false);
            builder.Property(e => e.CreditLimit).HasDefaultValue(0);
            builder.HasOne(p => p.LedgerDev).WithMany(s => s.Parties).HasForeignKey(p => p.Fk_PartyType).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.SubLedger).WithMany(s => s.Parties).HasForeignKey(p => p.Fk_SubledgerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.State).WithMany(s => s.Parties).HasForeignKey(p => p.Fk_StateId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.City).WithMany(s => s.Parties).HasForeignKey(p => p.Fk_CityId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
