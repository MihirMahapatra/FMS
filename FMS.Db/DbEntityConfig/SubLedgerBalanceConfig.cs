using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class SubLedgerBalanceConfig : IEntityTypeConfiguration<SubLedgerBalance>
    {
        public void Configure(EntityTypeBuilder<SubLedgerBalance> builder)
        {
            builder.ToTable("SubLedgerBalances", "dbo");
            builder.HasKey(e => e.SubLedgerBalanceId);
            builder.Property(e => e.SubLedgerBalanceId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_LedgerBalanceId).IsRequired(true);
            builder.Property(e => e.Fk_SubLedgerId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.OpeningBalance).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.OpeningBalanceType).HasMaxLength(10).IsRequired(true);
            builder.Property(e => e.RunningBalance).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.RunningBalanceType).HasMaxLength(10).IsRequired(true);
            builder.HasOne(bs => bs.SubLedger).WithMany(b => b.SubLedgerBalances).HasForeignKey(bs => bs.Fk_SubLedgerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bs => bs.Branch).WithMany(b => b.SubLedgerBalances).HasForeignKey(bs => bs.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bs => bs.FinancialYear).WithMany(b => b.SubLedgerBalances).HasForeignKey(bs => bs.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bs => bs.LedgerBalance).WithMany(b => b.SubLedgerBalances).HasForeignKey(bs => bs.Fk_LedgerBalanceId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
