﻿
namespace FMS.Db.DbEntity
{
    public class CompanyDetails
    {
        public Guid CompanyId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GSTIN { get; set; }
        public Branch Branch { get; set; }
        public string logo { get; set; }
    }
}
