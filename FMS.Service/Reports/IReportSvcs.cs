using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;

namespace FMS.Service.Reports
{
    public interface IReportSvcs
    {
        #region Stock Report 
        Task<StockReportSummerizedViewModel> GetSummerizedStockReports(StockReportDataRequest requestData);
        Task<StockReportDetailedViewModel> GetDetailedStockReport(StockReportDataRequest requestData);
        #endregion
        #region Labour Report
        Task<LaborReportViewModel> GetSummerizedLabourReport(LabourReportDataRequest requestData);
        Task<LabourViewModel> GetDetailedLabourReport(LabourReportDataRequest requestData);
        #endregion
        #region Customer Report
        Task<PartyReportViewModel> GetSummerizedCustomerReport(PartyReportDataRequest requestData);
        Task<PartyViewModel> GetDetailedCustomerReport(PartyReportDataRequest requestData);
        #endregion
        #region Supplyer Report
        Task<PartyReportViewModel> GetSummerizedSupplyerReport(PartyReportDataRequest requestData);
        Task<PartyViewModel> GetDetailedSupplyerReport(PartyReportDataRequest requestData);
        #endregion
        #region DaySheet
        public Task<DaySheetViewModel> GetDaySheet(string Date);
        #endregion
        #region CashBook
        public Task<CashBookViewModal> CashBookReport(CashBookDataRequest requestData);
        #endregion
        #region BankBook
        public Task<BankBookViewModal> BankBookReport(BankBookDataRequest requestData);
        #endregion
        #region LedgerBook
        public Task<LedgerBookViewModal> LagderBookReport(LedgerbookDataRequest requestData);
        #endregion
        #region TrialBalances
        public Task<LedgerTrialBalanceViewModel> TrialBalanceReport(LedgerbookDataRequest requestData);
        #endregion
        #region TrialBalances
        public Task<JournalViewModel> JournalBookReport(LedgerbookDataRequest requestData);
        #endregion
    }
}
