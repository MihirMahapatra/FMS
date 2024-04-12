using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class InwardSupplyOrderConfig : IEntityTypeConfiguration<InwardSupplyOrder>
    {
        public void Configure(EntityTypeBuilder<InwardSupplyOrder> builder)
        {
            builder.ToTable("InwardSupplyOrders", "dbo");
            builder.HasKey(e => e.InwardSupplyOrderId);
            builder.Property(e => e.InwardSupplyOrderId).ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.TransactionNo).IsRequired(true);
            builder.Property(e => e.ChallanNo).IsRequired(true);
            builder.Property(e => e.VehicleNo).IsRequired(true);
            builder.Property(e => e.FromBranch).IsRequired(true);
            builder.Property(e => e.Fk_ProductTypeId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.HasOne(p => p.ProductType).WithMany(po => po.InwardSupplyOrders).HasForeignKey(po => po.Fk_ProductTypeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Branch).WithMany(po => po.InwardSupplyOrders).HasForeignKey(po => po.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.FinancialYear).WithMany(po => po.InwardSupplyOrders).HasForeignKey(po => po.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
