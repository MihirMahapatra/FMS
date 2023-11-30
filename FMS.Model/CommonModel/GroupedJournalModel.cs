using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class GroupedJournalModel
    {
        public string VoucherNo { get; set; }
        public List<JournalModel> Journals { get; set; }
    }
}
