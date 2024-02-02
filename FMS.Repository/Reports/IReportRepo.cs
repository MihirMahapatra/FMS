using FMS.Model;
using FMS.Model.CommonModel;
namespace FMS.Repository.Reports
{
    public interface IReportRepo
    {
        #region Stock Report
        Task<Result<StockReportSummerizedModel>> GetSummerizedStockReports(StockReportDataRequest requestData);
        Task<Result<StockReportSummerizedInfoModel>> GetBranchWiseStockInfo(StockReportDataRequest requestData);
        Task<Result<StockReportDetailedModel2>> GetDetailedStockReport(StockReportDataRequest requestData);
        #endregion
        #region Labour Report
        Task<Result<LaborReportModel>> GetSummerizedLabourReport(LabourReportDataRequest requestData);
        Task<Result<LabourModel>> GetDetailedLabourReport(LabourReportDataRequest requestData);
        #endregion
        #region Customer Report
        Task<Result<PartyReportModel>> GetSummerizedCustomerReport(PartyReportDataRequest requestData);
        Task<Result<PartyModel>> GetDetailedCustomerReport(PartyReportDataRequest requestData);
        #endregion
        #region Supplyer Report
        Task<Result<PartyReportModel>> GetSummerizedSupplyerReport(PartyReportDataRequest requestData);
        Task<Result<PartyModel>> GetDetailedSupplyerReport(PartyReportDataRequest requestData);
        #endregion
        #region DaySheet
        public Task<Result<DaySheetModel>> GetDaySheet(string Date);

        #endregion
        #region CashBook
        Task<Result<CashBookModal>> CashBookReport(CashBookDataRequest requestData);

        #endregion
        #region BankBook
        Task<Result<BankBookModal>> BankBookReport(BankBookDataRequest requestData);

        #endregion
        #region LadgerBook
        Task<Result<LedgerBookModel>> LedgerBookReport(LedgerbookDataRequest requestData);

        #endregion
        #region Trial balance
        Task<Result<LedgerTrialBalanceModel>> TrialbalanceReport(LedgerbookDataRequest requestData);

        #endregion
        #region JournalBook
        Task<Result<GroupedJournalModel>> JournalBookreport(LedgerbookDataRequest requestData);

        #endregion
    }
}
