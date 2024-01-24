using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class LabourOrderConfig : IEntityTypeConfiguration<LabourOrder>
    {
        public void Configure(EntityTypeBuilder<LabourOrder> builder)
        {
            builder.ToTable("LabourOrders", "dbo");
            builder.HasKey(e => e.LabourOrderId);
            builder.Property(e => e.LabourOrderId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.TransactionDate).HasMaxLength(10).IsRequired(true);
            builder.Property(e => e.TransactionNo).IsRequired(true);
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_LabourId).IsRequired(true);
            builder.Property(e => e.Fk_LabourTypeId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.FK_BranchId).IsRequired(true);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime");
            builder.Property(e => e.Quantity).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.Rate).HasColumnType("decimal(18, 4)").HasDefaultValue(0);
            builder.Property(e => e.Amount).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.OTAmount).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.HasOne(p=> p.Product).WithMany(pe=>pe.LabourOrders).HasForeignKey(e=> e.Fk_ProductId).OnDelete(DeleteBehavior.Restrict); 
            builder.HasOne(l => l.Labour).WithMany(pe => pe.LabourOrders).HasForeignKey(e => e.Fk_LabourId).OnDelete(DeleteBehavior.Restrict); 
            builder.HasOne(f => f.FinancialYear).WithMany(pe => pe.LabourOrders).HasForeignKey(e => e.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict); 
            builder.HasOne(p => p.Branch).WithMany(pe => pe.LabourOrders).HasForeignKey(e => e.FK_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.LabourType).WithMany(pe => pe.LabourOrders).HasForeignKey(e => e.Fk_LabourTypeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
