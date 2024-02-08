using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class CityConfig : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities", "dbo");
            builder.HasKey(e => e.CityId);
            builder.Property(e => e.CityId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Fk_StateId).IsRequired(true);
            builder.Property(e => e.CityName).HasMaxLength(100).IsRequired(true);
            builder.HasOne(c => c.State).WithMany(s => s.Cities).HasForeignKey(c => c.Fk_StateId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
