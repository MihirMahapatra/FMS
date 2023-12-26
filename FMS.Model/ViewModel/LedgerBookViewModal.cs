using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class LedgerBookViewModal:Base
    {
        public LedgerBookViewModal()
        {
            LedgerBook = new LedgerBookModel();
            LedgerBooks = new List<LedgerBookModel>();
        }
        public List<LedgerBookModel> LedgerBooks { get; set; }
        public LedgerBookModel LedgerBook { get; set; }
    }
}

