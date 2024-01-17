using FMS.Model;
using FMS.Model.CommonModel;
using Microsoft.EntityFrameworkCore.Storage;

namespace FMS.Repository.Transaction
{
    public interface ITransactionRepo
    {
        #region Purchase Transaction
        Task<Result<SubLedgerModel>> GetSundryCreditors(Guid PartyTypeId);
        #region Purchase
        Task<Result<string>> GetLastPurchaseTransaction();
        Task<Result<PurchaseOrderModel>> GetPurchases();
        Task<Result<PurchaseOrderModel>> GetPurchaseById(Guid Id);
        Task<Result<bool>> CreatePurchase(PurchaseDataRequest data);
        Task<Result<bool>> UpdatePurchase(PurchaseDataRequest data);
        Task<Result<bool>> DeletePurchase(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region Purchase Return
        Task<Result<string>> GetLastPurchaseReturnTransaction();
        Task<Result<PurchaseReturnOrderModel>> GetPurchaseReturns();
        Task<Result<PurchaseReturnOrderModel>> GetPurchaseReturnById(Guid Id);
        Task<Result<bool>> CreatetPurchaseReturn(PurchaseDataRequest data);
        Task<Result<bool>> UpdatetPurchaseReturn(PurchaseDataRequest data);
        Task<Result<bool>> DeletetPurchaseReturn(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #endregion
        #region Production Entry
        Task<Result<string>> GetLastProductionNo();
        Task<Result<ProductionModel>> GetProductionConfig(Guid ProductId);
        Task<Result<LabourOrderModel>> GetProductionEntry();
        Task<Result<bool>> CreateProductionEntry(ProductionEntryRequest data);
        Task<Result<bool>> UpdateProductionEntry(LabourOrderModel data);
        Task<Result<bool>> DeleteProductionEntry(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region Service Entry
        Task<Result<string>> GetLastServiceNo();
        Task<Result<LabourOrderModel>> GetServiceEntry();
        Task<Result<bool>> CreateServiceEntry(ProductionEntryRequest data);
        Task<Result<bool>> UpdateServiceEntry(LabourOrderModel data);
        Task<Result<bool>> DeleteServiceEntry(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region Sales Transaction
        Task<Result<SubLedgerModel>> GetSundryDebtors(Guid PartyTypeId);
        #region Sales
        Task<Result<string>> GetLastSalesTransaction();
        Task<Result<bool>> CreateSales(SalesDataRequest data);
        Task<Result<SalesOrderModel>> GetSales();
        Task<Result<SalesOrderModel>> GetSalesById(Guid Id);
        Task<Result<bool>> UpdateSales(SalesDataRequest data);
        Task<Result<bool>> DeleteSales(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region Sales Return
        Task<Result<string>> GetLastSalesReturnTransaction();
        Task<Result<bool>> CreateSalesReturn(SalesDataRequest data);
        Task<Result<SalesReturnOrderModel>> GetSalesReturns();
        Task<Result<SalesReturnOrderModel>> GetSalesReturnById(Guid Id);
        Task<Result<bool>> UpdateSalesReturn(SalesDataRequest data);
        Task<Result<bool>> DeleteSalesReturn(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #endregion
        #region Inward Supply Transaction
        Task<Result<string>> GetLastInwardSupply();
        Task<Result<InwardSupplyOrderModel>> GetInwardSupply();
        Task<Result<InwardSupplyOrderModel>> GetInwardSupplyById(Guid Id);
        Task<Result<bool>> CreateInwardSupply(SupplyDataRequest data);
        Task<Result<bool>> UpdateInwardSupply(SupplyDataRequest data);
        Task<Result<bool>> DeleteInwardSupply(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region Outward Supply Transaction
        Task<Result<string>> GetLastOutwardSupply();
        Task<Result<OutwardSupplyOrderModel>> GetOutwardSupply();
        Task<Result<OutwardSupplyOrderModel>> GetOutwardSupplyById(Guid Id);
        Task<Result<bool>> CreateOutwardSupply(SupplyDataRequest data);
        Task<Result<bool>> UpdateOutwardSupply(SupplyDataRequest data);
        Task<Result<bool>> DeleteOutwardSupply(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region Damage Transaction
        Task<Result<string>> GetLastDamageEntry();
        Task<Result<DamageOrderModel>> GetDamages();
        Task<Result<DamageOrderModel>> GetDamageById(Guid Id);
        Task<Result<bool>> CreateDamage(DamageRequestData data);
        Task<Result<bool>> UpdateDamage(DamageRequestData data);
        Task<Result<bool>> DeleteDamage(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
    }
}
