using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Repository.Admin;
using FMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Service.Admin
{
    public class AdminSvcs : IAdminSvcs
    {
        private readonly IAdminRepo _adminRepo;
        public AdminSvcs(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }
        public async Task<Base> CreateToken(string Token)
        {
            var result = await _adminRepo.CreateToken(Token);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateCompany(CompanyDetailsModel data)
        {
            Base Obj;
            var result = await _adminRepo.CreateCompany(data);
            if (result.IsSuccess)
            {
                if (result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;

        }
        public async Task<CompanyDetailsModel> GetCompany()
        {
            CompanyDetailsModel Obj;
            var result = await _adminRepo.GetCompany();
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        //ResponseStatus = result.Response,
                        //ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        //Productions = result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        //ResponseStatus = result.Response,
                        //ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        //Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    //ResponseStatus = result.Response,
                    //ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                   // Exception = result.Exception,
                    //Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }

        #region Role & Claims
        public async Task<List<IdentityRole>> UserRoles()
        {
            var roles = await _adminRepo.UserRoles();
            return roles.CollectionObjData;
        }
        public async Task<Base> CreateRole(RoleModel model)
        {
            var result = await _adminRepo.CreateRole(model);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Save Data"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateRole(RoleModel model)
        {
            var result = await _adminRepo.UpdateRole(model);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteRole(string id)
        {
            var result = await _adminRepo.DeleteRole(id,null);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<IdentityRole> FindRoleById(string roleId)
        {
            var result = await _adminRepo.FindRoleById(roleId);
            return result.SingleObjData;
        }
        public async Task<RoleModel> FindUserwithClaimsForRole(string roleName)
        {
            var result = await _adminRepo.FindUserwithClaimsForRole(roleName);
            return result.SingleObjData;

        }
        public async Task<Base> UpdateUserwithClaimsForRole(RoleModel model, IdentityRole role)
        {
            var result = await _adminRepo.UpdateUserwithClaimsForRole(model, role);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        #endregion
        #region Allocate Branch
        public async Task<BranchAllocationModel> GetAllUserAndBranch()
        {
            BranchAllocationModel Obj;
            var result = await _adminRepo.GetAllUserAndBranch();

            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Branches = result.SingleObjData.Branches,
                        Users = result.SingleObjData.Users,
                        UserBranch = result.SingleObjData.UserBranch
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }


            return Obj;
        }
        public async Task<Base> CreateBranchAlloction(UserBranchModel data)
        {
            Base Obj;
            var result = await _adminRepo.CreateBranchAlloction(data);

            if (result.IsSuccess)
            {
                if (result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateBranchAlloction(UserBranchModel data)
        {
            var result = await _adminRepo.UpdateBranchAlloction(data);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteBranchAlloction(Guid Id)
        {
            var result = await _adminRepo.DeleteBranchAlloction(Id,null);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        #endregion
        #region Production Configuration
        public async Task<ProductViewModel> GetAllRawMaterial(Guid ProductTypeId)
        {
            ProductViewModel Obj;
            var result = await _adminRepo.GetAllRawMaterial(ProductTypeId);
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        products = result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<ProductViewModel> GetAllFinishedGood(Guid ProductTypeId)
        {
            ProductViewModel Obj;
            var result = await _adminRepo.GetAllFinishedGood(ProductTypeId);
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        products = result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<ProductViewModel> GetProductUnit(Guid ProductId)
        {
            ProductViewModel Obj;
            var result = await _adminRepo.GetProductUnit(ProductId);
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        product = result.SingleObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<ProductionViewModel> GetProductionConfig()
        {
            ProductionViewModel Obj;
            var result = await _adminRepo.GetProductionConfig();
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Productions = result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateProductConfig(ProductConfigDataRequest requestData)
        {
            Base Obj;
            var result = await _adminRepo.CreateProductConfig(requestData);
            if (result.IsSuccess)
            {
                if (result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateProductConfig(ProductionModel data)
        {
            var result = await _adminRepo.UpdateProductConfig(data);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteProductConfig(Guid Id)
        {
            var result = await _adminRepo.DeleteProductConfig(Id, null);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        #endregion
        #region Account Config
        #region LedgerGroup
        public async Task<LedgerGroupViewModel> GetLedgerGroups()
        {
            LedgerGroupViewModel Obj;
            var result = await _adminRepo.GetLedgerGroups();
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        LedgerGroups = result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        #endregion
        #region LedgerSubGroup
        public async Task<LedgerSubGroupViewModel> GetLedgerSubGroups(Guid GroupId)
        {
            LedgerSubGroupViewModel Obj;
            var result = await _adminRepo.GetLedgerSubGroups(GroupId);
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        LedgerSubGroups = result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateLedgerSubGroup(LedgerSubGroupModel data)
        {
            Base Obj;
            var result = await _adminRepo.CreateLedgerSubGroup(data);
            if (result.IsSuccess)
            {
                if (result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateLedgerSubGroup(LedgerSubGroupModel data)
        {
            var result = await _adminRepo.UpdateLedgerSubGroup(data);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteLedgerSubGroup(Guid Id)
        {
            var result = await _adminRepo.DeleteLedgerSubGroup(Id, null);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        #endregion
        #region Ledger
        public async Task<LedgerViewModel> GetLedgers()
        {
            LedgerViewModel Obj;
            var result = await _adminRepo.GetLedgers();
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Ledgers = result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<LedgerViewModel> GetLedgersHasSubLedger()
        {
            LedgerViewModel Obj;
            var result = await _adminRepo.GetLedgersHasSubLedger();
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Ledgers = result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<LedgerViewModel> GetLedgersHasNoSubLedger()
        {
            LedgerViewModel Obj;
            var result = await _adminRepo.GetLedgersHasNoSubLedger();
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Ledgers = result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateLedger(LedgerViewModel listData)
        {
            Base Obj;
            var result = await _adminRepo.CreateLedger(listData);
            if (result.IsSuccess)
            {
                if (result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateLedger(LedgerModel data)
        {
            var result = await _adminRepo.UpdateLedger(data);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteLedger(Guid Id)
        {
            var result = await _adminRepo.DeleteLedger(Id, null);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }

      

        #endregion
        #endregion
    }
}
