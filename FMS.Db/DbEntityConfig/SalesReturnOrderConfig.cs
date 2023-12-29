﻿using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class SalesReturnOrderConfig : IEntityTypeConfiguration<SalesReturnOrder>
    {
        public void Configure(EntityTypeBuilder<SalesReturnOrder> builder)
        {
            builder.ToTable("SalesReturnOrders", "dbo");
            builder.HasKey(e => e.SalesReturnOrderId);
            builder.Property(e => e.SalesReturnOrderId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_SubLedgerId).IsRequired(false);
            builder.Property(e => e.CustomerName).HasMaxLength(200).IsRequired(false);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.TransactionNo).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.TransactionType).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.PriceType).HasMaxLength(10).IsRequired(true);
            builder.Property(e => e.OrderNo).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.OrderDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.VehicleNo).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.ReceivingPerson).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.TranspoterName).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.Discount).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.Gst).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.GrandTotal).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.HasOne(e => e.SubLedger).WithMany(s => s.SalesReturnOrders).HasForeignKey(e => e.Fk_SubLedgerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e=>e.Branch).WithMany(s=>s.SalesReturnOrders).HasForeignKey(e=>e.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.FinancialYear).WithMany(s => s.SalesReturnOrders).HasForeignKey(e => e.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
