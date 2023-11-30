using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Service.Accounting;
using FMS.Service.Admin;
using FMS.Service.Devloper;
using FMS.Service.Master;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Controllers.Accounting
{
    public class AccountingController : Controller
    {
        private readonly IMasterSvcs _masterSvcs;
        private readonly IAdminSvcs _adminSvcs;
        private readonly IDevloperSvcs _devloperSvcs;
        private readonly IAccountingSvcs _accountingSvcs;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public AccountingController(IMasterSvcs masterSvcs, IAccountingSvcs accountingSvcs, IAdminSvcs adminSvcs, IDevloperSvcs devloperSvcs, IHttpContextAccessor HttpContextAccessor)
        {
            _masterSvcs = masterSvcs;
            _accountingSvcs = accountingSvcs;
            _adminSvcs = adminSvcs;
            _devloperSvcs = devloperSvcs;
            _HttpContextAccessor = HttpContextAccessor;
        }
        [HttpGet]
        #region Accounting
        [HttpGet]
        public async Task<IActionResult> GetLedgers()
        {
            var result = await _adminSvcs.GetLedgers();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSubLedgersById([FromQuery] Guid LedgerId)
        {
            var result = await _masterSvcs.GetSubLedgersById(LedgerId);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetBankLedgers()
        {
            var result = await _accountingSvcs.GetBankLedgers();
            return new JsonResult(result);
        }
        #region Journal
        public IActionResult Journal()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetJournalVoucherNo()
        {
            var result = await _accountingSvcs.GetJournalVoucherNo();
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateJournal([FromBody] JournalDataRequest requestData)
        {
            var result = await _accountingSvcs.CreateJournal(requestData);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetJournals()
        {
            var result = await _accountingSvcs.GetJournals();
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteJournal([FromQuery] string id)
        {
            var result = await _accountingSvcs.DeleteJournal(id);
            return new JsonResult(result);
        }
        #endregion
        #region Payment
        [HttpGet]
        public IActionResult Payment()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetPaymentVoucherNo(string CashBank)
        {
            var result = await _accountingSvcs.GetPaymentVoucherNo(CashBank);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDataRequest requestData)
        {
            var result = await _accountingSvcs.CreatePayment(requestData);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            var result = await _accountingSvcs.GetPayments();
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> DeletePayment([FromQuery] string id)
        {
            var result = await _accountingSvcs.DeletePayment(id);
            return new JsonResult(result);
        }
        #endregion
        #region Receipt
        [HttpGet]
        public IActionResult Receipt()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetReceiptVoucherNo(string CashBank)
        {
            var result = await _accountingSvcs.GetReceiptVoucherNo(CashBank);
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRecipt([FromBody] ReciptsDataRequest requestData)
        {
            var result = await _accountingSvcs.CreateRecipt(requestData);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetReceipts()
        {
            var result = await _accountingSvcs.GetReceipts();
             return new JsonResult(result);
        }
        #endregion
        #region Transfer
        [HttpGet]
        public IActionResult Transfer()
        {
            string branchName = _HttpContextAccessor.HttpContext.Session.GetString("BranchName");
            string FinancialYear = _HttpContextAccessor.HttpContext.Session.GetString("FinancialYear");
            ViewBag.BranchName = branchName;
            ViewBag.FinancialYear = FinancialYear;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteReceipt([FromQuery] string id)
        {
            var result = await _accountingSvcs.DeleteReceipt(id);
            return new JsonResult(result);
        }
        #endregion
        #endregion
    }
}
