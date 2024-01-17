using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class LedgerBalanceConfig : IEntityTypeConfiguration<LedgerBalance>
    {
        public void Configure(EntityTypeBuilder<LedgerBalance> builder)
        {
            builder.ToTable("LedgerBalances", "dbo");
            builder.HasKey(e => e.LedgerBalanceId);
            builder.Property(e => e.LedgerBalanceId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_LedgerId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(false);
            builder.Property(e => e.Fk_FinancialYear).IsRequired(true);
            builder.Property(e => e.OpeningBalance).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.OpeningBalanceType).HasMaxLength(10).IsRequired(true);
            builder.Property(e => e.RunningBalance).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.RunningBalanceType).HasMaxLength(10).IsRequired(true);
            builder.HasOne(bs => bs.Branch).WithMany(b => b.LedgerBalances).HasForeignKey(bs => bs.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bs => bs.FinancialYear).WithMany(b => b.LedgerBalances).HasForeignKey(bs => bs.Fk_FinancialYear).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
