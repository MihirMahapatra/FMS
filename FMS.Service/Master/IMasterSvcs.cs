using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;

namespace FMS.Service.Master
{
    public interface IMasterSvcs
    {
        #region Stock Master 
        Task<StockViewModel> GetStocks();
        Task<ProductViewModel> GetProductsWhichNotInStock(Guid GroupId, Guid SubGroupId);
        Task<Base> CreateStock(StockModel data);
        Task<Base> UpdateStock(StockModel data);
        Task<Base> DeleteStock(Guid Id);   
        #endregion
        #region labour Master
        #region Labour Type
        Task<LabourTypeViewModel> GetAllLabourTypes();
        #endregion
        #region Labour Details
        Task<LabourViewModel> GetAllLabourDetails();
        Task<LabourViewModel> GetLabourDetailById(Guid LabourId);
        Task<LabourViewModel> GetLaboursByLabourTypeId(Guid LabourTypeId);
        Task<Base> CreateLabourDetail(LabourModel data);
        Task<Base> UpdateLabourDetail(LabourModel data);
        Task<Base> DeleteLabourDetail(Guid Id);
        #endregion
        #endregion   
        #region Party Master
        #region Party
        Task<PartyViewModel> GetParties();
        Task<Base> CreateParty(PartyModel data);
        Task<Base> UpdateParty(PartyModel data);
        Task<Base> DeleteParty(Guid Id);
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
        Task<Base> CreateSubLedgerBalance(SubLedgerBalanceModel data);
        Task<Base> UpdateSubLedgerBalance(SubLedgerBalanceModel data);
        Task<Base> DeleteSubLedgerBalance(Guid Id);
        #endregion
        #endregion       
    }
}
