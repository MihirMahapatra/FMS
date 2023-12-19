using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class GroupConfig : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Groups", "dbo");
            builder.HasKey(e => e.GroupId);
            builder.Property(e=>e.Fk_ProductTypeId).IsRequired(false);
            builder.Property(e => e.GroupId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.GroupName).HasMaxLength(500).IsRequired(true);
            builder.HasOne(p => p.ProductType).WithMany(po => po.Groups).HasForeignKey(po => po.Fk_ProductTypeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
