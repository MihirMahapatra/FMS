using FMS.Db.DbEntity;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Service.Admin;
using FMS.Service.Devloper;
using FMS.Service.Master;
using FMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FMS.Controllers.Admin
{
    [Authorize(Roles = "Admin,Devloper")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly UserManager<AppUser> UserManager;
        private readonly IAdminSvcs _adminSvcs;
        private readonly IMasterSvcs _masterSvcs;
        private readonly IDevloperSvcs _devloperSvcs;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IConfiguration _configuration;
        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IAdminSvcs adminSvcs, IMasterSvcs masterSvcs, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IDevloperSvcs devloperSvcs) : base()
        {
            RoleManager = roleManager;
            UserManager = userManager;
            _adminSvcs = adminSvcs;
            _masterSvcs = masterSvcs;
            _devloperSvcs = devloperSvcs;
            _HttpContextAccessor = httpContextAccessor;
            _configuration = configuration;

        }
        #region Database
        [HttpGet]
        public IActionResult DataBaseBackup()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpPost]
        public IActionResult CreateDataBaseBackup()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DBCS");
                string databaseName = "Fms_Db_bhuasuni";
                string backupPath = Path.Combine(Environment.CurrentDirectory, "Backups");
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }
                string backupFileName = $"{databaseName}_Backup_{DateTime.Now:yyyyMMddHHmmss}.bak";
                string backupFilePath = Path.Combine(backupPath, backupFileName);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"BACKUP DATABASE [{databaseName}] TO DISK = '{backupFilePath}'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                ViewBag.Message = $"Backup created successfully at {backupFilePath}";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error creating backup: {ex.Message}";
            }
            return View("DatabaseBackup");
        }
        #endregion
        #region CompaniDetails
        [HttpGet]
        public IActionResult CompanyInfo()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyDetailsModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.CreateCompany(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCompany()
        {
            var result = await _adminSvcs.GetCompany();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBranch()
        {
            var result = await _devloperSvcs.GetAllBranch();
            return new JsonResult(result);
        }
        #endregion
        #region Generate SignUp Token
        #region Token
        [HttpGet]
        public IActionResult CreateToken()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateToken(string Token)
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            var IsCreatedSuccessFully = await _adminSvcs.CreateToken(Token);

            if (IsCreatedSuccessFully.ResponseCode == 201)
            {
                TempData["SuccessMsg"] = IsCreatedSuccessFully.SuccessMsg;
            }
            else
            {
                TempData["ErrorMsg"] = IsCreatedSuccessFully.ErrorMsg;
            }

            return View();
        }
        #endregion
        #endregion
        #region Role & Claims   
        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            var roles = await _adminSvcs.UserRoles();
            return View(roles);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.CreateRole(model);
                if (result.ResponseCode == 201)
                {
                    return RedirectToAction("ListRoles", "Admin", new { SuccessMsg = result.SuccessMsg.ToString() });
                }
                else
                {
                    return RedirectToAction("ListRoles", "Admin", new { ErrorMsg = result.ErrorMsg.ToString() });
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> EditRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.UpdateRole(model);

                if (result.ResponseCode == 200)
                {
                    return RedirectToAction("ListRoles", "Admin", new { SuccessMsg = result.SuccessMsg.ToString() });
                }
                else
                {
                    return RedirectToAction("ListRoles", "Admin", new { ErrorMsg = result.ErrorMsg.ToString() });
                }
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpGet, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _adminSvcs.DeleteRole(id);

            if (result.ResponseCode == 200)
            {
                return RedirectToAction("ListRoles", "Admin", new { SuccessMsg = result.Message });
            }
            else
            {
                return RedirectToAction("ListRoles", "Admin", new { ErrorMsg = result.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ManageUsersInRole(string roleId)
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            TempData["roleId"] = roleId;
            var Rolename = await _adminSvcs.FindRoleById(roleId);
            TempData["Rolename"] = Rolename;
            if (Rolename.Name != null)
            {
                // Get User and Claims 
                var getUserWithClaims = await _adminSvcs.FindUserwithClaimsForRole(Rolename.Name);
                return View(getUserWithClaims);
            }
            else
            {
                return RedirectToAction("ListRoles", "Admin", new { ErrorMsg = "Some Error Occured" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ManageUsersInRole(RoleModel model)
        {
        
                var Rolename = await _adminSvcs.FindRoleById(model.Id);
                if (Rolename != null)
                {
                    var UpdateUserWithClaims = await _adminSvcs.UpdateUserwithClaimsForRole(model, Rolename);
                    if (UpdateUserWithClaims.ResponseCode == 200)
                    {
                        return RedirectToAction("ListRoles", "Admin", new { SuccessMsg = UpdateUserWithClaims.SuccessMsg });
                    }
                    else
                    {
                        return RedirectToAction("ListRoles", "Admin", new { ErrorMsg = UpdateUserWithClaims.ErrorMsg });
                    }
                }

                return RedirectToAction("ListRoles", "Admin", new { ErrorMsg = "Some Error Occoured" });
            
         
        }
        #endregion
        #region Allocate Branch
        [HttpGet]
        public IActionResult AllocateBranch()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUserAndBranch()
        {
            var result = await _adminSvcs.GetAllUserAndBranch();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateBranchAlloction([FromBody] UserBranchModel data)
        {
            var result = await _adminSvcs.CreateBranchAlloction(data);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateBranchAlloction([FromBody] UserBranchModel data)
        {
            var result = await _adminSvcs.UpdateBranchAlloction(data);
            return RedirectToAction("AllocateBranch", "Admin", new { successMsg = result.SuccessMsg, ErrorMsg = result.ErrorMsg });
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteBranchAlloction(string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _adminSvcs.DeleteBranchAlloction(Id);
            return new JsonResult(result);
        }
        #endregion
        #region Product Configuration
        [HttpGet]
        public IActionResult ProductConfig()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetRawMaterials()
        {
            Guid ProductType = MappingProductType.RawMaterial;
            var result = await _adminSvcs.GetAllRawMaterial(ProductType);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetFinishedGoods()
        {
            Guid ProductType = MappingProductType.FinishedGood;
            var result = await _adminSvcs.GetAllFinishedGood(ProductType);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductUnit(Guid ProductId)
        {
            var result = await _adminSvcs.GetProductUnit(ProductId);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductionConfig()
        {
            var result = await _adminSvcs.GetProductionConfig();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateProductConfig([FromBody] ProductConfigDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.CreateProductConfig(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateProductConfig([FromBody] ProductionModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.UpdateProductConfig(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteProductConfig([FromQuery] string Id)
        {
            Guid id = Guid.Parse(Id);
            var result = await _adminSvcs.DeleteProductConfig(id);
            return new JsonResult(result);
        }
        #endregion
        #region Account Configuration
        [HttpGet]
        public IActionResult AccountConfig()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        #region LedgerGroup
        [HttpGet]
        public async Task<IActionResult> GetLedgerGroups()
        {
            var result = await _adminSvcs.GetLedgerGroups();
            return new JsonResult(result);
        }
        #endregion
        #region LedgerSubGroup
        [HttpGet]
        public async Task<IActionResult> GetLedgerSubGroups(Guid GroupId)
        {
            var result = await _adminSvcs.GetLedgerSubGroups(GroupId);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateLedgerSubGroup([FromBody] LedgerSubGroupModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.CreateLedgerSubGroup(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateLedgerSubGroup([FromBody] LedgerSubGroupModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.UpdateLedgerSubGroup(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteLedgerSubGroup([FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _adminSvcs.DeleteLedgerSubGroup(Id);
            return new JsonResult(result);
        }
        #endregion
        #region Ledger
        [HttpGet]
        public IActionResult Ledger()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateLedgers([FromBody] LedgerDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                LedgerViewModel model = new();
                foreach (var item in requestData.RowData)
                {
                    LedgerModel data = new()
                    {
                        Fk_LedgerGroupId = Guid.Parse(requestData.LedgerGroupId),
                        Fk_LedgerSubGroupId = !string.IsNullOrEmpty(requestData.LedgerSubGroupId) ? Guid.Parse(requestData.LedgerSubGroupId) : null,
                        LedgerType = item[0],
                        LedgerName = item[1],
                        HasSubLedger = item[2]
                    };
                    model.Ledgers.Add(data);
                }
                var result = await _adminSvcs.CreateLedger(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetLedgers()
        {
            var result = await _adminSvcs.GetLedgers();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateLedger([FromBody] LedgerModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.UpdateLedger(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteLedger([FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _adminSvcs.DeleteLedger(Id);
            return new JsonResult(result);
        }
        #endregion
        #endregion
    }
}
