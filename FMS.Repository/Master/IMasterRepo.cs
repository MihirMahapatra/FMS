using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using Microsoft.EntityFrameworkCore.Storage;

namespace FMS.Repository.Master
{
    public interface IMasterRepo
    {
        #region Stock Master
      
        Task<Result<StockModel>> GetStocks();
        Task<Result<ProductModel>> GetProductsWhichNotInStock(Guid GroupId, Guid SubGroupId);
        Task<Result<bool>> CreateStock(StockModel data);
        Task<Result<bool>> UpdateStock(StockModel data);
        Task<Result<bool>> DeleteStock(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region labour Master
        #region Labour Type
        Task<Result<LabourTypeModel>> GetAllLabourTypes();
        #endregion
        #region Labour Details
        Task<Result<LabourModel>> GetAllLabourDetails();
        Task<Result<LabourModel>> GetLabourDetailById(Guid LabourId);
        Task<Result<bool>> CreateLabourDetail(LabourModel data);
        Task<Result<bool>> UpdateLabourDetail(LabourModel data);
        Task<Result<bool>> DeleteLabourDetail(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #endregion
        #region Party Master
        #region Party
        Task<Result<PartyModel>> GetParties();
        Task<Result<bool>> CreateParty(PartyModel data);
        Task<Result<bool>> UpdateParty(PartyModel data);
        Task<Result<bool>> DeleteParty(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region State
        Task<Result<StateModel>> GetStates();
        Task<Result<bool>> CreateState(StateModel data);
        Task<Result<bool>> UpdateState(StateModel data);
        Task<Result<bool>> DeleteState(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region City
        Task<Result<CityModel>> GetCities(Guid Id);
        Task<Result<bool>> CreateCity(CityModel data);
        Task<Result<bool>> UpdateCity(CityModel data);
        Task<Result<bool>> DeleteCity(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #endregion
        #region Account Master
        #region LedgerBalance
        Task<Result<LedgerBalanceModel>> GetLedgerBalances();
        Task<Result<SubLedgerModel>> GetSubLedgersByBranch(Guid LedgerId);
        Task<Result<bool>> UpdateLedgerBalance(LedgerBalanceModel data);
        Task<Result<bool>> CreateLedgerBalance(LedgerBalanceRequest data);
        Task<Result<bool>> DeleteLedgerBalance(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region SubLedger
        Task<Result<SubLedgerModel>> GetSubLedgers();
        Task<Result<SubLedgerModel>> GetSubLedgersById(Guid LedgerId);
        Task<Result<bool>> CreateSubLedger(SubLedgerDataRequest data);
        Task<Result<bool>> UpdateSubLedger(SubLedgerModel data);
        Task<Result<bool>> DeleteSubLedger(Guid id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #region SubLedger Balance
        Task<Result<SubLedgerBalanceModel>> GetSubLedgerBalances();
        Task<Result<bool>> UpdateSubLedgerBalance(SubLedgerBalanceModel data);
        Task<Result<bool>> DeleteSubLedgerBalance(Guid Id, IDbContextTransaction transaction, bool IsCallBack);
        #endregion
        #endregion
        
    }
}
