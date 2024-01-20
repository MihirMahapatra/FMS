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
            builder.HasData(
                new LabourType() { LabourTypeId = Guid.Parse("6C2758A2-79B5-43A6-8851-C6F975433B0F"), Labour_Type = "SERVICE" },
                new LabourType() { LabourTypeId = Guid.Parse("5E514855-55A0-459C-8B8B-DEF7696D9AD0"), Labour_Type = "PRODUCTION" }
               );

        }
    }
}
