using FMS.Db.DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class PartyGroupModel
    {
        public Guid PartyGroupId { get; set; }
        public string PartyGruopName { get; set; }
        public List<PartyModel> Parties { get; set; }
    }
    public class PartyGroupViewModel : Base
    {
        public PartyGroupViewModel()
        {
            PartyGruops = new List<PartyGroupModel>();
            PartyGruop = new PartyGroupModel();
        }
        public List<PartyGroupModel> PartyGruops { get; set; }
        public PartyGroupModel PartyGruop { get; set; }
    }
}
