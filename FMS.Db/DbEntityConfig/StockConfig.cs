using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class StockConfig : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stocks", "dbo");
            builder.HasKey(e => e.StockId);
            builder.Property(e => e.StockId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYear).IsRequired(true);
            builder.Property(e=>e.MinQty).HasColumnType("decimal(18, 2)").HasDefaultValue(0.00);
            builder.Property(e => e.MaxQty).HasColumnType("decimal(18, 2)").HasDefaultValue(0.00);
            builder.Property(e => e.OpeningStock).HasColumnType("decimal(18, 2)").HasDefaultValue(0.00);
            builder.Property(e => e.Rate).HasColumnType("decimal(18, 2)").HasDefaultValue(0.00);
            builder.Property(e => e.Amount).HasColumnType("decimal(18, 2)").HasDefaultValue(0.00);
            builder.Property(e => e.AvilableStock).HasColumnType("decimal(18, 2)").HasDefaultValue(0.00);
            builder.HasOne(bs => bs.Branch).WithMany(b => b.Stocks).HasForeignKey(bs => bs.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bs => bs.Product).WithMany(b => b.Stocks).HasForeignKey(bs => bs.Fk_ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bs => bs.FinancialYear).WithMany(b => b.Stocks).HasForeignKey(bs => bs.Fk_FinancialYear).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
