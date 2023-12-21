using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace FMS.Service.Admin
{
    public interface IAdminSvcs
    {
        #region Generate SignUp Token
        Task<Base> CreateToken(string Token);
        #endregion
        #region Roles and Claims
        Task<List<IdentityRole>> UserRoles();
        Task<Base> CreateRole(RoleModel model);
        Task<Base> UpdateRole(RoleModel model);
        Task<Base> DeleteRole(string id);
        Task<IdentityRole> FindRoleById(string roleId);
        Task<RoleModel> FindUserwithClaimsForRole(string roleName);
        Task<Base> UpdateUserwithClaimsForRole(RoleModel model, IdentityRole role);
        #endregion
        #region Company Details
        Task<Base> CreateCompany(CompanyDetailsModel data);
        Task<CompanyDetailsViewModel> GetCompany();
        #endregion
        #region User Branch Allocation
        Task<BranchAllocationModel> GetAllUserAndBranch();
        Task<Base> CreateBranchAlloction(UserBranchModel data);
        Task<Base> UpdateBranchAlloction(UserBranchModel data);
        Task<Base> DeleteBranchAlloction(Guid Id);
        #endregion
        #region Product Setup
        #region Product Type
        Task<ProductTypeViewModel> GetProductTypes();
        #endregion
        #region Group
        Task<GroupViewModel> GetAllGroups();
        Task<GroupViewModel> GetAllGroups(Guid ProdutTypeId);
        Task<Base> CreateGroup(GroupModel data);
        Task<Base> UpdateGroup(GroupModel data);
        Task<Base> DeleteGroup(Guid Id);
        #endregion
        #region SubGroup
        Task<SubGroupViewModel> GetSubGroups(Guid GroupId);
        Task<Base> CreateSubGroup(SubGroupModel data);
        Task<Base> UpdateSubGroup(SubGroupModel data);
        Task<Base> DeleteSubGroup(Guid Id);
        #endregion
        #region Unit
        Task<UnitViewModel> GetAllUnits();
        Task<Base> CreateUnit(UnitModel data);
        Task<Base> UpdateUnit(UnitModel data);
        Task<Base> DeleteUnit(Guid Id);
        #endregion
        #region Product
        Task<ProductViewModel> GetAllProducts();
        Task<ProductViewModel> GetProductByTypeId(Guid ProductTypeId);
        Task<ProductViewModel> GetProductById(Guid ProductId);
        Task<ProductViewModel> GetProductGstWithRate(Guid id);
        Task<Base> CreateProduct(ProductModel data);
        Task<Base> UpdateProduct(ProductModel data);
        Task<Base> DeleteProduct(Guid Id);
        #endregion
        #endregion
        #region Alternate Unit
        Task<AlternateUnitViewModel> GetAlternateUnits();
        Task<Base> CreateAlternateUnit(AlternateUnitModel data);
        Task<Base> UpdateAlternateUnit(AlternateUnitModel data);
        Task<Base> DeleteAlternateUnit(Guid Id);
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
