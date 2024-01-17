using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class SubLedgerConfig : IEntityTypeConfiguration<SubLedger>
    {
        public void Configure(EntityTypeBuilder<SubLedger> builder)
        {
            builder.ToTable("SubLedgers", "dbo");
            builder.HasKey(e => e.SubLedgerId);
            builder.Property(e => e.SubLedgerId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.SubLedgerName).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(false);
            builder.Property(e => e.Fk_LedgerId).IsRequired(true);
            builder.HasOne(bs => bs.Branch).WithMany(b => b.SubLedgers).HasForeignKey(bs => bs.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
