using FMS.Db.DbEntity;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Service.Devloper;
using FMS.Service.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Controllers.DashBoard
{
    [Authorize(Roles = "Admin,Devloper,User")]
    public class DashBoardController : Controller
    {
        private readonly IDevloperSvcs _devloperSvcs;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IReportSvcs _reportSvcs;
        public DashBoardController(IDevloperSvcs devloperSvcs, IReportSvcs reportSvcs, IHttpContextAccessor httpContextAccessor) : base()
        {
            _devloperSvcs = devloperSvcs;
            _HttpContextAccessor = httpContextAccessor;
            _reportSvcs = reportSvcs;
        }
        [HttpGet]
        public IActionResult DashBoard(string SuccessMsg, string eMail)
        {
            if (SuccessMsg != null)
            {
                TempData["SuccessMsg"] = SuccessMsg;
            }
            DateTime currentDate = DateTime.Today;
            string formattedDate = currentDate.ToString("dd/MM/yyyy");
            var daysheet = _reportSvcs.GetDaySheet(formattedDate);
            return PartialView(daysheet.Result.DaySheet);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBranch()
        {
            var Branches = await _devloperSvcs.GetAllBranch();
            return new JsonResult(Branches);
        }
        [HttpGet]
        public async Task<IActionResult> GetFinancialYears(string BranchId)
        {
            if (BranchId == "All")
            {
                var result = await _devloperSvcs.GetFinancialYears();
                return new JsonResult(result);
            }
            else
            {
                var result = await _devloperSvcs.GetBranchFinancialYears(Guid.Parse(BranchId));
                return new JsonResult(result);
            }
        }
        [HttpGet, HttpPost]
        public async Task<IActionResult> BranchAdmin(string SuccessMsg, SessionModel model)
        {
            if (ModelState.IsValid)
            {
                var financialYear = await _devloperSvcs.GetFinancialYearById(Guid.Parse(model.FinancialYearId));
                if (Guid.TryParse(model.BranchId, out Guid BR))
                {
                    var branch = await _devloperSvcs.GetBranchById(BR);
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchId", model.BranchId.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchName", branch.Branch.BranchName.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYearId", model.FinancialYearId.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYear", financialYear.FinancialYear.Financial_Year.ToString());
                }
                else
                {
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchId", model.BranchId.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchName", model.BranchId);
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYearId", model.FinancialYearId);
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYear", financialYear.FinancialYear.Financial_Year.ToString());
                }
                return RedirectToAction("DashBoard", "DashBoard");
            }
            TempData["SuccessMsg"] = SuccessMsg != null ? SuccessMsg : null;
            return PartialView();
        }
        [HttpGet, HttpPost]
        public async Task<IActionResult> BranchUser(string SuccessMsg, string UserId, SessionModel model)
        {
            if (ModelState.IsValid)
            {
                var financialYear = await _devloperSvcs.GetFinancialYearById(Guid.Parse(model.FinancialYearId));
                if (Guid.Parse(model.BranchId) != Guid.Empty)
                {
                    var branch = await _devloperSvcs.GetBranchById(Guid.Parse(model.BranchId));
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchId", model.BranchId.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchName", branch.Branch.BranchName.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYearId", model.FinancialYearId.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYear", financialYear.FinancialYear.Financial_Year.ToString());
                    return RedirectToAction("DashBoard", "DashBoard");
                }
                return RedirectToAction("Login", "Account", new { ErrorMsg = "Some Error Occoured" });
            }
            else
            {
                var GetBranchByUser = await _devloperSvcs.GetBranchAccordingToUser(UserId);
                var data = new SessionModel
                {
                    Branches = GetBranchByUser.Branches,
                };
                TempData["SuccessMsg"] = SuccessMsg != null ? SuccessMsg : null;
                return PartialView(data);
            }
        }
    }

}
