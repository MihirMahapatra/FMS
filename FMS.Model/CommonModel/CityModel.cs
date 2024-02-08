namespace FMS.Model.CommonModel
{
    public class CityModel : Base
    {
        public Guid CityId { get; set; }
        public Guid Fk_StateId { get; set; }
        public string CityName { get; set; }
        public StateModel State { get; set; }
        public List<PartyModel> Parties { get; set; }
    }
}
