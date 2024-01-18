using AutoMapper;
using FMS.Api.Email.EmailService;
using FMS.Db.Context;
using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Security.Claims;

namespace FMS.Repository.Admin
{
    public class AdminRepo : IAdminRepo
    {
        #region Dependancy
        private readonly ILogger<AdminRepo> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        #endregion

        public AdminRepo(ILogger<AdminRepo> logger, IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IEmailService emailService, IMapper mapper)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _HttpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _emailService = emailService;
            _mapper = mapper;
        }
        #region Generate SignUp Token
        public async Task<Result<bool>> CreateToken(string token)
        {
            Result<bool> _Result = new();

            try
            {
                _Result.IsSuccess = false;
                var regToken = new RegisterToken
                {
                    TokenValue = token
                };
                var isExistToken = await (from s in _appDbContext.RegisterTokens where s.TokenValue == token select s).FirstOrDefaultAsync();
                if (isExistToken == null)
                {
                    await _appDbContext.RegisterTokens.AddAsync(regToken);
                    _Result.Count = await _appDbContext.SaveChangesAsync();
                    if (_Result.Count > 0)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        #endregion
        #region Role & Claims  Repo
        public async Task<Result<IdentityRole>> UserRoles()
        {
            Result<IdentityRole> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var roles = await _roleManager.Roles.ToListAsync();
                if (roles != null)
                {
                    _Result.CollectionObjData = roles;
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateRole(RoleModel model)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                bool ChkRoleExist = await _roleManager.RoleExistsAsync(model.RoleName);
                if (!ChkRoleExist)
                {
                    IdentityRole identityRole = new()
                    {
                        Name = model.RoleName
                    };
                    IdentityResult result = await _roleManager.CreateAsync(identityRole);
                    if (result.Succeeded)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateRole(RoleModel model)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role != null)
                {
                    role.Name = model.RoleName;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                    }
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteRole(string id, IDbContextTransaction transaction)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (id != null)
                    {
                        var findrole = await _roleManager.FindByIdAsync(id);
                        if (findrole != null)
                        {
                            var delterole = await _roleManager.DeleteAsync(findrole);

                            if (delterole.Succeeded)
                            {
                                _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted);
                            }
                        }
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/DeleteRole : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<IdentityRole>> FindRoleById(string roleId)
        {
            Result<IdentityRole> _Result = new();

            try
            {
                _Result.IsSuccess = false;
                var Role = await _roleManager.FindByIdAsync(roleId);
                if (Role != null)
                {
                    _Result.SingleObjData = Role;
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<RoleModel>> FindUserwithClaimsForRole(string roleName)
        {
            Result<RoleModel> _Result = new();

            try
            {
                _Result.IsSuccess = false;

                var roleModel = new RoleModel();

                foreach (var item in _userManager.Users)
                {
                    var userRole = new UserRoleModel
                    {
                        UserId = item.Id,

                        UserName = item.UserName
                    };

                    if (await _userManager.IsInRoleAsync(item, roleName))
                    {
                        userRole.IsRoleSelected = true;
                    }
                    else
                    {
                        userRole.IsRoleSelected = false;
                    }

                    /*Write Code For Claims Here*/

                    var User = await _userManager.FindByIdAsync(item.Id);

                    if (User != null)
                    {
                        var ExistingUserClaims = await _userManager.GetClaimsAsync(User);

                        foreach (Claim claim in ClaimsStoreModel.AllClaims)
                        {
                            UserClaimModel userClaim = new()
                            {
                                UserId = item.Id,

                                ClaimType = claim.Type
                            };

                            if (ExistingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                            {
                                userClaim.IsClaimSelected = true;
                            }

                            userRole.Cliams.Add(userClaim);
                        }
                    }
                    roleModel.Users.Add(userRole);
                }
                _Result.SingleObjData = roleModel;
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateUserwithClaimsForRole(RoleModel model, IdentityRole role)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                for (int i = 0; i < model.Users.Count; i++)
                {
                    var user = await _userManager.FindByIdAsync(model.Users[i].UserId);
                    var claims = await _userManager.GetClaimsAsync(user);
                    if (user != null)
                    {
                        if (model.Users[i].IsRoleSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                        {
                            await _userManager.RemoveClaimsAsync(user, claims);
                            await _userManager.AddClaimsAsync(user, model.Users[i].Cliams.Select(c => new Claim(c.ClaimType, c.IsClaimSelected ? "true" : "false")));
                        }
                        else if (model.Users[i].IsRoleSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                        {
                            await _userManager.AddToRoleAsync(user, role.Name);
                            await _userManager.RemoveClaimsAsync(user, claims);
                            await _userManager.AddClaimsAsync(user, model.Users[i].Cliams.Select(c => new Claim(c.ClaimType, c.IsClaimSelected ? "true" : "false")));
                        }
                        else if (!model.Users[i].IsRoleSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                        {
                            await _userManager.RemoveClaimsAsync(user, claims);
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }

            return _Result;
        }
        #endregion
        #region Company Info
        public async Task<Result<bool>> CreateCompany(CompanyModel data)
        {
            Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var newCompanyDetails = new Company
                {
                    State = data.State,
                    Name = data.Name,
                    Adress = data.Adress,
                    GSTIN = data.GSTIN,
                    Email = data.Email,
                    Phone = data.Phone,
                    Logo = data.logo,
                    Fk_BranchId = BranchId
                };
                await _appDbContext.Companies.AddAsync(newCompanyDetails);
                int count = await _appDbContext.SaveChangesAsync();
                _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);

                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateCreateCompany : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateCompany(CompanyModel model)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var companyId = Guid.Parse(model.CompanyId);
                var Query = await _appDbContext.Companies.Where(s => s.CompanyId == companyId).FirstOrDefaultAsync();
                if (Query != null)
                {
                    Query.Name = model.Name;
                    Query.Adress = model.Adress;
                    Query.Email = model.Email;
                    Query.GSTIN = model.GSTIN;
                    Query.Phone = model.Phone;
                    Query.State = model.State;
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<CompanyModel>> GetCompany()
        {
            Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            Result<CompanyModel> _Result = new();
            try
            {
                var Query = await _appDbContext.Companies.Where(s => s.Fk_BranchId == BranchId).Select(s => new CompanyModel
                {
                    Name = s.Name,
                    GSTIN = s.GSTIN,
                    Adress = s.Adress,
                    Email = s.Email,
                    Phone = s.Phone,
                    State = s.State,
                    logo = s.Logo,
                    CompanyId = Convert.ToString(s.CompanyId),
                    BranchName = branchName
                }).SingleOrDefaultAsync();
                if (Query != null)
                {
                    _Result.SingleObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        #endregion
        #region Allocate Branch
        public async Task<Result<BranchAllocationModel>> GetAllUserAndBranch()
        {
            Result<BranchAllocationModel> _Result = new();

            try
            {
                _Result.IsSuccess = false;
                var users = await (from s in _appDbContext.AppUsers
                                   join ub in _appDbContext.UserBranches
                                   on s.Id equals ub.UserId
                                  into BranchUser
                                   where !BranchUser.Any(ub => ub.UserId == s.Id)
                                   select new UserModel
                                   {
                                       id = s.Id,
                                       UserName = s.Name,
                                   }).ToListAsync();
                var branches = await (from s in _appDbContext.Branches
                                      select new BranchModel
                                      {
                                          BranchId = s.BranchId,
                                          BranchName = s.BranchName,
                                      }).ToListAsync();
                var AllUserAllocatedBranch = await (from s in _appDbContext.UserBranches
                                                    select new UserBranchModel
                                                    {
                                                        Id = s.Id,
                                                        BranchId = s.BranchId,
                                                        Branch = s.Branch != null ? new BranchModel { BranchName = s.Branch.BranchName } : null,
                                                        UserId = s.UserId,
                                                        User = s.User != null ? new UserModel { UserName = s.User.Name } : null
                                                    }).ToListAsync();
                if (users.Count > 0 && branches.Count > 0)
                {
                    BranchAllocationModel model = new()
                    {
                        Branches = branches,
                        Users = users,
                        UserBranch = AllUserAllocatedBranch
                    };
                    _Result.SingleObjData = model;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateBranchAlloction(UserBranchModel model)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.UserBranches where s.UserId == model.UserId /*&& s.BranchId == model.BranchId*/ select s).FirstOrDefaultAsync();
                if (Query == null)
                {
                    var NewUserBranch = new UserBranch()
                    {
                        UserId = model.UserId,
                        BranchId = model.BranchId
                    };
                    await _appDbContext.UserBranches.AddAsync(NewUserBranch);
                    int count = await _appDbContext.SaveChangesAsync();
                    if (count > 0)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateBranchAlloction(UserBranchModel model)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.UserBranches where s.Id == model.Id select s).FirstOrDefaultAsync();
                if (Query != null)
                {
                    Query.BranchId = model.BranchId;
                    Query.UserId = model.UserId;
                    int count = await _appDbContext.SaveChangesAsync();
                    if (count > 0)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteBranchAlloction(Guid Id, IDbContextTransaction transaction)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.UserBranches.FirstOrDefaultAsync(x => x.Id == Id);
                        if (Query != null)
                        {
                            int count = 0;
                            _appDbContext.UserBranches.Remove(Query);
                            count = await _appDbContext.SaveChangesAsync();
                            if (count > 0)
                            {
                                _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted);
                            }
                        }
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/DeleteBranchAlloction : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Product Setup
        #region Product Type
        public async Task<Result<ProductTypeModel>> GetProductTypes()
        {
            Result<ProductTypeModel> _Result = new();
            try
            {
                var Query = await _appDbContext.ProductTypes.Select(s => new ProductTypeModel { ProductTypeId = s.ProductTypeId, Product_Type = s.Product_Type }).ToListAsync();
                if (Query.Count > 0)
                {
                    var ItemTypeList = Query;
                    _Result.CollectionObjData = ItemTypeList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllItemTypes : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Group
        public async Task<Result<ProductGroupModel>> GetAllGroups()
        {
            Result<ProductGroupModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.ProductGroups.Select(s =>
                    new ProductGroupModel
                    {
                        ProductGroupId = s.ProductGroupId,
                        ProductGroupName = s.ProductGroupName,
                    }).ToListAsync();
                if (Query.Count > 0)
                {
                    var GroupList = Query;
                    _Result.CollectionObjData = GroupList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllGroups : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<ProductGroupModel>> GetAllGroups(Guid ProdutTypeId)
        {
            Result<ProductGroupModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.ProductGroups.Where(p => p.Fk_ProductTypeId == ProdutTypeId).Select(s =>
                    new ProductGroupModel
                    {
                        ProductGroupId = s.ProductGroupId,
                        ProductGroupName = s.ProductGroupName,
                    }).ToListAsync();
                if (Query.Count > 0)
                {
                    var GroupList = Query;
                    _Result.CollectionObjData = GroupList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllGroups : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateGroup(Model.CommonModel.ProductGroupModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.ProductGroups.Where(s => s.ProductGroupName == data.ProductGroupName).FirstOrDefaultAsync();
                if (Query == null)
                {
                    var newGroup = _mapper.Map<Db.DbEntity.ProductGroup>(data);
                    await _appDbContext.ProductGroups.AddAsync(newGroup);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateGroup(Model.CommonModel.ProductGroupModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.ProductGroups.Where(s => s.ProductGroupId == data.ProductGroupId).FirstOrDefaultAsync();
                if (Query != null)
                {
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteGroup(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.ProductGroups.FirstOrDefaultAsync(x => x.ProductGroupId == Id);
                        if (Query != null)
                        {
                            _appDbContext.ProductGroups.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteGroup : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region SubGroup
        public async Task<Result<ProductSubGroupModel>> GetSubGroups(Guid GroupId)
        {
            Result<ProductSubGroupModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.SubGroups.Where(s => s.Fk_ProductGroupId == GroupId).
                                   Select(s => new ProductSubGroupModel
                                   {
                                       ProductSubGroupId = s.ProductSubGroupId,
                                       ProductSubGroupName = s.ProductSubGroupName
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetSubGroups : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateSubGroup(ProductSubGroupModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.SubGroups.FirstOrDefaultAsync(s => s.Fk_ProductGroupId == data.Fk_ProductGroupId && s.ProductSubGroupName == data.ProductSubGroupName);
                if (Query == null)
                {
                    var newSubGroup = _mapper.Map<ProductSubGroup>(data);
                    await _appDbContext.SubGroups.AddAsync(newSubGroup);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateSubGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateSubGroup(ProductSubGroupModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.SubGroups.FirstOrDefaultAsync(s => s.ProductSubGroupId == data.ProductSubGroupId);
                if (Query != null)
                {
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateSubGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteSubGroup(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.SubGroups.FirstOrDefaultAsync(x => x.ProductSubGroupId == Id);
                        if (Query != null)
                        {
                            _appDbContext.SubGroups.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteSubGroup : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Unit
        public async Task<Result<UnitModel>> GetAllUnits()
        {
            Result<UnitModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Units.Select(s => new UnitModel
                {
                    UnitId = s.UnitId,
                    UnitName = s.UnitName
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    var UnitList = Query;
                    _Result.CollectionObjData = UnitList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllUnits : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateUnit(UnitModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Units.Where(s => s.UnitName == data.UnitName).FirstOrDefaultAsync();
                if (Query == null)
                {
                    var newUnit = _mapper.Map<Unit>(data);
                    await _appDbContext.Units.AddAsync(newUnit);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateUnit : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateUnit(UnitModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Units.Where(s => s.UnitId == data.UnitId).FirstOrDefaultAsync();
                if (Query != null)
                {
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateUnit : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteUnit(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.Units.FirstOrDefaultAsync(x => x.UnitId == Id);
                        if (Query != null)
                        {
                            _appDbContext.Units.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteUnit : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Product
        public async Task<Result<ProductModel>> GetAllProducts()
        {
            Result<ProductModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Products.Select(s =>
                                   new ProductModel()
                                   {
                                       ProductId = s.ProductId,
                                       ProductName = s.ProductName,
                                       Price = s.Price,
                                       WholeSalePrice = s.WholeSalePrice,
                                       GST = s.GST,
                                       ProductGroup = s.ProductGroup != null ? new ProductGroupModel { ProductGroupName = s.ProductGroup.ProductGroupName } : null,
                                       ProductSubGroup = s.ProductSubGroup != null ? new ProductSubGroupModel { ProductSubGroupName = s.ProductSubGroup.ProductSubGroupName } : null,
                                       Unit = s.Unit != null ? new UnitModel { UnitName = s.Unit.UnitName } : null,
                                       ProductType = s.ProductType != null ? new ProductTypeModel { Product_Type = s.ProductType.Product_Type } : null,
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    var ProductList = Query;
                    _Result.CollectionObjData = ProductList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllProducts : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<ProductModel>> GetProductById(Guid ProductId)
        {
            Result<ProductModel> _Result = new();
            try
            {
                var Query = await _appDbContext.Products.Where(s => s.ProductId == ProductId).Select(s => new ProductModel
                {
                    ProductId = s.ProductId,
                    ProductName = s.ProductName,
                    Unit = s.Unit != null ? new UnitModel() { UnitName = s.Unit.UnitName, UnitId = s.Unit.UnitId } : null,
                }).SingleOrDefaultAsync();
                if (Query != null)
                {
                    _Result.SingleObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetProductByTypeId : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<ProductModel>> GetProductByTypeId(Guid ProductTypeId)
        {
            Result<ProductModel> _Result = new();
            try
            {
                var Query = await _appDbContext.Products.Where(s => s.Fk_ProductTypeId == ProductTypeId).Select(s => new ProductModel { ProductId = s.ProductId, ProductName = s.ProductName }).ToListAsync();
                if (Query.Count > 0)
                {
                    var ItemTypeList = Query;
                    _Result.CollectionObjData = ItemTypeList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetProductByTypeId : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<ProductModel>> GetProductGstWithRate(Guid id, string RateType)
        {
            Result<ProductModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var product = await _appDbContext.Products.Where(s => s.ProductId == id)
                    .Select(s => new ProductModel
                    {
                        Price = RateType == "wholesalerate" ? Convert.ToDecimal(s.WholeSalePrice) : s.Price,
                        GST = s.GST,
                        ProductName = s.ProductName,
                        Unit = s.Unit != null ? new UnitModel() { UnitName = s.Unit.UnitName } : null
                    }).SingleOrDefaultAsync();

                if (product != null)
                {
                    _Result.SingleObjData = product;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }

                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Exception", $"GetProductDetailsSelected : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateProduct(ProductModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Products.Where(s => s.Fk_ProductGroupId == data.Fk_ProductGroupId && s.Fk_ProductTypeId == data.Fk_ProductTypeId && s.ProductName == data.ProductName).FirstOrDefaultAsync();
                if (Query == null)
                {
                    var newProduct = _mapper.Map<Product>(data);
                    await _appDbContext.Products.AddAsync(newProduct);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateProduct : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateProduct(ProductModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Products.Where(s => s.ProductId == data.ProductId).FirstOrDefaultAsync();
                if (Query != null)
                {
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateProduct : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteProduct(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    var Query = await _appDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == Id);
                    if (Query != null)
                    {
                        _appDbContext.Products.Remove(Query);
                        int count = await _appDbContext.SaveChangesAsync();
                        _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                    }
                    _Result.IsSuccess = true;
                    if (IsCallBack == false) localTransaction.Commit();
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteProduct : {_Exception.Message}");
            }

            return _Result;
        }
        #endregion
        #endregion
        #region Alternate Unit
        public async Task<Result<AlternateUnitModel>> GetAlternateUnits()
        {
            Result<AlternateUnitModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.AlternateUnits.Select(s => new AlternateUnitModel
                {
                    AlternateUnitId = s.AlternateUnitId,
                    Unit = s.Unit != null ? new UnitModel() { UnitName = s.Unit.UnitName } : null,
                    UnitQuantity = s.UnitQuantity,
                    Product = s.Product != null ? new ProductModel() { ProductName = s.Product.ProductName } : null,
                    AlternateQuantity = s.AlternateQuantity,
                    AlternateUnitName = s.AlternateUnitName,
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllUnits : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<AlternateUnitModel>> GetAlternateUnitByProductId(Guid ProductId)
        {
            Result<AlternateUnitModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == ProductId).Select(s => new AlternateUnitModel
                {
                    AlternateUnitId = s.AlternateUnitId,
                    AlternateUnitName = s.AlternateUnitName,
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllUnits : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<AlternateUnitModel>> GetAlternateUnitByAlternateUnitId(Guid AlternateUnitId)
        {
            Result<AlternateUnitModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.AlternateUnits.Where(s => s.AlternateUnitId == AlternateUnitId).Select(s => new AlternateUnitModel
                {
                    Unit = new UnitModel { UnitName = s.Unit.UnitName },
                    UnitQuantity = s.UnitQuantity,
                }).SingleOrDefaultAsync();
                if (Query != null)
                {
                    _Result.SingleObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllUnits : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateAlternateUnit(AlternateUnitModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var newAlternateUnit = _mapper.Map<AlternateUnit>(data);
                await _appDbContext.AlternateUnits.AddAsync(newAlternateUnit);
                int count = await _appDbContext.SaveChangesAsync();
                _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateUnit : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateAlternateUnit(AlternateUnitModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.AlternateUnits.Where(s => s.AlternateUnitId == data.AlternateUnitId).FirstOrDefaultAsync();
                if (Query != null)
                {
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateUnit : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteAlternateUnit(Guid Id)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.AlternateUnits.FirstOrDefaultAsync(x => x.AlternateUnitId == Id);
                        if (Query != null)
                        {
                            _appDbContext.AlternateUnits.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteUnit : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Production Configuration
        public async Task<Result<ProductModel>> GetAllRawMaterial(Guid ProductTypeId)
        {
            Result<ProductModel> _Result = new();
            try
            {
                var Query = await _appDbContext.Products.Where(s => s.Fk_ProductTypeId == ProductTypeId).Select(s => new ProductModel { ProductId = s.ProductId, ProductName = s.ProductName }).ToListAsync();
                if (Query.Count > 0)
                {
                    var ItemTypeList = Query;
                    _Result.CollectionObjData = ItemTypeList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;

            }
            return _Result;
        }
        public async Task<Result<ProductModel>> GetProductUnit(Guid ProductId)
        {
            Result<ProductModel> _Result = new();
            try
            {
                var Query = await _appDbContext.Products.Where(s => s.ProductId == ProductId).Select(s => new ProductModel
                {
                    Unit = s.Unit != null ? new UnitModel { UnitName = s.Unit.UnitName } : null

                }).SingleOrDefaultAsync();

                if (Query != null)
                {
                    var unit = Query;
                    _Result.SingleObjData = unit;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;

            }
            return _Result;
        }
        public async Task<Result<ProductModel>> GetAllFinishedGood(Guid ProductTypeId)
        {
            Result<ProductModel> _Result = new();
            try
            {
                var Query = await _appDbContext.Products.Where(s => s.Fk_ProductTypeId == ProductTypeId
               /* && !_appDbContext.Productions.Any(sb => sb.FinishedGoodId == s.ProductId)*/)
                    .Select(s => new ProductModel
                    {
                        ProductId = s.ProductId,
                        ProductName = s.ProductName
                    }).ToListAsync();
                if (Query.Count > 0)
                {
                    var ItemTypeList = Query;
                    _Result.CollectionObjData = ItemTypeList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<ProductionModel>> GetProductionConfig()
        {
            Result<ProductionModel> _Result = new();
            try
            {
                var Query = await _appDbContext.Productions.Select(s => new ProductionModel
                {
                    ProductionId = s.ProductionId,
                    Unit = s.Unit,
                    Quantity = s.Quantity,
                    FinishedGoodName = _appDbContext.Products.Where(p => p.ProductId == s.Fk_FinishedGoodId).Select(s => s.ProductName).SingleOrDefault(),
                    RawMaterialName = _appDbContext.Products.Where(p => p.ProductId == s.Fk_RawMaterialId).Select(s => s.ProductName).SingleOrDefault(),
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    var ProductionList = Query;
                    _Result.CollectionObjData = ProductionList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateProductConfig(ProductConfigDataRequest data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = _appDbContext.Productions.Where(s => s.Fk_FinishedGoodId == Guid.Parse(data.FinishedGoodId)).FirstOrDefaultAsync();
                if (Query.Result == null)
                {
                    List<Production> products = new();
                    foreach (var item in data.RowData)
                    {
                        var AddNewMixProduct = new Production
                        {
                            Fk_FinishedGoodId = Guid.Parse(data.FinishedGoodId),
                            Fk_RawMaterialId = Guid.Parse(item[0]),
                            Quantity = Convert.ToDecimal(item[1]),
                            Unit = item[2].ToString()
                        };
                        products.Add(AddNewMixProduct);
                    }
                    await _appDbContext.Productions.AddRangeAsync(products);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateProductConfig(ProductionModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Productions.Where(s => s.ProductionId == data.ProductionId).FirstOrDefaultAsync();
                if (Query != null)
                {
                    Query.Quantity = data.Quantity;
                    Query.Fk_FinishedGoodId = data.Fk_FinishedGoodId;
                    Query.Fk_RawMaterialId = data.Fk_RawMaterialId;
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteProductConfig(Guid Id, IDbContextTransaction transaction)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.Productions.FirstOrDefaultAsync(x => x.ProductionId == Id);
                        if (Query != null)
                        {
                            _appDbContext.Productions.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/DeleteProductConfig : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Labour Rate Configuration
        public async Task<Result<LabourRateModel>> GetAllLabourRates()
        {
            Result<LabourRateModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.LabourRates.Where(s => s.Fk_FinancialYearId == FinancialYear)
                                   .Select(lr => new LabourRateModel
                                   {
                                       LabourRateId = lr.LabourRateId,
                                       Date = lr.Date,
                                       ProductType = lr.ProductType != null ? new ProductTypeModel { Product_Type = lr.ProductType.Product_Type } : null,
                                       Product = lr.Product != null ? new ProductModel { ProductName = lr.Product.ProductName } : null,
                                       Rate = lr.Rate
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllLabourRates : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LabourRateModel>> GetProductionLabourRates(Guid ProductTypeId)
        {
            Result<LabourRateModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.LabourRates.Where(s => s.Fk_FinancialYearId == FinancialYear && s.Fk_ProductTypeId == ProductTypeId)
                                   .Select(lr => new LabourRateModel
                                   {
                                       LabourRateId = lr.LabourRateId,
                                       Date = lr.Date,
                                       ProductType = lr.ProductType != null ? new ProductTypeModel { Product_Type = lr.ProductType.Product_Type } : null,
                                       Product = lr.Product != null ? new ProductModel { ProductName = lr.Product.ProductName } : null,
                                       Rate = lr.Rate
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllLabourRates : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LabourRateModel>> GetServiceLabourRates(Guid ProductTypeId)
        {
            Result<LabourRateModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var Query = await _appDbContext.LabourRates.Where(s => s.Fk_FinancialYearId == FinancialYear && s.Fk_ProductTypeId == ProductTypeId && s.Fk_BranchId == BranchId)
                                   .Select(lr => new LabourRateModel
                                   {
                                       LabourRateId = lr.LabourRateId,
                                       Date = lr.Date,
                                       ProductType = lr.ProductType != null ? new ProductTypeModel { Product_Type = lr.ProductType.Product_Type } : null,
                                       Product = lr.Product != null ? new ProductModel { ProductName = lr.Product.ProductName } : null,
                                       Rate = lr.Rate
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllLabourRates : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LabourRateModel>> GetLabourRateByProductId(Guid ProductId)
        {
            Result<LabourRateModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                Guid ProductType = await _appDbContext.Products.Where(s => s.ProductId == ProductId).Select(s => s.Fk_ProductTypeId).SingleOrDefaultAsync();
                if (ProductType == MappingProductType.ServiceGoods)
                {
                    Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                    _Result.SingleObjData = await _appDbContext.LabourRates.Where(s => s.Fk_ProductId == ProductId && s.Fk_FinancialYearId == FinancialYear && s.Fk_BranchId== BranchId).OrderByDescending(s => s.Date).
                 Select(s => new LabourRateModel
                 {
                     Rate = s.Rate,
                     Product = new ProductModel
                     {
                         Unit = new UnitModel { UnitName = s.Product.Unit.UnitName }
                     }
                 }).FirstOrDefaultAsync();
                }
                else
                {
                    _Result.SingleObjData = await _appDbContext.LabourRates.Where(s => s.Fk_ProductId == ProductId && s.Fk_FinancialYearId == FinancialYear).OrderByDescending(s => s.Date).
                 Select(s => new LabourRateModel
                 {
                     Rate = s.Rate,
                     Product = new ProductModel
                     {
                         Unit = new UnitModel { UnitName = s.Product.Unit.UnitName }
                     }
                 }).FirstOrDefaultAsync();
                }

                if (_Result.SingleObjData != null)
                {
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetLabourRateByProductId : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateLabourRate(LabourRateModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                if (DateTime.TryParseExact(data.FormtedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedDate))
                {
                    data.Fk_ProductTypeId = await _appDbContext.Products.Where(s => s.ProductId == data.Fk_ProductId).Select(s => s.Fk_ProductTypeId).SingleOrDefaultAsync();
                    var newLabourRate = new LabourRate
                    {
                        Date = convertedDate,
                        Fk_ProductTypeId = data.Fk_ProductTypeId,
                        Fk_ProductId = data.Fk_ProductId,
                        Rate = data.Rate,
                        Fk_FinancialYearId = FinancialYear,
                        Fk_BranchId = data.Fk_ProductTypeId == MappingProductType.ServiceGoods ? BranchId : null
                    };
                    await _appDbContext.LabourRates.AddAsync(newLabourRate);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateLabourRate : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateLabourRate(LabourRateModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var Query = await _appDbContext.LabourRates.Where(s => s.LabourRateId == data.LabourRateId).SingleOrDefaultAsync();
                if (Query != null)
                {
                    if (DateTime.TryParseExact(data.FormtedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedDate))
                    {
                        Query.Date = convertedDate;
                        Query.Rate = data.Rate;
                        int count = await _appDbContext.SaveChangesAsync();
                        _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateLabourRate : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteLabourRate(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.LabourRates.FirstOrDefaultAsync(x => x.LabourRateId == Id);
                        if (Query != null)
                        {
                            _appDbContext.LabourRates.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteLabourRate : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Ledger Group & Ledger
        #region LedgerGroup
        public async Task<Result<LedgerGroupModel>> GetLedgerGroups()
        {
            Result<LedgerGroupModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.LedgerGroups.
                                   Select(s => new LedgerGroupModel
                                   {
                                       LedgerGroupId = s.LedgerGroupId,
                                       GroupName = s.GroupName
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/GetLedgerGroups : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region LedgerSubGroup
        public async Task<Result<LedgerSubGroupModel>> GetLedgerSubGroups(Guid GroupId)
        {
            Result<LedgerSubGroupModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<LedgerSubGroupModel> models = new();
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var defaultSubGroups = await _appDbContext.LedgerSubGroupDevs.
                                              Where(s => s.Fk_BranchId == BranchId && s.Fk_LedgerGroupId == GroupId).
                                              Select(s => new LedgerSubGroupModel
                                              {
                                                  LedgerSubGroupId = s.LedgerSubGroupId,
                                                  SubGroupName = s.SubGroupName
                                              }).ToListAsync();
                models.AddRange(defaultSubGroups);
                var UserCreatedSubGroups = await _appDbContext.LedgerSubGroups.
                                                  Where(s => s.Fk_BranchId == BranchId && s.Fk_LedgerGroupId == GroupId)
                                                  .Select(s => new LedgerSubGroupModel
                                                  {
                                                      LedgerSubGroupId = s.LedgerSubGroupId,
                                                      SubGroupName = s.SubGroupName
                                                  }).ToListAsync();
                models.AddRange(UserCreatedSubGroups);
                if (models.Count > 0)
                {
                    _Result.CollectionObjData = models;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/GetLedgerSubGroups : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateLedgerSubGroup(LedgerSubGroupModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var Query = await _appDbContext.LedgerSubGroups.Where(s => s.SubGroupName == data.SubGroupName && s.Fk_LedgerGroupId == data.Fk_LedgerGroupId && s.Fk_BranchId == BranchId).FirstOrDefaultAsync();
                if (Query == null)
                {
                    var newLedgerSubGroup = _mapper.Map<LedgerSubGroup>(data);
                    newLedgerSubGroup.Fk_BranchId = BranchId;
                    await _appDbContext.LedgerSubGroups.AddAsync(newLedgerSubGroup);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/CreateLedgerSubGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateLedgerSubGroup(LedgerSubGroupModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var Query = await _appDbContext.LedgerSubGroups.Where(s => s.LedgerSubGroupId == data.LedgerSubGroupId && s.Fk_BranchId == BranchId).FirstOrDefaultAsync();
                if (Query != null)
                {
                    data.Fk_BranchId = BranchId;
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/UpdateLedgerSubGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteLedgerSubGroup(Guid Id, IDbContextTransaction transaction)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.LedgerSubGroups.FirstOrDefaultAsync(x => x.LedgerSubGroupId == Id && x.Fk_BranchId == BranchId);
                        if (Query != null)
                        {
                            _appDbContext.LedgerSubGroups.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/DeleteLedgerSubGroup : {_Exception.Message}");
            }

            return _Result;
        }
        #endregion
        #region Ledger
        public async Task<Result<LedgerModel>> GetLedgers()
        {
            Result<LedgerModel> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                _Result.IsSuccess = false;
                LedgerViewModel models = new();
                var ledgerDevs = await (from l in _appDbContext.LedgersDev
                                        select new LedgerModel
                                        {
                                            LedgerId = l.LedgerId,
                                            LedgerName = l.LedgerName,
                                            LedgerType = l.LedgerType,
                                            LedgerGroup = l.LedgerGroup != null ? new LedgerGroupModel { GroupName = l.LedgerGroup.GroupName } : null,
                                            LedgerSubGroup = l.LedgerSubGroup != null ? new LedgerSubGroupModel { SubGroupName = l.LedgerSubGroup.SubGroupName } : null
                                        }).ToListAsync();
                models.Ledgers.AddRange(ledgerDevs);
                var ledgers = await (from l in _appDbContext.Ledgers
                                     select new LedgerModel
                                     {
                                         LedgerId = l.LedgerId,
                                         LedgerName = l.LedgerName,
                                         LedgerType = l.LedgerType,
                                         LedgerGroup = l.LedgerGroup != null ? new LedgerGroupModel { GroupName = l.LedgerGroup.GroupName } : null,
                                         LedgerSubGroup = l.LedgerSubGroup != null ? new LedgerSubGroupModel { SubGroupName = l.LedgerSubGroup.SubGroupName } : null
                                     }).ToListAsync();
                models.Ledgers.AddRange(ledgers);

                if (models.Ledgers.Count > 0)
                {
                    _Result.CollectionObjData = models.Ledgers;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $" AdminRepo/GetLedgers : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LedgerModel>> GetLedgersHasSubLedger()
        {
            Result<LedgerModel> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                _Result.IsSuccess = false;
                var ledgers = await (from l in _appDbContext.Ledgers.Where(x => x.HasSubLedger == "yes")
                                     select new LedgerModel
                                     {
                                         LedgerId = l.LedgerId,
                                         LedgerName = l.LedgerName,
                                     }).ToListAsync();

                if (ledgers.Count > 0)
                {
                    _Result.CollectionObjData = ledgers;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $" AdminRepo/GetLedgersHasSubLedger : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LedgerModel>> GetLedgersHasNoSubLedger()
        {
            Result<LedgerModel> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                _Result.IsSuccess = false;
                LedgerViewModel models = new();
                var ledgerDevs = await (from l in _appDbContext.LedgersDev
                                        select new LedgerModel
                                        {
                                            LedgerId = l.LedgerId,
                                            LedgerName = l.LedgerName,
                                        }).ToListAsync();
                models.Ledgers.AddRange(ledgerDevs);
                var ledgers = await (from l in _appDbContext.Ledgers.Where(x => x.HasSubLedger == "no")
                                     select new LedgerModel
                                     {
                                         LedgerId = l.LedgerId,
                                         LedgerName = l.LedgerName,
                                     }).ToListAsync();
                models.Ledgers.AddRange(ledgers);

                if (models.Ledgers.Count > 0)
                {
                    _Result.CollectionObjData = models.Ledgers;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $" AdminRepo/GetLedgersHasSubLedger : {_Exception.Message}");
            }
            return _Result;

        }
        public async Task<Result<bool>> CreateLedger(LedgerViewModel listData)
        {
            Result<bool> _Result = new();
            try
            {
                LedgerViewModel itemsToRemove = new();
                _Result.IsSuccess = false;
                using var transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    foreach (var item in listData.Ledgers)
                    {

                        var Query = await _appDbContext.Ledgers.Where(s => s.Fk_LedgerGroupId == item.Fk_LedgerGroupId && s.LedgerName == item.LedgerName).FirstOrDefaultAsync();
                        if (Query != null)
                        {
                            var ledgerModelToRemove = listData.Ledgers.FirstOrDefault(x => x.Fk_LedgerGroupId == item.Fk_LedgerGroupId && x.LedgerName == item.LedgerName);
                            itemsToRemove.Ledgers.Add(ledgerModelToRemove);
                        }
                    }
                    foreach (var itemToRemove in itemsToRemove.Ledgers)
                    {
                        listData.Ledgers.Remove(itemToRemove);
                    }
                    var ledgers = _mapper.Map<List<Ledger>>(listData.Ledgers);
                    await _appDbContext.Ledgers.AddRangeAsync(ledgers);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                    transaction.Commit();
                    _Result.IsSuccess = true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $" AdminRepo/CreateLedger : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateLedger(LedgerModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var ledger = await _appDbContext.Ledgers.Where(s => s.LedgerId == data.LedgerId).FirstOrDefaultAsync();
                if (ledger != null)
                {
                    // ledger.HasSubLedger = data.HasSubLedger;
                    _mapper.Map(data, ledger);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                else
                {
                    var ledgerDev = await _appDbContext.LedgersDev.Where(s => s.LedgerId == data.LedgerId).FirstOrDefaultAsync();
                    if (ledgerDev != null)
                    {
                        ledgerDev.LedgerType = data.LedgerType;
                        int count = await _appDbContext.SaveChangesAsync();
                        _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $" AdminRepo/UpdateLedger : {_Exception.Message}");
            }

            return _Result;
        }
        public async Task<Result<bool>> DeleteLedger(Guid Id, IDbContextTransaction transaction)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();

                try
                {
                    if (Id != Guid.Empty)
                    {
                        _Result.IsSuccess = false;
                        var Query = await _appDbContext.Ledgers.FirstOrDefaultAsync(x => x.LedgerId == Id);
                        if (Query != null)
                        {
                            _appDbContext.Ledgers.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;

                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/DeleteLedger : {_Exception.Message}");
            }
            return _Result;
        }

        #endregion
        #endregion
    }
}