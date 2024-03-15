using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class SalesReturnDataRequest
    {
        public string TransactionType { get; set; }
        public string RateType { get; set; }
        public Guid SalesRetunOrderId { get; set; }
        public string TransactionNo { get; set; }
        public Guid? Fk_SubLedgerId { get; set; }
        public string CustomerName { get; set; }
        public string TransactionDate { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string TranspoterName { get; set; }
        public string VehicleNo { get; set; } = null;
        public string ReceivingPerson { get; set; } = null;
        public string SiteAdress { get; set; } = null;
        public decimal TransportationCharges { get; set; }
        public string Naration { get; set; } = null;
        public decimal SubTotal { get; set; }
        public decimal Gst { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public List<List<string>> RowData { get; set; }
        public List<List<string>> PrintData { get; set; }
    }
}
