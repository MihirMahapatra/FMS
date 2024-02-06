using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class SalesConfigModel
    {
        public Guid SalesConfigId { get; set; }
        public Guid Fk_FinishedGoodId { get; set; }
        public Guid Fk_SubFinishedGoodId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        //other
        public string ProductName { get; set; }
        public string SubFinishedGoodName { get; set; }
        public string FinishedGoodName { get; set; }
        public ProductModel FinishedGood { get; set; }
        
    }
}
