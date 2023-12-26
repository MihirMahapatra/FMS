using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class BankBookModal
    {
        public string VouvherNo { get; set; }
        public string FromDate { get; set; }
        public string Todate { get; set; }
        public string PartyName { get; set; }
        public decimal OpeningBal { get; set; }
        public decimal ClosingBal { get; set; }
        public string OpeningBalType { get; set; }
        public decimal DrAmt { get; set; }
        public decimal CrAmt { get; set; }
        public decimal Balance { get; set; }
        public string BankName { get; set; }
        public string BalanceType { get; set; }
        public List<ReceiptModel> Receipts { get; set; }
        public List<PaymentModel> Payments { get; set; }
        public List<JournalModel> Journals { get; set; }
    }
}
