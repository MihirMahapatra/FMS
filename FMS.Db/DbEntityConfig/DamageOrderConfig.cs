using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class DamageOrderConfig : IEntityTypeConfiguration<DamageOrder>
    {
        public void Configure(EntityTypeBuilder<DamageOrder> builder)
        {
            builder.ToTable("DamageOrders", "dbo");
            builder.HasKey(e => e.DamageOrderId);
            builder.Property(e => e.DamageOrderId).ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.TransactionNo).IsRequired(true);
            builder.Property(e => e.Fk_LabourId).IsRequired(false);
            builder.Property(e => e.Fk_ProductTypeId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Reason).HasMaxLength(100).IsRequired(false);
            builder.HasOne(p => p.ProductType).WithMany(po => po.DamageOrders).HasForeignKey(po => po.Fk_ProductTypeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Labour).WithMany(po => po.DamageOrders).HasForeignKey(po => po.Fk_LabourId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Branch).WithMany(po => po.DamageOrders).HasForeignKey(po => po.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.FinancialYear).WithMany(po => po.DamageOrders).HasForeignKey(po => po.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
