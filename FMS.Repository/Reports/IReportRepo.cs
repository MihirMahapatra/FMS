using FMS.Model;
using FMS.Model.CommonModel;
namespace FMS.Repository.Reports
{
    public interface IReportRepo
    {
        #region Stock Report
        Task<Result<StockReportModel>> GetSummerizedStockReports(StockReportDataRequest requestData);
        Task<Result<ProductModel>> GetDetailedStockReport(StockReportDataRequest requestData);
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
    }
}
