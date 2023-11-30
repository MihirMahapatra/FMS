using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;

namespace FMS.Service.Devloper
{
    public interface IDevloperSvcs
    {
        #region Branch
        Task<BranchViewModel> GetAllBranch();
        Task<BranchViewModel> GetBranchAccordingToUser(string UserId);
        Task<Base> CreateBranch(BranchModel data);
        Task<Base> UpdateBranch(BranchModel data);
        Task<Base> DeleteBranch(Guid Id);
        #endregion
        #region FinancialYear
        Task<FinancialYearViewModel> GetFinancialYears(Guid BranchId);
        Task<FinancialYearViewModel> GetFinancialYears();
        Task<Base> CreateFinancialYear(FinancialYearModel data);
        Task<Base> UpdateFinancialYear(FinancialYearModel data);
        Task<Base> DeleteFinancialYear(Guid Id);
        #endregion
        #region Accounting Setup
        #region LedgerGroup
        Task<LedgerGroupViewModel> GetLedgerGroups();
        Task<Base> CreateLedgerGroup(LedgerGroupModel data);
        Task<Base> UpdateLedgerGroup(LedgerGroupModel data);
        Task<Base> DeleteLedgerGroup(Guid Id);
        #endregion
        #region LedgerSubGroup
        Task<LedgerSubGroupViewModel> GetLedgerSubGroups(Guid BranchId, Guid GroupId);
        Task<Base> CreateLedgerSubGroup(LedgerSubGroupModel data);
        Task<Base> UpdateLedgerSubGroup(LedgerSubGroupModel data);
        Task<Base> DeleteLedgerSubGroup(Guid BranchId, Guid Id);
        #endregion   
        #region Ledger
        Task<LedgerViewModel> GetLedgers();
        Task<Base> CreateLedger(List<LedgerModel> listData);
        Task<Base> UpdateLedger(LedgerModel data);
        Task<Base> DeleteLedger(Guid Id);
        #endregion
        #endregion
    }
}
