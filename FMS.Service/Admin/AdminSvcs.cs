using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Repository.Admin;
using FMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FMS.Service.Admin
{
    public class AdminSvcs : IAdminSvcs
    {
        private readonly IAdminRepo _adminRepo;
        public AdminSvcs(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }
        #region Generate SignUp Token
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
        #endregion
        #region Company Details
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
        public async Task<Base> UpdateCompany(CompanyDetailsModel model)
        {
            var result = await _adminRepo.UpdateCompany(model);
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
        public async Task<CompanyDetailsViewModel> GetCompany()
        {
            CompanyDetailsViewModel Obj;
            var result = await _adminRepo.GetCompany();
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        GetCompany = result.SingleObjData,
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
        #region Product Setup
        #region Product Type
        public async Task<ProductTypeViewModel> GetProductTypes()
        {
            ProductTypeViewModel Obj;
            var Result = await _adminRepo.GetProductTypes();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        ProductTypes = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        #endregion
        #region Group
        public async Task<GroupViewModel> GetAllGroups()
        {
            GroupViewModel Obj;
            var Result = await _adminRepo.GetAllGroups();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Groups = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<GroupViewModel> GetAllGroups(Guid ProdutTypeId)
        {
            GroupViewModel Obj;
            var Result = await _adminRepo.GetAllGroups(ProdutTypeId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Groups = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateGroup(GroupModel data)
        {
            Base Obj;
            var Result = await _adminRepo.CreateGroup(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Existe"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateGroup(GroupModel data)
        {
            var Result = await _adminRepo.UpdateGroup(data);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteGroup(Guid Id)
        {
            var Result = await _adminRepo.DeleteGroup(Id, null, false);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        #endregion
        #region SubGroup
        public async Task<SubGroupViewModel> GetSubGroups(Guid GroupId)
        {
            SubGroupViewModel Obj;
            var Result = await _adminRepo.GetSubGroups(GroupId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        SubGroups = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateSubGroup(SubGroupModel data)
        {
            Base Obj;
            var Result = await _adminRepo.CreateSubGroup(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateSubGroup(SubGroupModel data)
        {
            var Result = await _adminRepo.UpdateSubGroup(data);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteSubGroup(Guid Id)
        {
            var Result = await _adminRepo.DeleteSubGroup(Id, null, false);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        #endregion
        #region Unit
        public async Task<UnitViewModel> GetAllUnits()
        {
            UnitViewModel Obj;
            var Result = await _adminRepo.GetAllUnits();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Units = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateUnit(UnitModel data)
        {
            Base Obj;
            var Result = await _adminRepo.CreateUnit(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateUnit(UnitModel data)
        {
            var Result = await _adminRepo.UpdateUnit(data);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteUnit(Guid Id)
        {
            var Result = await _adminRepo.DeleteUnit(Id, null, false);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data\r\n"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        #endregion
        #region Product
        public async Task<ProductViewModel> GetAllProducts()
        {
            ProductViewModel Obj;
            var Result = await _adminRepo.GetAllProducts();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        products = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<ProductViewModel> GetProductById(Guid ProductId)
        {
            ProductViewModel Obj;
            var Result = await _adminRepo.GetProductById(ProductId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        product = Result.SingleObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<ProductViewModel> GetProductByTypeId(Guid ProductTypeId)
        {
            ProductViewModel Obj;
            var Result = await _adminRepo.GetProductByTypeId(ProductTypeId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        products = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<ProductViewModel> GetProductGstWithRate(Guid id)
        {
            ProductViewModel Obj;
            var Result = await _adminRepo.GetProductGstWithRate(id);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        product = Result.SingleObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateProduct(ProductModel data)
        {
            Base Obj;
            var Result = await _adminRepo.CreateProduct(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateProduct(ProductModel data)
        {
            var Result = await _adminRepo.UpdateProduct(data);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteProduct(Guid Id)
        {
            var Result = await _adminRepo.DeleteProduct(Id, null, false);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }

            return Obj;
        }
        #endregion
        #endregion
        #region Alternate Unit
        public async Task<AlternateUnitViewModel> GetAlternateUnits()
        {
            AlternateUnitViewModel Obj;
            var Result = await _adminRepo.GetAlternateUnits();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        AlternateUnits = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<AlternateUnitViewModel> GetAlternateUnitByProductId(Guid ProductId)
        {
            AlternateUnitViewModel Obj;
            var Result = await _adminRepo.GetAlternateUnitByProductId(ProductId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        AlternateUnits = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateAlternateUnit(AlternateUnitModel data)
        {
            Base Obj;
            var Result = await _adminRepo.CreateAlternateUnit(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateAlternateUnit(AlternateUnitModel data)
        {
            var Result = await _adminRepo.UpdateAlternateUnit(data);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteAlternateUnit(Guid Id)
        {
            var Result = await _adminRepo.DeleteAlternateUnit(Id);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data\r\n"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
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
        #region Labour Rate Master
        public async Task<LabourRateViewModel> GetAllLabourRates()
        {
            LabourRateViewModel Obj;
            var Result = await _adminRepo.GetAllLabourRates();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        LabourRates = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<LabourRateViewModel> GetLabourRateByProductId(Guid ProductId)
        {
            LabourRateViewModel obj;
            var Result = await _adminRepo.GetLabourRateByProductId(ProductId);
            obj=new ()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                LabourRate = Result.SingleObjData,
            };
            return obj;
        }
        public async Task<Base> CreateLabourRate(LabourRateModel data)
        {
            Base Obj;
            var Result = await _adminRepo.CreateLabourRate(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateLabourRate(LabourRateModel data)
        {
            var Result = await _adminRepo.UpdateLabourRate(data);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteLabourRate(Guid Id)
        {
            var Result = await _adminRepo.DeleteLabourRate(Id, null, false);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
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
