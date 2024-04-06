using FMS.Db.DbEntity;
using FMS.Db.DbEntityConfig;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FMS.Db.Context;

public partial class AppDbContext : IdentityDbContext<AppUser>, IDataProtectionKeyContext
{
    public AppDbContext()
    {

    }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    #region Entity
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
    public  DbSet<AppUser> AppUsers { get; set; }
    public  DbSet<RegisterToken> RegisterTokens { get; set; }
    public DbSet<FinancialYear> FinancialYears { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<BranchFinancialYear> BranchFinancialYears { get; set; }
    public  DbSet<UserBranch> UserBranches { get; set; }
    public DbSet<Company> Companies { get; set; }
    public  DbSet<ProductGroup> ProductGroups { get; set; }
    public  DbSet<ProductSubGroup> SubGroups { get; set; }
    public  DbSet<Unit> Units { get; set; }
    public DbSet<AlternateUnit> AlternateUnits { get; set; }
    public  DbSet<ProductType> ProductTypes { get; set; }
    public  DbSet<Product> Products { get; set; }
    public  DbSet<Production> Productions { get; set; }
    public  DbSet<LabourOrder> LabourOrders { get; set; }
    public DbSet<LabourTransaction> LabourTransactions { get; set; }
    public  DbSet<Stock> Stocks { get; set; }
    public  DbSet<Labour> Labours { get; set; }
    public  DbSet<LabourType> LabourTypes { get; set; }
    public DbSet<LabourRate> LabourRates { get; set; }
    public DbSet<LedgerGroup> LedgerGroups { get; set; }
    public DbSet<LedgerSubGroup> LedgerSubGroups { get; set; }
    public DbSet<LedgerSubGroupDev> LedgerSubGroupDevs { get; set; }
    public DbSet<Ledger> Ledgers { get; set; }
    public DbSet<LedgerDev> LedgersDev { get; set; }
    public DbSet<LedgerBalance> LedgerBalances { get; set; }
    public DbSet<SubLedger> SubLedgers { get; set; }
    public DbSet<SubLedgerBalance> SubLedgerBalances { get; set; }
    public DbSet<Party> Parties { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Journal> Journals { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<PurchaseTransaction> PurchaseTransactions { get; set; }
    public DbSet<SalesTransaction> SalesTransaction { get; set; }
    public DbSet<PurchaseReturnOrder> PurchaseReturnOrders { get; set; }
    public DbSet<PurchaseReturnTransaction> PurchaseReturnTransactions { get; set; }
    public DbSet<SalesReturnOrder> SalesReturnOrders { get; set; }
    public DbSet<SalesReturnTransaction> SalesReturnTransactions { get; set; }
    public DbSet<InwardSupplyOrder> InwardSupplyOrders { get; set; }
    public DbSet<InwardSupplyTransaction> InwardSupplyTransactions { get; set; }
    public DbSet<OutwardSupplyOrder> OutwardSupplyOrders { get; set; }
    public DbSet<OutwardSupplyTransaction> OutwardSupplyTransactions { get; set; }
    public DbSet<DamageOrder> DamageOrders { get; set; }
    public DbSet<DamageTransaction> DamageTransactions { get; set; }
    public DbSet<SalesConfig> SalesConfigs { get; set; }
    public DbSet<Referance> Referances { get; set; }
    public DbSet<PartyGroup> PartyGroups { get; set; }

    #endregion
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        #region EntityConfig
        new AppUserConfig().Configure(modelBuilder.Entity<AppUser>()); 
        new RegisterTokenConfig().Configure(modelBuilder.Entity<RegisterToken>());
        new FinancialYearConfig().Configure(modelBuilder.Entity<FinancialYear>());
        new BranchConfig().Configure(modelBuilder.Entity<Branch>());
        new BranchFinancialYearConfig().Configure(modelBuilder.Entity<BranchFinancialYear>());
        new UserBranchConfig().Configure(modelBuilder.Entity<UserBranch>());
        new CompanyConfig().Configure(modelBuilder.Entity<Company>());
        new ProductGroupConfig().Configure(modelBuilder.Entity<ProductGroup>());
        new ProductSubGroupConfig().Configure(modelBuilder.Entity<ProductSubGroup>());
        new UnitConfig().Configure(modelBuilder.Entity<Unit>());
        new AlternateUnitConfig().Configure(modelBuilder.Entity<AlternateUnit>());
        new ProductTypeConfig().Configure(modelBuilder.Entity<ProductType>());
        new ProductConfig().Configure(modelBuilder.Entity<Product>());
        new ProductionConfig().Configure(modelBuilder.Entity<Production>());
        new LabourOrderConfig().Configure(modelBuilder.Entity<LabourOrder>());
        new LabourTransactionConfig().Configure(modelBuilder.Entity<LabourTransaction>());
        new StockConfig().Configure(modelBuilder.Entity<Stock>());
        new LabourConfig().Configure(modelBuilder.Entity<Labour>());
        new LabourTypeConfig().Configure(modelBuilder.Entity<LabourType>());
        new LabourRateConfig().Configure(modelBuilder.Entity<LabourRate>());
        new LedgerGroupConfig().Configure(modelBuilder.Entity<LedgerGroup>());
        new LedgerSubGroupConfig().Configure(modelBuilder.Entity<LedgerSubGroup>());
        new LedgerSubGroupDevConfig().Configure(modelBuilder.Entity<LedgerSubGroupDev>());
        new LedgerConfig().Configure(modelBuilder.Entity<Ledger>());
        new LedgersDevConfig().Configure(modelBuilder.Entity<LedgerDev>());
        new LedgerBalanceConfig().Configure(modelBuilder.Entity<LedgerBalance>());
        new SubLedgerConfig().Configure(modelBuilder.Entity<SubLedger>());
        new SubLedgerBalanceConfig().Configure(modelBuilder.Entity<SubLedgerBalance>());
        new PartyConfig().Configure(modelBuilder.Entity<Party>());
        new StateConfig().Configure(modelBuilder.Entity<State>());
        new CityConfig().Configure(modelBuilder.Entity<City>());
        new JournalConfig().Configure(modelBuilder.Entity<Journal>());
        new PaymentConfig().Configure(modelBuilder.Entity<Payment>());
        new ReceiptConfig().Configure(modelBuilder.Entity<Receipt>());
        new PurchaseOrderConfig().Configure(modelBuilder.Entity<PurchaseOrder>());
        new SalesOrderConfig().Configure(modelBuilder.Entity<SalesOrder>());
        new PurchaseTransactionConfig().Configure(modelBuilder.Entity<PurchaseTransaction>());
        new SalesTransactionConfig().Configure(modelBuilder.Entity<SalesTransaction>());
        new PurchaseReturnTransactionConfig().Configure(modelBuilder.Entity<PurchaseReturnTransaction>());
        new PurchaseReturnOrderConfig().Configure(modelBuilder.Entity<PurchaseReturnOrder>());
        new SalesReturnOrderConfig().Configure(modelBuilder.Entity<SalesReturnOrder>());
        new SalesReturnTransactionConfig().Configure(modelBuilder.Entity<SalesReturnTransaction>());
        new InwardSupplyOrderConfig().Configure(modelBuilder.Entity<InwardSupplyOrder>());
        new InwardSupplyTransactionConfig().Configure(modelBuilder.Entity<InwardSupplyTransaction>());
        new OutwardSupplyOrderConfig().Configure(modelBuilder.Entity<OutwardSupplyOrder>());
        new OutwardSupplyTransactionConfig().Configure(modelBuilder.Entity<OutwardSupplyTransaction>());
        new DamageOrderConfig().Configure(modelBuilder.Entity<DamageOrder>());
        new DamageTransactionConfig().Configure(modelBuilder.Entity<DamageTransaction>());
        new SalesConfigConfig().Configure(modelBuilder.Entity<SalesConfig>());
        new ReferanceConfig().Configure(modelBuilder.Entity<Referance>());
        new PartyGruopConfig().Configure(modelBuilder.Entity<PartyGroup>());
        #endregion
        base.OnModelCreating(modelBuilder);
    }
}
