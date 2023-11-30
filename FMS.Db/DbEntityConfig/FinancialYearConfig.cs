using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class FinancialYearConfig : IEntityTypeConfiguration<FinancialYear>
    {
        public void Configure(EntityTypeBuilder<FinancialYear> builder)
        {
            builder.ToTable("FinancialYears", "dbo");
            builder.HasKey(e => e.FinancialYearId);
            builder.Property(e => e.FinancialYearId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.FK_BranchId);
            builder.Property(e => e.StartDate).HasColumnType("datetime");
            builder.Property(e => e.EndDate).HasColumnType("datetime");
            builder.HasOne(fy => fy.Branch).WithMany(b => b.FinancialYears).HasForeignKey(fy => fy.FK_BranchId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
