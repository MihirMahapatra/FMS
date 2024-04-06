using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Db.DbEntity
{
    public class Referance
    {
        public Guid RefaranceId { get; set; }
        public string ReferanceName { get; set; }
        public ICollection<Party> Parties { get; set; }
    }
}
