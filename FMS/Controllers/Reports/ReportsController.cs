﻿using FMS.Model.CommonModel;
using FMS.Service.Master;
using FMS.Service.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Controllers.Reports
{
    [Authorize(Roles = "Devloper,Admin,User")]
    public class ReportsController : Controller
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IReportSvcs _reportSvcs;
        private readonly IMasterSvcs _masterSvcs;
        public ReportsController(IHttpContextAccessor HttpContextAccessor, IReportSvcs reportSvcs, IMasterSvcs masterSvcs)
        {
            _HttpContextAccessor = HttpContextAccessor;
            _reportSvcs = reportSvcs;
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
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetProductByTypeId([FromQuery] Guid ProductTypeId)
        {
            var result = await _masterSvcs.GetProductByTypeId(ProductTypeId);
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
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
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
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
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
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
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
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetDaySheet([FromQuery] string Date)
        {
            var result = await _reportSvcs.GetDaySheet(Date);
            return new JsonResult(result);
        }
        #endregion
    }
}
