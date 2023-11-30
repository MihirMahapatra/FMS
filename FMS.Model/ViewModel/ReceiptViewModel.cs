using System;
using FMS.Model.CommonModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMS.Db.DbEntity;

namespace FMS.Model.ViewModel
{
    public class ReceiptViewModel:Base
    {
        public ReceiptViewModel()
        {
            Receipt = new ReceiptModel();
            Receipts = new List<ReceiptModel>();
        }
        public List<ReceiptModel> Receipts { get; set; }
        public ReceiptModel Receipt { get; set; }
        public List<GroupedReceptModel> GroupedReceipts { get; set; }
    }
}
