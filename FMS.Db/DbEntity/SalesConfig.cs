using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Db.DbEntity
{
    public class SalesConfig
    {
        public Guid SalesConfigId { get; set; }
        public Guid Fk_FinishedGoodId { get; set; }
        public Guid Fk_SubFinishedGoodId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}
