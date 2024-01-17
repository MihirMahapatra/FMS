using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Db.DbEntityConfig
{
    public class FinancialYearConfig : IEntityTypeConfiguration<FinancialYear>
    {
        public void Configure(EntityTypeBuilder<FinancialYear> builder)
        {
            builder.ToTable("FinancialYears", "dbo");
            builder.HasKey(e => e.FinancialYearId);
            builder.Property(e => e.FinancialYearId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Financial_Year).IsRequired(true);
            builder.Property(e => e.StartDate).IsRequired(true).HasColumnType("datetime"); ;
            builder.Property(e => e.EndDate).IsRequired(true).HasColumnType("datetime"); ;
        }
    }
}

