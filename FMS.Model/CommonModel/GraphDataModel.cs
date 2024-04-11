using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class GraphDataModel
    {
        public List<decimal> PurchaseAmount { get; set; }
        public List<decimal> ProductionAmount { get; set; }
        public List<decimal> SalesAmount { get; set; }
        public List<decimal> ReceivedAmount { get; set; }
    }
    public class GraphDataViewModel : Base
    {
        public GraphDataViewModel()
        {
            GraphDatas = new List<GraphDataModel>();
            GraphData = new GraphDataModel();
        }
        public List<GraphDataModel> GraphDatas { get; set; }
        public GraphDataModel GraphData { get; set; }
    }
    
}
