using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class SubGroupConfig : IEntityTypeConfiguration<SubGroup>
    {
        public void Configure(EntityTypeBuilder<SubGroup> builder)
        {
            builder.ToTable("SubGroups", "dbo");
            builder.HasKey(e => e.SubGroupId);
            builder.Property(e => e.SubGroupId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_GroupId).IsRequired(false);
            builder.Property(e => e.SubGroupName).HasMaxLength(200).IsRequired(true);
            builder.HasOne(s => s.Group).WithMany(t => t.SubGroups).HasForeignKey(s => s.Fk_GroupId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
