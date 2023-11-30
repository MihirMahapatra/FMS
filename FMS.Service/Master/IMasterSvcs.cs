using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;

namespace FMS.Service.Master
{
    public interface IMasterSvcs
    {
        #region Product Master 
        #region Product Type
        Task<ProductTypeViewModel> GetProductTypes();
        #endregion
        #region Group
        Task<GroupViewModel> GetAllGroups();
        Task<Base> CreateGroup(GroupModel data);
        Task<Base> UpdateGroup(GroupModel data);
        Task<Base> DeleteGroup(Guid Id);
        #endregion
        #region SubGroup
        Task<SubGroupViewModel> GetSubGroups(Guid GroupId);
        Task<Base> CreateSubGroup(SubGroupModel data);
        Task<Base> UpdateSubGroup(SubGroupModel data);
        Task<Base> DeleteSubGroup(Guid Id);
        #endregion
        #region Unit
        Task<UnitViewModel> GetAllUnits();
        Task<Base> CreateUnit(UnitModel data);
        Task<Base> UpdateUnit(UnitModel data);
        Task<Base> DeleteUnit(Guid Id);
        #endregion
        #region Product
        Task<ProductViewModel> GetAllProducts();
        Task<ProductViewModel> GetProductByTypeId(Guid ProductTypeId);
        Task<ProductViewModel> GetProductGstWithRate(Guid id);
        Task<Base> CreateProduct(ProductModel data);
        Task<Base> UpdateProduct(ProductModel data);
        Task<Base> DeleteProduct(Guid Id);
        #endregion
        #region  Stock
        Task<StockViewModel> GetStocks();
        Task<ProductViewModel> GetProductsWhichNotInStock();
        Task<Base> CreateStock(StockModel data);
        Task<Base> UpdateStock(StockModel data);
        Task<Base> DeleteStock(Guid Id);
        #endregion
        #endregion
        #region labour Master
        #region Labour Type
        Task<LabourTypeViewModel> GetAllLabourTypes();
        Task<Base> CreateLabourType(LabourTypeModel data);
        Task<Base> UpdateLabourType(LabourTypeModel data);
        Task<Base> DeleteLabourType(Guid Id);
        #endregion
        #region Labour Details
        Task<LabourViewModel> GetAllLabourDetails();
        Task<LabourViewModel> GetLabourDetailById(Guid LabourId);
        Task<Base> CreateLabourDetail(LabourModel data);
        Task<Base> UpdateLabourDetail(LabourModel data);
        Task<Base> DeleteLabourDetail(Guid Id);
        #endregion
        #region Labour Rate Master
        Task<LabourRateViewModel> GetAllLabourRates();
        Task<Base> GetLabourRateByProductId(Guid ProductId);
        Task<Base> CreateLabourRate(LabourRateModel data);
        Task<Base> UpdateLabourRate(LabourRateModel data);
        Task<Base> DeleteLabourRate(Guid Id);
        #endregion
        #endregion   
        #region Account Master
        #region LedgerBalance
        Task<LedgerBalanceViewModel> GetLedgerBalances();
        Task<SubLedgerViewModel> GetSubLedgersByBranch(Guid LedgerId);
        Task<Base> UpdateLedgerBalance(LedgerBalanceModel data);
        Task<Base> CreateLedgerBalance(LedgerBalanceRequest data);
        Task<Base> DeleteLedgerBalance(Guid Id);
        #endregion
        #region Subledger
        Task<SubLedgerViewModel> GetSubLedgers();
        Task<SubLedgerViewModel> GetSubLedgersById(Guid LedgerId);
        Task<Base> CreateSubLedger(SubLedgerDataRequest Data);
        Task<Base> UpdateSubLedger(SubLedgerModel data);
        Task<Base> DeleteSubLedger(Guid Id);
        #endregion
        #region SubLedger Balance
        Task<SubLedgerBalanceViewModel> GetSubLedgerBalances();
        Task<Base> UpdateSubLedgerBalance(SubLedgerBalanceModel data);
        Task<Base> DeleteSubLedgerBalance(Guid Id);
        #endregion
        #endregion
        #region Party Master
        #region Party
        Task<PartyViewModel> GetParties();
        Task<Base> CreateParty(PartyModel data);
        Task<Base> UpdateParty(PartyModel data);
        Task<Base> DeleteParty(Guid Id);
        #endregion
        #region PartyType
        //Task<PartyTypeViewModel> GetPartyTypes(bool IsActive);
        //Task<Base> CreatePartyType(PartyTypeModel data);
        //Task<Base> UpdatePartyType(PartyTypeModel data, bool IsActive);
        //Task<Base> DeletePartyType(Guid Id, bool IsActive);
        #endregion
        #region State
        Task<StateViewModel> GetStates();
        Task<Base> CreateState(StateModel data);
        Task<Base> UpdateState(StateModel data);
        Task<Base> DeleteState(Guid Id);
        #endregion
        #region City
        Task<CityViewModel> GetCities(Guid Id);
        Task<Base> CreateCity(CityModel data);
        Task<Base> UpdateCity(CityModel data);
        Task<Base> DeleteCity(Guid Id);
        #endregion
        #endregion
    }
}
