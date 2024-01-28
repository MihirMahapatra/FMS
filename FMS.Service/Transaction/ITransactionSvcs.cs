using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using Microsoft.EntityFrameworkCore.Storage;

namespace FMS.Service.Transaction
{
    public interface ITransactionSvcs
    {
        #region Purchase Transaction
        Task<SubLedgerViewModel> GetSundryCreditors(Guid PartyTypeId);
        #region Purchase
        Task<Base> GetLastPurchaseTransaction();
        Task<PurchaseOrderViewModel> GetPurchases();
        Task<PurchaseOrderViewModel> GetPurchaseById(Guid Id);
        Task<Base> CreatePurchase(PurchaseDataRequest data);
        Task<Base> UpdatePurchase(PurchaseDataRequest data);
        Task<Base> DeletePurchase(Guid Id);
        #endregion
        #region Purchase Return
        Task<Base> GetLastPurchaseReturnTransaction();
        Task<PurchaseReturnOrderViewModel> GetPurchaseReturns();
        Task<PurchaseReturnOrderViewModel> GetPurchaseReturnById(Guid Id);
        Task<Base> CreatetPurchaseReturn(PurchaseDataRequest data);
        Task<Base> UpdatetPurchaseReturn(PurchaseDataRequest data);
        Task<Base> DeletetPurchaseReturn(Guid Id);
        #endregion
        #endregion
        #region Production Entry
        Task<Base> GetLastProductionNo();
        Task<ProductionViewModel> GetProductionConfig(Guid ProductId);
        Task<LabourOrderViewModel> GetProductionEntry();
        Task<Base> CreateProductionEntry(ProductionEntryRequest data);
        Task<Base> UpdateProductionEntry(LabourOrderModel data);
        Task<Base> DeleteProductionEntry(Guid Id);
        #endregion
        #region Service Entry
        Task<Base> GetLastServiceNo();
        Task<LabourOrderViewModel> GetServiceEntry();
        Task<Base> CreateServiceEntry(ProductionEntryRequest data);
        Task<Base> UpdateServiceEntry(LabourOrderModel data);
        Task<Base> DeleteServiceEntry(Guid Id);
        #endregion
        #region Sales Transaction
        Task<SubLedgerViewModel> GetSundryDebtors(Guid PartyTypeId);
        #region Sales
        Task<Base> GetLastSalesTransaction();
        Task<Base> CreateSale(SalesDataRequest data);
        Task<SalesOrderViewModel> GetSales();
        Task<SalesOrderViewModel> GetSalesById(Guid Id);
        Task<Base> UpdatSales(SalesDataRequest data);
        Task<Base> DeleteSales(Guid Id);
        #endregion
        #region Sales Return
        Task<Base> GetLastSalesReturnTransaction();
        Task<Base> CreateSalesReturn(SalesReturnDataRequest data);
        Task<SalesReturnOrderViewModel> GetSalesReturns();
        Task<SalesReturnOrderViewModel> GetSalesReturnById(Guid Id);
        Task<Base> UpdateSalesReturn(SalesReturnDataRequest data);
        Task<Base> DeleteSalesReturn(Guid Id);
        #endregion
        #endregion
        #region Inward Supply Transaction
        Task<Base> GetLastInwardSupply();
        Task<InwardSupplyViewModel> GetInwardSupply();
        Task<InwardSupplyViewModel> GetInwardSupplyById(Guid Id);
        Task<Base> CreateInwardSupply(SupplyDataRequest data);
        Task<Base> UpdateInwardSupply(SupplyDataRequest data);
        Task<Base> DeleteInwardSupply(Guid Id);
        #endregion
        #region Outward Supply Transaction
        Task<Base> GetLastOutwardSupply();
        Task<OutwardSupplyViewModel> GetOutwardSupply();
        Task<OutwardSupplyViewModel> GetOutwardSupplyById(Guid Id);
        Task<Base> CreateOutwardSupply(SupplyDataRequest data);
        Task<Base> UpdateOutwardSupply(SupplyDataRequest data);
        Task<Base> DeleteOutwardSupply(Guid Id);
        #endregion
        #region Damage Transaction
        Task<Base> GetLastDamageEntry();
        Task<DamageViewModel> GetDamages();
        Task<DamageViewModel> GetDamageById(Guid Id);
        Task<Base> CreateDamage(DamageRequestData data);
        Task<Base> UpdateDamage(DamageRequestData data);
        Task<Base> DeleteDamage(Guid Id);
        #endregion
    }
}
