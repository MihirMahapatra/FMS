using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;

namespace FMS.Service.Accounting
{
    public interface IAccountingSvcs
    {
        #region Journal
        Task<Base> GetJournalVoucherNo();
        Task<Base> CreateJournal(JournalDataRequest requestData);
        Task<JournalViewModel> GetJournals();
        Task<JournalViewModel> GetJournalById(string Id);
        Task<Base> DeleteJournal(string Id);
        #endregion
        #region Payment
        Task<Base> GetPaymentVoucherNo(string CashBank);
        Task<LedgerViewModel> GetBankLedgers();
        Task<Base> CreatePayment(PaymentDataRequest requestData);
        Task<PaymentViewModel> GetPayments();
        Task<PaymentViewModel> GetPaymentById(string Id);
        Task<Base> DeletePayment(string Id);
        #endregion
        #region Receipt
        Task<Base> GetReceiptVoucherNo(string CashBank);
        Task<Base> CreateRecipt(ReciptsDataRequest requestData);
        Task<ReceiptViewModel> GetReceipts();
        Task<ReceiptViewModel> GetReceiptById(string Id);
        Task<Base> DeleteReceipt(string Id);
        #endregion
    }
}
