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
        #region Stock Master  
        [HttpGet]
        public IActionResult StockMaster()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            var result = await _masterSvcs.GetStocks();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _adminSvcs.GetAllProducts();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var result = await _adminSvcs.GetAllGroups();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSubGroups(Guid GroupId)
        {
            var result = await _adminSvcs.GetSubGroups(GroupId);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsWhichNotInStock(Guid GroupId, Guid SubGroupId)
        {
            var result = await _masterSvcs.GetProductsWhichNotInStock(GroupId, SubGroupId);
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
        [HttpPost, Authorize(Policy = "Edit")]
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
        #region PartyMaster
        [HttpGet]
        public IActionResult PartyMaster()
        {
            return PartialView();
        }
        public IActionResult PartyList()
        {
            return PartialView();
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
        [HttpPost, Authorize(Policy = "Edit")]
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
        [HttpPost, Authorize(Policy = "Edit")]
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
        [HttpPost, Authorize(Policy = "Edit")]
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
            return PartialView();
        }
        #region Labour Type
        [HttpGet]
        public async Task<IActionResult> GetAllLabourTypes()
        {
            var LabourTypes = await _masterSvcs.GetAllLabourTypes();
            return new JsonResult(LabourTypes);
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
        [HttpPost, Authorize(Policy = "Edit")]
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
        #endregion          
        #region Account Master
        [HttpGet]
        public IActionResult AccountMaster()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return PartialView();
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
        [HttpPost, Authorize(Policy = "Edit")]
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
        public IActionResult SubLedger()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetLedgersHasSubLedger()
        {
            var result = await _adminSvcs.GetLedgersHasSubLedger();
            return new JsonResult(result);
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
        [HttpPost, Authorize(Policy = "Edit")]
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
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateSubLedgerBalance([FromBody] SubLedgerBalanceModel data)
        {
            if (ModelState.IsValid)
            {
                var result = await _masterSvcs.CreateSubLedgerBalance(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Edit")]
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
    }
}