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
    }
    public class PartyReportModel2
    {
        public String BranchName;
        public PartyModel PartyInfo { get; set; }
    }
    public class PartyReportViewModel : Base
    {
        public PartyReportViewModel()
        {
            PartyDetailed = new List<PartyReportModel2>();
            PartySummerized = new List<PartyReportModel>();
        }
        public List<PartyReportModel2> PartyDetailed { get; set; }
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
