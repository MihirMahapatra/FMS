using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Db.DbEntityConfig
{
    public class CompanyDetailsConfig : IEntityTypeConfiguration<CompanyDetails>
    {
        public void Configure(EntityTypeBuilder<CompanyDetails> builder)
        {
            builder.ToTable("CompanyDetails", "dbo");
            builder.HasKey(e => e.CompanyId);
            builder.Property(e => e.CompanyId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.Logo).IsRequired(true);
            builder.Property(e => e.State).IsRequired(true);
            builder.Property(e => e.Adress).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.GSTIN).IsRequired(true);
            builder.Property(e => e.Email).IsRequired(true);
            builder.Property(e => e.Phone).IsRequired(true);
            builder.HasOne(s => s.Branch).WithMany(e => e.CompanyDetails).HasForeignKey(e => e.Fk_BranchId);
        }
    }
}
