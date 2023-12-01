using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Service.Admin;
using FMS.Service.Reports;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace FMS.Controllers.Print
{
    public class PrintController : Controller
    {
        private readonly IAdminSvcs _adminSvcs;
        private readonly IReportSvcs _reportSvcs;
        public PrintController(IAdminSvcs adminSvcs, IReportSvcs reportSvcs)
        {
            _adminSvcs = adminSvcs;
            _reportSvcs = reportSvcs;
        }
        [HttpPost]
        public IActionResult SalesPrintData([FromBody] SalesDataRequest requestData)
        {
            var json = JsonConvert.SerializeObject(requestData);
            var url = Url.Action("SalesPrint", "Print", new { data = json });
            return Json(new { redirectTo = url });
        }

        [HttpGet]
        public IActionResult SalesPrint(string data)
        {
            var requestData = JsonConvert.DeserializeObject<SalesDataRequest>(data);
            return View(requestData);
        }
        [HttpPost]
        public IActionResult DaySheetPrintData([FromBody] DaySheetModel requestData)
        {
            var json = JsonConvert.SerializeObject(requestData);
            var url = Url.Action("DaySheetPrint", "Print", new { data = json });
            return Json(new { redirectTo = url });
        }
        [HttpGet]
        public IActionResult DaySheetPrint(string data)
        {
            var requestPrintData = JsonConvert.DeserializeObject<DaySheetModel>(data);
            var DaysheetPrintModel = new DaysheetPrintModel()
            {
                Cmopany = _adminSvcs.GetCompany().Result.GetCompany,
                daySheet = requestPrintData
            };
            return View(DaysheetPrintModel);
        }
    }
}
