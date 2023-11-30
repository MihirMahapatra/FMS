using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class LedgerConfig : IEntityTypeConfiguration<Ledger>
    {
        public void Configure(EntityTypeBuilder<Ledger> builder)
        {
            builder.ToTable("Ledgers", "dbo");
            builder.HasKey(e => e.LedgerId);
            builder.Property(e => e.LedgerId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.LedgerName).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.LedgerType).HasMaxLength(10).IsRequired(false);
            builder.Property(e => e.HasSubLedger).HasMaxLength(10).IsRequired(true);
            builder.Property(e => e.Fk_LedgerGroupId).IsRequired(true);
            builder.Property(e => e.Fk_LedgerSubGroupId).IsRequired(false);
            builder.HasOne(l => l.LedgerGroup).WithMany(g => g.Ledgers).HasForeignKey(l => l.Fk_LedgerGroupId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(l => l.LedgerSubGroup).WithMany(g => g.Ledgers).HasForeignKey(l => l.Fk_LedgerSubGroupId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
