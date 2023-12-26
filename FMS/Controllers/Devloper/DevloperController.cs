using FMS.Model.CommonModel;
using FMS.Service.Devloper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Controllers.Devloper
{
    [Authorize(Roles = "Devloper")]
    public class DevloperController : Controller
    {
        private readonly IDevloperSvcs _devloperSvcs;
        public DevloperController(IDevloperSvcs devloperSvcs)
        {
            _devloperSvcs = devloperSvcs;
        }
        [HttpGet]
        public IActionResult DevloperMaster()
        {
            return PartialView();
        }
        #region Branch
        [HttpGet]
        public async Task<IActionResult> GetAllBranch()
        {
            var Branches = await _devloperSvcs.GetAllBranch();
            return new JsonResult(Branches);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateBranch([FromBody] BranchModel model)
        {
            var result = await _devloperSvcs.CreateBranch(model);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateBranch([FromBody] BranchModel model)
        {
            var result = await _devloperSvcs.UpdateBranch(model);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteBranch([FromQuery] string id)
        {
            Guid BranchId = Guid.Parse(id);
            var result = await _devloperSvcs.DeleteBranch(BranchId);
            return new JsonResult(result);
        }

        #endregion
        #region Financial Year
        [HttpGet]
        public async Task<IActionResult> GetFinancialYears()
        {
            var result = await _devloperSvcs.GetFinancialYears();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateFinancialYear([FromBody] FinancialYearModel model)
        {
            var result = await _devloperSvcs.CreateFinancialYear(model);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateFinancialYear([FromBody] FinancialYearModel model)
        {
            var result = await _devloperSvcs.UpdateFinancialYear(model);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteFinancialYear([FromQuery] string id)
        {
            Guid BranchId = Guid.Parse(id);
            var result = await _devloperSvcs.DeleteFinancialYear(BranchId);
            return new JsonResult(result);
        }
        #endregion
        #region Accounting Setup
        #region LedgerGroup
        [HttpGet]
        public async Task<IActionResult> GetLedgerGroups()
        {
            var result = await _devloperSvcs.GetLedgerGroups();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateLedgerGroup([FromBody] LedgerGroupModel model)
        {
            var result = await _devloperSvcs.CreateLedgerGroup(model);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateLedgerGroup([FromBody] LedgerGroupModel model)
        {
            var result = await _devloperSvcs.UpdateLedgerGroup(model);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteLedgerGroup([FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _devloperSvcs.DeleteLedgerGroup(Id);
            return new JsonResult(result);
        }
        #endregion
        #region LedgerSubGroup
        [HttpGet]
        public async Task<IActionResult> GetLedgerSubGroups(Guid BranchId, Guid GroupId)
        {
            var result = await _devloperSvcs.GetLedgerSubGroups(BranchId, GroupId);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateLedgerSubGroup([FromBody] LedgerSubGroupModel model)
        {
            var result = await _devloperSvcs.CreateLedgerSubGroup(model);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateLedgerSubGroup([FromBody] LedgerSubGroupModel model)
        {
            var result = await _devloperSvcs.UpdateLedgerSubGroup(model);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteLedgerSubGroup([FromQuery] Guid BranchId, [FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _devloperSvcs.DeleteLedgerSubGroup(BranchId, Id);
            return new JsonResult(result);
        }
        #endregion
        #endregion
    }
}
