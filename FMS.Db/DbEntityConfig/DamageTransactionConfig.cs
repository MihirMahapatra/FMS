using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class DamageTransactionConfig : IEntityTypeConfiguration<DamageTransaction>
    {
        public void Configure(EntityTypeBuilder<DamageTransaction> builder)
        {
            builder.ToTable("DamageTransactions", "dbo");
            builder.HasKey(e => e.DamageTransactionId);
            builder.Property(e => e.DamageTransactionId).ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
            builder.Property(e => e.Fk_DamageOrderId).IsRequired(true);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime").IsRequired(true);
            builder.Property(e => e.TransactionNo).IsRequired(true);
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Fk_FinancialYearId).IsRequired(true);
            builder.Property(e => e.Quantity).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Rate).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired(true);
            builder.HasOne(p => p.DamageOrder).WithMany(po => po.DamageTransactions).HasForeignKey(po => po.Fk_DamageOrderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Product).WithMany(po => po.DamageTransactions).HasForeignKey(po => po.Fk_ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Branch).WithMany(po => po.DamageTransactions).HasForeignKey(po => po.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.FinancialYear).WithMany(po => po.DamageTransactions).HasForeignKey(po => po.Fk_FinancialYearId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
