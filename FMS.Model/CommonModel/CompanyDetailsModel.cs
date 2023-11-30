using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class CompanyDetailsModel
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GSTIN { get; set; }
        public Guid Fk_BranchId { get; set; }
        public string BranchName { get; set; }
        public string logo { get; set; }
    }
}
