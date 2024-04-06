using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class ReferanceModel
    {
        public Guid RefaranceId { get; set; }
        public string ReferanceName { get; set; }
        public List<PartyModel> Parties { get; set; }
    }
    public class ReferanceViewModel : Base
    {
        public ReferanceViewModel()
        {
            Referances = new List<ReferanceModel>();
            Referance = new ReferanceModel();
        }
        public List<ReferanceModel> Referances { get; set; }
        public ReferanceModel Referance { get; set; }
    }
}
