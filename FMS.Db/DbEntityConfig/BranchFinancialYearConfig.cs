using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class BranchFinancialYearConfig : IEntityTypeConfiguration<BranchFinancialYear>
    {
        public void Configure(EntityTypeBuilder<BranchFinancialYear> builder)
        {
            builder.ToTable("BranchFinancialYears", "dbo");
            builder.HasKey(e => e.BranchFinancialYearId);
            builder.Property(e => e.BranchFinancialYearId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.HasOne(fy => fy.FinancialYear).WithMany(b => b.BranchFinancialYears).HasForeignKey(fy => fy.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(br => br.Branch).WithMany(b => b.BranchFinancialYears).HasForeignKey(fy => fy.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
