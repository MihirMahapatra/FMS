using FMS.Db.DbEntity;
using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class BranchFinancialYearViewModel:Base
    {
        public BranchFinancialYearViewModel()
        {
            BranchFinancialYears = new List<BranchFinancialYearModel>();
            BranchFinancialYear = new BranchFinancialYearModel();
        }
        public List<BranchFinancialYearModel> BranchFinancialYears { get; set; }
        public BranchFinancialYearModel BranchFinancialYear { get; set; }
    }
}
