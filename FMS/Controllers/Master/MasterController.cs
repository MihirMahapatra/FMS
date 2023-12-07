using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Service.Admin;
using FMS.Service.Devloper;
using FMS.Service.Master;
using FMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FMS.Controllers.Master
{
    [Authorize(Roles = "Devloper,Admin,User")]
    public class MasterController : Controller
    {
        private readonly IMasterSvcs _masterSvcs;
        private readonly IAdminSvcs _adminSvcs;
        private readonly IDevloperSvcs _devloperSvcs;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public MasterController(IMasterSvcs masterSvcs, IDevloperSvcs devloperSvcs, IAdminSvcs adminSvcs, IHttpContextAccessor httpContextAccessor) : base()
        {
            _masterSvcs = masterSvcs;
            _devloperSvcs = devloperSvcs;
            _adminSvcs = adminSvcs;
            _HttpContextAccessor = httpContextAccessor;
        }
        #region Account Master
        [HttpGet]
        public IActionResult AccountMaster()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        #region LedgerBalance
        [HttpGet]
        public async Task<IActionResult> GetLedgersHasNoSubLedger()
        {
            var result = await _adminSvcs.GetLedgersHasNoSubLedger();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetLedgerBalances()
        {
            var result = await _masterSvcs.GetLedgerBalances();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSubLedgersByBranch(Guid LedgerId)
        {
            var result = await _masterSvcs.GetSubLedgersByBranch(LedgerId);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateLedgerBalance([FromBody] LedgerBalanceRequest data)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateLedgerBalance(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateLedgerBalance([FromBody] LedgerBalanceModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateLedgerBalance(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteLedgerBalance([FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _masterSvcs.DeleteLedgerBalance(Id);
            return new JsonResult(result);
        }
        #endregion
        #region SubLedger
        [HttpGet]
        public async Task<IActionResult> GetLedgersHasSubLedger()
        {
            var result = await _adminSvcs.GetLedgersHasSubLedger();
            return new JsonResult(result);
        }
        [HttpGet]
        public IActionResult SubLedger()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateSubLedgers([FromBody] SubLedgerDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateSubLedger(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetSubLedgers()
        {
            var result = await _masterSvcs.GetSubLedgers();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSubLedgersById(Guid LedgerId)
        {
            var result = await _masterSvcs.GetSubLedgersById(LedgerId);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateSubLedger([FromBody] SubLedgerModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateSubLedger(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteSubLedger([FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _masterSvcs.DeleteSubLedger(Id);
            return new JsonResult(result);
        }

        #endregion
        #region SubLedger Balance
        [HttpGet]
        public async Task<IActionResult> GetSubLedgerBalances()
        {
            var result = await _masterSvcs.GetSubLedgerBalances();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateSubLedgerBalance([FromBody] SubLedgerBalanceModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateSubLedgerBalance(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteSubLedgerBalance([FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _masterSvcs.DeleteSubLedgerBalance(Id);
            return new JsonResult(result);
        }
        #endregion
        #endregion
        #region Product Master
        [HttpGet]
        public IActionResult ProductMaster()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        #region Product Type
        [HttpGet]
        public async Task<IActionResult> GetAllProductTypes()
        {
            var result = await _masterSvcs.GetProductTypes();
            return new JsonResult(result);
        }
        #endregion
        #region Group
        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var Groups = await _masterSvcs.GetAllGroups();
            return new JsonResult(Groups);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateGroup([FromBody] GroupModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateGroup(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateGroup([FromBody] GroupModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateGroup(model);
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
            var result = await _masterSvcs.DeleteGroup(GroupId);
            return new JsonResult(result);
        }
        #endregion
        #region SubGroup
        [HttpGet]
        public async Task<IActionResult> GetSubGroups([FromQuery] Guid Groupid)
        {
            var SubGroups = await _masterSvcs.GetSubGroups(Groupid);
            return new JsonResult(SubGroups);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateSubGroup([FromBody] SubGroupModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateSubGroup(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateSubGroup([FromBody] SubGroupModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateSubGroup(model);
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
            var result = await _masterSvcs.DeleteSubGroup(SubGroupId);
            return new JsonResult(result);
        }
        #endregion
        #region Unit
        public async Task<IActionResult> GetAllUnits()
        {
            var result = await _masterSvcs.GetAllUnits();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateUnit([FromBody] UnitModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateUnit(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateUnit([FromBody] UnitModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateUnit(model);
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
            var result = await _masterSvcs.DeleteUnit(UnitId);
            return new JsonResult(result);
        }
        #endregion
        #region Product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _masterSvcs.GetAllProducts();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateProduct(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateProduct(model);
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
            var result = await _masterSvcs.DeleteProduct(GroupId);
            return new JsonResult(result);
        }
        #endregion
        #region  Stock
        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            var result = await _masterSvcs.GetStocks();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsWhichNotInStock()
        {
            var result = await _masterSvcs.GetProductsWhichNotInStock();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateStock([FromBody] StockModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateStock(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateStock([FromBody] StockModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateStock(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteStock([FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _masterSvcs.DeleteStock(Id);
            return new JsonResult(result);
        }
        #endregion
        #endregion       
        #region PartyMaster
        [HttpGet]
        public IActionResult PartyMaster()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        public IActionResult PartyList()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        #region PartyType
        [HttpGet]
        public async Task<IActionResult> GetPartyTypes()
        {
            var result = await _devloperSvcs.GetLedgers();
            var data = result.Ledgers
                        .Where(s => s.LedgerId == MappingLedgers.SundryDebtors || s.LedgerId == MappingLedgers.SundryCreditors)
                        .Select(s => new { s.LedgerId, s.LedgerName })
                        .ToList();
            return new JsonResult(data);
        }
        #endregion
        #region State
        [HttpGet]
        public async Task<IActionResult> GetStates()
        {
            var result = await _masterSvcs.GetStates();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateState([FromBody] StateModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateState(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateState([FromBody] StateModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateState(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteState([FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _masterSvcs.DeleteState(Id);
            return new JsonResult(result);
        }
        #endregion
        #region City
        [HttpGet]
        public async Task<IActionResult> GetCities(string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _masterSvcs.GetCities(Id);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateCity([FromBody] CityModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateCity(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateCity([FromBody] CityModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateCity(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteCity([FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _masterSvcs.DeleteCity(Id);
            return new JsonResult(result);
        }
        #endregion
        #region Party
        [HttpGet]
        public async Task<IActionResult> GetParties()
        {
            var result = await _masterSvcs.GetParties();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateParty([FromBody] PartyModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateParty(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateParty([FromBody] PartyModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateParty(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteParty([FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _masterSvcs.DeleteParty(Id);
            return new JsonResult(result);
        }
        #endregion   
        #endregion
        #region labour Master
        [HttpGet]
        public IActionResult LabourMaster()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        #region Labour Type
        [HttpGet]
        public async Task<IActionResult> GetAllLabourTypes()
        {
            var LabourTypes = await _masterSvcs.GetAllLabourTypes();
            return new JsonResult(LabourTypes);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateLabourType([FromBody] LabourTypeModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateLabourType(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateLabourType([FromBody] LabourTypeModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateLabourType(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteLabourType([FromQuery] string Id)
        {
            Guid LabourTypeId = Guid.Parse(Id);
            var result = await _masterSvcs.DeleteLabourType(LabourTypeId);
            return new JsonResult(result);
        }
        #endregion
        #region Labour Details
        [HttpGet]
        public async Task<IActionResult> GetAllLabourDetails()
        {
            var LabourDetails = await _masterSvcs.GetAllLabourDetails();
            return new JsonResult(LabourDetails);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateLabourDetail([FromBody] LabourModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateLabourDetail(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateLabourDetail([FromBody] LabourModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateLabourDetail(data);
                return new JsonResult(result);
            }
            else
            {
                var result = await _masterSvcs.UpdateLabourDetail(data);
                return new JsonResult(result);
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteLabourDetail([FromQuery] string Id)
        {
            Guid LabourDetailId = Guid.Parse(Id);
            var result = await _masterSvcs.DeleteLabourDetail(LabourDetailId);
            return new JsonResult(result);
        }
        #endregion
        #region Labour Rate
        [HttpGet]
        public async Task<IActionResult> GetAllLabourRates()
        {
            var result = await _masterSvcs.GetAllLabourRates();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFinishedGood()
        {
            Guid ProductType = MappingProductType.FinishedGood;
            var result = await _masterSvcs.GetProductByTypeId(ProductType);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateLabourRate([FromBody] LabourRateModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateLabourRate(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateLabourRate([FromBody] LabourRateModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.UpdateLabourRate(data);
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
            var result = await _masterSvcs.DeleteLabourRate(LabourRateId);
            return new JsonResult(result);
        }
        #endregion
        #endregion          
    }
}