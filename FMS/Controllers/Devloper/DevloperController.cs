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
            return View();
        }
        #region Branch
        [HttpGet]
        public async Task<IActionResult> GetAllBranch()
        {
            var Branches = await _devloperSvcs.GetAllBranch();
            return new JsonResult(Branches);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBranch([FromBody] BranchModel model)
        {
            var result = await _devloperSvcs.CreateBranch(model);
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBranch([FromBody] BranchModel model)
        {
            var result = await _devloperSvcs.UpdateBranch(model);
            return new JsonResult(result);
        }
        [HttpPost]
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
        [HttpPost]
        public async Task<IActionResult> CreateFinancialYear([FromBody] FinancialYearModel model)
        {
            var result = await _devloperSvcs.CreateFinancialYear(model);
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFinancialYear([FromBody] FinancialYearModel model)
        {
            var result = await _devloperSvcs.UpdateFinancialYear(model);
            return new JsonResult(result);
        }
        [HttpPost]
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
        [HttpPost]
        public async Task<IActionResult> CreateLedgerGroup([FromBody] LedgerGroupModel model)
        {
            var result = await _devloperSvcs.CreateLedgerGroup(model);
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLedgerGroup([FromBody] LedgerGroupModel model)
        {
            var result = await _devloperSvcs.UpdateLedgerGroup(model);
            return new JsonResult(result);
        }
        [HttpPost]
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
        [HttpPost]
        public async Task<IActionResult> CreateLedgerSubGroup([FromBody] LedgerSubGroupModel model)
        {
            var result = await _devloperSvcs.CreateLedgerSubGroup(model);
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLedgerSubGroup([FromBody] LedgerSubGroupModel model)
        {
            var result = await _devloperSvcs.UpdateLedgerSubGroup(model);
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteLedgerSubGroup([FromQuery] Guid BranchId, [FromQuery] string id)
        {
            Guid Id = Guid.Parse(id);
            var result = await _devloperSvcs.DeleteLedgerSubGroup(BranchId, Id);
            return new JsonResult(result);
        }
        #endregion
        #region Ledger
        //[HttpPost]
        //public async Task<IActionResult> CreateLedgers([FromBody] LedgerDataRequest requestData)
        //{
        //    List<LedgerModel> model = new();
        //    foreach (var item in requestData.RowData)
        //    {
        //        LedgerModel data = new()
        //        {
        //            Fk_LedgerGroupId = Guid.Parse(requestData.LedgerGroupId),
        //            Fk_LedgerSubGroupId = !string.IsNullOrEmpty(requestData.LedgerSubGroupId)? Guid.Parse(requestData.LedgerSubGroupId) : null,
        //            Fk_BranchId = Guid.Parse(requestData.BranchId),
        //            LedgerType = item[0],
        //            LedgerName = item[1],
        //            OpeningBalance = item[2] == "" ? 0 : Convert.ToDecimal(item[2]),
        //            OpeningBalanceType = item[3]
        //        };
        //        model.Add(data);
        //    }
        //    var result = await _devloperSvcs.CreateLedger(model);
        //    return new JsonResult(result);
        //}
        //[HttpGet]
        //public IActionResult Ledger()
        //{
        //    return View();
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetLedgers([FromQuery] Guid BranchId)
        //{
        //    var result = await _devloperSvcs.GetLedgers(BranchId);
        //    return new JsonResult(result);
        //}
        //[HttpPost]
        //public async Task<IActionResult> UpdateLedger([FromBody] LedgerModel model)
        //{
        //    var result = await _devloperSvcs.UpdateLedger(model);
        //    return new JsonResult(result);
        //}
        //[HttpPost]
        //public async Task<IActionResult> DeleteLedger([FromQuery] string id, [FromQuery] Guid BranchId)
        //{
        //    Guid Id = Guid.Parse(id);
        //    var result = await _devloperSvcs.DeleteLedger(Id, BranchId);
        //    return new JsonResult(result);
        //}
        #endregion
        #endregion
    }
}
