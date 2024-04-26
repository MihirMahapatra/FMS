using FMS.Db.Context;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Service.Admin;
using FMS.Service.Reports;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FMS.Controllers.Print
{
    public class PrintController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IAdminSvcs _adminSvcs;
        private readonly IReportSvcs _reportSvcs;
        public PrintController(IAdminSvcs adminSvcs, IReportSvcs reportSvcs, AppDbContext appDbContext)
        {
            _adminSvcs = adminSvcs;
            _reportSvcs = reportSvcs;
            _appDbContext = appDbContext;
        }
        #region Sales Print
        [HttpPost]
        public IActionResult SalesPrintData([FromBody] SalesDataRequest requestData)
        {
            if(requestData.CustomerName == "")
            {
                var SundrydebitorName = _appDbContext.SubLedgers.Where(s => s.SubLedgerId == requestData.Fk_SubLedgerId).FirstOrDefault();
                requestData.CustomerName = SundrydebitorName.SubLedgerName;
            }
            TempData["SalesPrintData"] = JsonConvert.SerializeObject(requestData);
            return Json(new { redirectTo = Url.Action("SalesPrint", "Print") });
        }
        [HttpGet]
        public IActionResult SalesPrint()
        {
            if (TempData.TryGetValue("SalesPrintData", out object tempData) && tempData is string jsonData)
            {
                var requestPrintData = JsonConvert.DeserializeObject<SalesDataRequest>(jsonData);
                var SalesPrintModel = new SalesPrintModel()
                {
                    Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                    SalesData = requestPrintData
                };

                return View(SalesPrintModel);
            }
            return RedirectToAction("Error");
        }
        #endregion
        #region Daysheet Report
        [HttpGet]
        public async Task<IActionResult> DaySheetPrint([FromQuery] string Date)
        {
            var result = await _reportSvcs.GetDaySheet(Date);
            var DaySheetData = new DaySheetModel();
            DaySheetData = result.DaySheet;
            var DaySheetPrintModel = new DaysheetPrintModel()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                daySheet = DaySheetData
            };
            return View(DaySheetPrintModel);
        }
        #endregion
        #region Labour Reports
        [HttpPost]
        public IActionResult LabourSummarizedPrintData([FromBody] LabourDetailsModal requestData)
        {
            TempData["LabourSummarizedData"] = JsonConvert.SerializeObject(requestData);
            return Json(new { redirectTo = Url.Action("LabourSummarizedPrint", "Print") });
        }
        [HttpGet]
        public IActionResult LabourSummarizedPrint()
        {
            if (TempData.TryGetValue("LabourSummarizedData", out object tempData) && tempData is string jsonData)
            {
                var requestPrintData = JsonConvert.DeserializeObject<LabourDetailsModal>(jsonData);
                var LabourDetailedReportModal = new LabourDetailedReportModal()
                {
                    Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                    LabourDetails = requestPrintData
                };
                return View(LabourDetailedReportModal);
            }
            return RedirectToAction("Error");

        }

        [HttpGet]
        public async Task<IActionResult> LabourDetailedPrint([FromQuery] LabourReportDataRequest requestData)
        {
            var result = await _reportSvcs.GetDetailedLabourReport(requestData);
            var LabourReportData = new LabourDetailsModal();
            LabourReportData.FromDate = requestData.FromDate;
            LabourReportData.ToDate = requestData.ToDate;
            LabourReportData.LaborReportDetailed = result.DetailedLabour;
            var LabourDetailedReportModal = new LabourDetailedReportModal()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                LabourDetails = LabourReportData
            };
            return View(LabourDetailedReportModal);

        }
        #endregion
        #region Stock Reports
        [HttpGet]
        public async Task<IActionResult> StockSumrizedReportPrint([FromQuery] StockReportDataRequest requestData)
        {

            var result = await _reportSvcs.GetSummerizedStockReports(requestData);
            var StockReportData = new StockReportDataModel();
            StockReportData.FromDate = requestData.FromDate;
            StockReportData.ToDate = requestData.ToDate;
            StockReportData.StockReport = result.StockReports;
            var StockSumrizedReportModal = new StockSumrizedReportModal()
                {
                    Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                    StockReports = StockReportData
            };
            return View(StockSumrizedReportModal);
        }
        [HttpGet]
        public async Task<IActionResult> StockDetailedReportPrint([FromQuery] StockReportDataRequest requestData)
        {
            var result = await _reportSvcs.GetDetailedStockReport(requestData);
            var StockReportData = new StockReportDataModel();
            StockReportData.FromDate = requestData.FromDate;
            StockReportData.ToDate = requestData.ToDate;
            StockReportData.Stocks = result.DetailedStock;
            var StockDetailedReportModal = new StockSumrizedReportModal()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                StockReports = StockReportData
            };
            return View(StockDetailedReportModal);

        }
        #endregion
        #region Customer Report
        [HttpGet]
        public async Task<IActionResult> CustomerSummarizedReportPrint([FromQuery] PartyReportDataRequest requestData)
        {
            
            var result = await _reportSvcs.GetSummerizedCustomerReport(requestData);
            var CustomerSumrizedData = new CustomerSummarizedModel();
            CustomerSumrizedData.FromDate = requestData.FromDate;
            CustomerSumrizedData.ToDate = requestData.ToDate;
            CustomerSumrizedData.PartyReports = result.PartySummerized;
            var CustomerSumrizedReportModal = new CustomerReportDataModel()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                Customers = CustomerSumrizedData
            };
            return View(CustomerSumrizedReportModal);

        }
        [HttpGet]
        public async Task<IActionResult> CustomerDetailedReportPrint([FromQuery] PartyReportDataRequest requestData)
        {
            var result = await _reportSvcs.GetDetailedCustomerReport(requestData);
            var CustomerDetailedData = new CustomerSummarizedModel();
            CustomerDetailedData.FromDate = requestData.FromDate;
            CustomerDetailedData.ToDate = requestData.ToDate;
            CustomerDetailedData.PartyDetailedReports = result.PartyDetailed;
            var CustomerDetailedReportModal = new CustomerReportDataModel()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                Customers = CustomerDetailedData
            };
            return View(CustomerDetailedReportModal);

        }

        [HttpGet]
        public async Task<IActionResult> CustomerDetailedReportShortPrint([FromQuery] PartyReportDataRequest requestData)
        {
            var result = await _reportSvcs.GetDetailedCustomerReport(requestData);
           var CustomerDetailedData = new CustomerSummarizedModel();
            CustomerDetailedData.FromDate = requestData.FromDate;
           CustomerDetailedData.ToDate = requestData.ToDate;
            CustomerDetailedData.PartyDetailedReports = result.PartyDetailed;
            var CustomerDetailedReportModal = new CustomerReportDataModel()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                Customers = CustomerDetailedData
            };
            return View(CustomerDetailedReportModal);

        }
        #endregion
        #region Supplyer Report
        [HttpGet]
        public async Task<IActionResult> SupplyerSummarizedReportPrint([FromQuery] PartyReportDataRequest requestData)
        {
            var result = await _reportSvcs.GetSummerizedSupplyerReport(requestData);
            var SupplyerReportData = new CustomerSummarizedModel();
            SupplyerReportData.FromDate = requestData.FromDate;
            SupplyerReportData.ToDate = requestData.ToDate;
            SupplyerReportData.PartyReports = result.PartySummerized;
            var SupplyerSumrizedReportModal = new CustomerReportDataModel()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                Customers = SupplyerReportData
            };
            return View(SupplyerSumrizedReportModal);

        }
        [HttpGet]
        public async Task<IActionResult> SupplyerDetailedReportPrint([FromQuery] PartyReportDataRequest requestData)
        {
            var result = await _reportSvcs.GetDetailedSupplyerReport(requestData);
            var SupplyerDetailedData = new CustomerSummarizedModel();
            SupplyerDetailedData.FromDate = requestData.FromDate;
            SupplyerDetailedData.ToDate = requestData.ToDate;
            SupplyerDetailedData.PartyDetailedReports = result.PartyDetailed;
            var CustomerDetailedReportModal = new CustomerReportDataModel()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                Customers = SupplyerDetailedData
            };
            return View(CustomerDetailedReportModal);

        }
        #endregion
        #region Cash Book
        [HttpPost]
        public IActionResult CashBookPrintData([FromBody] CashBookModal requestData)
        {
            TempData["CashBookData"] = JsonConvert.SerializeObject(requestData);
            return Json(new { redirectTo = Url.Action("CashBookPrint", "Print") });
        }
        [HttpGet]
        public IActionResult CashBookPrint()
        {
            if (TempData.TryGetValue("CashBookData", out object tempData) && tempData is string jsonData)
            {
                var requestPrintData = JsonConvert.DeserializeObject<CashBookModal>(jsonData);
                var CashBookPrintModel = new CashBookPrintModel()
                {
                    Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                    cashbook = requestPrintData
                };

                return View(CashBookPrintModel);
            }
            return RedirectToAction("Error");
        }
        #endregion
        #region Bank Book Reports
        [HttpPost]
        public IActionResult BankBookPrintData([FromBody] BankBookModal requestData)
        {
            TempData["BankBookData"] = JsonConvert.SerializeObject(requestData);
            return Json(new { redirectTo = Url.Action("BankBookPrint", "Print") });
        }
        [HttpGet]
        public IActionResult BankBookPrint()
        {
            if (TempData.TryGetValue("BankBookData", out object tempData) && tempData is string jsonData)
            {
                var requestPrintData = JsonConvert.DeserializeObject<BankBookModal>(jsonData);
                var BankBookPrintModel = new BankBookPrintModel()
                {
                    Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                    bankbook = requestPrintData
                };

                return View(BankBookPrintModel);
            }
            return RedirectToAction("Error");

        }
        #endregion
        #region Ledger Book Reports
        [HttpPost]
        public IActionResult LedgerBookPrintData([FromBody] LedgerBookModel requestData)
        {
            TempData["LedgerBookData"] = JsonConvert.SerializeObject(requestData);
            return Json(new { redirectTo = Url.Action("LedgerBookPrint", "Print") });
        }
        [HttpGet]
        public IActionResult LedgerBookPrint()
        {
            if (TempData.TryGetValue("LedgerBookData", out object tempData) && tempData is string jsonData)
            {
                var requestPrintData = JsonConvert.DeserializeObject<LedgerBookModel>(jsonData);
                var LedgerBookPrintModel = new LedgerBookPrintModel()
                {
                    Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                    ledgerBook = requestPrintData
                };
                return View(LedgerBookPrintModel);
            }
            return RedirectToAction("Error");

        }
        #endregion
        #region SubLadger Reports
        [HttpGet]
        public async Task<IActionResult> SubladgerSumrizedReportPrint([FromQuery] LedgerbookDataRequest requestData)
        {

            var result = await _reportSvcs.GetSummerizedSubLadgerReport(requestData);
            var SubLedgerReportData = new CustomerSummarizedModel();
            SubLedgerReportData.FromDate = requestData.FromDate;
            SubLedgerReportData.ToDate = requestData.ToDate;
            SubLedgerReportData.PartyReports = result.PartySummerized;

            var SubLedgerSumrizedReportModal = new CustomerReportDataModel()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                Customers = SubLedgerReportData
            };
            return View(SubLedgerSumrizedReportModal);
        }
        [HttpGet]
        public async Task<IActionResult> SubLadgerDetailedReportPrint([FromQuery] LedgerbookDataRequest requestData)
        {
            var result = await _reportSvcs.SubLadgerDetailedBookReport(requestData);
            var SubladgerModel = new CustomerSummarizedModel();
            SubladgerModel.FromDate = requestData.FromDate;
            SubladgerModel.ToDate = requestData.ToDate;
            SubladgerModel.PartyDetailedReports = result.PartyDetailed;
            var SubLadgerDetailedReportModal = new CustomerReportDataModel()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                Customers = SubladgerModel
            };
            return View(SubLadgerDetailedReportModal);

        }
        #endregion
        #region TrialBalance Reports
        [HttpPost]
        public IActionResult TrialBalancePrintData([FromBody] TrailBalancesModel requestData)
        {
            TempData["TrialBalanceData"] = JsonConvert.SerializeObject(requestData);
            return Json(new { redirectTo = Url.Action("TrialBalancePrint", "Print") });
        }
        [HttpGet]
        public IActionResult TrialBalancePrint()
        {
            if (TempData.TryGetValue("TrialBalanceData", out object tempData) && tempData is string jsonData)
            {
                var requestPrintData = JsonConvert.DeserializeObject<TrailBalancesModel>(jsonData);
                var TrialBalancePrintModel = new TrialBalancePrintModel()
                {
                    Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                    Balances = requestPrintData
                };
                return View(TrialBalancePrintModel);
            }
            return RedirectToAction("Error");

        }
        #endregion
        #region Journal Book 
        [HttpPost]
        public IActionResult JournalBookPrintData([FromBody] JournalBookModel requestData)
        {
            TempData["JournalBookData"] = JsonConvert.SerializeObject(requestData);
            return Json(new { redirectTo = Url.Action("JournalBookPrint", "Print") });
        }
        [HttpGet]
        public IActionResult JournalBookPrint()
        {
            if (TempData.TryGetValue("JournalBookData", out object tempData) && tempData is string jsonData)
            {
                var requestPrintData = JsonConvert.DeserializeObject<JournalBookModel>(jsonData);
                var JournalBookPrintModel = new JournalBookPrintModel()
                {
                    Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                    JournalBook = requestPrintData
                };

                return View(JournalBookPrintModel);
            }
            return RedirectToAction("Error");

        }
        #endregion
        #region ClientRefarance Report
        [HttpGet]
        public async Task<IActionResult> ClientRefarencePrint([FromQuery] PartyReportDataRequest requestData)
        {
            var result = await _reportSvcs.GetCustomerRefranceReport(requestData);
            var ClientRefarance = new CustomerSummarizedModel();
            ClientRefarance.PartyReports = result.PartySummerized;
            ClientRefarance.FromDate = requestData.FromDate;
            ClientRefarance.ToDate = requestData.ToDate;
            var ClientRefarancePrintModel = new CustomerReportDataModel()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                Customers = ClientRefarance
            };
            return View(ClientRefarancePrintModel);
        }
        #endregion
    }
}
