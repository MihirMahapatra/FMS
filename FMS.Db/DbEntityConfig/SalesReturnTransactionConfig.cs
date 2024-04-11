using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class SalesReturnTransactionConfig : IEntityTypeConfiguration<SalesReturnTransaction>
    {
        public void Configure(EntityTypeBuilder<SalesReturnTransaction> builder)
        {
            builder.ToTable("SalesReturnTransactions", "dbo");
            builder.HasKey(e => e.SalesReturnId);
            builder.Property(e => e.SalesReturnId).ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
            builder.Property(e => e.Fk_SalesReturnOrderId).IsRequired(true);
            builder.Property(e => e.TransactionNo).IsRequired(true);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.TransactionType).HasMaxLength(10).IsRequired(true);
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_SubProductId).IsRequired(false);
            builder.Property(e => e.Fk_AlternateUnitId).IsRequired(false);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.AlternateQuantity).HasColumnType("decimal(18, 5)").HasDefaultValue(0);
            builder.Property(e => e.UnitQuantity).HasColumnType("decimal(18, 5)").HasDefaultValue(0);
            builder.Property(e => e.Rate).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Discount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Gst).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.GstAmount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.HasOne(p => p.SalesReturnOrder).WithMany(po => po.SalesReturnTransactions).HasForeignKey(po => po.Fk_SalesReturnOrderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.AlternateUnit).WithMany(po => po.SalesReturnTransactions).HasForeignKey(po => po.Fk_AlternateUnitId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Product).WithMany(po => po.SalesReturnTransactions).HasForeignKey(po => po.Fk_ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.SubProduct).WithMany().HasForeignKey(po => po.Fk_SubProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Branch).WithMany(po => po.SalesReturnTransactions).HasForeignKey(po => po.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.FinancialYear).WithMany(po => po.SalesReturnTransactions).HasForeignKey(po => po.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
