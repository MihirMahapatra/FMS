using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class GroupedLederwiseJournals
    {
        public string VoucherNo { get; set; }
        public List<LederwiseJournals> LederwiseJournals { get; set; }
    }
    public class LederwiseJournals
    {
        public Guid Fk_LedgerId { get; set; }
        public List<JournalModel> Journals { get; set; }
    }
}
