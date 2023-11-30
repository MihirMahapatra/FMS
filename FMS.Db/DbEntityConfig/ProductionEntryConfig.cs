using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class ProductionEntryConfig : IEntityTypeConfiguration<ProductionEntry>
    {
        public void Configure(EntityTypeBuilder<ProductionEntry> builder)
        {
            builder.ToTable("ProductionEntries", "dbo");
            builder.HasKey(e => e.ProductionEntryId);
            builder.Property(e => e.ProductionEntryId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.ProductionDate).HasMaxLength(10).IsRequired(true);
            builder.Property(e => e.ProductionNo).IsRequired(true);
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_LabourId).IsRequired(true);
            builder.Property(e => e.LabourType).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.FK_BranchId).IsRequired(true);
            builder.Property(e => e.ProductionDate).HasColumnType("datetime");
            builder.Property(e => e.Quantity).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.Rate).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.Property(e => e.Amount).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.HasOne(p=> p.Product).WithMany(pe=>pe.ProductionEntries).HasForeignKey(e=> e.Fk_ProductId).OnDelete(DeleteBehavior.Restrict); 
            builder.HasOne(l => l.Labour).WithMany(pe => pe.ProductionEntries).HasForeignKey(e => e.Fk_LabourId).OnDelete(DeleteBehavior.Restrict); 
            builder.HasOne(f => f.FinancialYear).WithMany(pe => pe.ProductionEntries).HasForeignKey(e => e.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict); 
            builder.HasOne(p => p.Branch).WithMany(pe => pe.ProductionEntries).HasForeignKey(e => e.FK_BranchId).OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
