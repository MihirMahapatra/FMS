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
        public DashBoardController(IDevloperSvcs devloperSvcs,IReportSvcs reportSvcs, IHttpContextAccessor httpContextAccessor):base()
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
             var daysheet =_reportSvcs.GetDaySheet(formattedDate);
            return PartialView(daysheet.Result.DaySheet);
        }
        [HttpGet]
        public async Task<IActionResult> GetFinancialYears(string BranchId)
        {
            FinancialYearViewModel financialYear = null;
            if (BranchId == "All")
            {
                financialYear = await _devloperSvcs.GetFinancialYears(Guid.Empty);
            }
            else
            {
                financialYear = await _devloperSvcs.GetFinancialYears(Guid.Parse(BranchId));
            }

            return new JsonResult(financialYear);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBranch()
        {
            var Branches = await _devloperSvcs.GetAllBranch();
            return new JsonResult(Branches);
        }
        [HttpGet, HttpPost]
        public async Task<IActionResult> BranchAdmin(string SuccessMsg, SessionModel model)
        {
            if (ModelState.IsValid)
            {
                if (Guid.TryParse(model.BranchId, out Guid BR) && Guid.TryParse(model.FinancialYearId, out Guid FY))
                {
                    var financialYears = await _devloperSvcs.GetFinancialYears();
                    var branches = await _devloperSvcs.GetAllBranch();
                    branches.Branches.Add(new BranchModel { BranchId = Guid.Empty, BranchName = "All" });
                    var financialyear = financialYears.FinancialYears.Where(s => s.FinancialYearId == FY).FirstOrDefault();
                    var branch = branches.Branches.Where(s => s.BranchId == BR).FirstOrDefault();
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchId", model.BranchId.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchName", branch.BranchName.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYearId", model.FinancialYearId.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYear", financialyear.Financial_Year.ToString());
                }
                else
                {
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchId", model.BranchId.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchName", model.BranchId);
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYearId", model.FinancialYearId);
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYear", model.FinancialYearId);
                }
                return RedirectToAction("DashBoard", "DashBoard");
            }
            return PartialView();
        }
        [HttpGet, HttpPost]
        public async Task<IActionResult> BranchUser(string SuccessMsg, string UserId, SessionModel model)
        {
            if (ModelState.IsValid)
            {
                if (Guid.Parse(model.BranchId) != Guid.Empty && Guid.Parse(model.FinancialYearId) != Guid.Empty)
                {
                    var financialYears = await _devloperSvcs.GetFinancialYears();
                    var branches = await _devloperSvcs.GetAllBranch();
                    var branch = branches.Branches.Where(s => s.BranchId == Guid.Parse(model.BranchId)).FirstOrDefault();
                    var financialyear = financialYears.FinancialYears.Where(s => s.FinancialYearId == Guid.Parse(model.FinancialYearId)).FirstOrDefault();
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchId", model.BranchId.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("BranchName", branch.BranchName.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYearId", model.FinancialYearId.ToString());
                    _HttpContextAccessor.HttpContext.Session.SetString("FinancialYear", financialyear.Financial_Year.ToString());
                    return RedirectToAction("DashBoard", "DashBoard", new { SuccessMsg = model.SuccessMsg });
                }
                return RedirectToAction("Login", "Account", new { ErrorMsg ="Some Error Occoured"});
            }
            else
            {
                var GetBranchByUser = await _devloperSvcs.GetBranchAccordingToUser(UserId);
                var financialYears = await _devloperSvcs.GetFinancialYears(GetBranchByUser.Branch.BranchId);
                if (SuccessMsg != null)
                {
                    TempData["SuccessMsg"] = SuccessMsg;
                }
                var data = new SessionModel
                {
                    FinancialYears = financialYears.FinancialYears,
                    Branches = GetBranchByUser.Branches,
                };
                return PartialView(data);
            }
        }
    }

}
