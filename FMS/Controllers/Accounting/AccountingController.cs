using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Service.Accounting;
using FMS.Service.Admin;
using FMS.Service.Devloper;
using FMS.Service.Master;
using Microsoft.AspNetCore.Authorization;
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

            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetJournalVoucherNo()
        {
            var result = await _accountingSvcs.GetJournalVoucherNo();
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateJournal([FromBody] JournalDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountingSvcs.CreateJournal(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetJournals()
        {
            var result = await _accountingSvcs.GetJournals();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetJournalById([FromQuery] string Id)
        {
            var result = await _accountingSvcs.GetJournalById(Id);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Delete")]
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
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetPaymentVoucherNo(string CashBank)
        {
            var result = await _accountingSvcs.GetPaymentVoucherNo(CashBank);
            return new JsonResult(result);
        }

        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountingSvcs.CreatePayment(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            var result = await _accountingSvcs.GetPayments();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPaymentById([FromQuery] string Id)
        {
            var result = await _accountingSvcs.GetPaymentById(Id);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Delete")]
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
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetReceiptVoucherNo(string CashBank)
        {
            var result = await _accountingSvcs.GetReceiptVoucherNo(CashBank);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateRecipt([FromBody] ReciptsDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountingSvcs.CreateRecipt(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetReceipts()
        {
            var result = await _accountingSvcs.GetReceipts();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetReceiptById([FromQuery] string Id)
        {
            var result = await _accountingSvcs.GetReceiptById(Id);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteReceipt([FromQuery] string id)
        {
            var result = await _accountingSvcs.DeleteReceipt(id);
            return new JsonResult(result);
        }
        #endregion
        #region Transfer
        [HttpGet]
        public IActionResult Transfer()
        {
            return PartialView();
        }

        #endregion
        #endregion
    }
}
