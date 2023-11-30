using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace FMS.Service.Admin
{
    public interface IAdminSvcs
    {
        Task<Base> CreateToken(string Token);
        Task<Base> CreateCompany(CompanyDetailsModel data);
        Task<CompanyDetailsModel> GetCompany();
        #region Roles and Claims
        Task<List<IdentityRole>> UserRoles();
        Task<Base> CreateRole(RoleModel model);
        Task<Base> UpdateRole(RoleModel model);
        Task<Base> DeleteRole(string id);
        Task<IdentityRole> FindRoleById(string roleId);
        Task<RoleModel> FindUserwithClaimsForRole(string roleName);
        Task<Base> UpdateUserwithClaimsForRole(RoleModel model, IdentityRole role);
        #endregion
        #region User Branch Allocation
        Task<BranchAllocationModel> GetAllUserAndBranch();
        Task<Base> CreateBranchAlloction(UserBranchModel data);
        Task<Base> UpdateBranchAlloction(UserBranchModel data);
        Task<Base> DeleteBranchAlloction(Guid Id);
        #endregion
        #region Product Configuration
        Task<ProductViewModel> GetAllRawMaterial(Guid ProductTypeId);
        Task<ProductViewModel> GetAllFinishedGood(Guid ProductTypeId);
        Task<ProductViewModel> GetProductUnit(Guid ProductId);
        Task<ProductionViewModel> GetProductionConfig();
        Task<Base> CreateProductConfig(ProductConfigDataRequest requestData);
        Task<Base> UpdateProductConfig(ProductionModel data);
        Task<Base> DeleteProductConfig(Guid Id);
        #endregion
        #region Account Config
        #region LedgerGroup
        Task<LedgerGroupViewModel> GetLedgerGroups();
        #endregion
        #region LedgerSubGroup
        Task<LedgerSubGroupViewModel> GetLedgerSubGroups(Guid GroupId);
        Task<Base> CreateLedgerSubGroup(LedgerSubGroupModel data);
        Task<Base> UpdateLedgerSubGroup(LedgerSubGroupModel data);
        Task<Base> DeleteLedgerSubGroup(Guid Id);
        #endregion
        #region Ledger
        Task<LedgerViewModel> GetLedgers();
        Task<LedgerViewModel> GetLedgersHasSubLedger();
        Task<LedgerViewModel> GetLedgersHasNoSubLedger();
        Task<Base> CreateLedger(LedgerViewModel listData);
        Task<Base> UpdateLedger(LedgerModel data);
        Task<Base> DeleteLedger(Guid Id);

        #endregion
        
        #endregion
    }
}
