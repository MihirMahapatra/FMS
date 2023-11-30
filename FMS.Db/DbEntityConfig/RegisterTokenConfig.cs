namespace FMS.Db.DbEntityConfig
{
    using FMS.Db.DbEntity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RegisterTokenConfig : IEntityTypeConfiguration<RegisterToken>
    {
        public void Configure(EntityTypeBuilder<RegisterToken> builder)
        {
            builder.ToTable("RegisterTokens", "dbo");
            builder.HasKey(e => e.TokenId);
            builder.Property(e => e.TokenId).HasDefaultValueSql("newid()");
            builder.Property(e => e.TokenValue).HasMaxLength(150).IsUnicode(false);
        }
    }
}
