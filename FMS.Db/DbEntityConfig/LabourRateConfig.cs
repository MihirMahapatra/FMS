﻿using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class LabourRateConfig : IEntityTypeConfiguration<LabourRate>
    {
        public void Configure(EntityTypeBuilder<LabourRate> builder)
        {
            builder.ToTable("LabourRates", "dbo");
            builder.HasKey(e => e.LabourRateId);
            builder.Property(e => e.LabourRateId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.FinancialYear).IsRequired(false);
            builder.Property(e => e.Date).HasColumnType("datetime");
            builder.Property(e => e.Fk_ProductId).IsRequired(true);
            builder.Property(e => e.Fk_ProductTypeId).IsRequired(false);
            builder.Property(e => e.Rate).HasColumnType("decimal(18, 2)").HasDefaultValue(0);
            builder.HasOne(lr => lr.Product).WithMany(i => i.LabourRates).HasForeignKey(lr => lr.Fk_ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(lr => lr.ProductType).WithMany(i => i.LabourRates).HasForeignKey(lr => lr.Fk_ProductTypeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
