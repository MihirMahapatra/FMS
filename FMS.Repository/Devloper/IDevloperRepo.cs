using FMS.Model;
using FMS.Model.CommonModel;
using Microsoft.EntityFrameworkCore.Storage;

namespace FMS.Repository.Devloper
{
    public interface IDevloperRepo
    {
        #region Branch
        Task<Result<BranchModel>> GetAllBranch();
        Task<Result<BranchModel>> GetBranchById(Guid BranchId);
        Task<Result<BranchModel>> GetBranchAccordingToUser(string UserId);
        Task<Result<bool>> CreateBranch(BranchModel data);
        Task<Result<bool>> UpdateBranch(BranchModel data);
        Task<Result<bool>> DeleteBranch(Guid Id, IDbContextTransaction transaction);
        #endregion
        #region Branch Financial Year
        Task<Result<BranchFinancialYearModel>> GetBranchFinancialYears();
        Task<Result<BranchFinancialYearModel>> GetBranchFinancialYears(Guid BranchId);
        Task<Result<bool>> CreateBranchFinancialYear(BranchFinancialYearModel data);
        Task<Result<bool>> UpdateBranchFinancialYear(BranchFinancialYearModel data);
        Task<Result<bool>> DeleteBranchFinancialYear(Guid Id, IDbContextTransaction transaction);
        #endregion
        #region Financial Year
        Task<Result<FinancialYearModel>> GetFinancialYears(Guid BranchId);
        Task<Result<FinancialYearModel>> GetFinancialYears();
        Task<Result<FinancialYearModel>> GetFinancialYearById(Guid FinancialYearId);
        Task<Result<bool>> CreateFinancialYear(FinancialYearModel data);
        Task<Result<bool>> UpdateFinancialYear(FinancialYearModel data);
        Task<Result<bool>> DeleteFinancialYear(Guid Id, IDbContextTransaction transaction);
        #endregion
        #region Accounting Setup
        #region LedgerGroup
        Task<Result<LedgerGroupModel>> GetLedgerGroups();
        Task<Result<bool>> CreateLedgerGroup(LedgerGroupModel data);
        Task<Result<bool>> UpdateLedgerGroup(LedgerGroupModel data);
        Task<Result<bool>> DeleteLedgerGroup(Guid Id, IDbContextTransaction transaction);
        #endregion
        #region LedgerSubGroup
        Task<Result<LedgerSubGroupModel>> GetLedgerSubGroups(Guid BranchId, Guid GroupId);
        Task<Result<bool>> CreateLedgerSubGroup(LedgerSubGroupModel data);
        Task<Result<bool>> UpdateLedgerSubGroup(LedgerSubGroupModel data);
        Task<Result<bool>> DeleteLedgerSubGroup(Guid BranchId, Guid Id, IDbContextTransaction transaction);
        #endregion
        #region Ledger
        Task<Result<LedgerModel>> GetLedgers();
        Task<Result<bool>> CreateLedger(List<LedgerModel> listData);
        Task<Result<bool>> UpdateLedger(LedgerModel data);
        Task<Result<bool>> DeleteLedger(Guid Id, IDbContextTransaction transaction);
        #endregion
        #endregion
    }
}
