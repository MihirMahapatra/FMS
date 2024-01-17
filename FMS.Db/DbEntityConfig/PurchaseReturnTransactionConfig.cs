using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class PurchaseReturnTransactionConfig : IEntityTypeConfiguration<PurchaseReturnTransaction>
    {
        public void Configure(EntityTypeBuilder<PurchaseReturnTransaction> builder)
        {
            builder.ToTable("PurchaseReturnTransactions", "dbo");
            builder.HasKey(e => e.PurchaseReturnId);
            builder.Property(e => e.PurchaseReturnId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.TransactionNo).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.Fk_PurchaseReturnOrderId).IsRequired(true);
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_AlternateUnitId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.Quantity).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.Rate).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.Discount).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.Gst).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.GstAmount).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.Amount).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.HasOne(p => p.PurchaseReturnOrder).WithMany(po => po.PurchaseReturnTransactions).HasForeignKey(po => po.Fk_PurchaseReturnOrderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.AlternateUnit).WithMany(po => po.PurchaseReturnTransactions).HasForeignKey(po => po.Fk_AlternateUnitId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Product).WithMany(po => po.PurchaseReturnTransactions).HasForeignKey(po => po.Fk_ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Branch).WithMany(po => po.PurchaseReturnTransactions).HasForeignKey(po => po.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.FinancialYear).WithMany(po => po.PurchaseReturnTransactions).HasForeignKey(po => po.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
