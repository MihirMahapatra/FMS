using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class BankBookViewModal:Base
    {
        public BankBookViewModal()
        {
            BankBook = new BankBookModal();
            BankBooks = new List<BankBookModal>();
        }
        public List<BankBookModal> BankBooks { get; set; }
        public BankBookModal BankBook { get; set; }
    }
}
