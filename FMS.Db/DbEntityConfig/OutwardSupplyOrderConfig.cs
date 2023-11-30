using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class OutwardSupplyOrderConfig : IEntityTypeConfiguration<OutwardSupplyOrder>
    {
        public void Configure(EntityTypeBuilder<OutwardSupplyOrder> builder)
        {
            builder.ToTable("OutwardSupplyOrders", "dbo");
            builder.HasKey(e => e.OutwardSupplyOrderId);
            builder.Property(e => e.OutwardSupplyOrderId).ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.TransactionNo).IsRequired(true);
            builder.Property(e => e.ToBranch).IsRequired(true);
            builder.Property(e => e.Fk_ProductTypeId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.HasOne(p => p.ProductType).WithMany(po => po.OutwardSupplyOrders).HasForeignKey(po => po.Fk_ProductTypeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Branch).WithMany(po => po.OutwardSupplyOrders).HasForeignKey(po => po.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.FinancialYear).WithMany(po => po.OutwardSupplyOrders).HasForeignKey(po => po.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
