using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace FMS.Repository.Admin
{
    public interface IAdminRepo
    {
        #region Generate SignUp Token
        Task<Result<bool>> CreateToken(string token);
        #endregion
        #region Company Details
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
        #region Product Setup
        #region Product Type
        Task<Result<ProductTypeModel>> GetProductTypes();
        #endregion
        #region Group
        Task<Result<GroupModel>> GetAllGroups();
        Task<Result<GroupModel>> GetAllGroups(Guid ProdutTypeId);
        Task<Result<bool>> CreateGroup(GroupModel data);
        Task<Result<bool>> UpdateGroup(GroupModel data);
        Task<Result<bool>> DeleteGroup(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region SubGroup
        Task<Result<SubGroupModel>> GetSubGroups(Guid GroupId);
        Task<Result<bool>> CreateSubGroup(SubGroupModel data);
        Task<Result<bool>> UpdateSubGroup(SubGroupModel data);
        Task<Result<bool>> DeleteSubGroup(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region Unit
        Task<Result<UnitModel>> GetAllUnits();
        Task<Result<bool>> CreateUnit(UnitModel data);
        Task<Result<bool>> UpdateUnit(UnitModel data);
        Task<Result<bool>> DeleteUnit(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region Product
        Task<Result<ProductModel>> GetAllProducts();
        public Task<Result<ProductModel>> GetProductById(Guid ProductId);
        public Task<Result<ProductModel>> GetProductByTypeId(Guid ProductTypeId);
        public Task<Result<ProductModel>> GetProductGstWithRate(Guid id);
        Task<Result<bool>> CreateProduct(ProductModel data);
        Task<Result<bool>> UpdateProduct(ProductModel data);
        Task<Result<bool>> DeleteProduct(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #endregion
        #region Alternate Units
        Task<Result<AlternateUnitModel>> GetAlternateUnits();
        Task<Result<bool>> CreateAlternateUnit(AlternateUnitModel data);
        Task<Result<bool>> UpdateAlternateUnit(AlternateUnitModel data);
        Task<Result<bool>> DeleteAlternateUnit(Guid Id);
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
        #region Labour Rate Configration
        Task<Result<LabourRateModel>> GetAllLabourRates();
        Task<Result<decimal>> GetLabourRateByProductId(Guid ProductId);
        Task<Result<bool>> CreateLabourRate(LabourRateModel data);
        Task<Result<bool>> UpdateLabourRate(LabourRateModel data);
        Task<Result<bool>> DeleteLabourRate(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
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
