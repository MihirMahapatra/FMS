using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class LedgerSubGroupConfig : IEntityTypeConfiguration<LedgerSubGroup>
    {
        public void Configure(EntityTypeBuilder<LedgerSubGroup> builder)
        {
            builder.ToTable("LedgerSubGroups", "dbo");
            builder.HasKey(e => e.LedgerSubGroupId);
            builder.Property(e => e.LedgerSubGroupId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_LedgerGroupId).IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true);
            builder.Property(e => e.SubGroupName).IsRequired(true).HasMaxLength(200);
            builder.HasOne(sg => sg.LedgerGroup).WithMany(g => g.LedgerSubGroups).HasForeignKey(sg => sg.Fk_LedgerGroupId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sg => sg.Branch).WithMany(g => g.SubGroups).HasForeignKey(sg => sg.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
