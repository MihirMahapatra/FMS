using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using Microsoft.EntityFrameworkCore.Storage;

namespace FMS.Repository.Accounting
{
    public interface IAccountingRepo
    {
        #region Journal
        Task<Result<string>> GetJournalVoucherNo();
        Task<Result<bool>> CreateJournal(JournalDataRequest requestData);
        Task<Result<GroupedJournalModel>> GetJournals();
        Task<Result<GroupedJournalModel>> GetJournalById(string Id);
        Task<Result<bool>> DeleteJournal(string Id, IDbContextTransaction transaction);
        #endregion
        #region Payment
        Task<Result<string>> GetPaymentVoucherNo(string CashBank);
        Task<Result<LedgerModel>> GetBankLedgers(); 
        Task<Result<bool>> CreatePayment(PaymentDataRequest requestData);
        Task<Result<GroupedPaymentModel>> GetPayments();
        Task<Result<GroupedPaymentModel>> GetPaymentById(string Id);
        Task<Result<bool>> DeletePayment(string Id, IDbContextTransaction transaction);
        #endregion
        #region Receipt
        Task<Result<string>> GetReceiptVoucherNo(string CashBank);
        Task<Result<bool>> CreateRecipt(ReciptsDataRequest requestData);
        Task<Result<GroupedReceptModel>> GetReceipts();
        Task<Result<GroupedReceptModel>> GetReceiptById(string Id);
        Task<Result<bool>> DeleteRecipt(string Id, IDbContextTransaction transaction);
        #endregion
    }
}
