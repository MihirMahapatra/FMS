using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class ProductionEntryTransactionConfig : IEntityTypeConfiguration<ProductionEntryTransaction>
    {
        public void Configure(EntityTypeBuilder<ProductionEntryTransaction> builder)
        {
            builder.ToTable("ProductionEntryTransactions", "dbo");
            builder.HasKey(e => e.ProductionEntryTransactionId);
            builder.Property(e => e.ProductionEntryTransactionId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_ProductionEntryId).IsRequired(true);
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.TransactionNo).IsRequired(true);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.Quantity).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.HasOne(s=>s.ProductionEntry).WithMany(pe=>pe.ProductionEntryTransactions).HasForeignKey(e => e.Fk_ProductionEntryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Product).WithMany(pe => pe.ProductionEntryTransactions).HasForeignKey(e => e.Fk_ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.FinancialYear).WithMany(pe => pe.ProductionEntryTransactions).HasForeignKey(e => e.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Branch).WithMany(pe => pe.ProductionEntryTransactions).HasForeignKey(e => e.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
