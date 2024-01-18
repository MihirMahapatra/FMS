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
        Task<Base> CreateCompany(CompanyModel data);
        Task<Base> UpdateCompany(CompanyModel model);
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
        Task<Base> CreateGroup(ProductGroupModel data);
        Task<Base> UpdateGroup(ProductGroupModel data);
        Task<Base> DeleteGroup(Guid Id);
        #endregion
        #region SubGroup
        Task<SubGroupViewModel> GetSubGroups(Guid GroupId);
        Task<Base> CreateSubGroup(ProductSubGroupModel data);
        Task<Base> UpdateSubGroup(ProductSubGroupModel data);
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
        Task<ProductViewModel> GetProductGstWithRate(Guid id, string RateType);
        Task<Base> CreateProduct(ProductModel data);
        Task<Base> UpdateProduct(ProductModel data);
        Task<Base> DeleteProduct(Guid Id);
        #endregion
        #endregion
        #region Alternate Unit
        Task<AlternateUnitViewModel> GetAlternateUnits();
        Task<AlternateUnitViewModel> GetAlternateUnitByProductId(Guid ProductId);
        Task<AlternateUnitViewModel> GetAlternateUnitByAlternateUnitId(Guid AlternateUnitId);
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
        #region Labour Rate Configuration
        Task<LabourRateViewModel> GetAllLabourRates();
        Task<LabourRateViewModel> GetProductionLabourRates(Guid ProductTypeId);
        Task<LabourRateViewModel> GetServiceLabourRates(Guid ProductTypeId);
        Task<LabourRateViewModel> GetLabourRateByProductId(Guid ProductId);
        Task<Base> CreateLabourRate(LabourRateModel data);
        Task<Base> UpdateLabourRate(LabourRateModel data);
        Task<Base> DeleteLabourRate(Guid Id);
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
