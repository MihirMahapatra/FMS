using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class PartyReportModel
    {
        public Guid PartyId { get; set; }
        public Guid Fk_SubledgerId { get; set; }
        public string PartyName { get; set; }
        public decimal OpeningBal { get; set; }
        public string OpeningBalType { get; set; }
        public decimal DrAmt { get; set; }
        public decimal CrAmt { get; set; }
        public decimal Balance { get; set; }
        public string BalanceType { get; set; }
        public string Adress { get; set; }
        public string RefarenceName { get; set; }
    }
    public class PartyReportOrderModel
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionNo { get; set; }
        public string MaterialReceiptNo { get; set; }
        public string TransactionType { get; set; }
        public decimal HandlingCharges { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal TransportCharges { get; set; }
        public string SiteAdress { get; set; }
        public string ChallanNo { get; set; }
        public string Naration { get; set; }
        public string BranchName {  get; set; }
        public string DrCr { get; set; }
        public string VoucherType { get; set; }
        public List<PartyReportTransactionModel> Transactions { get; set; } = new List<PartyReportTransactionModel>();

    }
    public class PartyReportTransactionModel
    {
        public string ProductName { get; set; }
        public string SubProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal AlternateQuantity { get; set; }
        public string Unit { get; set; }
        public string SubProductUnit { get; set; }
        public string AlternateUnit { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }

    }
    public class PartyReportModel2
    {
        public string BranchName { get; set; }
        public decimal OpeningBal { get; set; }
        public string OpeningBalType { get; set; }
        public string PartyName { get; set; }
        public string Adress { get; set; }
        public List<PartyReportOrderModel> Orders { get; set; } = new List<PartyReportOrderModel>();
    }
    public class PartyReportViewModel : Base
    {
        public PartyReportViewModel()
        {
            PartyDetailed = new PartyReportModel2();
            PartySummerized = new List<PartyReportModel>();
        }
        public PartyReportModel2 PartyDetailed { get; set; }
        public List<PartyReportModel> PartySummerized { get; set; }
    }
    public class PartyReportInfoModel
    {
        public string BranchName { get; set; }
        public decimal OpeningBalance { get; set; }
        public string OpeningBalanceType { get; set; }
        public decimal RunningBalance { get; set; }
        public string RunningBalanceType { get; set; }
    }
    public class PartyReportInfoViewModel : Base
    {
        public PartyReportInfoViewModel()
        {
            PartyInfos = new List<PartyReportInfoModel>();
        }
        public List<PartyReportInfoModel> PartyInfos { get; set; }
    }
}
