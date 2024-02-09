using FMS.Db.DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class AlternateUnitModel
    {
        public Guid AlternateUnitId { get; set; }
        public string AlternateUnitName { get; set; }
        public decimal AlternateQuantity { get; set; }
        public Guid FK_ProductId { get; set; }
        public Guid Fk_UnitId { get; set; }
        public decimal UnitQuantity { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal RetailPrice { get; set; }
        public UnitModel Unit { get; set; }
        public ProductModel Product { get; set; }
        public List<PurchaseTransactionModel> PurchaseTransactions { get; set; }
        public List<PurchaseReturnTransactionModel> PurchaseReturnTransactions { get; set; }

    }
}
