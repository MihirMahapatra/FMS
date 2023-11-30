namespace FMS.Model.CommonModel
{
    public class SubGroupModel : Base
    {
        public Guid SubGroupId { get; set; }
        public Guid? Fk_GroupId { get; set; }
        public string SubGroupName { get; set; }
        public GroupModel Group { get; set; }
        public List<ProductModel> products { get; set; } 
    }
}
