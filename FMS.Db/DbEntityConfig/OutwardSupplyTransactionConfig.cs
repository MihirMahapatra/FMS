using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class OutwardSupplyTransactionConfig : IEntityTypeConfiguration<OutwardSupplyTransaction>
    {
        public void Configure(EntityTypeBuilder<OutwardSupplyTransaction> builder)
        {
            builder.ToTable("OutwardSupplyTransactions", "dbo");
            builder.HasKey(e => e.OutwardSupplyTransactionId);
            builder.Property(e => e.OutwardSupplyTransactionId).ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
            builder.Property(e => e.Fk_OutwardSupplyOrderId).IsRequired(true);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.TransactionNo).IsRequired(true);
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.Quantity).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Rate).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.HasOne(p => p.OutwardSupplyOrder).WithMany(po => po.OutwardSupplyTransactions).HasForeignKey(po => po.Fk_OutwardSupplyOrderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Product).WithMany(po => po.OutwardSupplyTransactions).HasForeignKey(po => po.Fk_ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Branch).WithMany(po => po.OutwardSupplyTransactions).HasForeignKey(po => po.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.FinancialYear).WithMany(po => po.OutwardSupplyTransactions).HasForeignKey(po => po.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
