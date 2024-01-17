using AutoMapper;
using FMS.Db.DbEntity;
using FMS.Model.CommonModel;

namespace FMS.Model.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Branch, BranchModel>();
            CreateMap<BranchModel, Branch>();
            CreateMap<BranchFinancialYear, BranchFinancialYearModel>();
            CreateMap<ProductType, ProductTypeModel>();
            CreateMap<ProductTypeModel, ProductType>();
            CreateMap<ProductGroup, GroupModel>();
            CreateMap<GroupModel, ProductGroup>();
            CreateMap<ProductSubGroup, SubGroupModel>();
            CreateMap<SubGroupModel, ProductSubGroup>();
            CreateMap<Unit, UnitModel>();
            CreateMap<UnitModel, Unit>();
            CreateMap<AlternateUnit, AlternateUnitModel>();
            CreateMap<AlternateUnitModel, AlternateUnit>();
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();
            CreateMap<LabourType, LabourTypeModel>();
            CreateMap<LabourTypeModel, LabourType>();
            CreateMap<Labour, LabourModel>();
            CreateMap<LabourModel, Labour>();
            CreateMap<LabourRate, LabourRateModel>();
            CreateMap<LabourRateModel, LabourRate>();
            CreateMap<UserBranch, UserBranchModel>()
               .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch != null ? new BranchModel { BranchName = src.Branch.BranchName } : null))
               .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User != null ? new UserModel { UserName = src.User.Name } : null));
            CreateMap<UserBranchModel, UserBranch>();
            CreateMap<LedgerGroup, LedgerGroupModel>();
            CreateMap<LedgerGroupModel, LedgerGroup>();
            CreateMap<LedgerSubGroup, LedgerSubGroupModel>();
            CreateMap<LedgerSubGroupModel, LedgerSubGroup>();
            CreateMap<LedgerSubGroupDev, LedgerSubGroupModel>();
            CreateMap<LedgerSubGroupModel, LedgerSubGroupDev>();
            CreateMap<Ledger, LedgerModel>();
            CreateMap<LedgerModel, Ledger>();
            CreateMap<LedgerDev, LedgerModel>();
            CreateMap<LedgerModel, LedgerDev>();
            CreateMap<SubLedger, SubLedgerModel>();
            CreateMap<SubLedgerModel, SubLedger>();
            CreateMap<Party, PartyModel>();
            //.ForMember(dest => dest.PartyType, opt => opt.MapFrom(src => src.PartyType != null ? new PartyTypeModel { Party_Type = src.PartyType.Party_Type } : null))
            //.ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State != null ? new StateModel { StateName = src.State.StateName } : null))
            //.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City != null ? new CityModel { CityName = src.City.CityName } : null));
            CreateMap<PartyModel, Party>();
            CreateMap<State, StateModel>();
            CreateMap<StateModel, State>();
            CreateMap<City, CityModel>();
            CreateMap<CityModel, City>();
        }
    }
}
