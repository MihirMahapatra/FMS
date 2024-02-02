using FMS.Model.CommonModel;
using FMS.Service.Admin;
using FMS.Service.Master;
using FMS.Service.Reports;
using FMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Controllers.Reports
{
    [Authorize(Roles = "Devloper,Admin,User")]
    public class ReportsController : Controller
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IReportSvcs _reportSvcs;
        private readonly IAdminSvcs _adminSvcs;
        private readonly IMasterSvcs _masterSvcs;
        public ReportsController(IHttpContextAccessor HttpContextAccessor, IReportSvcs reportSvcs, IAdminSvcs adminSvcs, IMasterSvcs masterSvcs)
        {
            _HttpContextAccessor = HttpContextAccessor;
            _reportSvcs = reportSvcs;
            _adminSvcs = adminSvcs;
            _masterSvcs = masterSvcs;
        }
        #region Stock Report
        [HttpGet]
        public IActionResult StockReport()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProductTypes([FromQuery] Guid ProductTypeId)
        {
            var result = await _adminSvcs.GetProductTypes();
            var elementToRemove = result.ProductTypes.FirstOrDefault(x => x.ProductTypeId == MappingProductType.ServiceGoods);
            if (elementToRemove != null)
            {
                result.ProductTypes.Remove(elementToRemove);
            }
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductByTypeId([FromQuery] Guid ProductTypeId)
        {
            var result = await _adminSvcs.GetProductByTypeId(ProductTypeId);
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> GetSummerizedStockReports([FromBody] StockReportDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.GetSummerizedStockReports(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> GetBranchWiseStockInfo([FromBody] StockReportDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.GetBranchWiseStockInfo(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> GetDetailedStockReport([FromBody] StockReportDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.GetDetailedStockReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        #endregion
        #region Labour Report
        [HttpGet]
        public IActionResult LabourReport()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetLabourTypes()
        {
            var LabourTypes = await _masterSvcs.GetAllLabourTypes();
            return new JsonResult(LabourTypes);
        }
        [HttpGet]
        public async Task<IActionResult> GetLaboursByLabourTypeId(Guid LabourTypeId)
        {
            var LabourTypes = await _masterSvcs.GetLaboursByLabourTypeId(LabourTypeId);
            return new JsonResult(LabourTypes);
        }
        [HttpPost]
        public async Task<IActionResult> GetSummerizedLabourReport([FromBody] LabourReportDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.GetSummerizedLabourReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> GetDetailedLabourReport([FromBody] LabourReportDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.GetDetailedLabourReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        #endregion
        #region Customer Report
        [HttpGet]
        public IActionResult CustomerReport()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> GetSummerizedCustomerReport([FromBody] PartyReportDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.GetSummerizedCustomerReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> GetBranchWiseCustomerInfo([FromBody] PartyReportDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.GetBranchWiseCustomerInfo(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> GetDetailedCustomerReport([FromBody] PartyReportDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.GetDetailedCustomerReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        #endregion
        #region Supplyer Report
        [HttpGet]
        public IActionResult SupplyerReport()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> GetSummerizedSupplyerReport([FromBody] PartyReportDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.GetSummerizedSupplyerReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> GetDetailedSupplyerReport([FromBody] PartyReportDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.GetDetailedSupplyerReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        #endregion
        #region DaySheet
        [HttpGet]
        public IActionResult DaySheet()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetDaySheet([FromQuery] string Date)
        {
            var result = await _reportSvcs.GetDaySheet(Date);
            return new JsonResult(result);
        }
        #endregion
        #region CashBook
        [HttpGet]
        public IActionResult CashBook()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> CashBookReport([FromBody] CashBookDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.CashBookReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        #endregion
        #region BankBook
        [HttpGet]
        public IActionResult BankBook()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> BankBookReport([FromBody] BankBookDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.BankBookReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        #endregion
        #region LedgerBook
        [HttpGet]
        public IActionResult LadgerBook()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LedgerBookReport([FromBody] LedgerbookDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.LagderBookReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        #endregion
        #region TrialBalance
        [HttpGet]
        public IActionResult TrialBalance()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TrialBalanceReport([FromBody] LedgerbookDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.TrialBalanceReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        #endregion
        #region JournalBook
        [HttpGet]
        public IActionResult JournalBook()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> JournalBookReport([FromBody] LedgerbookDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _reportSvcs.JournalBookReport(requestData);
                return new JsonResult(result);
            }
            return BadRequest(ModelState);
        }
        #endregion
    }
}
