using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class DaySheetModel
    {
        public decimal CashSale { get; set; }
        public decimal CreditSale { get; set; }
        public decimal OpeningCashBal { get; set; }
        public decimal ClosingCashBal { get; set; }
        public decimal OpeningBankBal { get; set; }
        public decimal ClosingBankBal { get; set; }
        public List<PurchaseOrderModel> Purchases { get; set; }
        public List<SalesOrderModel> CashSales { get; set; }
        public List<SalesOrderModel> CreditSales { get; set; }
        public List<ReceiptModel> Receipts { get; set; }
        public List<PaymentModel> Payments { get; set; }
        public string Day { get; set; }
    }
}
