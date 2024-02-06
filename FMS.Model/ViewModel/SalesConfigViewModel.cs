using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class SalesConfigViewModel : Base
    {
        public SalesConfigViewModel()
        {
            SalesConfigs = new List<SalesConfigModel>();
            SalesConfig = new SalesConfigModel();
        }
        public List<SalesConfigModel> SalesConfigs { get; set; }
        public SalesConfigModel SalesConfig { get; set; }
    }
}
