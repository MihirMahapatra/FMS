using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace FMS.Repository.Admin
{
    public interface IAdminRepo
    {
        #region Token
        Task<Result<bool>> CreateToken(string token);
        Task<Result<bool>> CreateCompany(CompanyDetailsModel data);
        Task<Result<CompanyDetailsModel>> GetCompany();
        #endregion
        #region Role & Claims 
        Task<Result<IdentityRole>> UserRoles();
        Task<Result<bool>> CreateRole(RoleModel model);
       
        Task<Result<bool>> UpdateRole(RoleModel model);
        Task<Result<bool>> DeleteRole(string id, IDbContextTransaction transaction);
        Task<Result<IdentityRole>> FindRoleById(string roleId);
        Task<Result<RoleModel>> FindUserwithClaimsForRole(string roleName);
        Task<Result<bool>> UpdateUserwithClaimsForRole(RoleModel model, IdentityRole role);
        #endregion
        #region Branch Allocation
        Task<Result<BranchAllocationModel>> GetAllUserAndBranch();
        Task<Result<bool>> CreateBranchAlloction(UserBranchModel data);
        Task<Result<bool>> UpdateBranchAlloction(UserBranchModel model);
        Task<Result<bool>> DeleteBranchAlloction(Guid Id, IDbContextTransaction transaction);
        #endregion
        #region Product Configuration
        Task<Result<ProductModel>> GetAllRawMaterial(Guid ProductTypeId);
        Task<Result<ProductModel>> GetAllFinishedGood(Guid ProductTypeId);
        Task<Result<ProductModel>> GetProductUnit(Guid ProductId);
        Task<Result<ProductionModel>> GetProductionConfig();

        Task<Result<bool>> CreateProductConfig(ProductConfigDataRequest requestData);
        Task<Result<bool>> UpdateProductConfig(ProductionModel data);
        Task<Result<bool>> DeleteProductConfig(Guid Id, IDbContextTransaction transaction);
        #endregion
        #region Account Configuration
        #region LedgerGroup
        Task<Result<LedgerGroupModel>> GetLedgerGroups();
        #endregion
        #region LedgerSubGroup
        Task<Result<LedgerSubGroupModel>> GetLedgerSubGroups(Guid GroupId);
        Task<Result<bool>> CreateLedgerSubGroup(LedgerSubGroupModel data);
        Task<Result<bool>> UpdateLedgerSubGroup(LedgerSubGroupModel data);
        Task<Result<bool>> DeleteLedgerSubGroup(Guid Id, IDbContextTransaction transaction);
        #endregion
        #region Ledger
        Task<Result<LedgerModel>> GetLedgers();
        Task<Result<LedgerModel>> GetLedgersHasSubLedger();
        Task<Result<LedgerModel>> GetLedgersHasNoSubLedger();
        Task<Result<bool>> CreateLedger(LedgerViewModel listData);
        Task<Result<bool>> UpdateLedger(LedgerModel data);
        Task<Result<bool>> DeleteLedger(Guid Id, IDbContextTransaction transaction);
        #endregion      
        #endregion
    }
}
