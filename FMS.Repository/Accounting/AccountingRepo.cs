using FMS.Api.Email.EmailService;
using FMS.Db.Context;
using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Security.Policy;
using System.Text;

namespace FMS.Repository.Accounting
{
    public class AccountingRepo : IAccountingRepo
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<AccountingRepo> _logger;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IEmailService _emailService;
        public AccountingRepo(ILogger<AccountingRepo> logger, AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _HttpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }
        #region Journal
        public async Task<Result<string>> GetJournalVoucherNo()
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var lastJournalNo = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId== FinancialYear).OrderByDescending(s => s.VouvherNo).Select(s => new { s.VouvherNo }).SingleOrDefaultAsync();
                if (lastJournalNo != null)
                {
                    var lastVoucherNo = lastJournalNo.VouvherNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastVoucherNo.AsSpan(2), out int currentId))
                    {
                        currentId++;
                        var newTransactionId = $"JN{currentId:D6}";
                        _Result.SingleObjData = newTransactionId;
                    }
                }
                else
                {
                    _Result.SingleObjData = "JN000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AccountingRepo/GetJournalVoucherNo : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<GroupedJournalModel>> GetJournals()
        {
            Result<GroupedJournalModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s =>
                                   new JournalModel()
                                   {
                                       JournalId = s.JournalId,
                                       VouvherNo = s.VouvherNo,
                                       VoucherDate = s.VoucherDate,
                                       LedgerDevName = _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                                       LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                                       SubLedgerName = s.SubLedger != null ? s.SubLedger.SubLedgerName : "-",
                                       narration = s.Narration,
                                       Amount = s.Amount,
                                       DrCr = s.DrCr
                                   }).ToListAsync();

                var groupedQuery = Query.GroupBy(journal => journal.VouvherNo)
                        .Select(group => new GroupedJournalModel
                        {
                            VoucherNo = group.Key,
                            Journals = group.ToList()
                        }).ToList();
                if (groupedQuery.Count > 0)
                {
                    _Result.CollectionObjData = groupedQuery;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllProducts : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<GroupedJournalModel>> GetJournalById(string Id)
        {
            Result<GroupedJournalModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.VouvherNo == Id).Select(s =>
                                   new JournalModel()
                                   {
                                       JournalId = s.JournalId,
                                       VouvherNo = s.VouvherNo,
                                       VoucherDate = s.VoucherDate,
                                       LedgerDevName = _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                                       LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                                       SubLedgerName = s.SubLedger != null ? s.SubLedger.SubLedgerName : "-",
                                       narration = s.Narration,
                                       Amount = s.Amount,
                                       DrCr = s.DrCr
                                   }).ToListAsync();

                var groupedQuery = Query.GroupBy(journal => journal.VouvherNo)
                        .Select(group => new GroupedJournalModel
                        {
                            VoucherNo = group.Key,
                            Journals = group.ToList()
                        }).ToList();

                if (groupedQuery.Count > 0)
                {
                    _Result.CollectionObjData = groupedQuery;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllProducts : {_Exception.Message}");
            }
            return _Result;
        }
        private Guid GetFkLedgerGroupId(string ledgerId)
        {
            Guid ledgerIdGuid = Guid.Parse(ledgerId);
            var ledgerGroup = _appDbContext.Ledgers.SingleOrDefaultAsync(s => s.LedgerId == ledgerIdGuid);
            if (ledgerGroup != null)
            {
                return ledgerGroup.Result.Fk_LedgerGroupId;
            }
            var ledgerDevGroup = _appDbContext.LedgersDev.SingleOrDefaultAsync(s => s.LedgerId == ledgerIdGuid);
            if (ledgerDevGroup != null)
            {
                return ledgerDevGroup.Result.Fk_LedgerGroupId;
            }
            return Guid.Empty;
        }
        public async Task<Result<bool>> CreateJournal(JournalDataRequest requestData)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (DateTime.TryParseExact(requestData.VoucherDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedVoucherDate))
                    {
                        foreach (var item in requestData.arr)
                        {
                            if (item.subledgerData.Count > 0)
                            {
                                int i = 0;
                                foreach (var data in item.subledgerData)
                                {
                                    foreach (var innerData in data.ddlSubledgerId)
                                    {
                                        #region Journal
                                        var newJournal = new Journal()
                                        {
                                            VouvherNo = requestData.VoucherNo,
                                            VoucherDate = convertedVoucherDate,
                                            Fk_LedgerGroupId = GetFkLedgerGroupId(item.ddlLedgerId),
                                            Fk_LedgerId = Guid.Parse(item.ddlLedgerId),
                                            Fk_SubLedgerId = Guid.Parse(data.ddlSubledgerId[i]),
                                            Fk_BranchId = BranchId,
                                            Fk_FinancialYearId = FinancialYear,
                                            Narration = requestData.Narration,
                                            Amount = Convert.ToDecimal(data.SubledgerAmunt[i]),
                                            DrCr = item.BalanceType,
                                        };
                                        i++;
                                        await _appDbContext.Journals.AddAsync(newJournal);
                                        await _appDbContext.SaveChangesAsync();
                                        #endregion
                                        #region Ledger & SubLedger Balance
                                        var updateSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == newJournal.Fk_SubLedgerId && s.Fk_FinancialYearId == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                        if (updateSubledgerBalance != null)
                                        {
                                            decimal Amount = newJournal.DrCr == "Dr" ? newJournal.Amount : -newJournal.Amount;
                                            updateSubledgerBalance.RunningBalance += Amount;
                                            updateSubledgerBalance.RunningBalanceType = (updateSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateSubledgerBalance.Fk_LedgerBalanceId).FirstOrDefaultAsync();
                                            if (updateLedgerBalance != null)
                                            {
                                                updateLedgerBalance.RunningBalance += Amount;
                                                updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            }
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        #endregion
                                    }
                                }
                            }
                            else
                            {
                                #region Journal
                                var newJournal = new Journal()
                                {
                                    VouvherNo = requestData.VoucherNo,
                                    VoucherDate = convertedVoucherDate,
                                    Fk_LedgerGroupId = GetFkLedgerGroupId(item.ddlLedgerId),
                                    Fk_LedgerId = Guid.Parse(item.ddlLedgerId),
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYearId = FinancialYear,
                                    Narration = requestData.Narration,
                                    DrCr = item.BalanceType,
                                    Amount = item.CrBalance != null ? Convert.ToDecimal(item.CrBalance) : Convert.ToDecimal(item.DrBalance)
                                };
                                await _appDbContext.Journals.AddAsync(newJournal);
                                await _appDbContext.SaveChangesAsync();
                                #endregion
                                #region  Ledger Balance
                                var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == Guid.Parse(item.ddlLedgerId) && s.Fk_FinancialYear == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                if (updateLedgerBalance != null)
                                {
                                    decimal Amount = newJournal.DrCr == "Dr" ? newJournal.Amount : -newJournal.Amount;
                                    updateLedgerBalance.RunningBalance += Amount;
                                    updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                            }
                        }
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AccountingRepo/CreateJournal : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteJournal(string Id, IDbContextTransaction transaction)
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
                    if (Id != null)
                    {
                        //*****************************Delete Journal**************************************//
                        var GetJournals = await _appDbContext.Journals.Where(x => x.VouvherNo == Id && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId== FinancialYear).ToListAsync();
                        foreach (var item in GetJournals)
                        {
                            if (item.Fk_LedgerId != Guid.Empty)
                            {
                                var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(l => l.Fk_LedgerId == item.Fk_LedgerId && l.Fk_BranchId == BranchId && l.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateLedgerBalance != null)
                                {
                                    ;
                                    decimal Balance = item.DrCr == "DR" ? item.Amount : -item.Amount;
                                    updateLedgerBalance.RunningBalance += Balance;
                                    updateLedgerBalance.RunningBalanceType = updateLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                if (item.Fk_SubLedgerId != Guid.Empty)
                                {
                                    var updateSubLedgerBalance = await _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == item.Fk_SubLedgerId && l.Fk_BranchId == BranchId && l.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSubLedgerBalance != null)
                                    {
                                        decimal Balance = item.DrCr == "DR" ? item.Amount : -item.Amount;
                                        updateSubLedgerBalance.RunningBalance += Balance;
                                        updateSubLedgerBalance.RunningBalanceType = updateSubLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }

                            if (!string.IsNullOrEmpty(item.TransactionNo) && item.TransactionNo.StartsWith("SI"))
                            {
                                var SalesTrn = await _appDbContext.SalesTransaction.Where(s => s.TransactionNo == item.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                                _appDbContext.RemoveRange(SalesTrn);
                                await _appDbContext.SaveChangesAsync();
                                var SalesOdr = await _appDbContext.SalesOrders.Where(s => s.TransactionNo == item.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                                _appDbContext.RemoveRange(SalesOdr);
                                await _appDbContext.SaveChangesAsync();
                            }
                            else if (!string.IsNullOrEmpty(item.TransactionNo) && item.TransactionNo.StartsWith("PI"))
                            {
                                var PurchaseTrn = await _appDbContext.PurchaseTransactions.Where(s => s.TransactionNo == item.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                                _appDbContext.RemoveRange(PurchaseTrn);
                                await _appDbContext.SaveChangesAsync();
                                var PurchaseOdr = await _appDbContext.PurchaseOrders.Where(s => s.TransactionNo == item.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                                _appDbContext.RemoveRange(PurchaseOdr);
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        _appDbContext.Journals.RemoveRange(GetJournals);
                        await _appDbContext.SaveChangesAsync();
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AccountingRepo/DeleteJournal : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Payment
        public async Task<Result<GroupedPaymentModel>> GetPayments()
        {
            Result<GroupedPaymentModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.Payments.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s =>
                                   new PaymentModel()
                                   {
                                       PaymentId = s.PaymentId,
                                       VouvherNo = s.VouvherNo,
                                       VoucherDate = s.VoucherDate,
                                       LedgerDevName = _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                                       LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                                       SubLedgerName = s.SubLedger != null ? s.SubLedger.SubLedgerName : "-",
                                       narration = s.narration,
                                       Amount = s.Amount,
                                       DrCr = s.DrCr
                                   }).ToListAsync();
                var groupedQuery = Query.GroupBy(Payment => Payment.VouvherNo)
                        .Select(group => new GroupedPaymentModel
                        {
                            VoucherNo = group.Key,
                            Payments = group.ToList()
                        }).ToList();
                if (groupedQuery.Count > 0)
                {
                    _Result.CollectionObjData = groupedQuery;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllProducts : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<GroupedPaymentModel>> GetPaymentById(string Id)
        {
            Result<GroupedPaymentModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.Payments.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.VouvherNo == Id).Select(s =>
                                   new PaymentModel()
                                   {
                                       PaymentId = s.PaymentId,
                                       VouvherNo = s.VouvherNo,
                                       VoucherDate = s.VoucherDate,
                                       LedgerDevName = _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                                       LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                                       SubLedgerName = s.SubLedger != null ? s.SubLedger.SubLedgerName : "-",
                                       narration = s.narration,
                                       Amount = s.Amount,
                                       DrCr = s.DrCr
                                   }).ToListAsync();
                var groupedQuery = Query.GroupBy(Payment => Payment.VouvherNo)
                        .Select(group => new GroupedPaymentModel
                        {
                            VoucherNo = group.Key,
                            Payments = group.ToList()
                        }).ToList();
                if (groupedQuery.Count > 0)
                {
                    _Result.CollectionObjData = groupedQuery;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllProducts : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<string>> GetPaymentVoucherNo(string CashBank)
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var lastPaymentNo = await _appDbContext.Payments.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.CashBank == CashBank).OrderByDescending(s => s.VouvherNo).Select(s => new { s.VouvherNo }).SingleOrDefaultAsync();
                if (lastPaymentNo != null)
                {
                    var lastVoucherNo = lastPaymentNo.VouvherNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastVoucherNo.AsSpan(2), out int currentId))
                    {
                        currentId++;
                        var prefix = (CashBank == "Bank") ? "BP" : "CP";
                        var newTransactionId = $"{prefix}{currentId:D6}";
                        _Result.SingleObjData = newTransactionId;
                    }
                }
                else
                {
                    var prefix = (CashBank == "Bank") ? "BP" : "CP";
                    _Result.SingleObjData = $"{prefix}000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AccountingRepo/GetPaymentVoucherNo : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LedgerModel>> GetBankLedgers()
        {
            Result<LedgerModel> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                _Result.IsSuccess = false;
                var Query = await _appDbContext.Ledgers.Where(s => s.LedgerType == "Bank").Select(s => new LedgerModel
                {
                    LedgerId = s.LedgerId,
                    LedgerName = s.LedgerName,
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AccountingRepo/GetBankLedgers : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreatePayment(PaymentDataRequest requestData)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    DateTime? convertedChqDate = null;
                    if (DateTime.TryParseExact(requestData.VoucherDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedVoucherDate))
                    {
                        if (!string.IsNullOrEmpty(requestData.ChqDate) && DateTime.TryParseExact(requestData.ChqDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedChqDate))
                        {
                            convertedChqDate = parsedChqDate;
                        }
                        foreach (var item in requestData.arr)
                        {
                            if (item.subledgerData.Count > 0)
                            {
                                int i = 0;
                                foreach (var data in item.subledgerData)
                                {
                                    foreach (var innerData in data.ddlSubledgerId)
                                    {
                                        #region Payment
                                        var ledDev = await _appDbContext.LedgersDev.Where(x => x.LedgerId == Guid.Parse(item.ddlLedgerId)).Select(x => x.Fk_LedgerGroupId).SingleOrDefaultAsync();
                                        var led = await _appDbContext.Ledgers.Where(x => x.LedgerId == Guid.Parse(item.ddlLedgerId)).Select(x => x.Fk_LedgerGroupId).SingleOrDefaultAsync();
                                        var newPayment = new Payment()
                                        {
                                            CashBank = requestData.CashBank,
                                            CashBankLedgerId = requestData.BankLedgerId != null ? Guid.Parse(requestData.BankLedgerId) : null,
                                            VouvherNo = requestData.VoucherNo,
                                            VoucherDate = convertedVoucherDate,
                                            ChequeNo = requestData.ChqNo,
                                            ChequeDate = convertedChqDate,
                                            narration = requestData.Narration,
                                            DrCr = "Dr",
                                            Fk_LedgerId = Guid.Parse(item.ddlLedgerId),
                                            Fk_LedgerGroupId = ledDev != Guid.Empty ? ledDev : led,
                                            Fk_SubLedgerId = Guid.Parse(data.ddlSubledgerId[i]),
                                            Amount = Convert.ToDecimal(data.SubledgerAmunt[i]),
                                            Fk_BranchId = BranchId,
                                            Fk_FinancialYearId = FinancialYear
                                        };
                                        i++;
                                        await _appDbContext.Payments.AddAsync(newPayment);
                                        await _appDbContext.SaveChangesAsync();
                                        #endregion
                                        #region Ledger & SubLedger Balance
                                        //@AnyLedger --------------Dr
                                        var updateSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == newPayment.Fk_SubLedgerId && s.Fk_FinancialYearId == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                        if (updateSubledgerBalance != null)
                                        {
                                            updateSubledgerBalance.RunningBalance += newPayment.Amount;
                                            updateSubledgerBalance.RunningBalanceType = (updateSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateSubledgerBalance.Fk_LedgerBalanceId && s.Fk_FinancialYear == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                            if (updateLedgerBalance != null)
                                            {
                                                updateLedgerBalance.RunningBalance += newPayment.Amount;
                                                updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            }
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        #endregion
                                    }
                                }
                            }
                            else
                            {
                                #region Payment
                                var newPayment = new Payment()
                                {
                                    CashBank = requestData.CashBank,
                                    CashBankLedgerId = requestData.BankLedgerId != null ? Guid.Parse(requestData.BankLedgerId) : null,
                                    VouvherNo = requestData.VoucherNo,
                                    VoucherDate = convertedVoucherDate,
                                    ChequeNo = requestData.ChqNo,
                                    ChequeDate = convertedChqDate,
                                    narration = requestData.Narration,
                                    DrCr = "Dr",
                                    Fk_LedgerId = Guid.Parse(item.ddlLedgerId),
                                    Fk_LedgerGroupId = MappingLedgerGroup.CashBankBalance,
                                    Amount = Convert.ToDecimal(item.DrBalance),
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYearId = FinancialYear
                                };
                                await _appDbContext.Payments.AddAsync(newPayment);
                                await _appDbContext.SaveChangesAsync();
                                #endregion
                                #region Ledger Balance
                                //@AnyLedger --------------Dr
                                var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == Guid.Parse(item.ddlLedgerId) && s.Fk_FinancialYear == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                if (updateLedgerBalance != null)
                                {
                                    updateLedgerBalance.RunningBalance += Convert.ToDecimal(item.DrBalance);
                                    updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                            }
                            #region update Bank/Cash Ledger Balance
                            //@Bank/Cash A/c ------------Cr
                            if (requestData.CashBank == "Bank")
                            {
                                var updateBankLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == Guid.Parse(requestData.BankLedgerId) && s.Fk_FinancialYear == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                if (updateBankLedgerBalance != null)
                                {
                                    updateBankLedgerBalance.RunningBalance -= Convert.ToDecimal(item.DrBalance);
                                    updateBankLedgerBalance.RunningBalanceType = (updateBankLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    _Result.WarningMessage = "Bank Ledger Balance Not Set";
                                    return _Result;
                                }
                            }
                            else
                            {
                                var updateCashLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_FinancialYear == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                if (updateCashLedgerBalance != null)
                                {
                                    updateCashLedgerBalance.RunningBalance -= Convert.ToDecimal(item.DrBalance);
                                    updateCashLedgerBalance.RunningBalanceType = (updateCashLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    _Result.WarningMessage = "Cash Ledger Balance Not Set";
                                    return _Result;
                                }
                            }
                            #endregion
                        }
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AccountingRepo/CreatePayment : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeletePayment(string Id, IDbContextTransaction transaction)
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
                    if (Id != null)
                    {

                        //*****************************Delete Payment**************************************//
                        var GetPayments = await _appDbContext.Payments.Where(x => x.VouvherNo == Id && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId==FinancialYear).ToListAsync();
                        foreach (var item in GetPayments)
                        {
                            decimal Balance = item.DrCr == "DR" ? item.Amount : -item.Amount;
                            if (item.Fk_LedgerId != Guid.Empty)
                            {
                                var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(l => l.Fk_LedgerId == item.Fk_LedgerId && l.Fk_BranchId == BranchId && l.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateLedgerBalance != null)
                                {
                                    updateLedgerBalance.RunningBalance += Balance;
                                    updateLedgerBalance.RunningBalanceType = updateLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                if (item.Fk_SubLedgerId != Guid.Empty)
                                {
                                    var updateSubLedgerBalance = await _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == item.Fk_SubLedgerId && l.Fk_BranchId == BranchId && l.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSubLedgerBalance != null)
                                    {
                                        updateSubLedgerBalance.RunningBalance += Balance;
                                        updateSubLedgerBalance.RunningBalanceType = updateSubLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                }
                            }

                            if (item.CashBank == "bank")
                            {
                                var updateSubLedgerBalance = await _appDbContext.LedgerBalances.Where(l => l.Fk_LedgerId == item.CashBankLedgerId && l.Fk_BranchId == BranchId && l.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateSubLedgerBalance!=null)
                                {
                                    updateSubLedgerBalance.RunningBalance += Balance;
                                    updateSubLedgerBalance.RunningBalanceType = updateSubLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                              
                            }
                            else
                            {
                                var updateSubLedgerBalance = await _appDbContext.LedgerBalances.Where(l => l.Fk_LedgerId == MappingLedgers.CashAccount && l.Fk_BranchId == BranchId && l.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateSubLedgerBalance != null)
                                {
                                    updateSubLedgerBalance.RunningBalance += Balance;
                                    updateSubLedgerBalance.RunningBalanceType = updateSubLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                            }
                        }
                        _appDbContext.Payments.RemoveRange(GetPayments);
                        await _appDbContext.SaveChangesAsync();
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AccountingRepo/DeletePayment : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Receipt
        public async Task<Result<GroupedReceptModel>> GetReceipts()
        {
            Result<GroupedReceptModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.Receipts.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s =>
                                   new ReceiptModel()
                                   {
                                       ReceiptId = s.ReceiptId,
                                       VouvherNo = s.VouvherNo,
                                       VoucherDate = s.VoucherDate,
                                       LedgerDevName = _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                                       LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                                       SubLedgerName = s.SubLedger != null ? s.SubLedger.SubLedgerName : "-",
                                       narration = s.narration,
                                       Amount = s.Amount,
                                       DrCr = s.DrCr
                                   }).ToListAsync();
                var groupedQuery = Query.GroupBy(Receipt => Receipt.VouvherNo)
                       .Select(group => new GroupedReceptModel
                       {
                           VoucherNo = group.Key,
                           Receipts = group.ToList()
                       }).ToList();

                if (groupedQuery.Count > 0)
                {
                    _Result.CollectionObjData = groupedQuery;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllProducts : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<GroupedReceptModel>> GetReceiptById(string Id)
        {
            Result<GroupedReceptModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.Receipts.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.VouvherNo == Id).Select(s =>
                                   new ReceiptModel()
                                   {
                                       ReceiptId = s.ReceiptId,
                                       VouvherNo = s.VouvherNo,
                                       VoucherDate = s.VoucherDate,
                                       SubLedgerName = s.SubLedger != null ? s.SubLedger.SubLedgerName : "-",
                                       narration = s.narration,
                                       Amount = s.Amount,
                                       DrCr = s.DrCr
                                   }).ToListAsync();
                var groupedQuery = Query.GroupBy(Receipt => Receipt.VouvherNo)
                       .Select(group => new GroupedReceptModel
                       {
                           VoucherNo = group.Key,
                           Receipts = group.ToList()
                       }).ToList();
                if (groupedQuery.Count > 0)
                {
                    _Result.CollectionObjData = groupedQuery;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllProducts : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<string>> GetReceiptVoucherNo(string CashBank)
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var lastReceiptNo = await _appDbContext.Receipts.Where(s => s.Fk_BranchId == BranchId && s.CashBank == CashBank && s.Fk_FinancialYearId == FinancialYear).OrderByDescending(s => s.VouvherNo).Select(s => new { s.VouvherNo }).SingleOrDefaultAsync();
                if (lastReceiptNo != null)
                {
                    var lastVoucherNo = lastReceiptNo.VouvherNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastVoucherNo.AsSpan(2), out int currentId))
                    {
                        currentId++;
                        var prefix = (CashBank == "Bank") ? "BR" : "CR";
                        var newTransactionId = $"{prefix}{currentId:D6}";
                        _Result.SingleObjData = newTransactionId;
                    }
                }
                else
                {
                    var prefix = (CashBank == "Bank") ? "BR" : "CR";
                    _Result.SingleObjData = $"{prefix}000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AccountingRepo/GetReceiptVoucherNo : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateRecipt(ReciptsDataRequest requestData)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                using var transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    DateTime? convertedChqDate = null;
                    if (DateTime.TryParseExact(requestData.VoucherDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedVoucherDate))
                    {
                        if (!string.IsNullOrEmpty(requestData.ChqDate) && DateTime.TryParseExact(requestData.ChqDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedChqDate))
                        {
                            convertedChqDate = parsedChqDate;
                        }
                        foreach (var item in requestData.arr)
                        {
                            if (item.subledgerData.Count > 0)
                            {
                                int i = 0;
                                foreach (var data in item.subledgerData)
                                {
                                    foreach (var innerData in data.ddlSubledgerId)
                                    {
                                        #region  Receipt
                                        var ledDev = await _appDbContext.LedgersDev.Where(x => x.LedgerId == Guid.Parse(item.ddlLedgerId)).Select(x => x.Fk_LedgerGroupId).SingleOrDefaultAsync();
                                        var led = await _appDbContext.Ledgers.Where(x => x.LedgerId == Guid.Parse(item.ddlLedgerId)).Select(x => x.Fk_LedgerGroupId).SingleOrDefaultAsync();
                                        var newRecipts = new Receipt()
                                        {
                                            CashBank = requestData.CashBank,
                                            CashBankLedgerId = requestData.BankLedgerId != null ? Guid.Parse(requestData.BankLedgerId) : null,
                                            VouvherNo = requestData.VoucherNo,
                                            VoucherDate = convertedVoucherDate,
                                            ChequeNo = requestData.ChqNo,
                                            ChequeDate = convertedChqDate,
                                            narration = requestData.Narration,
                                            DrCr = "Cr",
                                            Fk_LedgerId = Guid.Parse(item.ddlLedgerId),
                                            Fk_LedgerGroupId = ledDev != Guid.Empty ? ledDev : led,
                                            Fk_SubLedgerId = Guid.Parse(data.ddlSubledgerId[i]),
                                            Amount = Convert.ToDecimal(data.SubledgerAmunt[i]),
                                            Fk_BranchId = BranchId,
                                            Fk_FinancialYearId = FinancialYear
                                        };
                                        i++;
                                        await _appDbContext.Receipts.AddAsync(newRecipts);
                                        await _appDbContext.SaveChangesAsync();
                                        #endregion
                                        #region Ledger & SubLedger Balance
                                        var updateSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == newRecipts.Fk_SubLedgerId && s.Fk_FinancialYearId == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                        if (updateSubledgerBalance != null)
                                        {
                                            updateSubledgerBalance.RunningBalance -= newRecipts.Amount;
                                            updateSubledgerBalance.RunningBalanceType = (updateSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateSubledgerBalance.Fk_LedgerBalanceId && s.Fk_FinancialYear == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                            if (updateLedgerBalance != null)
                                            {
                                                updateLedgerBalance.RunningBalance -= newRecipts.Amount;
                                                updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            }
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        #endregion
                                    }
                                }
                            }
                            else
                            {
                                if (item.ddlLedgerId != null)
                                {
                                    #region Receipt
                                    var newRecipts = new Receipt()
                                    {
                                        CashBank = requestData.CashBank,
                                        VouvherNo = requestData.VoucherNo,
                                        VoucherDate = convertedVoucherDate,
                                        ChequeNo = requestData.ChqNo,
                                        ChequeDate = convertedChqDate,
                                        narration = requestData.Narration,
                                        DrCr = "Dr",
                                        Fk_LedgerId = Guid.Parse(item.ddlLedgerId),
                                        Fk_LedgerGroupId = MappingLedgerGroup.CashBankBalance,
                                        Amount = Convert.ToDecimal(item.CrBalance),
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear
                                    };
                                    await _appDbContext.Receipts.AddAsync(newRecipts);
                                    await _appDbContext.SaveChangesAsync();
                                    #endregion
                                    #region Ledger Balance
                                    var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == Guid.Parse(item.ddlLedgerId) && s.Fk_FinancialYear == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                    if (updateLedgerBalance != null)
                                    {
                                        updateLedgerBalance.RunningBalance -= Convert.ToDecimal(item.CrBalance);
                                        updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Cr" : "Dr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    #endregion
                                }

                            }
                            #region update Bank/Cash Ledger Balance
                            if (requestData.CashBank == "Bank")
                            {
                                var updateBankLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == Guid.Parse(requestData.BankLedgerId) && s.Fk_FinancialYear == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                if (updateBankLedgerBalance != null)
                                {
                                    updateBankLedgerBalance.RunningBalance += Convert.ToDecimal(item.CrBalance);
                                    updateBankLedgerBalance.RunningBalanceType = (updateBankLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    _Result.WarningMessage = "Bank Ledger Balance Not Set";
                                    return _Result;
                                }
                            }
                            else
                            {
                                var updateCashLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_FinancialYear == FinancialYear && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                if (updateCashLedgerBalance != null)
                                {
                                    updateCashLedgerBalance.RunningBalance += Convert.ToDecimal(item.CrBalance);
                                    updateCashLedgerBalance.RunningBalanceType = (updateCashLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    _Result.WarningMessage = "Cash Ledger Balance Not Set";
                                    return _Result;
                                }
                            }
                            #endregion
                        }

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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AccountingRepo/CreateRecipt : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteRecipt(string Id, IDbContextTransaction transaction)
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
                    if (Id != null)
                    {
                        //*****************************Delete DeleteRecipt**************************************//
                        var GetRecipts = await _appDbContext.Receipts.Where(x => x.VouvherNo == Id && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId== FinancialYear).ToListAsync();
                        foreach (var item in GetRecipts)
                        {
                            decimal Balance = item.DrCr == "DR" ? item.Amount : -item.Amount;
                            if (item.Fk_LedgerId != Guid.Empty)
                            {
                                var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(l => l.Fk_LedgerId == item.Fk_LedgerId && l.Fk_BranchId == BranchId && l.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateLedgerBalance != null)
                                {
                                    updateLedgerBalance.RunningBalance += Balance;
                                    updateLedgerBalance.RunningBalanceType = updateLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                if (item.Fk_SubLedgerId != Guid.Empty)
                                {
                                    var updateSubLedgerBalance = await _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == item.Fk_SubLedgerId && l.Fk_BranchId == BranchId && l.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSubLedgerBalance != null)
                                    {
                                        updateSubLedgerBalance.RunningBalance += Balance;
                                        updateSubLedgerBalance.RunningBalanceType = updateSubLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                }
                            }

                            if (item.CashBank == "bank")
                            {
                                var updateSubLedgerBalance = await _appDbContext.LedgerBalances.Where(l => l.Fk_LedgerId == item.CashBankLedgerId && l.Fk_BranchId == BranchId && l.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateSubLedgerBalance!=null)
                                {
                                    updateSubLedgerBalance.RunningBalance += Balance;
                                    updateSubLedgerBalance.RunningBalanceType = updateSubLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                var updateSubLedgerBalance = await _appDbContext.LedgerBalances.Where(l => l.Fk_LedgerId == MappingLedgers.CashAccount && l.Fk_BranchId == BranchId && l.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateSubLedgerBalance!=null)
                                {
                                    updateSubLedgerBalance.RunningBalance += Balance;
                                    updateSubLedgerBalance.RunningBalanceType = updateSubLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                            }
                        }
                        _appDbContext.Receipts.RemoveRange(GetRecipts);
                        await _appDbContext.SaveChangesAsync();
                        _Result.IsSuccess = true;
                        localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"AccountingRepo/DeleteRecipt : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
    }
}
