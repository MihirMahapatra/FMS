﻿using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class StateConfig : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.ToTable("States", "dbo");
            builder.HasKey(e => e.StateId);
            builder.Property(e => e.StateId).HasDefaultValueSql("(newid())").IsRequired(true);
            builder.Property(e => e.Fk_BranchId).IsRequired(true).IsRequired(true);
            builder.Property(e => e.StateName).HasMaxLength(100).IsRequired(true);
            builder.HasOne(bs => bs.Branch).WithMany(b => b.States).HasForeignKey(bs => bs.Fk_BranchId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
