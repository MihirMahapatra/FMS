using AutoMapper;
using FMS.Api.Email.EmailService;
using FMS.Db.Context;
using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace FMS.Repository.Devloper
{

    public class DevloperRepo : IDevloperRepo
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<DevloperRepo> _logger;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        public DevloperRepo(ILogger<DevloperRepo> logger, AppDbContext appDbContext, IMapper mapper, IEmailService emailService, IHttpContextAccessor HttpContextAccessor)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _mapper = mapper;
            _emailService = emailService;
            _HttpContextAccessor = HttpContextAccessor;
        }
        #region Branch
        public async Task<Result<BranchModel>> GetAllBranch()
        {
            Result<BranchModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.Branches select s).ToListAsync();
                if (Query.Count > 0)
                {
                    var BranchList = _mapper.Map<List<BranchModel>>(Query);
                    _Result.CollectionObjData = BranchList;
                    _Result.Count = BranchList.Count;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<BranchModel>> GetBranchAccordingToUser(string UserId)
        {
            Result<BranchModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await (from userBranch in _appDbContext.UserBranches
                                   where userBranch.UserId == UserId
                                   join branch in _appDbContext.Branches on userBranch.BranchId equals branch.BranchId
                                   into branchGroup
                                   from branch in branchGroup
                                   select new BranchModel()
                                   {
                                       BranchName = branch.BranchName,
                                       BranchId = branch.BranchId
                                   }).FirstOrDefaultAsync();

                if (Query != null)
                {
                    //var UserBranchList = _mapper.Map<List<BranchModel>>(Query);
                    _Result.SingleObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateBranch(BranchModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.Branches where s.BranchName == data.BranchName select s).FirstOrDefaultAsync();
                if (Query == null)
                {
                    #region Create Branch
                    var newBranch = new Branch()
                    {
                        BranchName = data.BranchName,
                        BranchAddress = data.BranchAddress,
                        BranchCode = data.BranchCode,
                        ContactNumber = data.ContactNumber,
                    };
                    await _appDbContext.Branches.AddAsync(newBranch);
                    int count = await _appDbContext.SaveChangesAsync();
                    #endregion
                    if (count > 0)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateBranch(BranchModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.Branches where s.BranchId == data.BranchId select s).FirstOrDefaultAsync();
                if (Query != null)
                {
                    Query.BranchId = data.BranchId;
                    Query.BranchName = data.BranchName;
                    Query.BranchAddress = data.BranchAddress;
                    Query.ContactNumber = data.ContactNumber;
                    Query.BranchCode = data.BranchCode;
                    int count = await _appDbContext.SaveChangesAsync();
                    if (count > 0)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteBranch(Guid Id, IDbContextTransaction transaction)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.Branches.FirstOrDefaultAsync(x => x.BranchId == Id);
                        if (Query != null)
                        {
                            _appDbContext.Branches.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            if (count > 0)
                            {
                                _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted);
                            }
                        }
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        #endregion
        #region Financial Year
        public async Task<Result<FinancialYearModel>> GetFinancialYears(Guid BranchId)
        {
            Result<FinancialYearModel> _Result = new();
            List<FinancialYearModel> Query = null;
            try
            {
                _Result.IsSuccess = false;
                if (BranchId != Guid.Empty)
                {
                    Query = await (from s in _appDbContext.FinancialYears
                                   where s.FK_BranchId == BranchId
                                   orderby s.StartDate descending
                                   select new FinancialYearModel
                                   {
                                       FinancialYearId = s.FinancialYearId,
                                       Branch = s.Branch != null ? new BranchModel { BranchName = s.Branch.BranchName } : null,
                                       Financial_Year = s.Financial_Year,
                                       StartDate = s.StartDate.ToString(),
                                       EndDate = s.EndDate.ToString(),
                                   }).ToListAsync();
                }
                else
                {
                    Query = await (from s in _appDbContext.FinancialYears
                                   orderby s.StartDate descending
                                   group s by s.Financial_Year into g
                                   select new FinancialYearModel
                                   {
                                       Financial_Year = g.Key,
                                   }).ToListAsync();
                }
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Count = Query.Count;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<FinancialYearModel>> GetFinancialYears()
        {
            Result<FinancialYearModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.FinancialYears
                                   orderby s.StartDate descending
                                   select new FinancialYearModel
                                   {
                                       FinancialYearId = s.FinancialYearId,
                                       Branch = s.Branch != null ? new BranchModel { BranchName = s.Branch.BranchName } : null,
                                       Financial_Year = s.Financial_Year,
                                       StartDate = s.StartDate.ToString(),
                                       EndDate = s.EndDate.ToString(),
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Count = Query.Count;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateFinancialYear(FinancialYearModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;

                var Query = await (from s in _appDbContext.FinancialYears where s.Financial_Year == data.Financial_Year && s.FK_BranchId == data.FK_BranchId select s).FirstOrDefaultAsync();

                if (Query == null)
                {
                    if (DateTime.TryParseExact(data.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedStartDate) && DateTime.TryParseExact(data.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedEndDate))
                    {
                        var newYear = new FinancialYear()
                        {
                            Financial_Year = data.Financial_Year,
                            FK_BranchId = data.FK_BranchId,
                            StartDate = convertedStartDate,
                            EndDate = convertedEndDate
                        };
                        await _appDbContext.FinancialYears.AddAsync(newYear);
                        await _appDbContext.SaveChangesAsync();
                        //update stocks
                        var previousFinancialYearId = await _appDbContext.FinancialYears.OrderByDescending(fy => fy.StartDate).Skip(1).Select(fy => fy.FinancialYearId).FirstOrDefaultAsync();
                        if (previousFinancialYearId != Guid.Empty)
                        {
                            var PeviousYearStock = await _appDbContext.Stocks.Where(s => s.Fk_FinancialYear == previousFinancialYearId && s.Fk_BranchId == data.FK_BranchId).ToListAsync();
                            foreach (var item in PeviousYearStock)
                            {
                                var addOpeningStock = new Stock()
                                {
                                    Fk_ProductId = item.Fk_ProductId,
                                    Fk_FinancialYear = newYear.FinancialYearId,
                                    Fk_BranchId = item.Fk_BranchId,
                                    OpeningStock = item.AvilableStock,
                                    AvilableStock = item.AvilableStock,
                                    MinQty = item.MinQty,
                                    MaxQty = item.MaxQty,

                                };
                                await _appDbContext.Stocks.AddAsync(addOpeningStock);
                                await _appDbContext.SaveChangesAsync();
                            }
                        }
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateFinancialYear(FinancialYearModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.FinancialYears where s.FinancialYearId == data.FinancialYearId select s).FirstOrDefaultAsync();
                if (Query != null)
                {
                    if (DateTime.TryParseExact(data.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedStartDate) && DateTime.TryParseExact(data.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedEndDate))
                    {
                        Query.Financial_Year = data.Financial_Year;
                        Query.StartDate = convertedStartDate;
                        Query.EndDate = convertedEndDate;
                        int count = await _appDbContext.SaveChangesAsync();
                        if (count > 0)
                        {
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                        }
                    }

                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteFinancialYear(Guid Id, IDbContextTransaction transaction)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.FinancialYears.FirstOrDefaultAsync(x => x.FinancialYearId == Id);
                        if (Query != null)
                        {
                            _appDbContext.FinancialYears.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            if (count > 0)
                            {
                                _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted);
                            }
                        }
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }


            return _Result;
        }
        #endregion
        #region Accounting Setup
        #region LedgerGroup
        public async Task<Result<LedgerGroupModel>> GetLedgerGroups()
        {
            Result<LedgerGroupModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.LedgerGroups
                                   select new LedgerGroupModel
                                   {
                                       LedgerGroupId = s.LedgerGroupId,
                                       GroupName = s.GroupName
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"DevloperRepo/GetLedgerGroups : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateLedgerGroup(LedgerGroupModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.LedgerGroups.Where(s => s.GroupName == data.GroupName).FirstOrDefaultAsync();
                if (Query == null)
                {
                    var newLedgerGroup = _mapper.Map<LedgerGroup>(data);
                    await _appDbContext.LedgerGroups.AddAsync(newLedgerGroup);
                    int count = await _appDbContext.SaveChangesAsync();
                    if (count > 0)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"DevloperRepo/CreateLedgerGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateLedgerGroup(LedgerGroupModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;

                var Query = await _appDbContext.LedgerGroups.Where(s => s.LedgerGroupId == data.LedgerGroupId).FirstOrDefaultAsync();
                if (Query != null)
                {
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    if (count > 0)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"DevloperRepo/UpdateLedgerGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteLedgerGroup(Guid Id, IDbContextTransaction transaction)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.LedgerGroups.FirstOrDefaultAsync(x => x.LedgerGroupId == Id);
                        if (Query != null)
                        {
                            _appDbContext.LedgerGroups.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            if (count > 0)
                            {
                                _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted);
                            }
                        }
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"DevloperRepo/DeleteLedgerGroup : {_Exception.Message}");
            }                               
            return _Result;
        }
        #endregion
        #region LedgerSubGroup
        public async Task<Result<LedgerSubGroupModel>> GetLedgerSubGroups(Guid BranchId, Guid GroupId)
        {
            Result<LedgerSubGroupModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.LedgerSubGroupDevs
                                   where s.Fk_BranchId == BranchId && s.Fk_LedgerGroupId == GroupId
                                   select new LedgerSubGroupModel
                                   {
                                       LedgerSubGroupId = s.LedgerSubGroupId,
                                       SubGroupName = s.SubGroupName
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"DevloperRepo/GetLedgerSubGroups : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateLedgerSubGroup(LedgerSubGroupModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.LedgerSubGroupDevs.Where(s => s.SubGroupName == data.SubGroupName && s.Fk_LedgerGroupId == data.Fk_LedgerGroupId && s.Fk_BranchId == data.Fk_BranchId).FirstOrDefaultAsync();
                if (Query == null)
                {
                    var newLedgerSubGroup = _mapper.Map<LedgerSubGroupDev>(data);
                    await _appDbContext.LedgerSubGroupDevs.AddAsync(newLedgerSubGroup);
                    int count = await _appDbContext.SaveChangesAsync();
                    if (count > 0)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"DevloperRepo/CreateLedgerSubGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateLedgerSubGroup(LedgerSubGroupModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;

                var Query = await _appDbContext.LedgerSubGroupDevs.Where(s => s.LedgerSubGroupId == data.LedgerSubGroupId && s.Fk_BranchId == data.Fk_BranchId).FirstOrDefaultAsync();
                if (Query != null)
                {
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    if (count > 0)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"DevloperRepo/UpdateLedgerSubGroup : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteLedgerSubGroup(Guid BranchId, Guid Id, IDbContextTransaction transaction)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid Branch = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.LedgerSubGroupDevs.FirstOrDefaultAsync(x => x.LedgerSubGroupId == Id && x.Fk_BranchId == BranchId);
                        if (Query != null)
                        {
                            _appDbContext.LedgerSubGroupDevs.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            if (count > 0)
                            {
                                _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted);
                            }
                        }
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"DevloperRepo/DeleteLedgerSubGroup : {_Exception.Message}");
            }                                
            return _Result;
        }
        #endregion
        #region Ledger
        public async Task<Result<LedgerModel>> GetLedgers()
        {
            Result<LedgerModel> _Result = new();
            try
            {

                _Result.IsSuccess = false;
                var Query = await (from l in _appDbContext.LedgersDev
                                   select new LedgerModel
                                   {
                                       LedgerId = l.LedgerId,
                                       LedgerType = l.LedgerType,
                                       LedgerName = l.LedgerName,
                                       LedgerGroup = l.LedgerGroup != null ? new LedgerGroupModel { GroupName = l.LedgerGroup.GroupName } : null,
                                       LedgerSubGroup = l.LedgerSubGroup != null ? new LedgerSubGroupModel { SubGroupName = l.LedgerSubGroup.SubGroupName } : null
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    _Result.CollectionObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $" DevloperRepo/GetLedgers : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateLedger(List<LedgerModel> listData)
        {
            Result<bool> _Result = new();
            try
            {
                var itemsToRemove = new List<LedgerModel>();
                _Result.IsSuccess = false;
                using var transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    foreach (var item in listData)
                    {
                        var Query = await _appDbContext.LedgersDev.Where(s => s.Fk_LedgerGroupId == item.Fk_LedgerGroupId && s.LedgerName == item.LedgerName).FirstOrDefaultAsync();
                        if (Query != null)
                        {
                            var ledgerModelToRemove = listData.FirstOrDefault(x => x.Fk_LedgerGroupId == item.Fk_LedgerGroupId && x.LedgerName == item.LedgerName);
                            itemsToRemove.Add(ledgerModelToRemove);
                        }
                    }
                    foreach (var itemToRemove in itemsToRemove)
                    {
                        listData.Remove(itemToRemove);
                    }
                    var ledgers = _mapper.Map<List<LedgerDev>>(listData);
                    await _appDbContext.LedgersDev.AddRangeAsync(ledgers);
                    int count = await _appDbContext.SaveChangesAsync();
                    if (count > 0)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                    }
                    transaction.Commit();
                    _Result.IsSuccess = true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $" DevloperRepo/CreateLedger : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateLedger(LedgerModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;

                var Query = await _appDbContext.LedgersDev.Where(s => s.LedgerId == data.LedgerId).FirstOrDefaultAsync();
                if (Query != null)
                {
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    if (count > 0)
                    {
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $" DevloperRepo/UpdateLedger : {_Exception.Message}");
            }

            return _Result;
        }
        public async Task<Result<bool>> DeleteLedger(Guid Id, IDbContextTransaction transaction)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var localTransaction = transaction ?? await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (Id != Guid.Empty)
                    {
                        var Query = await _appDbContext.LedgersDev.FirstOrDefaultAsync(x => x.LedgerId == Id);
                        if (Query != null)
                        {
                            _appDbContext.LedgersDev.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            if (count > 0)
                            {
                                _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted);
                            }
                        }
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
                    }
                }
                catch
                {
                    localTransaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"DevloperRepo/DeleteLedger : {_Exception.Message}");
            }
            
            
            
            return _Result;
        }

        #endregion
        #endregion
    }
}
