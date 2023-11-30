using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class LabourConfig : IEntityTypeConfiguration<Labour>
    {
        public void Configure(EntityTypeBuilder<Labour> builder)
        {
            builder.ToTable("Labours", "dbo");
            builder.HasKey(e => e.LabourId);
            builder.Property(e => e.LabourId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_Labour_TypeId).IsRequired(true);
            builder.Property(e => e.Fk_SubLedgerId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(false);
            builder.Property(e => e.LabourName).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.Address).HasMaxLength(500).IsRequired(true);
            builder.Property(e => e.Phone).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.Reference).HasMaxLength(100).IsRequired(false);
            builder.HasOne(d => d.SubLedger).WithMany(e => e.Labours).HasForeignKey(d => d.Fk_SubLedgerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(d => d.LabourType).WithMany(e => e.Labours).HasForeignKey(d => d.Fk_Labour_TypeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(d => d.Branch).WithMany(e => e.Labours).HasForeignKey(d => d.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
