using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class InwardSupplyTransactionConfig : IEntityTypeConfiguration<InwardSupplyTransaction>
    {
        public void Configure(EntityTypeBuilder<InwardSupplyTransaction> builder)
        {
            builder.ToTable("InwardSupplyTransactions", "dbo");
            builder.HasKey(e => e.InwardSupplyTransactionId);
            builder.Property(e => e.InwardSupplyTransactionId).ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
            builder.Property(e => e.Fk_InwardSupplyOrderId).IsRequired(true);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.TransactionNo).IsRequired(true);
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.Quantity).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Rate).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.HasOne(p => p.InwardSupplyOrder).WithMany(po => po.InwardSupplyTransactions).HasForeignKey(po => po.Fk_InwardSupplyOrderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Product).WithMany(po => po.InwardSupplyTransactions).HasForeignKey(po => po.Fk_ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Branch).WithMany(po => po.InwardSupplyTransactions).HasForeignKey(po => po.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.FinancialYear).WithMany(po => po.InwardSupplyTransactions).HasForeignKey(po => po.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
