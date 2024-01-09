using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class PurchaseOrderConfig : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.ToTable("PurchaseOrders", "dbo");
            builder.HasKey(e => e.PurchaseOrderId);
            builder.Property(e => e.PurchaseOrderId).ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
            builder.Property(e => e.TransactionNo).HasMaxLength(10).IsRequired(true);
            builder.Property(e => e.Fk_SubLedgerId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.InvoiceNo).HasMaxLength(200).IsRequired(true);
            builder.Property(e => e.InvoiceDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.OrderNo).HasMaxLength(200).IsRequired(true);
            builder.Property(e => e.OrderDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.VehicleNo).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.TranspoterName).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.ReceivingPerson).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.SubTotal).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Discount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Gst).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(e => e.GrandTotal).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.HasOne(e => e.SubLedger).WithMany(s => s.PurchaseOrders).HasForeignKey(e => e.Fk_SubLedgerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Branch).WithMany(po => po.PurchaseOrders).HasForeignKey(po => po.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.FinancialYear).WithMany(po => po.PurchaseOrders).HasForeignKey(po => po.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
