using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class CashBookViewModal:Base
    {
        public CashBookViewModal()
        {
            CashBook = new CashBookModal();
            CashBooks = new List<CashBookModal>();
        }
        public List<CashBookModal> CashBooks { get; set; }
        public CashBookModal CashBook { get; set; }
        
    }
}
