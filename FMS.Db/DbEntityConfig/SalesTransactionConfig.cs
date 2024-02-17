using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class SalesTransactionConfig : IEntityTypeConfiguration<SalesTransaction>
    {
        public void Configure(EntityTypeBuilder<SalesTransaction> builder)
        {
            builder.ToTable("SalesTransaction", "dbo");
            builder.HasKey(e => e.SalesId);
            builder.Property(e => e.SalesId).ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
            builder.Property(e => e.Fk_SalesOrderId).IsRequired(true);
            builder.Property(e => e.TransactionNo).IsRequired(true).HasMaxLength(10);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.AlternateQuantity).HasColumnType("decimal(18,5)").IsRequired(true);
            builder.Property(e => e.UnitQuantity).HasColumnType("decimal(18,5)").IsRequired(true);
            builder.Property(e => e.Rate).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Discount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Gst).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.GstAmount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.HasOne(p => p.SalesOrder).WithMany(po => po.SalesTransactions).HasForeignKey(po => po.Fk_SalesOrderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.AlternateUnit).WithMany(po => po.SalesTransactions).HasForeignKey(po => po.Fk_AlternateUnitId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Product).WithMany(po => po.SalesTransactions).HasForeignKey(po => po.Fk_ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Branch).WithMany(po => po.SalesTransactions).HasForeignKey(po => po.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.FinancialYear).WithMany(po => po.SalesTransactions).HasForeignKey(po => po.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
