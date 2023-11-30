using FMS.Model.CommonModel;
namespace FMS.Model.ViewModel
{
    public class CityViewModel : Base
    {
        public CityViewModel()
        {
            Cities = new List<CityModel>();
            City = new CityModel();
        }
        public List<CityModel> Cities { get; set; }
        public CityModel City { get; set; }
    }
}
