using AutoMapper;
using FMS.Api.Email.EmailService;
using FMS.Db.Context;
using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Repository.Transaction;
using FMS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Group = FMS.Db.DbEntity.ProductGroup;

namespace FMS.Repository.Master
{
    public class MasterRepo : IMasterRepo
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<MasterRepo> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly ITransactionRepo _transactionRepo;
        public MasterRepo(ILogger<MasterRepo> logger, AppDbContext appDbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService, ITransactionRepo transactionRepo)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _mapper = mapper;
            _HttpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _transactionRepo = transactionRepo;
        }
        #region Account Master
        #region LedgerBalance
        public async Task<Result<LedgerBalanceModel>> GetLedgerBalances()
        {
            Result<LedgerBalanceModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var Query = await _appDbContext.LedgerBalances.Where(s => s.Fk_BranchId == BranchId).Select(s => new LedgerBalanceModel
                {
                    LedgerBalanceId = s.LedgerBalanceId,
                    Ledger = _appDbContext.Ledgers.Any(l => l.LedgerId == s.Fk_LedgerId)
                        ? _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => new LedgerModel
                        {
                            LedgerName = l.LedgerName,
                            LedgerGroup = new LedgerGroupModel { GroupName = l.LedgerGroup.GroupName }
                        }).SingleOrDefault()
                        : _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => new LedgerModel
                        {
                            LedgerName = l.LedgerName,
                            LedgerGroup = new LedgerGroupModel { GroupName = l.LedgerGroup.GroupName }
                        }).SingleOrDefault(),
                    OpeningBalance = s.OpeningBalance,
                    OpeningBalanceType = s.OpeningBalanceType,
                    RunningBalance = s.RunningBalance,
                    RunningBalanceType = s.RunningBalanceType,
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetLedgerBalances : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<SubLedgerModel>> GetSubLedgersByBranch(Guid LedgerId)
        {
            Result<SubLedgerModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await (from sl in _appDbContext.SubLedgers
                                   where sl.Fk_LedgerId == LedgerId && sl.Fk_BranchId == BranchId
                                   && !_appDbContext.SubLedgerBalances.Any(sb => sb.Fk_SubLedgerId == sl.SubLedgerId && sb.Fk_BranchId == BranchId && sb.Fk_FinancialYearId == FinancialYear)
                                   select new SubLedgerModel()
                                   {
                                       SubLedgerId = sl.SubLedgerId,
                                       SubLedgerName = sl.SubLedgerName,
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetSubLedgersByBranch : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateLedgerBalance(LedgerBalanceRequest data)
        {
            Result<bool> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                using var transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    var Query = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == data.Fk_LedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                    if (Query == null)
                    {
                        var newLedgerBalance = new LedgerBalance
                        {
                            Fk_LedgerId = data.Fk_LedgerId,
                            OpeningBalanceType = data.OpeningBalanceType,
                            OpeningBalance = data.OpeningBalanceType == "Dr" ? data.OpeningBalance : -data.OpeningBalance,
                            RunningBalanceType = data.OpeningBalanceType,
                            RunningBalance = data.OpeningBalanceType == "Dr" ? data.OpeningBalance : -data.OpeningBalance,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYear = FinancialYear
                        };
                        await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                        await _appDbContext.SaveChangesAsync();
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                        transaction.Commit();
                        _Result.IsSuccess = true;
                    }
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateLedgerBalance : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateLedgerBalance(LedgerBalanceModel data)
        {
            Result<bool> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var Query = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == data.LedgerBalanceId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                if (Query != null)
                {
                    Query.OpeningBalanceType = data.OpeningBalanceType;
                    Query.OpeningBalance = data.OpeningBalanceType == "Dr" ? data.OpeningBalance : -data.OpeningBalance;
                    Query.RunningBalanceType = data.RunningBalanceType;
                    Query.RunningBalance = data.RunningBalanceType == "Dr" ? data.RunningBalance : -data.RunningBalance;

                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateLedgerBalance : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteLedgerBalance(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
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
                        var Query = await _appDbContext.LedgerBalances.SingleOrDefaultAsync(x => x.LedgerBalanceId == Id && x.Fk_BranchId == BranchId);
                        if (Query != null)
                        {
                            _appDbContext.LedgerBalances.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteLedgerBalance : {_Exception.Message}");
            }

            return _Result;
        }
        #endregion
        #region SubLedger
        public async Task<Result<SubLedgerModel>> GetSubLedgers()
        {
            Result<SubLedgerModel> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                _Result.IsSuccess = false;
                SubLedgerViewModel models = new();
                var Query1 = await (from sl in _appDbContext.SubLedgers
                                    join l in _appDbContext.Ledgers on sl.Fk_LedgerId equals l.LedgerId
                                    where sl.Fk_BranchId == BranchId || sl.Fk_BranchId == null
                                    select new SubLedgerModel()
                                    {
                                        SubLedgerId = sl.SubLedgerId,
                                        SubLedgerName = sl.SubLedgerName,
                                        LedgerName = l.LedgerName
                                    }).ToListAsync();
                models.SubLedgers.AddRange(Query1);
                var Query2 = await (from sl in _appDbContext.SubLedgers
                                    join l in _appDbContext.LedgersDev on sl.Fk_LedgerId equals l.LedgerId
                                    where sl.Fk_BranchId == BranchId || sl.Fk_BranchId == null
                                    select new SubLedgerModel()
                                    {
                                        SubLedgerId = sl.SubLedgerId,
                                        SubLedgerName = sl.SubLedgerName,
                                        LedgerName = l.LedgerName
                                    }).ToListAsync();
                models.SubLedgers.AddRange(Query2);

                if (models.SubLedgers.Count > 0)
                {
                    _Result.CollectionObjData = models.SubLedgers;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/GetSubLedgers : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<SubLedgerModel>> GetSubLedgersById(Guid LedgerId)
        {
            Result<SubLedgerModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var Query = await (from sl in _appDbContext.SubLedgers
                                   where sl.Fk_LedgerId == LedgerId && sl.Fk_BranchId == BranchId
                                   select new SubLedgerModel()
                                   {
                                       SubLedgerId = sl.SubLedgerId,
                                       SubLedgerName = sl.SubLedgerName,
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/GetSubLedgersById : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateSubLedger(SubLedgerDataRequest data)
        {
            Result<bool> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                using var transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    var getLedgerBalanceExist = await _appDbContext.LedgerBalances.Where(x => x.Fk_LedgerId == data.Fk_LedgerId && x.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                    if (getLedgerBalanceExist != null)
                    {
                        #region SubLedger
                        var NewSubLedger = new SubLedger
                        {
                            Fk_LedgerId = data.Fk_LedgerId,
                            SubLedgerName = data.SubLedgerName,
                            Fk_BranchId = BranchId,
                        };
                        await _appDbContext.SubLedgers.AddAsync(NewSubLedger);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        #region SubLedgerBalance
                        var NewSubLedgerBalance = new SubLedgerBalance
                        {
                            Fk_LedgerBalanceId = getLedgerBalanceExist.LedgerBalanceId,
                            Fk_SubLedgerId = NewSubLedger.SubLedgerId,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear,
                            OpeningBalance = data.BalanceType == "Dr" ? data.OpeningBalance : -data.OpeningBalance,
                            OpeningBalanceType = data.BalanceType,
                            RunningBalance = data.BalanceType == "Dr" ? data.OpeningBalance : -data.OpeningBalance,
                            RunningBalanceType = data.BalanceType,
                        };
                        await _appDbContext.SubLedgerBalances.AddAsync(NewSubLedgerBalance);
                        await _appDbContext.SaveChangesAsync();
                        #endregion 
                        getLedgerBalanceExist.RunningBalance += data.BalanceType == "Dr" ? data.OpeningBalance : -data.OpeningBalance;
                        getLedgerBalanceExist.RunningBalanceType = getLedgerBalanceExist.RunningBalance > 0 ? "Dr" : "Cr";
                        await _appDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        #region LedgerBalance
                        var NewLedgerBalance = new LedgerBalance
                        {
                            Fk_LedgerId = data.Fk_LedgerId,
                            OpeningBalance = data.BalanceType == "Dr" ? data.OpeningBalance : -data.OpeningBalance,
                            OpeningBalanceType = data.BalanceType,
                            RunningBalance = data.BalanceType == "Dr" ? data.OpeningBalance : -data.OpeningBalance,
                            RunningBalanceType = data.BalanceType,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYear = FinancialYear,
                        };
                        await _appDbContext.LedgerBalances.AddAsync(NewLedgerBalance);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        #region SubLedger
                        var NewSubLedger = new SubLedger
                        {
                            Fk_LedgerId = data.Fk_LedgerId,
                            SubLedgerName = data.SubLedgerName,
                            Fk_BranchId = BranchId,
                        };
                        await _appDbContext.SubLedgers.AddAsync(NewSubLedger);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        #region SubLedgerBalance
                        var NewSubLedgerBalance = new SubLedgerBalance
                        {
                            Fk_LedgerBalanceId = NewLedgerBalance.LedgerBalanceId,
                            Fk_SubLedgerId = NewSubLedger.SubLedgerId,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear,
                            OpeningBalance = data.BalanceType == "Dr" ? data.OpeningBalance : -data.OpeningBalance,
                            OpeningBalanceType = data.BalanceType,
                            RunningBalance = data.BalanceType == "Dr" ? data.OpeningBalance : -data.OpeningBalance,
                            RunningBalanceType = data.BalanceType,
                        };
                        await _appDbContext.SubLedgerBalances.AddAsync(NewSubLedgerBalance);
                        await _appDbContext.SaveChangesAsync();
                        #endregion 
                    }
                    transaction.Commit();
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $" AdminRepo/CreateSubLedger : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateSubLedger(SubLedgerModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.SubLedgers.Where(s => s.SubLedgerId == data.SubLedgerId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                if (Query != null)
                {
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AdminRepo/UpdateSubLedger : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteSubLedger(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
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
                        var Query = await _appDbContext.SubLedgers.SingleOrDefaultAsync(x => x.SubLedgerId == Id);
                        if (Query != null)
                        {
                            _appDbContext.SubLedgers.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $" AdminRepo/DeleteSubLedger : {_Exception.Message}");
            }

            return _Result;
        }
        #endregion
        #region SubLedger Balance
        public async Task<Result<SubLedgerBalanceModel>> GetSubLedgerBalances()
        {
            Result<SubLedgerBalanceModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var Query = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_BranchId == BranchId).Select(s => new SubLedgerBalanceModel
                {
                    SubLedgerBalanceId = s.SubLedgerBalanceId,
                    SubLedger = s.SubLedger != null ? new SubLedgerModel { SubLedgerName = s.SubLedger.SubLedgerName } : null,
                    LedgerBalance = s.LedgerBalance != null ? new LedgerBalanceModel
                    {
                        Ledger = _appDbContext.Ledgers.Any(l => l.LedgerId == s.LedgerBalance.Fk_LedgerId)
                        ? _appDbContext.Ledgers.Where(l => l.LedgerId == s.LedgerBalance.Fk_LedgerId).Select(l => new LedgerModel
                        {
                            LedgerName = l.LedgerName
                        }).SingleOrDefault()
                        : _appDbContext.LedgersDev.Where(l => l.LedgerId == s.LedgerBalance.Fk_LedgerId).Select(l => new LedgerModel
                        {
                            LedgerName = l.LedgerName
                        }).SingleOrDefault()
                    } : null,
                    OpeningBalance = s.OpeningBalance,
                    OpeningBalanceType = s.OpeningBalanceType,
                    RunningBalance = s.RunningBalance,
                    RunningBalanceType = s.RunningBalanceType
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetSubLedgerBalances : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateSubLedgerBalance(SubLedgerBalanceModel data)
        {
            Result<bool> _Result = new();
            Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
            Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            _Result.IsSuccess = false;
            try
            {
                var isSubLedgerBalanceExist = await _appDbContext.SubLedgerBalances.Where(s => s.SubLedgerBalanceId == data.SubLedgerBalanceId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                if (isSubLedgerBalanceExist != null)
                {
                    decimal difference = 0;
                    //var difference = (data.OpeningBalanceType == "Dr" ? data.OpeningBalance : -data.OpeningBalance) -
                    //                 (isSubLedgerBalanceExist.OpeningBalanceType == "Dr"
                    //                     ? isSubLedgerBalanceExist.OpeningBalance
                    //                     : isSubLedgerBalanceExist.OpeningBalanceType == "Cr"
                    //                         ? -isSubLedgerBalanceExist.OpeningBalance
                    //                         : isSubLedgerBalanceExist.OpeningBalance);
                    if (data.OpeningBalanceType == isSubLedgerBalanceExist.OpeningBalanceType)
                    {
                        if (data.OpeningBalance > isSubLedgerBalanceExist.OpeningBalance)
                        {
                            difference = (data.OpeningBalanceType == "Dr" ? 1 : -1) * data.OpeningBalance - (isSubLedgerBalanceExist.OpeningBalanceType == "Dr" ? 1 : -1) * isSubLedgerBalanceExist.OpeningBalance;
                        }
                        else
                        {
                            difference = (data.OpeningBalanceType == "Dr" ? 1 : -1) * data.OpeningBalance - (isSubLedgerBalanceExist.OpeningBalanceType == "Dr" ? 1 : -1) * isSubLedgerBalanceExist.OpeningBalance;

                        }
                    }
                    else
                    {
                        difference = (data.OpeningBalanceType == "Dr" ? 1 : -1) * data.OpeningBalance + (isSubLedgerBalanceExist.OpeningBalanceType == "Dr" ? 1 : -1) * isSubLedgerBalanceExist.OpeningBalance;
                    }
                    var isLedgerBalanceExist = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == isSubLedgerBalanceExist.Fk_LedgerBalanceId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                    if (isLedgerBalanceExist != null)
                    {
                        isLedgerBalanceExist.OpeningBalance += data.OpeningBalance > isSubLedgerBalanceExist.OpeningBalance ? (data.OpeningBalance - difference) :(difference-data.OpeningBalance);
                        isLedgerBalanceExist.OpeningBalanceType = isLedgerBalanceExist.OpeningBalance > 0 ? "Dr" : "Cr";
                        isLedgerBalanceExist.RunningBalance += difference;
                        isLedgerBalanceExist.RunningBalanceType = isLedgerBalanceExist.RunningBalance > 0 ? "Dr" : "Cr";
                    }
                    #region Update SubLedger Balance &&  Ledger Balances
                    isSubLedgerBalanceExist.OpeningBalance = data.OpeningBalance;
                    isSubLedgerBalanceExist.OpeningBalanceType = data.OpeningBalanceType;
                    isSubLedgerBalanceExist.RunningBalance += difference;
                    isSubLedgerBalanceExist.RunningBalanceType = isSubLedgerBalanceExist.RunningBalance > 0 ? "Dr" : "Cr";



                    await _appDbContext.SaveChangesAsync();
                    #endregion
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                    transaction.Commit();
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                transaction.Rollback();
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateSubLedgerBalance : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteSubLedgerBalance(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
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
                        var Query = await _appDbContext.SubLedgerBalances.SingleOrDefaultAsync(x => x.SubLedgerBalanceId == Id && x.Fk_BranchId == BranchId);
                        if (Query != null)
                        {
                            _appDbContext.SubLedgerBalances.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteSubLedgerBalance : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #endregion
        #region Stock Master
        public async Task<Result<StockModel>> GetStocks()
        {
            Result<StockModel> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.Stocks
                                   where s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear
                                   select new StockModel
                                   {
                                       StockId = s.StockId,
                                       MinQty = s.MinQty,
                                       MaxQty = s.MaxQty,
                                       AvilableStock = s.AvilableStock,
                                       OpeningStock = s.OpeningStock,
                                       Rate = s.Rate,
                                       Amount = s.Amount,
                                       Product = s.Product != null ? new ProductModel { ProductName = s.Product.ProductName } : null,
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    var StockList = Query;
                    _Result.CollectionObjData = StockList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetStocks : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<ProductModel>> GetProductsWhichNotInStock(Guid GroupId, Guid SubGroupId)
        {
            Result<ProductModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;

                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await (from p in _appDbContext.Products
                                   join s in _appDbContext.Stocks
                                   on p.ProductId equals s.Fk_ProductId
                                   into stockGroup
                                   where !stockGroup.Any(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear) && p.Fk_ProductGroupId == GroupId && p.Fk_ProductSubGroupId == (SubGroupId == Guid.Empty ? null : SubGroupId)
                                   select new ProductModel()
                                   {
                                       ProductId = p.ProductId,
                                       ProductName = p.ProductName
                                   }).ToListAsync();

                if (Query.Count > 0)
                {
                    var ProductList = Query;
                    _Result.CollectionObjData = ProductList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetProductsWhichNotInStock : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateStock(StockModel data)
        {
            Result<bool> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                using var transaction = _appDbContext.Database.BeginTransaction();
                try
                {
                    var Query = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == data.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                    if (Query == null)
                    {
                        var newStock = new Stock
                        {
                            OpeningStock = data.OpeningStock,
                            AvilableStock = data.OpeningStock,
                            MinQty = data.MinQty,
                            MaxQty = data.MaxQty,
                            Rate = data.Rate,
                            Amount = data.OpeningStock * data.Rate,
                            Fk_BranchId = BranchId,
                            Fk_ProductId = data.Fk_ProductId,
                            Fk_FinancialYear = FinancialYear
                        };
                        _appDbContext.Stocks.Add(newStock);
                        await _appDbContext.SaveChangesAsync();
                        transaction.Commit();
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                    }
                    _Result.IsSuccess = true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateStock : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateStock(StockModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;

                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var transaction = _appDbContext.Database.BeginTransaction();
                try
                {
                    var stock = await _appDbContext.Stocks.Where(s => s.StockId == data.StockId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).FirstOrDefaultAsync();
                    if (stock != null)
                    {

                        if (stock.OpeningStock != data.OpeningStock)
                        {
                            if (stock.OpeningStock > data.OpeningStock)
                            {
                                stock.AvilableStock = data.AvilableStock - (stock.OpeningStock - data.OpeningStock);
                            }
                            else
                            {
                                stock.AvilableStock = data.AvilableStock + (data.OpeningStock - stock.OpeningStock);
                            }
                        }
                        else
                        {
                            stock.AvilableStock = data.AvilableStock;
                        }
                        stock.OpeningStock = data.OpeningStock;
                        stock.Rate = data.Rate;
                        stock.Amount = data.OpeningStock * data.Rate;
                        stock.Fk_FinancialYear = FinancialYear;
                        stock.Fk_ProductId = data.Fk_ProductId;
                        stock.Fk_BranchId = BranchId;
                        stock.MinQty = data.MinQty;
                        stock.MaxQty = data.MaxQty;
                        await _appDbContext.SaveChangesAsync();
                        transaction.Commit();
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                    }
                    _Result.IsSuccess = true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Exception", $"MasterRepo/UpdateStock : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteStock(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
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
                        var Query = await _appDbContext.Stocks.FirstOrDefaultAsync(x => x.StockId == Id);
                        if (Query != null)
                        {
                            _appDbContext.Stocks.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteStock : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Party Master
        #region Party
        public async Task<Result<PartyModel>> GetParties()
        {
            Result<PartyModel> _Result = new();
            try
            {
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Parties.Select(s =>
                                   new PartyModel()
                                   {
                                       PartyId = s.PartyId,
                                       PartyName = s.PartyName,
                                       Phone = s.Phone,
                                       Email = s.Email,
                                       Address = s.Address,
                                       GstNo = s.GstNo,
                                       Ledger = s.LedgerDev != null ? new LedgerModel { LedgerName = s.LedgerDev.LedgerName } : null,
                                       State = s.State != null ? new StateModel { StateName = s.State.StateName } : null,
                                       City = s.City != null ? new CityModel { CityName = s.City.CityName } : null,
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetParties : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateParty(PartyModel data)
        {
            Result<bool> _Result = new();
            try
            {
                using var transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                    Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    _Result.IsSuccess = false;
                    var existingParty = await _appDbContext.Parties.FirstOrDefaultAsync(s => s.PartyName == data.PartyName && s.Fk_PartyType == data.Fk_PartyType);
                    if (existingParty == null)
                    {
                        #region create SubLedger
                        var newSubLedger = new SubLedger
                        {
                            Fk_LedgerId = data.Fk_PartyType,
                            SubLedgerName = data.PartyName,
                        };
                        await _appDbContext.SubLedgers.AddAsync(newSubLedger);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        #region Check LedgerBalance Exist 
                        var isledgerBalanceExist = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == data.Fk_PartyType && s.Fk_FinancialYear == FinancialYear && s.Fk_BranchId == BranchId).FirstOrDefaultAsync();
                        Guid LedgerBalanceId = Guid.Empty;
                        if (isledgerBalanceExist == null)
                        {
                            var newLedgerBalance = new LedgerBalance
                            {
                                Fk_LedgerId = data.Fk_PartyType,
                                OpeningBalance = 0,
                                OpeningBalanceType = data.Fk_PartyType == MappingLedgers.SundryCreditors ? "Cr" : "Dr",
                                RunningBalance = 0,
                                RunningBalanceType = data.Fk_PartyType == MappingLedgers.SundryCreditors ? "Cr" : "Dr",
                                Fk_FinancialYear = FinancialYear,
                                Fk_BranchId = BranchId
                            };

                            await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                            await _appDbContext.SaveChangesAsync();
                            LedgerBalanceId = newLedgerBalance.LedgerBalanceId;
                        }
                        else
                        {
                            LedgerBalanceId = isledgerBalanceExist.LedgerBalanceId;
                        }
                        #endregion
                        #region Create SubLedger Balance
                        var newSubLedgerBalance = new SubLedgerBalance
                        {
                            Fk_LedgerBalanceId = LedgerBalanceId,
                            Fk_SubLedgerId = newSubLedger.SubLedgerId,
                            OpeningBalanceType = data.Fk_PartyType == MappingLedgers.SundryCreditors ? "Cr" : "Dr",
                            OpeningBalance = 0,
                            RunningBalanceType = data.Fk_PartyType == MappingLedgers.SundryCreditors ? "Cr" : "Dr",
                            RunningBalance = 0,
                            Fk_FinancialYearId = FinancialYear,
                            Fk_BranchId = BranchId
                        };
                        await _appDbContext.SubLedgerBalances.AddAsync(newSubLedgerBalance);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        #region Create Party
                        data.Fk_SubledgerId = newSubLedger.SubLedgerId;
                        var newParty = _mapper.Map<Party>(data);
                        await _appDbContext.Parties.AddAsync(newParty);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        transaction.Commit();
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                        _Result.IsSuccess = true;
                    }
                    else
                    {
                        _Result.WarningMessage = "A Party With Same Name Already Exist";
                    }
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $" MasterRepo/CreateParty : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateParty(PartyModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Parties.FirstOrDefaultAsync(s => s.PartyId == data.PartyId);
                if (Query != null)
                {
                    Query.Address = data.Address;
                    Query.Email = data.Email;
                    Query.Fk_CityId = data.Fk_CityId;
                    Query.Fk_StateId = data.Fk_StateId;
                    Query.Fk_PartyType = data.Fk_PartyType;
                    Query.GstNo = data.GstNo;
                    Query.PartyName = data.PartyName;
                    Query.Phone = data.Phone;
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateParty : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteParty(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
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
                        var Query = await _appDbContext.Parties.FirstOrDefaultAsync(x => x.PartyId == Id);
                        if (Query != null)
                        {
                            if (Query.Fk_PartyType == MappingLedgers.SundryCreditors)
                            {
                                var PurchaseOdr = await _appDbContext.PurchaseOrders.Where(x => x.Fk_SubLedgerId == Query.Fk_SubledgerId).ToListAsync();
                                if (PurchaseOdr.Count > 0)
                                {
                                    foreach (var item in PurchaseOdr)
                                    {
                                        IsCallBack = true;
                                        var IsSuccess = await _transactionRepo.DeletePurchase(item.PurchaseOrderId, localTransaction, IsCallBack);
                                        if (IsSuccess.IsSuccess) IsCallBack = false;
                                    }
                                }
                                var PurchaseReturnOdr = await _appDbContext.PurchaseReturnOrders.Where(x => x.Fk_SubLedgerId == Query.Fk_SubledgerId).ToListAsync();
                                if (PurchaseReturnOdr.Count > 0)
                                {
                                    foreach (var item in PurchaseReturnOdr)
                                    {
                                        IsCallBack = true;
                                        var IsSuccess = await _transactionRepo.DeletetPurchaseReturn(item.PurchaseReturnOrderId, localTransaction, IsCallBack);
                                        if (IsSuccess.IsSuccess) IsCallBack = false;
                                    }
                                }
                                var deletePayments = await _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == Query.Fk_SubledgerId).ToListAsync();
                                if (deletePayments.Count > 0) _appDbContext.Payments.RemoveRange(deletePayments);
                                await _appDbContext.SaveChangesAsync();
                            }
                            if (Query.Fk_PartyType == MappingLedgers.SundryDebtors)
                            {
                                var SalesOdr = await _appDbContext.SalesOrders.Where(x => x.Fk_SubLedgerId == Query.Fk_SubledgerId).ToListAsync();
                                if (SalesOdr.Count > 0)
                                {
                                    foreach (var item in SalesOdr)
                                    {
                                        IsCallBack = true;
                                        var IsSuccess = await _transactionRepo.DeleteSales(item.SalesOrderId, localTransaction, IsCallBack);
                                        if (IsSuccess.IsSuccess) IsCallBack = false;
                                    }
                                }
                                //var SalesReturnOdr = await _appDbContext.SalesReturnOrders.Where(x => x.Fk_SubLedgerId == Query.Fk_SubledgerId).ToListAsync();
                                //if (SalesReturnOdr.Count > 0)
                                //{
                                //    foreach (var item in SalesReturnOdr)
                                //    {
                                //        IsCallBack = true;
                                //        var IsSuccess = await _transactionRepo.DeleteSalesReturn(item.SalesReturnOrderId, localTransaction, IsCallBack);
                                //        if (IsSuccess.IsSuccess) IsCallBack = false;
                                //    }
                                //}
                                var deleteRecipts = await _appDbContext.Receipts.Where(x => x.Fk_SubLedgerId == Query.Fk_SubledgerId).ToListAsync();
                                if (deleteRecipts.Count > 0) _appDbContext.Receipts.RemoveRange(deleteRecipts);
                                await _appDbContext.SaveChangesAsync();
                            }
                            var deleteSubLederBalance = await _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == Query.Fk_SubledgerId).FirstOrDefaultAsync();
                            if (deleteSubLederBalance != null) _appDbContext.SubLedgerBalances.Remove(deleteSubLederBalance);
                            await _appDbContext.SaveChangesAsync();
                            _appDbContext.Parties.Remove(Query);
                            await _appDbContext.SaveChangesAsync();
                            var deleteSubLeder = await _appDbContext.SubLedgers.Where(x => x.SubLedgerId == Query.Fk_SubledgerId).FirstOrDefaultAsync();
                            if (deleteSubLeder != null) _appDbContext.SubLedgers.Remove(deleteSubLeder);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteParty : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region State
        public async Task<Result<StateModel>> GetStates()
        {
            Result<StateModel> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.States
                                   where s.Fk_BranchId == BranchId
                                   select new StateModel
                                   {
                                       StateId = s.StateId,
                                       StateName = s.StateName
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetStates : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateState(StateModel data)
        {
            Result<bool> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                _Result.IsSuccess = false;
                var Query = await _appDbContext.States.FirstOrDefaultAsync(s => s.StateName == data.StateName && s.Fk_BranchId == BranchId);
                if (Query == null)
                {
                    var newState = _mapper.Map<State>(data);
                    newState.Fk_BranchId = BranchId;
                    await _appDbContext.States.AddAsync(newState);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateState : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateState(StateModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var Query = await _appDbContext.States.FirstOrDefaultAsync(s => s.StateId == data.StateId && s.Fk_BranchId == BranchId);
                if (Query != null)
                {
                    data.Fk_BranchId = BranchId;
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateState : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteState(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
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
                        var Query = await _appDbContext.States.FirstOrDefaultAsync(x => x.StateId == Id);
                        if (Query != null)
                        {
                            _appDbContext.States.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteState : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region City
        public async Task<Result<CityModel>> GetCities(Guid Id)
        {
            Result<CityModel> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                _Result.IsSuccess = false;
                var Query = await (from s in _appDbContext.Cities
                                   where s.Fk_StateId == Id && s.Fk_BranchId == BranchId
                                   select new CityModel
                                   {
                                       CityId = s.CityId,
                                       CityName = s.CityName
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetCities : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateCity(CityModel data)
        {
            Result<bool> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Cities.FirstOrDefaultAsync(s => s.CityName == data.CityName && s.Fk_BranchId == BranchId);
                if (Query == null)
                {
                    var newCity = _mapper.Map<City>(data);
                    newCity.Fk_BranchId = BranchId;
                    await _appDbContext.Cities.AddAsync(newCity);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateCity : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateCity(CityModel data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var Query = await _appDbContext.Cities.FirstOrDefaultAsync(s => s.CityId == data.CityId && s.Fk_BranchId == BranchId);
                if (Query != null)
                {
                    data.Fk_BranchId = BranchId;
                    _mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateCity : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteCity(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
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
                        var Query = await _appDbContext.Cities.FirstOrDefaultAsync(x => x.CityId == Id);
                        if (Query != null)
                        {
                            _appDbContext.Cities.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();
                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteCity : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #endregion
        #region labour Master
        #region Labour Type
        public async Task<Result<LabourTypeModel>> GetAllLabourTypes()
        {
            Result<LabourTypeModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.LabourTypes.
                                   Select(s => new LabourTypeModel
                                   {
                                       LabourTypeId = s.LabourTypeId,
                                       Labour_Type = s.Labour_Type
                                   }).ToListAsync();
                if (Query.Count > 0)
                {
                    var LabourTypeList = Query;
                    _Result.CollectionObjData = LabourTypeList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllLabourTypes : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Labour Detail
        public async Task<Result<LabourModel>> GetAllLabourDetails()
        {
            Result<LabourModel> _Result = new();
            List<LabourModel> Models = new();
            try
            {
                _Result.IsSuccess = false;
                if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                {
                    Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                    Models = await _appDbContext.Labours.Where(s => s.Fk_BranchId == BranchId)
                                       .Select(l => new LabourModel
                                       {
                                           LabourId = l.LabourId,
                                           LabourName = l.LabourName,
                                           LabourType = l.LabourType != null ? new LabourTypeModel() { Labour_Type = l.LabourType.Labour_Type } : null,
                                           Address = l.Address,
                                           Phone = l.Phone,
                                           Fk_SubLedgerId = l.Fk_SubLedgerId,
                                           Reference = l.Reference,
                                       }).ToListAsync();
                }
                else
                {
                    Models = await _appDbContext.Labours
                                      .Select(l => new LabourModel
                                      {
                                          LabourId = l.LabourId,
                                          LabourName = l.LabourName,
                                          LabourType = l.LabourType != null ? new LabourTypeModel() { Labour_Type = l.LabourType.Labour_Type } : null,
                                          Address = l.Address,
                                          Phone = l.Phone,
                                          Fk_SubLedgerId = l.Fk_SubLedgerId,
                                          Reference = l.Reference,
                                      }).ToListAsync();
                }
                if (Models.Count > 0)
                {
                    _Result.CollectionObjData = Models;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllLabourDetails : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LabourModel>> GetLabourDetailById(Guid LabourId)
        {
            Result<LabourModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var Query = await _appDbContext.Labours.Where(s => s.LabourId == LabourId && s.Fk_BranchId == BranchId)
                                   .Select(l => new LabourModel
                                   {
                                       LabourId = l.LabourId,
                                       LabourName = l.LabourName,
                                       LabourType = l.LabourType != null ? new LabourTypeModel() { Labour_Type = l.LabourType.Labour_Type } : null,
                                       Address = l.Address,
                                       Phone = l.Phone,
                                       Reference = l.Reference,
                                   }).SingleOrDefaultAsync();
                if (Query != null)
                {
                    _Result.SingleObjData = Query;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetLabourDetailById : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LabourModel>> GetLaboursByLabourTypeId(Guid LabourTypeId)
        {
            Result<LabourModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                {
                    Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                    _Result.CollectionObjData = await _appDbContext.Labours.Where(s => s.Fk_Labour_TypeId == LabourTypeId && s.Fk_BranchId == BranchId)
                       .Select(l => new LabourModel
                       {
                           LabourId = l.LabourId,
                           LabourName = l.LabourName,
                           LabourType = l.LabourType != null ? new LabourTypeModel() { Labour_Type = l.LabourType.Labour_Type } : null,
                           Address = l.Address,
                           Phone = l.Phone,
                           Reference = l.Reference,
                       }).ToListAsync();
                }
                else
                {
                    _Result.CollectionObjData = await _appDbContext.Labours.Where(s => s.Fk_Labour_TypeId == LabourTypeId)
                      .Select(l => new LabourModel
                      {
                          LabourId = l.LabourId,
                          LabourName = l.LabourName,
                          LabourType = l.LabourType != null ? new LabourTypeModel() { Labour_Type = l.LabourType.Labour_Type } : null,
                          Address = l.Address,
                          Phone = l.Phone,
                          Reference = l.Reference,
                      }).ToListAsync();
                }

                if (_Result.CollectionObjData.Count > 0)
                {
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    _Result.IsSuccess = true;
                }


            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetLabourDetailById : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateLabourDetail(LabourModel data)
        {
            Result<bool> _Result = new();
            try
            {
                using var transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    _Result.IsSuccess = false;
                    Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                    Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    var existingLabur = await _appDbContext.Labours.FirstOrDefaultAsync(s => s.LabourName == data.LabourName && s.Fk_BranchId == BranchId);
                    if (existingLabur == null)
                    {
                        #region create SubLedger
                        var newSubLedger = new SubLedger
                        {
                            Fk_LedgerId = MappingLedgers.LabourAccount,
                            SubLedgerName = data.LabourName,
                            Fk_BranchId = BranchId
                        };
                        await _appDbContext.SubLedgers.AddAsync(newSubLedger);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        #region Ledger Balance 
                        var isLedgerbalanceExist = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).FirstOrDefaultAsync();
                        Guid LedgerBalanceId = Guid.Empty;
                        if (isLedgerbalanceExist == null)
                        {
                            var newLedgerBalance = new LedgerBalance
                            {
                                Fk_LedgerId = MappingLedgers.LabourAccount,
                                OpeningBalance = 0,
                                OpeningBalanceType = "Cr",
                                RunningBalance = 0,
                                RunningBalanceType = "Cr",
                                Fk_BranchId = BranchId,
                                Fk_FinancialYear = FinancialYear
                            };
                            await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                            await _appDbContext.SaveChangesAsync();
                            LedgerBalanceId = newLedgerBalance.LedgerBalanceId;
                        }
                        else
                        {
                            LedgerBalanceId = isLedgerbalanceExist.LedgerBalanceId;
                        }
                        #endregion
                        #region Create SubLedger Balance
                        var newSubLedgerBalance = new SubLedgerBalance
                        {
                            Fk_LedgerBalanceId = LedgerBalanceId,
                            Fk_SubLedgerId = newSubLedger.SubLedgerId,
                            OpeningBalance = 0,
                            OpeningBalanceType = "Cr",
                            RunningBalance = 0,
                            RunningBalanceType = "Cr",
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear
                        };
                        await _appDbContext.SubLedgerBalances.AddAsync(newSubLedgerBalance);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        #region Create Labour
                        data.Fk_SubLedgerId = newSubLedger.SubLedgerId;
                        var newLabour = _mapper.Map<Labour>(data);
                        newLabour.Fk_BranchId = BranchId;
                        await _appDbContext.Labours.AddAsync(newLabour);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                    }
                    transaction.Commit();
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/CreateLabourDetail : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateLabourDetail(LabourModel data)
        {
            Result<bool> _Result = new();
            Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
            try
            {
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Labours.Where(s => s.LabourId == data.LabourId).FirstOrDefaultAsync();
                if (Query != null)
                {
                    data.Fk_BranchId = BranchId;
                    Query.Address = data.Address;
                    Query.LabourName = data.LabourName;
                    Query.Fk_Labour_TypeId = data.Fk_Labour_TypeId;
                    Query.Phone = data.Phone;
                    Query.Reference = data.Reference;
                    //_mapper.Map(data, Query);
                    int count = await _appDbContext.SaveChangesAsync();
                    _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/UpdateLabourDetail : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteLabourDetail(Guid Id, IDbContextTransaction transaction, bool IsCallBack)
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
                        var Query = await _appDbContext.Labours.SingleOrDefaultAsync(x => x.LabourId == Id && x.Fk_BranchId == BranchId);
                        if (Query != null)
                        {
                            var DamageOdr = await _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == Id && x.Fk_BranchId == BranchId).ToListAsync();
                            if (DamageOdr.Count > 0)
                            {
                                foreach (var item in DamageOdr)
                                {
                                    IsCallBack = true;
                                    var IsSuccess = await _transactionRepo.DeleteDamage(item.DamageOrderId, localTransaction, IsCallBack);
                                    if (IsSuccess.IsSuccess) IsCallBack = false;
                                }
                            }
                            var ProductionEntry = await _appDbContext.LabourOrders.Where(x => x.Fk_LabourId == Id && x.FK_BranchId == BranchId).ToListAsync();
                            if (ProductionEntry.Count > 0)
                            {
                                foreach (var item in ProductionEntry)
                                {
                                    IsCallBack = true;
                                    var IsSuccess = await _transactionRepo.DeleteProductionEntry(item.LabourOrderId, localTransaction, IsCallBack);
                                    if (IsSuccess.IsSuccess) IsCallBack = false;
                                }
                            }
                            var deleteSubLederBalance = await _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == Query.Fk_SubLedgerId).FirstOrDefaultAsync();
                            if (deleteSubLederBalance != null) _appDbContext.SubLedgerBalances.Remove(deleteSubLederBalance);
                            await _appDbContext.SaveChangesAsync();

                            var deletePayments = await _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == Query.Fk_SubLedgerId).ToListAsync();
                            if (deletePayments.Count > 0) _appDbContext.Payments.RemoveRange(deletePayments);
                            await _appDbContext.SaveChangesAsync();

                            _appDbContext.Labours.Remove(Query);
                            int count = await _appDbContext.SaveChangesAsync();

                            var deleteSubLeder = await _appDbContext.SubLedgers.Where(x => x.SubLedgerId == Query.Fk_SubLedgerId).FirstOrDefaultAsync();
                            if (deleteSubLeder != null) _appDbContext.SubLedgers.Remove(deleteSubLeder);
                            await _appDbContext.SaveChangesAsync();

                            _Result.Response = (count > 0) ? ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted) : ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Error);
                        }
                        _Result.IsSuccess = true;
                        if (IsCallBack == false) localTransaction.Commit();
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Deleted);
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/DeleteLabourDetail : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #endregion 
    }
}
