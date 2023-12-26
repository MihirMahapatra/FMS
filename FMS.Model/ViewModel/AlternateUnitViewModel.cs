using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class AlternateUnitViewModel : Base
    {
        public AlternateUnitViewModel()
        {
            AlternateUnit = new();
            AlternateUnits = new();
        }
        public AlternateUnitModel AlternateUnit { get; set; }
        public List<AlternateUnitModel> AlternateUnits { get; set; }
    }
}
