using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Db.DbEntity
{
    public class PartyGroup
    {
        public Guid PartyGroupId { get; set; }
        public string PartyGroupName { get; set; }
        public ICollection<Party> Parties { get; set; }

    }
}
