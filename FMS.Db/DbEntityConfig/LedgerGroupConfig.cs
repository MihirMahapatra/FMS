using FMS.Db.DbEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FMS.Db.DbEntityConfig
{
    public class LedgerGroupConfig : IEntityTypeConfiguration<LedgerGroup>
    {
        public void Configure(EntityTypeBuilder<LedgerGroup> builder)
        {
            builder.ToTable("LedgerGroups", "dbo");
            builder.HasKey(e => e.LedgerGroupId);
            builder.Property(e => e.LedgerGroupId).HasDefaultValueSql("(newid())");
            builder.Property(e => e.GroupName).HasMaxLength(100).IsRequired(true);
            builder.Property(e => e.GroupAlias).HasMaxLength(100).IsRequired(false);
            builder.HasData(
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("4458BCE5-4546-4120-A7DE-03ACEFD07B85"), GroupName = "Purchase", GroupAlias = "PLTR-DR" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("39B5514A-9359-46F3-8C3E-0EABD6880CF6"), GroupName = "Unsecured Loan", GroupAlias = "LB" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("C3C725D0-A502-4275-B0F9-1585AB6EDCC7"), GroupName = "Depreciation", GroupAlias = "PL-DR" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("F3EEF2DD-09BB-4E21-B036-1E5BBA920EFE"), GroupName = "Cash & Bank Balance", GroupAlias = "AS" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("58639324-2180-4E0B-932D-33024AA3FC5F"), GroupName = "Indirect Expenses", GroupAlias = "PL-DR" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("23D58228-DB08-4C3D-9177-343B1CFDCF7A"), GroupName = "Liability for Expenses", GroupAlias = "LB" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("4D80E68F-FF00-486A-945A-3941761441D9"), GroupName = "Fixed Assets", GroupAlias = "AS" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("EA3F21AD-B0D4-4C27-9F9D-3C36A7A585C2"), GroupName = "Direct Income", GroupAlias = "PLTR-CR" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("15FE2512-D922-45C5-9E03-64C32B903A5B"), GroupName = "Sales", GroupAlias = "PLTR-CR" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("ACA9CAF1-EA9B-4602-BB60-6C354EAC5CE6"), GroupName = "Current liabilities & Provisions", GroupAlias = "LB" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("9BBC2C1F-FFA4-44B8-9916-6EDF0A47D8DB"), GroupName = "Capital A/c", GroupAlias = "PL-DR" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("BDCF2EE2-8AAB-44F6-BD1E-77B53074389A"), GroupName = "Opening Stock", GroupAlias = "PLTR-DR" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("2FC89E45-7365-46B7-933C-9ABAE2E5967A"), GroupName = "Current Assets", GroupAlias = "AS" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("68EEFFE7-02F7-4FFC-81B3-AEB0CADC764B"), GroupName = "Duties & Taxes", GroupAlias = "LB" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("345B0D2A-8FCA-414F-A6F2-C5F7FD9246AC"), GroupName = "Indirect Income", GroupAlias = "PL-CR" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("01548EF6-3FE2-4C0F-9A5F-CEED35066136"), GroupName = "Direct Expenses", GroupAlias = "PLTR-DR" },
                    new LedgerGroup() { LedgerGroupId = Guid.Parse("84A336C6-E48A-43E8-984E-F45B0BF2984F"), GroupName = "Secured Loan", GroupAlias = "LB" }
                );
        }
    }
}
