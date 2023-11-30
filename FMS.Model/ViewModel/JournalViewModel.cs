using FMS.Db.DbEntity;
using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class JournalViewModel:Base
    {
        public JournalViewModel()
        {
            Journal = new JournalModel();
            Journals = new List<JournalModel>();
        }

        public List<JournalModel> Journals { get; set; }
        public JournalModel Journal { get; set; }
        public List<GroupedJournalModel> GroupedJournals { get; set; }
    }
}

