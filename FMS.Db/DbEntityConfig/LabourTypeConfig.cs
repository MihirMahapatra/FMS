using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class LabourTypeConfig : IEntityTypeConfiguration<LabourType>
    {
        public void Configure(EntityTypeBuilder<LabourType> builder)
        {
            builder.ToTable("LabourTypes", "dbo");
            builder.HasKey(e => e.LabourTypeId);
            builder.Property(e => e.LabourTypeId).HasDefaultValueSql("(newid())");
            builder.Property(e=>e.Labour_Type).HasMaxLength(100).IsRequired();
        }
    }
}
