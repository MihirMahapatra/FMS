using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class PartyReportDataRequest
    {
        public Guid PartyId { get; set; }
        public Guid Fk_SubledgerId {  get; set; }
        public Guid Fk_PartyGroupId { get; set; }
        public Guid Fk_RefaranceId { get; set; }
        public string FromDate {  get; set; }
        public string ToDate { get; set; }
        public string Type { get; set; }
    }
}
