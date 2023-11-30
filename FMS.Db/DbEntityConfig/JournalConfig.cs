using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class JournalConfig : IEntityTypeConfiguration<Journal>
    {
        public void Configure(EntityTypeBuilder<Journal> builder)
        {
            builder.ToTable("Journals", "dbo");
            builder.HasKey(e => e.JournalId);
            builder.Property(e => e.JournalId).HasDefaultValueSql("(newid())");

            builder.Property(e => e.VouvherNo).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.VoucherDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.Fk_LedgerGroupId).IsRequired(true);
            builder.Property(e => e.Fk_LedgerId).IsRequired(true);
            builder.Property(e => e.Fk_SubLedgerId).IsRequired(false);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.TransactionId).IsRequired(false);
            builder.Property(e => e.TransactionNo).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Narration).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.Amount).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.DrCr).HasMaxLength(10).IsRequired(true);
            builder.HasOne(e=>e.LedgerGroup).WithMany(s=>s.Journals).HasForeignKey(e => e.Fk_LedgerGroupId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.SubLedger).WithMany(s => s.Journals).HasForeignKey(e => e.Fk_SubLedgerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Branch).WithMany(s => s.Journals).HasForeignKey(e => e.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.FinancialYear).WithMany(s => s.Journals).HasForeignKey(e => e.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
