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
            return PartialView();
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
        #region Generate SignUp Token
        [HttpGet]
        public IActionResult CreateToken()
        {
            return PartialView();
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

            return PartialView();
        }
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
        #region Company Details
        [HttpGet]
        public IActionResult CompanyInfo()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyModel data)
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
        [HttpPost]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.UpdateCompany(model);
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
        #region Allocate Branch
        [HttpGet]
        public IActionResult AllocateBranch()
        {
            return PartialView();
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
        #region Product Setup
        [HttpGet]
        public IActionResult ProductSetup()
        {
            return PartialView();
        }
        #region Product Type
        [HttpGet]
        public async Task<IActionResult> GetAllProductTypes()
        {
            var result = await _adminSvcs.GetProductTypes();
            return new JsonResult(result);
        }
        #endregion
        #region Group
        [HttpGet]
        public async Task<IActionResult> GetAllGroups(Guid ProdutTypeId)
        {
            var Groups = await _adminSvcs.GetAllGroups(ProdutTypeId);
            return new JsonResult(Groups);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateGroup([FromBody] ProductGroupModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.CreateGroup(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateGroup([FromBody] Model.CommonModel.ProductGroupModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.UpdateGroup(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteGroup([FromQuery] string id)
        {
            Guid GroupId = Guid.Parse(id);
            var result = await _adminSvcs.DeleteGroup(GroupId);
            return new JsonResult(result);
        }
        #endregion
        #region SubGroup
        [HttpGet]
        public async Task<IActionResult> GetSubGroups([FromQuery] Guid Groupid)
        {
            var SubGroups = await _adminSvcs.GetSubGroups(Groupid);
            return new JsonResult(SubGroups);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateSubGroup([FromBody] ProductSubGroupModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.CreateSubGroup(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateSubGroup([FromBody] ProductSubGroupModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.UpdateSubGroup(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteSubGroup([FromQuery] string id)
        {
            Guid SubGroupId = Guid.Parse(id);
            var result = await _adminSvcs.DeleteSubGroup(SubGroupId);
            return new JsonResult(result);
        }
        #endregion
        #region Unit
        [HttpGet]
        public async Task<IActionResult> GetAllUnits()
        {
            var result = await _adminSvcs.GetAllUnits();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateUnit([FromBody] UnitModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.CreateUnit(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateUnit([FromBody] UnitModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.UpdateUnit(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteUnit([FromQuery] string id)
        {
            Guid UnitId = Guid.Parse(id);
            var result = await _adminSvcs.DeleteUnit(UnitId);
            return new JsonResult(result);
        }
        #endregion
        #region Product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _adminSvcs.GetAllProducts();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.CreateProduct(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.UpdateProduct(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteProduct([FromQuery] string id)
        {
            Guid GroupId = Guid.Parse(id);
            var result = await _adminSvcs.DeleteProduct(GroupId);
            return new JsonResult(result);
        }
        #endregion
        #endregion
        #region Alternate Unit
        [HttpGet]
        public IActionResult AlternateUnit()
        {

            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> GetAlternateUnits()
        {
            var result = await _adminSvcs.GetAlternateUnits();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductByTypeId([FromQuery] Guid ProductTypeId)
        {
            var result = await _adminSvcs.GetProductByTypeId(ProductTypeId);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductById(Guid ProductId)
        {
            var result = await _adminSvcs.GetProductById(ProductId);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateAlternateUnit([FromBody] AlternateUnitModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.CreateAlternateUnit(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateAlternateUnit([FromBody] AlternateUnitModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.UpdateAlternateUnit(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteAlternateUnit([FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _adminSvcs.DeleteAlternateUnit(Id);
            return new JsonResult(result);
        }
        #endregion
        #region Production Configuration
        [HttpGet]
        public IActionResult ProductionConfig()
        {

            return PartialView();
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
        public async Task<IActionResult> CreateProductionConfig([FromBody] ProductConfigDataRequest requestData)
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
        public async Task<IActionResult> UpdateProductionConfig([FromBody] ProductionModel data)
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
        public async Task<IActionResult> DeleteProductionConfig([FromQuery] string Id)
        {
            Guid id = Guid.Parse(Id);
            var result = await _adminSvcs.DeleteProductConfig(id);
            return new JsonResult(result);
        }
        #endregion
        #region Labour Rate Configuration
        #region Production Labour Rate Configuration
        [HttpGet]
        public IActionResult ProductionLabourRateConfig()
        {

            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetProductFinishedGoods()
        {
            var result = await _adminSvcs.GetProductByTypeId(MappingProductType.FinishedGood);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductionLabourRates()
        {
            var result = await _adminSvcs.GetProductionLabourRates(MappingProductType.FinishedGood);
            return new JsonResult(result);
        }
        #endregion
        #region Services Labour Rate Configuration
        [HttpGet]
        public IActionResult ServicesLabourRateConfig()
        {

            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetProductServiceGoods()
        {
            var result = await _adminSvcs.GetProductByTypeId(MappingProductType.ServiceGoods);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetServiceLabourRates()
        {
            var result = await _adminSvcs.GetServiceLabourRates(MappingProductType.ServiceGoods);
            return new JsonResult(result);
        }
        #endregion
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateLabourRate([FromBody] LabourRateModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.CreateLabourRate(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateLabourRate([FromBody] LabourRateModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminSvcs.UpdateLabourRate(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteLabourRate([FromQuery] string Id)
        {
            Guid LabourRateId = Guid.Parse(Id);
            var result = await _adminSvcs.DeleteLabourRate(LabourRateId);
            return new JsonResult(result);
        }
        #endregion

        #region Account Configuration
        [HttpGet]
        public IActionResult AccountConfig()
        {

            return PartialView();
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

            return PartialView();
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
