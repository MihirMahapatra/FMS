using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class UserBranchConfig : IEntityTypeConfiguration<UserBranch>
    {
        public void Configure(EntityTypeBuilder<UserBranch> builder)
        {
            builder.ToTable("UserBranches", "dbo");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.HasOne(ub => ub.User).WithMany(u => u.UserBranch).HasForeignKey(ub => ub.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ub => ub.Branch).WithMany(b => b.UserBranch).HasForeignKey(ub => ub.BranchId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
