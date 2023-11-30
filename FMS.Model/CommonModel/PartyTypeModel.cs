namespace FMS.Model.CommonModel
{
    public class PartyTypeModel : Base
    {
        public Guid PartyTypeId { get; set; }
        public string Party_Type { get; set; }
        public Guid Fk_BranchId { get; set; }
        public BranchModel Branch { get; set; }
        public List<PartyModel> Parties { get; set; }
    }
}
