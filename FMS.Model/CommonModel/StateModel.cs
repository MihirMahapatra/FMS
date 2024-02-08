namespace FMS.Model.CommonModel
{
    public class StateModel : Base
    {
        public Guid StateId { get; set; }
        public string StateName { get; set; }
        public List<PartyModel> Parties { get; set; } 
        public List<CityModel> Cities { get; set; } 
    }
}
