using FMS.Db.Context;
using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Utility;
using System.Globalization;
namespace FMS.Repository.Reports
{
    public interface IReportRepo
    {
        #region GrapData
        Task<Result<GraphDataModel>> GetGraphData();
        Task<Result<GraphDataModel>> GetGraphDataforProductWise(StockReportDataRequest requestData);

        #endregion
        #region Stock Report
        Task<Result<StockReportSummerizedModel>> GetSummerizedStockReports(StockReportDataRequest requestData);
        Task<Result<StockReportSummerizedInfoModel>> GetBranchWiseStockInfo(StockReportDataRequest requestData);
        Task<Result<StockReportDetailedModel2>> GetDetailedStockReport(StockReportDataRequest requestData);
        #endregion
        #region Labour Report
        Task<Result<LaborReportModel>> GetSummerizedLabourReport(LabourReportDataRequest requestData);
        Task<Result<LabourReportDetailedModel2>> GetDetailedLabourReport(LabourReportDataRequest requestData);
        #endregion
        #region Customer Report
        Task<Result<PartyReportModel>> GetSummerizedCustomerReport(PartyReportDataRequest requestData);
        Task<Result<PartyReportInfoModel>> GetBranchWiseCustomerInfo(PartyReportDataRequest requestData);
        Task<Result<PartyReportModel2>> GetDetailedCustomerReport(PartyReportDataRequest requestData);
        #endregion
        #region Supplyer Report
        Task<Result<PartyReportModel>> GetSummerizedSupplyerReport(PartyReportDataRequest requestData);
        Task<Result<PartyReportInfoModel>> GetBranchWiseSupllayerInfo(PartyReportDataRequest requestData);
        Task<Result<PartyReportModel2>> GetDetailedSupplyerReport(PartyReportDataRequest requestData);
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
        #region SubLadgerLadgerBook
        Task<Result<SubLedgerModel>> GetSubLadgers();
        Task<Result<PartyReportModel>> GetSummerizedSubLadgerReport(LedgerbookDataRequest requestData);
        Task<Result<PartyReportModel2>> SubLadgerDetailedBookReport(LedgerbookDataRequest requestData);

        #endregion
        #region Trial balance
        Task<Result<LedgerTrialBalanceModel>> TrialbalanceReport(LedgerbookDataRequest requestData);

        #endregion
        #region JournalBook
        Task<Result<GroupedJournalModel>> JournalBookreport(LedgerbookDataRequest requestData);

        #endregion
        #region Refarance Report
        Task<Result<PartyReportModel>> GetCustomerRefranceReport(PartyReportDataRequest requestData);
        #endregion
        #region All Branch Customer Details
        Task<Result<PartyReportModel2>> GetDetailedCustomerReportForAll(PartyReportDataRequest requestData);
        #endregion
        #region Supplyer Report All Branches
        Task<Result<PartyReportModel2>> GetDetailedSupplyerReportForAll(PartyReportDataRequest requestData);
        #endregion
    }
}
