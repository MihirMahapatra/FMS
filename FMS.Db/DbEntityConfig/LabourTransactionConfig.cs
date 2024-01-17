using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class LabourTransactionConfig : IEntityTypeConfiguration<LabourTransaction>
    {
        public void Configure(EntityTypeBuilder<LabourTransaction> builder)
        {
            builder.ToTable("LabourTransactions", "dbo");
            builder.HasKey(e => e.LabourTransactionId);
            builder.Property(e => e.LabourTransactionId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_LabourOdrId).IsRequired(true);
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.TransactionNo).IsRequired(true);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.Quantity).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.HasOne(s=>s.LabourOrder).WithMany(pe=>pe.LabourTransactions).HasForeignKey(e => e.Fk_LabourOdrId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Product).WithMany(pe => pe.LabourTransactions).HasForeignKey(e => e.Fk_ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.FinancialYear).WithMany(pe => pe.LabourTransactions).HasForeignKey(e => e.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Branch).WithMany(pe => pe.LabourTransactions).HasForeignKey(e => e.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
