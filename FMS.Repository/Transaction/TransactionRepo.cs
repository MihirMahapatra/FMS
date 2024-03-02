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
using System.ComponentModel.Design;
using System.Globalization;
using System.Text;

namespace FMS.Repository.Transaction
{
    public class TransactionRepo : ITransactionRepo
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<TransactionRepo> _logger;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IEmailService _emailService;
        public TransactionRepo(ILogger<TransactionRepo> logger, AppDbContext appDbContext/*, IMapper mapper*/, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            //_mapper = mapper;
            _HttpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }
        #region Purchase Transaction
        public async Task<Result<SubLedgerModel>> GetSundryCreditors(Guid PartyTypeId)
        {
            Result<SubLedgerModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                _Result.CollectionObjData = await (from s in _appDbContext.SubLedgers
                                                   where s.Fk_LedgerId == PartyTypeId
                                                   select new SubLedgerModel
                                                   {
                                                       SubLedgerId = s.SubLedgerId,
                                                       SubLedgerName = s.SubLedgerName,
                                                   }).ToListAsync();

                if (_Result.CollectionObjData.Count > 0)
                {
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    _Result.IsSuccess = true;
                }
                else
                {
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetSundryCreditors : {_Exception.Message}");
            }
            return _Result;
        }
        #region Purchase
        public async Task<Result<string>> GetLastPurchaseTransaction()
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var lastPurchaseOrder = await _appDbContext.PurchaseOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).OrderByDescending(s => s.TransactionNo).Select(s => new { s.TransactionNo }).FirstOrDefaultAsync();
                if (lastPurchaseOrder != null)
                {
                    var lastTransactionId = lastPurchaseOrder.TransactionNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastTransactionId.AsSpan(2), out int currentId))
                    {
                        currentId++;
                        var newTransactionId = $"PI{currentId:D6}";
                        _Result.SingleObjData = newTransactionId;
                    }
                }
                else
                {
                    _Result.SingleObjData = "PI000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetLastPurchaseTransaction : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<PurchaseOrderModel>> GetPurchases()
        {
            Result<PurchaseOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.PurchaseOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new PurchaseOrderModel
                {
                    PurchaseOrderId = s.PurchaseOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    InvoiceNo = s.InvoiceNo,
                    GrandTotal = s.GrandTotal,
                    SubLedger = s.SubLedger != null ? new SubLedgerModel { SubLedgerName = s.SubLedger.SubLedgerName } : null,
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    var OrderList = Query;
                    _Result.CollectionObjData = OrderList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetPurchases : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<PurchaseOrderModel>> GetPurchaseById(Guid Id)
        {
            Result<PurchaseOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.PurchaseOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.PurchaseOrderId == Id).Select(s => new PurchaseOrderModel
                {
                    PurchaseOrderId = s.PurchaseOrderId,
                    Fk_ProductTypeId = s.Fk_ProductTypeId,
                    Fk_SubLedgerId = s.Fk_SubLedgerId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    InvoiceNo = s.InvoiceNo,
                    InvoiceDate = s.InvoiceDate,
                    TransportationCharges = s.TransportationCharges,
                    VehicleNo = s.VehicleNo,
                    TranspoterName = s.TranspoterName,
                    ReceivingPerson = s.ReceivingPerson,
                    Naration = s.Narration,
                    Discount = s.Discount,
                    SubTotal = s.SubTotal,
                    Gst = s.Gst,
                    GrandTotal = s.GrandTotal,
                    PurchaseTransactions = _appDbContext.PurchaseTransactions.Where(x => x.Fk_PurchaseOrderId == s.PurchaseOrderId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(x => new PurchaseTransactionModel
                    {
                        PurchaseId = x.PurchaseId,
                        AlternateQuantity = x.AlternateQuantity,
                        UnitQuantity = x.UnitQuantity,
                        UnitName = x.Product.Unit.UnitName,
                        Fk_AlternateUnitId = x.Fk_AlternateUnitId,
                        AlternateUnit = x.AlternateUnit != null ? new AlternateUnitModel { AlternateUnitName = x.AlternateUnit.AlternateUnitName } : null,
                        Rate = x.Rate,
                        Discount = x.Discount,
                        DiscountAmount = x.DiscountAmount,
                        Gst = x.Gst,
                        GstAmount = x.GstAmount,
                        Amount = x.Amount,
                        Fk_ProductId = x.Fk_ProductId,
                        Product = x.Product != null ? new ProductModel { ProductName = x.Product.ProductName } : null,
                    }).ToList(),
                }).SingleOrDefaultAsync();
                if (Query != null)
                {
                    var Order = Query;
                    _Result.SingleObjData = Order;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetPurchaseById : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreatePurchase(PurchaseDataRequest data)
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
                    if (DateTime.TryParseExact(data.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedInvoiceDate) && DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTransactionDate))
                    {
                        #region Purchase Odr
                        var newPurchaseOrder = new PurchaseOrder
                        {
                            Fk_ProductTypeId = data.Fk_ProductTypeId,
                            Fk_SubLedgerId = data.Fk_SubLedgerId,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear,
                            TransactionDate = convertedTransactionDate,
                            TransactionNo = data.TransactionNo,
                            InvoiceNo = data.InvoiceNo,
                            InvoiceDate = convertedInvoiceDate,
                            TranspoterName = data.TranspoterName,
                            VehicleNo = data.VehicleNo,
                            ReceivingPerson = data.ReceivingPerson,
                            TransportationCharges = data.TransportationCharges,
                            SubTotal = data.SubTotal,
                            Narration = data.Naration,
                            Discount = data.DiscountAmount,
                            Gst = data.GstAmount,
                            GrandTotal = data.GrandTotal,
                        };
                        await _appDbContext.PurchaseOrders.AddAsync(newPurchaseOrder);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        #region Ledger & Sub Ledger
                        // @ Purchase A/c --------------- Dr
                        var updatePurchaseledgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.PurchaseAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                        if (updatePurchaseledgerBalance != null)
                        {
                            updatePurchaseledgerBalance.RunningBalance += newPurchaseOrder.GrandTotal;
                            updatePurchaseledgerBalance.RunningBalanceType = (updatePurchaseledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                            await _appDbContext.SaveChangesAsync();
                        }
                        else
                        {
                            var newLedgerBalance = new LedgerBalance
                            {
                                Fk_LedgerId = MappingLedgers.PurchaseAccount,
                                OpeningBalance = 0,
                                OpeningBalanceType = "Dr",
                                RunningBalance = newPurchaseOrder.GrandTotal,
                                RunningBalanceType = "Dr",
                                Fk_BranchId = BranchId,
                                Fk_FinancialYear = FinancialYear
                            };
                            await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                            await _appDbContext.SaveChangesAsync();
                        }
                        // @SundryCreditor A/c ------------ Cr
                        var LedgerBalanceId = Guid.Empty;
                        var updateSundryCreditorLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SundryCreditors && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                        if (updateSundryCreditorLedgerBalance != null)
                        {
                            updateSundryCreditorLedgerBalance.RunningBalance -= newPurchaseOrder.GrandTotal;
                            updateSundryCreditorLedgerBalance.RunningBalanceType = (updateSundryCreditorLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                            await _appDbContext.SaveChangesAsync();
                            LedgerBalanceId = updateSundryCreditorLedgerBalance.LedgerBalanceId;
                        }
                        else
                        {
                            var newLedgerBalance = new LedgerBalance
                            {
                                Fk_LedgerId = MappingLedgers.SundryCreditors,
                                OpeningBalance = 0,
                                OpeningBalanceType = "Cr",
                                RunningBalance = -data.GrandTotal,
                                RunningBalanceType = "Cr",
                                Fk_BranchId = BranchId,
                                Fk_FinancialYear = FinancialYear
                            };
                            await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                            await _appDbContext.SaveChangesAsync();
                            LedgerBalanceId = newLedgerBalance.LedgerBalanceId;
                        }
                        var updateSundryCreditorSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == newPurchaseOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                        if (updateSundryCreditorSubledgerBalance != null)
                        {
                            updateSundryCreditorSubledgerBalance.RunningBalance -= newPurchaseOrder.GrandTotal;
                            updateSundryCreditorSubledgerBalance.RunningBalanceType = (updateSundryCreditorSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                            await _appDbContext.SaveChangesAsync();
                        }
                        else
                        {

                            var newSubLedgerBalance = new SubLedgerBalance
                            {
                                Fk_LedgerBalanceId = LedgerBalanceId,
                                Fk_SubLedgerId = data.Fk_SubLedgerId,
                                OpeningBalanceType = "Cr",
                                OpeningBalance = 0,
                                RunningBalanceType = "Cr",
                                RunningBalance = -data.GrandTotal,
                                Fk_FinancialYearId = FinancialYear,
                                Fk_BranchId = BranchId
                            };
                            await _appDbContext.SubLedgerBalances.AddAsync(newSubLedgerBalance);
                            await _appDbContext.SaveChangesAsync();
                        }
                        //@TransportingChargePayment A/c--------Cr
                        var updateTransportingChargePaymentBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.TransportingChargePayment && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                        if (updateTransportingChargePaymentBalance != null)
                        {
                            updateTransportingChargePaymentBalance.RunningBalance -= newPurchaseOrder.TransportationCharges;
                            updateTransportingChargePaymentBalance.RunningBalanceType = (updateTransportingChargePaymentBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                            await _appDbContext.SaveChangesAsync();
                        }
                        else
                        {
                            var newLedgerBalance = new LedgerBalance
                            {
                                Fk_LedgerId = MappingLedgers.TransportingChargePayment,
                                OpeningBalance = 0,
                                OpeningBalanceType = "Cr",
                                RunningBalance = -data.TransportationCharges,
                                RunningBalanceType = "Cr",
                                Fk_BranchId = BranchId,
                                Fk_FinancialYear = FinancialYear
                            };
                            await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                            await _appDbContext.SaveChangesAsync();
                        }
                        #endregion
                        #region Journal
                        var JournalVoucherNo = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                        string VoucherNo = "";
                        if (JournalVoucherNo != null)
                        {
                            if (int.TryParse(JournalVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                            {
                                currentId++;
                                VoucherNo = $"JN{currentId:D6}";
                            }
                        }
                        else
                        {
                            VoucherNo = "JN000001";
                        }
                        var NewJournalPurchaseOrder = new Journal
                        {
                            VouvherNo = VoucherNo,
                            VoucherDate = newPurchaseOrder.InvoiceDate,
                            Fk_LedgerGroupId = MappingLedgerGroup.CurrentliabilitiesAndProvisions,
                            Fk_LedgerId = MappingLedgers.SundryCreditors,
                            Fk_SubLedgerId = newPurchaseOrder.Fk_SubLedgerId,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear,
                            TransactionNo = newPurchaseOrder.TransactionNo,
                            TransactionId = newPurchaseOrder.PurchaseOrderId,
                            Narration = newPurchaseOrder.TransactionNo.ToString(),
                            Amount = newPurchaseOrder.GrandTotal,
                            DrCr = "Cr"
                        };
                        await _appDbContext.Journals.AddAsync(NewJournalPurchaseOrder);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        foreach (var item in data.RowData)
                        {
                            #region Purchase Trn
                            var newPurchaseTransaction = new PurchaseTransaction()
                            {
                                Fk_PurchaseOrderId = newPurchaseOrder.PurchaseOrderId,
                                TransactionNo = newPurchaseOrder.TransactionNo,
                                TransactionDate = newPurchaseOrder.TransactionDate,
                                Fk_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                Fk_ProductId = Guid.Parse(item[1]),
                                AlternateQuantity = Convert.ToDecimal(item[2]),
                                Fk_AlternateUnitId = Guid.Parse(item[3]),
                                UnitQuantity = Convert.ToDecimal(item[4]),
                                Rate = Convert.ToDecimal(item[5]),
                                Discount = Convert.ToDecimal(item[6]),
                                DiscountAmount = Convert.ToDecimal(item[7]),
                                Gst = Convert.ToDecimal(item[8]),
                                GstAmount = Convert.ToDecimal(item[9]),
                                Amount = Convert.ToDecimal(item[10]),
                            };
                            await _appDbContext.PurchaseTransactions.AddAsync(newPurchaseTransaction);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                            #region Journal
                            var NewJournalPurchaseTransactionr = new Journal
                            {
                                VouvherNo = VoucherNo,
                                VoucherDate = newPurchaseOrder.InvoiceDate,
                                Fk_LedgerGroupId = MappingLedgerGroup.Purchase,
                                Fk_LedgerId = MappingLedgers.PurchaseAccount,
                                Fk_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                TransactionNo = newPurchaseOrder.TransactionNo,
                                TransactionId = newPurchaseTransaction.PurchaseId,
                                Narration = newPurchaseOrder.TransactionNo.ToString(),
                                Amount = newPurchaseTransaction.Amount,
                                DrCr = "Dr"
                            };
                            await _appDbContext.Journals.AddAsync(NewJournalPurchaseTransactionr);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                            #region Update Stock
                            var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newPurchaseTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (UpdateStock != null)
                            {
                                UpdateStock.AvilableStock += newPurchaseTransaction.UnitQuantity;
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var AddNewStock = new Stock
                                {
                                    Fk_BranchId = BranchId,
                                    Fk_ProductId = newPurchaseTransaction.Fk_ProductId,
                                    Fk_FinancialYear = FinancialYear,
                                    AvilableStock = newPurchaseTransaction.UnitQuantity
                                };
                                await _appDbContext.Stocks.AddAsync(AddNewStock);
                                await _appDbContext.SaveChangesAsync();
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/CreatePurchase : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdatePurchase(PurchaseDataRequest data)
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
                    if (DateTime.TryParseExact(data.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedInvoiceDate) && DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTransactionDate))
                    {
                        var UpdatePurchaseOrder = await _appDbContext.PurchaseOrders.Where(s => s.PurchaseOrderId == data.PurchaseOrderId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                        if (UpdatePurchaseOrder != null)
                        {
                            if (UpdatePurchaseOrder.Fk_SubLedgerId == data.Fk_SubLedgerId)
                            {
                                #region Ledger & SubLedger
                                if (UpdatePurchaseOrder.GrandTotal != data.GrandTotal)
                                {
                                    var difference = data.GrandTotal - UpdatePurchaseOrder.GrandTotal;
                                    // @ Purchase A/c --------------- Dr
                                    var updatePurchaseledgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.PurchaseAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (updatePurchaseledgerBalance != null)
                                    {
                                        updatePurchaseledgerBalance.RunningBalance += difference;
                                        updatePurchaseledgerBalance.RunningBalanceType = (updatePurchaseledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    // @SundryCreditor A/c ------------ Cr
                                    var updateSundryCreditorSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == UpdatePurchaseOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSundryCreditorSubledgerBalance != null)
                                    {
                                        updateSundryCreditorSubledgerBalance.RunningBalance -= difference;
                                        updateSundryCreditorSubledgerBalance.RunningBalanceType = (updateSundryCreditorSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateSundryCreditorSubledgerBalance.Fk_LedgerBalanceId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                        if (updateLedgerBalance != null)
                                        {
                                            updateLedgerBalance.RunningBalance -= difference;
                                            updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "SubLedger Balance Not Exist";
                                        return _Result;
                                    }
                                    if (UpdatePurchaseOrder.TransportationCharges != data.TransportationCharges)
                                    {
                                        var TransportChargedifference = data.TransportationCharges - UpdatePurchaseOrder.TransportationCharges;
                                        //@TransportingChargePayment A/c--------Cr
                                        var updateTransportingChargePaymentBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.TransportingChargePayment && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (updateTransportingChargePaymentBalance != null)
                                        {
                                            updateTransportingChargePaymentBalance.RunningBalance -= TransportChargedifference;
                                            updateTransportingChargePaymentBalance.RunningBalanceType = (updateTransportingChargePaymentBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                    }
                                }
                                #endregion
                                #region Journal
                                var UpdateOrderJournal = await _appDbContext.Journals.Where(s => s.TransactionId == UpdatePurchaseOrder.PurchaseOrderId && s.Fk_SubLedgerId == UpdatePurchaseOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (UpdateOrderJournal != null)
                                {
                                    UpdateOrderJournal.Amount = data.GrandTotal;
                                    UpdateOrderJournal.VoucherDate = convertedTransactionDate;
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    _Result.WarningMessage = "Journal Entry Not Exist";
                                    return _Result;
                                }
                                #endregion
                                #region Purchase Odr
                                UpdatePurchaseOrder.Fk_ProductTypeId = data.Fk_ProductTypeId;
                                UpdatePurchaseOrder.TransactionDate = convertedTransactionDate;
                                UpdatePurchaseOrder.InvoiceNo = data.InvoiceNo;
                                UpdatePurchaseOrder.InvoiceDate = convertedInvoiceDate;
                                UpdatePurchaseOrder.TransportationCharges = data.TransportationCharges;
                                UpdatePurchaseOrder.VehicleNo = data.VehicleNo;
                                UpdatePurchaseOrder.TranspoterName = data.TranspoterName;
                                UpdatePurchaseOrder.Narration = data.Naration;
                                UpdatePurchaseOrder.ReceivingPerson = data.ReceivingPerson;
                                UpdatePurchaseOrder.SubTotal = Convert.ToDecimal(data.SubTotal);
                                UpdatePurchaseOrder.Discount = Convert.ToDecimal(data.DiscountAmount);
                                UpdatePurchaseOrder.Gst = Convert.ToDecimal(data.GstAmount);
                                UpdatePurchaseOrder.GrandTotal = Convert.ToDecimal(data.GrandTotal);
                                await _appDbContext.SaveChangesAsync();
                                #endregion
                                //************************************************************Purchase Transaction**************************************************//
                                foreach (var item in data.RowData)
                                {
                                    Guid PurchaseId = Guid.TryParse(item[0], out var parsedGuid) ? parsedGuid : Guid.Empty;
                                    if (PurchaseId != Guid.Empty)
                                    {
                                        var UpdatePurchaseTransaction = await _appDbContext.PurchaseTransactions.Where(s => s.PurchaseId == PurchaseId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                        #region Update Stock
                                        var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == UpdatePurchaseTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateStock != null)
                                        {
                                            if (UpdatePurchaseTransaction.Fk_ProductId != Guid.Parse(item[1]))
                                            {
                                                UpdateStock.AvilableStock -= UpdatePurchaseTransaction.UnitQuantity;
                                                await _appDbContext.SaveChangesAsync();
                                                var UpdateNewStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == Guid.Parse(item[1]) && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                                if (UpdateNewStock != null)
                                                {
                                                    UpdateNewStock.AvilableStock += Convert.ToDecimal(item[4]);
                                                    await _appDbContext.SaveChangesAsync();
                                                }
                                                else
                                                {
                                                    var newStock = new Stock
                                                    {
                                                        Fk_BranchId = BranchId,
                                                        Fk_FinancialYear = FinancialYear,
                                                        Fk_ProductId = Guid.Parse(item[1]),
                                                        AvilableStock = Convert.ToDecimal(item[4])
                                                    };
                                                    await _appDbContext.Stocks.AddAsync(newStock);
                                                    await _appDbContext.SaveChangesAsync();
                                                }
                                            }
                                            else if (UpdatePurchaseTransaction.UnitQuantity != Convert.ToDecimal(item[4]))
                                            {
                                                UpdateStock.AvilableStock -= (UpdatePurchaseTransaction.UnitQuantity - Convert.ToDecimal(item[4]));
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        #endregion
                                        #region Update Journal
                                        var UpdateTransactionJournal = await _appDbContext.Journals.Where(s => s.TransactionId == UpdatePurchaseTransaction.PurchaseId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateTransactionJournal != null)
                                        {
                                            UpdateTransactionJournal.Amount = Convert.ToDecimal(item[10]);
                                            UpdateTransactionJournal.VoucherDate = convertedTransactionDate;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Journal Entry Not Exist";
                                            return _Result;
                                        }
                                        #endregion
                                        #region Update Purchase Trn
                                        UpdatePurchaseTransaction.TransactionDate = convertedTransactionDate;
                                        UpdatePurchaseTransaction.Fk_ProductId = Guid.Parse(item[1]);
                                        UpdatePurchaseTransaction.AlternateQuantity = Convert.ToDecimal(item[2]);
                                        UpdatePurchaseTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                        UpdatePurchaseTransaction.UnitQuantity = Convert.ToDecimal(item[4]);
                                        UpdatePurchaseTransaction.Rate = Convert.ToDecimal(item[5]);
                                        UpdatePurchaseTransaction.Discount = Convert.ToDecimal(item[6]);
                                        UpdatePurchaseTransaction.DiscountAmount = Convert.ToDecimal(item[7]);
                                        UpdatePurchaseTransaction.Gst = Convert.ToDecimal(item[8]);
                                        UpdatePurchaseTransaction.GstAmount = Convert.ToDecimal(item[9]);
                                        UpdatePurchaseTransaction.Amount = Convert.ToDecimal(item[10]);
                                        await _appDbContext.SaveChangesAsync();
                                        #endregion
                                    }
                                    else
                                    {
                                        #region New Purchase Trn
                                        var newPurchaseTransaction = new PurchaseTransaction
                                        {
                                            Fk_PurchaseOrderId = data.PurchaseOrderId,
                                            TransactionNo = data.TransactionNo,
                                            Fk_BranchId = BranchId,
                                            Fk_FinancialYearId = FinancialYear,
                                            Fk_ProductId = Guid.Parse(item[1]),
                                            AlternateQuantity = Convert.ToDecimal(item[2]),
                                            Fk_AlternateUnitId = Guid.Parse(item[3]),
                                            UnitQuantity = Convert.ToDecimal(item[4]),
                                            Rate = Convert.ToDecimal(item[5]),
                                            Discount = Convert.ToDecimal(item[6]),
                                            DiscountAmount = Convert.ToDecimal(item[7]),
                                            Gst = Convert.ToDecimal(item[8]),
                                            GstAmount = Convert.ToDecimal(item[9]),
                                            Amount = Convert.ToDecimal(item[10])
                                        };
                                        await _appDbContext.PurchaseTransactions.AddAsync(newPurchaseTransaction);
                                        await _appDbContext.SaveChangesAsync();
                                        #endregion
                                        #region New Journal
                                        var NewJournalPurchaseTransactionr = new Journal
                                        {
                                            VouvherNo = UpdateOrderJournal.VouvherNo,
                                            VoucherDate = UpdatePurchaseOrder.InvoiceDate,
                                            Fk_LedgerGroupId = MappingLedgerGroup.Purchase,
                                            Fk_LedgerId = MappingLedgers.PurchaseAccount,
                                            Fk_BranchId = BranchId,
                                            Fk_FinancialYearId = FinancialYear,
                                            TransactionNo = UpdatePurchaseOrder.TransactionNo,
                                            TransactionId = newPurchaseTransaction.PurchaseId,
                                            Narration = UpdatePurchaseOrder.TransactionNo.ToString(),
                                            Amount = newPurchaseTransaction.Amount,
                                            DrCr = "Dr"
                                        };
                                        await _appDbContext.Journals.AddAsync(NewJournalPurchaseTransactionr);
                                        await _appDbContext.SaveChangesAsync();
                                        #endregion
                                        #region Update Stock
                                        var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newPurchaseTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateStock != null)
                                        {
                                            UpdateStock.AvilableStock += newPurchaseTransaction.UnitQuantity;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            var AddNewStock = new Stock
                                            {
                                                Fk_BranchId = BranchId,
                                                Fk_ProductId = newPurchaseTransaction.Fk_ProductId,
                                                Fk_FinancialYear = FinancialYear,
                                                AvilableStock = newPurchaseTransaction.UnitQuantity
                                            };
                                            await _appDbContext.Stocks.AddAsync(AddNewStock);
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        #endregion
                                    }
                                }
                                _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                                transaction.Commit();
                                _Result.IsSuccess = true;
                            }
                            else
                            {
                                _Result.WarningMessage = "If Your Intension To Update Party Name Plz Delate This Record And Then Crate It";
                                return _Result;
                            }
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    _Result.Exception = ex;
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/UpdatePurchase : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeletePurchase(Guid Id, IDbContextTransaction transaction, bool IsCallback)
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
                        //*****************************Delete PurchaseTransactions**************************************//
                        var deletePurchaseTransaction = await _appDbContext.PurchaseTransactions.Where(s => s.Fk_PurchaseOrderId == Id && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                        if (deletePurchaseTransaction.Count > 0)
                        {
                            foreach (var item in deletePurchaseTransaction)
                            {
                                #region Stock
                                var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (UpdateStock != null)
                                {
                                    UpdateStock.AvilableStock -= item.UnitQuantity;
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    continue;
                                }
                                #endregion
                            }
                            _appDbContext.PurchaseTransactions.RemoveRange(deletePurchaseTransaction);
                            await _appDbContext.SaveChangesAsync();
                        }
                        //*******************************Delete PurchaseOrders***************************************//
                        var deletePurchaseOrders = await _appDbContext.PurchaseOrders.SingleOrDefaultAsync(x => x.PurchaseOrderId == Id && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYear);
                        if (deletePurchaseOrders != null)
                        {
                            #region Journal
                            var DeleteJournal = await _appDbContext.Journals.Where(s => s.TransactionNo == deletePurchaseOrders.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                            if (DeleteJournal != null)
                            {
                                _appDbContext.Journals.RemoveRange(DeleteJournal);
                                await _appDbContext.SaveChangesAsync();
                            }
                            #endregion
                            #region Ledger & SubLedger
                            var updateSundryCreditorSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == deletePurchaseOrders.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                            if (updateSundryCreditorSubledgerBalance != null)
                            {
                                updateSundryCreditorSubledgerBalance.RunningBalance += deletePurchaseOrders.GrandTotal;
                                await _appDbContext.SaveChangesAsync();
                                var updateSundryCreditorLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SundryCreditors && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateSundryCreditorLedgerBalance != null)
                                {
                                    updateSundryCreditorLedgerBalance.RunningBalance += deletePurchaseOrders.GrandTotal;
                                    updateSundryCreditorLedgerBalance.RunningBalanceType = updateSundryCreditorLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    _Result.WarningMessage = "Ledger Balance Not Exist";
                                    return _Result;
                                }
                            }
                            else
                            {
                                _Result.WarningMessage = "SubLedger Balance Not Exist";
                                return _Result;
                            }
                            var updatePurchasedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.PurchaseAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updatePurchasedgerBalance != null)
                            {
                                updatePurchasedgerBalance.RunningBalance -= deletePurchaseOrders.GrandTotal;
                                updatePurchasedgerBalance.RunningBalanceType = updatePurchasedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                _Result.WarningMessage = "Ledger Balance Not Exist";
                                return _Result;
                            }
                            #endregion
                            _appDbContext.PurchaseOrders.Remove(deletePurchaseOrders);
                            await _appDbContext.SaveChangesAsync();
                        }
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/DeletePurchase : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Purchase Return
        public async Task<Result<string>> GetLastPurchaseReturnTransaction()
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var lastPurchaseReturnOrder = await _appDbContext.PurchaseReturnOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).OrderByDescending(s => s.TransactionNo).Select(s => new { s.TransactionNo }).FirstOrDefaultAsync();
                if (lastPurchaseReturnOrder != null)
                {
                    var lastTransactionId = lastPurchaseReturnOrder.TransactionNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastTransactionId.AsSpan(2), out int currentId))
                    {
                        currentId++;
                        var newTransactionId = $"PR{currentId:D6}";
                        _Result.SingleObjData = newTransactionId;
                    }
                }
                else
                {
                    _Result.SingleObjData = "PR000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetLastPurchaseReturnTransaction : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<PurchaseReturnOrderModel>> GetPurchaseReturns()
        {
            Result<PurchaseReturnOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.PurchaseReturnOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new PurchaseReturnOrderModel
                {
                    PurchaseReturnOrderId = s.PurchaseReturnOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    InvoiceNo = s.InvoiceNo,
                    GrandTotal = s.GrandTotal,
                    SubLedger = s.SubLedger != null ? new SubLedgerModel { SubLedgerName = s.SubLedger.SubLedgerName } : null,
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    var OrderList = Query;
                    _Result.CollectionObjData = OrderList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetPurchaseReturns : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<PurchaseReturnOrderModel>> GetPurchaseReturnById(Guid Id)
        {
            Result<PurchaseReturnOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.PurchaseReturnOrders.Where(s => s.PurchaseReturnOrderId == Id && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new PurchaseReturnOrderModel
                {
                    PurchaseReturnOrderId = s.PurchaseReturnOrderId,
                    Fk_ProductTypeId = s.Fk_ProductTypeId,
                    Fk_SubLedgerId = s.Fk_SubLedgerId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    InvoiceNo = s.InvoiceNo,
                    InvoiceDate = s.InvoiceDate,
                    TransportationCharges = s.TransportationCharges,
                    VehicleNo = s.VehicleNo,
                    TranspoterName = s.TranspoterName,
                    ReceivingPerson = s.ReceivingPerson,
                    Naration = s.Narration,
                    Discount = s.Discount,
                    Gst = s.Gst,
                    SubTotal = s.SubTotal,
                    GrandTotal = s.GrandTotal,
                    PurchaseReturnTransactions = _appDbContext.PurchaseReturnTransactions.Where(x => x.Fk_PurchaseReturnOrderId == s.PurchaseReturnOrderId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(x => new PurchaseReturnTransactionModel
                    {
                        PurchaseReturnId = x.PurchaseReturnId,
                        UnitQuantity = x.UnitQuantity,
                        UnitName = x.Product.Unit.UnitName,
                        AlternateQuantity = x.AlternateQuantity,
                        Fk_AlternateUnitId = x.Fk_AlternateUnitId,
                        AlternateUnit = x.AlternateUnit != null ? new AlternateUnitModel { AlternateUnitName = x.AlternateUnit.AlternateUnitName } : null,
                        Rate = x.Rate,
                        Discount = x.Discount,
                        DiscountAmount = x.DiscountAmount,
                        Gst = x.Gst,
                        GstAmount = x.GstAmount,
                        Amount = x.Amount,
                        Fk_ProductId = x.Fk_ProductId,
                        Product = x.Product != null ? new ProductModel { ProductName = x.Product.ProductName } : null,
                    }).ToList(),
                }).SingleOrDefaultAsync();
                if (Query != null)
                {
                    var Order = Query;
                    _Result.SingleObjData = Order;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetPurchaseReturnById : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreatetPurchaseReturn(PurchaseDataRequest data)
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
                    if (DateTime.TryParseExact(data.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedInvoiceDate) && DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTransactionDate))
                    {
                        #region purchase Return Order
                        var newPurchaseReturnOrder = new PurchaseReturnOrder
                        {
                            Fk_ProductTypeId = data.Fk_ProductTypeId,
                            Fk_SubLedgerId = data.Fk_SubLedgerId,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear,
                            TransactionDate = convertedTransactionDate,
                            TransactionNo = data.TransactionNo,
                            InvoiceNo = data.InvoiceNo,
                            InvoiceDate = convertedInvoiceDate,
                            TransportationCharges = data.TransportationCharges,
                            TranspoterName = data.TranspoterName,
                            ReceivingPerson = data.ReceivingPerson,
                            Narration = data.Naration,
                            VehicleNo = data.VehicleNo,
                            SubTotal = data.SubTotal,
                            Discount = data.DiscountAmount,
                            Gst = data.GstAmount,
                            GrandTotal = data.GrandTotal,
                        };
                        await _appDbContext.PurchaseReturnOrders.AddAsync(newPurchaseReturnOrder);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        #region Ledger & Sub Ledger
                        // @ PurchaseReturn A/c --------- Cr
                        var updatePurchaseReturnledgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.PurchaseReturnAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                        if (updatePurchaseReturnledgerBalance != null)
                        {
                            updatePurchaseReturnledgerBalance.RunningBalance -= newPurchaseReturnOrder.GrandTotal;
                            updatePurchaseReturnledgerBalance.RunningBalanceType = (updatePurchaseReturnledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                            await _appDbContext.SaveChangesAsync();
                        }
                        else
                        {
                            var newLedgerBalance = new LedgerBalance
                            {
                                Fk_LedgerId = MappingLedgers.PurchaseReturnAccount,
                                OpeningBalance = 0,
                                OpeningBalanceType = "Cr",
                                RunningBalance = -newPurchaseReturnOrder.GrandTotal,
                                RunningBalanceType = "Cr",
                                Fk_BranchId = BranchId,
                                Fk_FinancialYear = FinancialYear
                            };
                            await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                            await _appDbContext.SaveChangesAsync();
                        }
                        //@TransportingChargePayment A/c--------Cr
                        /* var updateTransportingChargePaymentBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.TransportingChargePayment && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                        if (updateTransportingChargePaymentBalance != null)
                        {
                            updateTransportingChargePaymentBalance.RunningBalance -= newPurchaseReturnOrder.TransportationCharges;
                            updateTransportingChargePaymentBalance.RunningBalanceType = (updateTransportingChargePaymentBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                            await _appDbContext.SaveChangesAsync();
                        }
                        else
                        {
                            var newLedgerBalance = new LedgerBalance
                            {
                                Fk_LedgerId = MappingLedgers.TransportingChargePayment,
                                OpeningBalance = 0,
                                OpeningBalanceType = "Cr",
                                RunningBalance = -data.TransportationCharges,
                                RunningBalanceType = "Cr",
                                Fk_BranchId = BranchId,
                                Fk_FinancialYear = FinancialYear
                            };
                            await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                            await _appDbContext.SaveChangesAsync();
                        }*/
                        // @SundryCreditor A/c ------------ Dr
                        var LedgerBalanceId = Guid.Empty;
                        var updateSundryCreditorLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SundryCreditors && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                        if (updateSundryCreditorLedgerBalance != null)
                        {
                            updateSundryCreditorLedgerBalance.RunningBalance += newPurchaseReturnOrder.GrandTotal;
                            updateSundryCreditorLedgerBalance.RunningBalanceType = (updateSundryCreditorLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                            await _appDbContext.SaveChangesAsync();
                            LedgerBalanceId = updateSundryCreditorLedgerBalance.LedgerBalanceId;
                        }
                        else
                        {
                            var newLedgerBalance = new LedgerBalance
                            {
                                Fk_LedgerId = MappingLedgers.SundryCreditors,
                                OpeningBalance = 0,
                                OpeningBalanceType = "Dr",
                                RunningBalance = data.GrandTotal,
                                RunningBalanceType = "Dr",
                                Fk_BranchId = BranchId,
                                Fk_FinancialYear = FinancialYear
                            };
                            await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                            await _appDbContext.SaveChangesAsync();
                            LedgerBalanceId = newLedgerBalance.LedgerBalanceId;
                        }
                        var updateSundryCreditorSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == newPurchaseReturnOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                        if (updateSundryCreditorSubledgerBalance != null)
                        {
                            updateSundryCreditorSubledgerBalance.RunningBalance += newPurchaseReturnOrder.GrandTotal;
                            updateSundryCreditorSubledgerBalance.RunningBalanceType = (updateSundryCreditorSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                            await _appDbContext.SaveChangesAsync();
                        }
                        else
                        {
                            var newSubLedgerBalance = new SubLedgerBalance
                            {
                                Fk_LedgerBalanceId = LedgerBalanceId,
                                Fk_SubLedgerId = data.Fk_SubLedgerId,
                                OpeningBalanceType = "Dr",
                                OpeningBalance = 0,
                                RunningBalanceType = "Dr",
                                RunningBalance = data.GrandTotal,
                                Fk_FinancialYearId = FinancialYear,
                                Fk_BranchId = BranchId
                            };
                            await _appDbContext.SubLedgerBalances.AddAsync(newSubLedgerBalance);
                            await _appDbContext.SaveChangesAsync();
                        }

                        #endregion
                        #region Journal
                        var JournalVoucherNo = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                        string VoucherNo = "";
                        if (JournalVoucherNo != null)
                        {
                            if (int.TryParse(JournalVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                            {
                                currentId++;
                                VoucherNo = $"JN{currentId:D6}";
                            }
                        }
                        else
                        {
                            VoucherNo = "JN000001";
                        }
                        var NewJournalPurchaseReturnOrder = new Journal
                        {
                            VouvherNo = VoucherNo,
                            VoucherDate = newPurchaseReturnOrder.InvoiceDate,
                            Fk_LedgerGroupId = MappingLedgerGroup.CurrentliabilitiesAndProvisions,
                            Fk_LedgerId = MappingLedgers.SundryCreditors,
                            Fk_SubLedgerId = newPurchaseReturnOrder.Fk_SubLedgerId,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear,
                            TransactionNo = newPurchaseReturnOrder.TransactionNo,
                            TransactionId = newPurchaseReturnOrder.PurchaseReturnOrderId,
                            Narration = newPurchaseReturnOrder.TransactionNo.ToString(),
                            Amount = newPurchaseReturnOrder.GrandTotal,
                            DrCr = "Dr"
                        };
                        await _appDbContext.Journals.AddAsync(NewJournalPurchaseReturnOrder);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        foreach (var item in data.RowData)
                        {
                            #region Purchase Return Transaction
                            var newPurchaseReturnTransaction = new PurchaseReturnTransaction
                            {
                                Fk_PurchaseReturnOrderId = newPurchaseReturnOrder.PurchaseReturnOrderId,
                                TransactionNo = newPurchaseReturnOrder.TransactionNo,
                                TransactionDate = newPurchaseReturnOrder.TransactionDate,
                                Fk_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                Fk_ProductId = Guid.Parse(item[1]),
                                AlternateQuantity = Convert.ToDecimal(item[2]),
                                Fk_AlternateUnitId = Guid.Parse(item[3]),
                                UnitQuantity = Convert.ToDecimal(item[4]),
                                Rate = Convert.ToDecimal(item[5]),
                                Discount = Convert.ToDecimal(item[6]),
                                DiscountAmount = Convert.ToDecimal(item[7]),
                                Gst = Convert.ToDecimal(item[8]),
                                GstAmount = Convert.ToDecimal(item[9]),
                                Amount = Convert.ToDecimal(item[10])
                            };
                            await _appDbContext.PurchaseReturnTransactions.AddAsync(newPurchaseReturnTransaction);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                            #region Journal
                            var NewJournalPurchaseTransaction = new Journal
                            {
                                VouvherNo = VoucherNo,
                                VoucherDate = newPurchaseReturnOrder.InvoiceDate,
                                Fk_LedgerGroupId = MappingLedgerGroup.Purchase,
                                Fk_LedgerId = MappingLedgers.PurchaseReturnAccount,
                                Fk_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                TransactionNo = newPurchaseReturnOrder.TransactionNo,
                                TransactionId = newPurchaseReturnTransaction.PurchaseReturnId,
                                Narration = newPurchaseReturnOrder.TransactionNo.ToString(),
                                Amount = newPurchaseReturnOrder.GrandTotal,
                                DrCr = "Cr"
                            };
                            await _appDbContext.Journals.AddAsync(NewJournalPurchaseTransaction);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                            #region Stock
                            var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newPurchaseReturnTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (UpdateStock != null)
                            {
                                UpdateStock.AvilableStock -= newPurchaseReturnTransaction.UnitQuantity;
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var AddNewStock = new Stock
                                {
                                    Fk_ProductId = newPurchaseReturnTransaction.Fk_ProductId,
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear,
                                    AvilableStock = -newPurchaseReturnTransaction.UnitQuantity
                                };
                                await _appDbContext.Stocks.AddAsync(AddNewStock);
                                await _appDbContext.SaveChangesAsync();
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/CreatetPurchaseReturn : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdatetPurchaseReturn(PurchaseDataRequest data)
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
                    if (DateTime.TryParseExact(data.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedInvoiceDate) && DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTransactionDate))
                    {
                        //************************************************************purchase Return Order*************************************************************//
                        var UpdatePurchaseReturnOrder = await _appDbContext.PurchaseReturnOrders.Where(s => s.PurchaseReturnOrderId == data.PurchaseOrderId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                        if (UpdatePurchaseReturnOrder != null)
                        {
                            if (UpdatePurchaseReturnOrder.Fk_SubLedgerId == data.Fk_SubLedgerId)
                            {
                                if (UpdatePurchaseReturnOrder.GrandTotal != data.GrandTotal)
                                {
                                    var difference = data.GrandTotal - UpdatePurchaseReturnOrder.GrandTotal;
                                    #region Ledger & Sub Ledger
                                    // @ PurchaseReturn A/c --------- Cr
                                    var updatePurchaseReturnledgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.PurchaseReturnAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (updatePurchaseReturnledgerBalance != null)
                                    {
                                        updatePurchaseReturnledgerBalance.RunningBalance -= difference;
                                        updatePurchaseReturnledgerBalance.RunningBalanceType = (updatePurchaseReturnledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    // @SundryCreditor A/c ------------ Dr
                                    var updateSundryCreditorSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == UpdatePurchaseReturnOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSundryCreditorSubledgerBalance != null)
                                    {
                                        updateSundryCreditorSubledgerBalance.RunningBalance += difference;
                                        updateSundryCreditorSubledgerBalance.RunningBalanceType = (updateSundryCreditorSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        var updateSundryCreditorLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateSundryCreditorSubledgerBalance.Fk_LedgerBalanceId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (updateSundryCreditorLedgerBalance != null)
                                        {
                                            updateSundryCreditorLedgerBalance.RunningBalance += difference;
                                            updateSundryCreditorLedgerBalance.RunningBalanceType = (updateSundryCreditorLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "SubLedger Balance Not Exist";
                                        return _Result;
                                    }
                                    #endregion
                                }
                                #region Journal
                                var UpdateOrderJournal = await _appDbContext.Journals.Where(s => s.TransactionId == UpdatePurchaseReturnOrder.PurchaseReturnOrderId && s.Fk_SubLedgerId == UpdatePurchaseReturnOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (UpdateOrderJournal != null)
                                {
                                    UpdateOrderJournal.Amount = data.GrandTotal;
                                    UpdateOrderJournal.VoucherDate = convertedTransactionDate;
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                                #region PurchaseReturn Odr
                                UpdatePurchaseReturnOrder.Fk_ProductTypeId = data.Fk_ProductTypeId;
                                UpdatePurchaseReturnOrder.TransactionDate = convertedTransactionDate;
                                UpdatePurchaseReturnOrder.InvoiceNo = data.InvoiceNo;
                                UpdatePurchaseReturnOrder.InvoiceDate = convertedInvoiceDate;
                                UpdatePurchaseReturnOrder.TransportationCharges = data.TransportationCharges;
                                UpdatePurchaseReturnOrder.VehicleNo = data.VehicleNo;
                                UpdatePurchaseReturnOrder.TranspoterName = data.TranspoterName;
                                UpdatePurchaseReturnOrder.ReceivingPerson = data.ReceivingPerson;
                                UpdatePurchaseReturnOrder.Narration = data.Naration;
                                UpdatePurchaseReturnOrder.SubTotal = Convert.ToDecimal(data.SubTotal);
                                UpdatePurchaseReturnOrder.Discount = Convert.ToDecimal(data.DiscountAmount);
                                UpdatePurchaseReturnOrder.Gst = Convert.ToDecimal(data.GstAmount);
                                UpdatePurchaseReturnOrder.GrandTotal = Convert.ToDecimal(data.GrandTotal);
                                await _appDbContext.SaveChangesAsync();
                                #endregion
                                //************************************************************Purchase Transaction**************************************************//
                                foreach (var item in data.RowData)
                                {
                                    Guid PurchaseReturnId = Guid.TryParse(item[0], out var parsedGuid) ? parsedGuid : Guid.Empty;
                                    if (PurchaseReturnId != Guid.Empty)
                                    {
                                        var UpdatePurchaseReturnTransaction = await _appDbContext.PurchaseReturnTransactions.Where(s => s.PurchaseReturnId == PurchaseReturnId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                        #region Stock
                                        var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == UpdatePurchaseReturnTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateStock != null)
                                        {
                                            if (UpdatePurchaseReturnTransaction.Fk_ProductId != Guid.Parse(item[1]))
                                            {
                                                UpdateStock.AvilableStock -= UpdatePurchaseReturnTransaction.UnitQuantity;
                                                await _appDbContext.SaveChangesAsync();
                                                var UpdateNewStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == Guid.Parse(item[1]) && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                                if (UpdateNewStock != null)
                                                {
                                                    UpdateNewStock.AvilableStock -= Convert.ToDecimal(item[4]);
                                                    await _appDbContext.SaveChangesAsync();
                                                }
                                                else
                                                {
                                                    var newStock = new Stock
                                                    {
                                                        Fk_BranchId = BranchId,
                                                        Fk_FinancialYear = FinancialYear,
                                                        Fk_ProductId = Guid.Parse(item[1]),
                                                        AvilableStock = Convert.ToDecimal(item[4])
                                                    };
                                                    await _appDbContext.Stocks.AddAsync(newStock);
                                                    await _appDbContext.SaveChangesAsync();
                                                }
                                            }
                                            if (UpdatePurchaseReturnTransaction.UnitQuantity != Convert.ToDecimal(item[4]))
                                            {
                                                UpdateStock.AvilableStock += UpdatePurchaseReturnTransaction.UnitQuantity - Convert.ToDecimal(item[4]);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                        #endregion
                                        #region Journal
                                        var UpdateTransactionJournal = await _appDbContext.Journals.Where(s => s.TransactionId == UpdatePurchaseReturnTransaction.PurchaseReturnId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateTransactionJournal != null)
                                        {
                                            UpdateTransactionJournal.Amount = Convert.ToDecimal(item[8]);
                                            UpdateTransactionJournal.VoucherDate = convertedTransactionDate;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        #endregion
                                        #region PurchaseReturn Trn
                                        UpdatePurchaseReturnTransaction.Fk_ProductId = Guid.Parse(item[1]);
                                        UpdatePurchaseReturnTransaction.AlternateQuantity = Convert.ToDecimal(item[2]);
                                        UpdatePurchaseReturnTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                        UpdatePurchaseReturnTransaction.UnitQuantity = Convert.ToDecimal(item[4]);
                                        UpdatePurchaseReturnTransaction.Rate = Convert.ToDecimal(item[5]);
                                        UpdatePurchaseReturnTransaction.Discount = Convert.ToDecimal(item[6]);
                                        UpdatePurchaseReturnTransaction.DiscountAmount = Convert.ToDecimal(item[7]);
                                        UpdatePurchaseReturnTransaction.Gst = Convert.ToDecimal(item[8]);
                                        UpdatePurchaseReturnTransaction.GstAmount = Convert.ToDecimal(item[9]);
                                        UpdatePurchaseReturnTransaction.Amount = Convert.ToDecimal(item[10]);
                                        UpdatePurchaseReturnTransaction.TransactionDate = convertedTransactionDate;
                                        await _appDbContext.SaveChangesAsync();
                                        #endregion
                                    }
                                    else
                                    {
                                        #region PurchaseReturn Trn
                                        var newPurchaseReturnTransaction = new PurchaseReturnTransaction
                                        {
                                            Fk_PurchaseReturnOrderId = data.PurchaseOrderId,
                                            TransactionNo = data.TransactionNo,
                                            TransactionDate = convertedTransactionDate,
                                            Fk_BranchId = BranchId,
                                            Fk_FinancialYearId = FinancialYear,
                                            Fk_ProductId = Guid.Parse(item[1]),
                                            AlternateQuantity = Convert.ToDecimal(item[2]),
                                            Fk_AlternateUnitId = Guid.Parse(item[3]),
                                            UnitQuantity = Convert.ToDecimal(item[4]),
                                            Rate = Convert.ToDecimal(item[5]),
                                            Discount = Convert.ToDecimal(item[6]),
                                            DiscountAmount = Convert.ToDecimal(item[7]),
                                            Gst = Convert.ToDecimal(item[8]),
                                            GstAmount = Convert.ToDecimal(item[9]),
                                            Amount = Convert.ToDecimal(item[10])
                                        };
                                        await _appDbContext.PurchaseReturnTransactions.AddAsync(newPurchaseReturnTransaction);
                                        await _appDbContext.SaveChangesAsync();
                                        #endregion
                                        #region Journal
                                        var NewJournalPurchaseReturnTransaction = new Journal
                                        {
                                            VouvherNo = UpdateOrderJournal.VouvherNo,
                                            VoucherDate = UpdatePurchaseReturnOrder.InvoiceDate,
                                            Fk_LedgerGroupId = MappingLedgerGroup.CurrentliabilitiesAndProvisions,
                                            Fk_LedgerId = MappingLedgers.SundryCreditors,
                                            Fk_SubLedgerId = UpdatePurchaseReturnOrder.Fk_SubLedgerId,
                                            Fk_BranchId = BranchId,
                                            Fk_FinancialYearId = FinancialYear,
                                            TransactionNo = UpdatePurchaseReturnOrder.TransactionNo,
                                            TransactionId = newPurchaseReturnTransaction.PurchaseReturnId,
                                            Narration = UpdatePurchaseReturnOrder.TransactionNo.ToString(),
                                            Amount = newPurchaseReturnTransaction.Amount,
                                            DrCr = "Cr"
                                        };
                                        await _appDbContext.Journals.AddAsync(NewJournalPurchaseReturnTransaction);
                                        await _appDbContext.SaveChangesAsync();
                                        #endregion
                                        #region Stock
                                        var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newPurchaseReturnTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateStock != null)
                                        {
                                            UpdateStock.AvilableStock -= newPurchaseReturnTransaction.UnitQuantity;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            var newStock = new Stock
                                            {
                                                Fk_BranchId = BranchId,
                                                Fk_FinancialYear = FinancialYear,
                                                Fk_ProductId = Guid.Parse(item[1]),
                                                AvilableStock = Convert.ToDecimal(item[4])
                                            };
                                            await _appDbContext.Stocks.AddAsync(newStock);
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        #endregion
                                    }
                                }
                                _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                                transaction.Commit();
                                _Result.IsSuccess = true;
                            }
                            else
                            {
                                _Result.WarningMessage = "If Your Intension To Update Party Name Plz Delate It And Then Crate It";
                                return _Result;
                            }
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    _Result.Exception = ex;
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/UpdatetPurchaseReturn : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeletetPurchaseReturn(Guid Id, IDbContextTransaction transaction, bool IsCallback)
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
                        //*****************************Delete PurchaseTransactions**************************************//
                        var deletePurchaseReturnTransaction = await _appDbContext.PurchaseReturnTransactions.Where(s => s.Fk_PurchaseReturnOrderId == Id && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                        if (deletePurchaseReturnTransaction.Count > 0)
                        {
                            foreach (var item in deletePurchaseReturnTransaction)
                            {
                                #region Stock
                                var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (UpdateStock != null)
                                {
                                    UpdateStock.AvilableStock += item.UnitQuantity;
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                                _appDbContext.PurchaseReturnTransactions.Remove(item);
                                await _appDbContext.SaveChangesAsync();
                            }
                        }
                        //*******************************Delete PurchaseOrders***************************************//
                        var deletePurchaseReturnsOrders = await _appDbContext.PurchaseReturnOrders.SingleOrDefaultAsync(x => x.PurchaseReturnOrderId == Id && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYear);
                        if (deletePurchaseReturnsOrders != null)
                        {
                            #region Journal
                            var DeleteJournal = await _appDbContext.Journals.Where(s => s.TransactionNo == deletePurchaseReturnsOrders.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                            _appDbContext.Journals.RemoveRange(DeleteJournal);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                            #region LedgerBalance & Subledger
                            var updateSundryCreditorSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == deletePurchaseReturnsOrders.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                            if (updateSundryCreditorSubledgerBalance != null)
                            {
                                updateSundryCreditorSubledgerBalance.RunningBalance -= deletePurchaseReturnsOrders.GrandTotal;
                                updateSundryCreditorSubledgerBalance.RunningBalanceType = updateSundryCreditorSubledgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                var updateSundryCreditorLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateSundryCreditorSubledgerBalance.Fk_LedgerBalanceId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateSundryCreditorLedgerBalance != null)
                                {
                                    updateSundryCreditorLedgerBalance.RunningBalance -= deletePurchaseReturnsOrders.GrandTotal;
                                    updateSundryCreditorLedgerBalance.RunningBalanceType = updateSundryCreditorLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                }
                                await _appDbContext.SaveChangesAsync();
                            }
                            var updatePurchaseReturnldgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.PurchaseReturnAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updatePurchaseReturnldgerBalance != null)
                            {
                                updatePurchaseReturnldgerBalance.RunningBalance += deletePurchaseReturnsOrders.GrandTotal;
                                updatePurchaseReturnldgerBalance.RunningBalanceType = updatePurchaseReturnldgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            #endregion
                            _appDbContext.PurchaseReturnOrders.Remove(deletePurchaseReturnsOrders);
                            await _appDbContext.SaveChangesAsync();
                        }
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/DeletetPurchaseReturn : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #endregion
        #region Production Entry
        public async Task<Result<string>> GetLastProductionNo()
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var lastProduction = await _appDbContext.LabourOrders.Where(s => s.FK_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.Fk_LabourTypeId == MappingLabourType.Production).OrderByDescending(s => s.TransactionNo).Select(s => new { s.TransactionNo }).FirstOrDefaultAsync();
                if (lastProduction != null)
                {
                    var lastProductionNo = lastProduction.TransactionNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastProductionNo.AsSpan(2), out int currentId))
                    {
                        currentId++;
                        var newProductionNo = $"PN{currentId:D6}";
                        _Result.SingleObjData = newProductionNo;
                    }
                }
                else
                {
                    _Result.SingleObjData = "PN000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/GetLastProductionNo : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<ProductionModel>> GetProductionConfig(Guid ProductId)
        {
            Result<ProductionModel> _Result = new();
            try
            {
                var Query = await (from Production in _appDbContext.Productions
                                   join product in _appDbContext.Products
                                        on Production.Fk_RawMaterialId equals product.ProductId
                                   where Production.Fk_FinishedGoodId == ProductId
                                   select new ProductionModel
                                   {
                                       Fk_RawMaterialId = Production.Fk_RawMaterialId,
                                       ProductName = product.ProductName,
                                       Quantity = Production.Quantity,
                                       Unit = Production.Unit
                                   }).ToListAsync();


                if (Query.Count > 0)
                {
                    var ItemTypeList = Query;
                    _Result.CollectionObjData = ItemTypeList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/GetProductionConfig : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LabourOrderModel>> GetProductionEntry()
        {
            Result<LabourOrderModel> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.LabourOrders.Where(s => s.Fk_LabourTypeId == MappingLabourType.Production && s.Fk_FinancialYearId == FinancialYear && s.FK_BranchId == BranchId).Select(s => new LabourOrderModel
                {
                    LabourOrderId = s.LabourOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    Product = s.Product != null ? new ProductModel { ProductName = s.Product.ProductName } : null,
                    Labour = s.Labour != null ? new LabourModel { LabourName = s.Labour.LabourName } : null,
                    LabourType = s.LabourType != null ? new LabourTypeModel { Labour_Type = s.LabourType.Labour_Type } : null,
                    Quantity = s.Quantity,
                    Rate = s.Rate,
                    Narration = s.Narration,
                    OTAmount = s.OTAmount,
                    Amount = s.Amount
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    var ProductEntryList = Query;
                    _Result.CollectionObjData = ProductEntryList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/GetProductionEntry : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateProductionEntry(ProductionEntryRequest data)
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
                    if (DateTime.TryParseExact(data.ProductionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedProductionDate))
                    {
                        foreach (var item in data.RowData)
                        {
                            #region Production Entry
                            var newProductionEntry = new LabourOrder
                            {
                                TransactionDate = convertedProductionDate,
                                TransactionNo = data.ProductionNo,
                                FK_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                Fk_ProductId = Guid.Parse(item[0]),
                                Fk_LabourId = data.Fk_LabourId,
                                Fk_LabourTypeId = MappingLabourType.Production,
                                Quantity = Convert.ToDecimal(item[1]),
                                Rate = Convert.ToDecimal(item[2]),
                                OTAmount = Convert.ToDecimal(item[3]),
                                Narration = item[4],
                                Amount = Convert.ToDecimal(item[5])
                            };
                            await _appDbContext.LabourOrders.AddAsync(newProductionEntry);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                            #region Stock
                            //Update Finishedgood stock
                            var UpdateFinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newProductionEntry.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (UpdateFinishedGoodStock != null)
                            {
                                UpdateFinishedGoodStock.AvilableStock += newProductionEntry.Quantity;
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var AddNewFinishedGoodStock = new Stock
                                {
                                    Fk_BranchId = BranchId,
                                    Fk_ProductId = newProductionEntry.Fk_ProductId,
                                    Fk_FinancialYear = FinancialYear,
                                    AvilableStock = newProductionEntry.Quantity
                                };
                                await _appDbContext.Stocks.AddAsync(AddNewFinishedGoodStock);
                                await _appDbContext.SaveChangesAsync();
                            }
                            //update Rawmterial Stock
                            var getFinishedGoodRawmaterial = await _appDbContext.Productions.Where(s => s.Fk_FinishedGoodId == newProductionEntry.Fk_ProductId).ToListAsync();
                            if (getFinishedGoodRawmaterial.Count > 0)
                            {
                                foreach (var item1 in getFinishedGoodRawmaterial)
                                {
                                    var UpdateRawMaterialStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item1.Fk_RawMaterialId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateRawMaterialStock != null)
                                    {
                                        UpdateRawMaterialStock.AvilableStock -= (newProductionEntry.Quantity * item1.Quantity);
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var AddNewRawMaterialStock = new Stock
                                        {
                                            Fk_BranchId = BranchId,
                                            Fk_ProductId = item1.Fk_RawMaterialId,
                                            Fk_FinancialYear = FinancialYear,
                                            AvilableStock = -(item1.Quantity)
                                        };
                                        await _appDbContext.Stocks.AddAsync(AddNewRawMaterialStock);
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    //******************************************Production Entry Transaction***************************************************//
                                    var newProductionEntryTransaction = new LabourTransaction()
                                    {
                                        Fk_LabourOdrId = newProductionEntry.LabourOrderId,
                                        TransactionNo = newProductionEntry.TransactionNo,
                                        TransactionDate = convertedProductionDate,
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        Fk_ProductId = item1.Fk_RawMaterialId,
                                        Quantity = (newProductionEntry.Quantity * item1.Quantity),
                                    };
                                    await _appDbContext.LabourTransactions.AddAsync(newProductionEntryTransaction);
                                    await _appDbContext.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                _Result.WarningMessage = "Finshed Good Not Configure For Deduct RawMaterial";
                                return _Result;
                            }

                            #endregion
                            #region SubLedgerBalance & Ledger Balance
                            // @Labour A/c------Cr
                            var LedgerBalanceId = Guid.Empty;
                            var getLabourSubLedger = await _appDbContext.Labours.Where(s => s.LabourId == newProductionEntry.Fk_LabourId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                            if (getLabourSubLedger != null)
                            {
                                var updateLaourLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateLaourLedgerBalance != null)
                                {
                                    updateLaourLedgerBalance.RunningBalance -= newProductionEntry.Amount;
                                    updateLaourLedgerBalance.RunningBalanceType = (updateLaourLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                    LedgerBalanceId = updateLaourLedgerBalance.LedgerBalanceId;
                                }
                                else
                                {
                                    var newLedgerBalance = new LedgerBalance
                                    {
                                        Fk_LedgerId = MappingLedgers.LabourAccount,
                                        OpeningBalance = 0,
                                        OpeningBalanceType = "Cr",
                                        RunningBalance = -newProductionEntry.Amount,
                                        RunningBalanceType = "Cr",
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYear = FinancialYear
                                    };
                                    await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                    await _appDbContext.SaveChangesAsync();
                                    LedgerBalanceId = newLedgerBalance.LedgerBalanceId;
                                }
                                var updateLaourSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == getLabourSubLedger.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (updateLaourSubledgerBalance != null)
                                {
                                    updateLaourSubledgerBalance.RunningBalance -= newProductionEntry.Amount;
                                    updateLaourSubledgerBalance.RunningBalanceType = (updateLaourSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    var getSubledgerId = await _appDbContext.Labours.Where(s => s.LabourId == data.Fk_LabourId && s.Fk_BranchId == BranchId).Select(s => s.Fk_SubLedgerId).SingleOrDefaultAsync();
                                    var newSubLedgerBalance = new SubLedgerBalance
                                    {
                                        Fk_LedgerBalanceId = LedgerBalanceId,
                                        Fk_SubLedgerId = getSubledgerId,
                                        OpeningBalanceType = "Cr",
                                        OpeningBalance = 0,
                                        RunningBalanceType = "Cr",
                                        RunningBalance = -newProductionEntry.Amount,
                                        Fk_FinancialYearId = FinancialYear,
                                        Fk_BranchId = BranchId
                                    };
                                    await _appDbContext.SubLedgerBalances.AddAsync(newSubLedgerBalance);
                                    await _appDbContext.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                _Result.WarningMessage = "Labour Not Found";
                                return _Result;
                            }
                            // @labourCharges A/c--------Dr
                            var updatelabourChargesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourCharges && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updatelabourChargesLedgerBalance != null)
                            {
                                updatelabourChargesLedgerBalance.RunningBalance += newProductionEntry.Amount;
                                updatelabourChargesLedgerBalance.RunningBalanceType = (updatelabourChargesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.LabourCharges,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Dr",
                                    RunningBalance = newProductionEntry.Amount,
                                    RunningBalanceType = "Dr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/CreateProductionEntry : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateProductionEntry(LabourOrderModel data)
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
                    if (DateTime.TryParseExact(data.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedProductionDate))
                    {
                        var UpdateProductionEntry = await (from s in _appDbContext.LabourOrders where s.LabourOrderId == data.LabourOrderId && s.FK_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear select s).SingleOrDefaultAsync();
                        if (UpdateProductionEntry != null)
                        {
                            var getFinishedGoodRawmaterial = await _appDbContext.Productions.Where(s => s.Fk_FinishedGoodId == data.Fk_ProductId).ToListAsync();
                            if (getFinishedGoodRawmaterial.Count > 0)
                            {
                                var UpdateFinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == data.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (data.Fk_ProductId == UpdateProductionEntry.Fk_ProductId)
                                {
                                    #region Update Stock
                                    //Update FinishedGood Stock
                                    if (UpdateFinishedGoodStock != null)
                                    {
                                        var quantityDifference = (UpdateProductionEntry.Quantity - data.Quantity);
                                        UpdateFinishedGoodStock.AvilableStock -= quantityDifference;
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var AddNewStock = new Stock
                                        {
                                            Fk_BranchId = BranchId,
                                            Fk_ProductId = data.Fk_ProductId,
                                            Fk_FinancialYear = FinancialYear,
                                            AvilableStock = data.Quantity
                                        };
                                        await _appDbContext.Stocks.AddAsync(AddNewStock);
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    //Update RawMaterial Stock
                                    foreach (var item in getFinishedGoodRawmaterial)
                                    {
                                        var UpdateRawMaterialStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item.Fk_RawMaterialId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateRawMaterialStock != null)
                                        {
                                            var quantityDifference = ((UpdateProductionEntry.Quantity - data.Quantity) * item.Quantity);
                                            UpdateRawMaterialStock.AvilableStock += quantityDifference;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        //******************************************Production Entry Transaction***************************************************//
                                        var getSingleProductionEntryTransaction = await _appDbContext.LabourTransactions.Where(s => s.Fk_LabourOdrId == UpdateProductionEntry.LabourOrderId && s.Fk_ProductId == item.Fk_RawMaterialId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                        if (getSingleProductionEntryTransaction != null)
                                        {
                                            var differencetrn = ((UpdateProductionEntry.Quantity - data.Quantity) * item.Quantity);
                                            getSingleProductionEntryTransaction.Quantity -= differencetrn;
                                            getSingleProductionEntryTransaction.TransactionDate = convertedProductionDate;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Revert Stock
                                    //Revert RawMaterial Stock
                                    var getOldProductFinishedGoodRawmaterial = await _appDbContext.Productions.Where(s => s.Fk_FinishedGoodId == UpdateProductionEntry.Fk_ProductId).ToListAsync();
                                    if (getOldProductFinishedGoodRawmaterial.Count > 0)
                                    {
                                        foreach (var item in getOldProductFinishedGoodRawmaterial)
                                        {
                                            var UpdateRawMaterialStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item.Fk_RawMaterialId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateRawMaterialStock != null)
                                            {
                                                UpdateRawMaterialStock.AvilableStock += (item.Quantity * UpdateProductionEntry.Quantity);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                        //Revert FinishedGood Stock
                                        var UpdateOldFinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == UpdateProductionEntry.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateOldFinishedGoodStock != null)
                                        {
                                            UpdateOldFinishedGoodStock.AvilableStock -= UpdateProductionEntry.Quantity;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Finshed Good Not Configure For Deduct RawMaterial";
                                        return _Result;
                                    }
                                    #endregion
                                    #region Delete Production Entry Transaction
                                    var deleteProductionEntryTransaction = await _appDbContext.LabourTransactions.Where(s => s.Fk_LabourOdrId == UpdateProductionEntry.LabourOrderId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                                    _appDbContext.RemoveRange(deleteProductionEntryTransaction);
                                    await _appDbContext.SaveChangesAsync();
                                    #endregion
                                    #region Update Stock
                                    //Update RawMaterial Stock
                                    foreach (var item in getFinishedGoodRawmaterial)
                                    {
                                        var UpdateRawMaterialStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item.Fk_RawMaterialId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateRawMaterialStock != null)
                                        {
                                            UpdateRawMaterialStock.AvilableStock -= (data.Quantity * item.Quantity);
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        //******************************************Production Entry Transaction***************************************************//
                                        var newProductionEntryTransaction = new LabourTransaction()
                                        {
                                            Fk_LabourOdrId = UpdateProductionEntry.LabourOrderId,
                                            TransactionNo = UpdateProductionEntry.TransactionNo,
                                            TransactionDate = convertedProductionDate,
                                            Fk_BranchId = BranchId,
                                            Fk_FinancialYearId = FinancialYear,
                                            Fk_ProductId = item.Fk_RawMaterialId,
                                            Quantity = (data.Quantity * item.Quantity),
                                        };
                                        await _appDbContext.LabourTransactions.AddAsync(newProductionEntryTransaction);
                                        await _appDbContext.SaveChangesAsync();
                                    }

                                    //Update FinishedGood Stock
                                    if (UpdateFinishedGoodStock != null)
                                    {
                                        UpdateFinishedGoodStock.AvilableStock += data.Quantity;
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var AddNewStock = new Stock
                                        {
                                            Fk_BranchId = BranchId,
                                            Fk_ProductId = data.Fk_ProductId,
                                            Fk_FinancialYear = FinancialYear,
                                            AvilableStock = data.Quantity
                                        };
                                        await _appDbContext.Stocks.AddAsync(AddNewStock);
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                _Result.WarningMessage = "Finshed Good Not Configure For Deduct RawMaterial";
                                return _Result;
                            }

                            #region Ledger & SubLedger Balance
                            var difference = data.Amount - UpdateProductionEntry.Amount;
                            // @Labour A/c------Cr
                            var getLabourSubLedger = await _appDbContext.Labours.Where(s => s.LabourId == UpdateProductionEntry.Fk_LabourId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                            if (getLabourSubLedger != null)
                            {
                                var updateSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == getLabourSubLedger.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (updateSubledgerBalance != null)
                                {
                                    updateSubledgerBalance.RunningBalance -= difference;
                                    updateSubledgerBalance.RunningBalanceType = (updateSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (updateLedgerBalance != null)
                                    {
                                        updateLedgerBalance.RunningBalance -= difference;
                                        updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "SubLedger Balance Not Exist";
                                        return _Result;
                                    }
                                }
                                else
                                {
                                    _Result.WarningMessage = "Ledger Balance Not Exist";
                                    return _Result;
                                }
                            }
                            else
                            {
                                _Result.WarningMessage = "Labour Not Exist";
                                return _Result;
                            }
                            // @labourCharges A/c--------Dr
                            var updatelabourChargesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourCharges && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updatelabourChargesLedgerBalance != null)
                            {
                                updatelabourChargesLedgerBalance.RunningBalance += difference;
                                updatelabourChargesLedgerBalance.RunningBalanceType = (updatelabourChargesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                _Result.WarningMessage = "Ledger Balance Not Exist";
                                return _Result;
                            }
                            #endregion
                            #region Update Production Entry
                            UpdateProductionEntry.TransactionDate = convertedProductionDate;
                            UpdateProductionEntry.Fk_ProductId = data.Fk_ProductId;
                            UpdateProductionEntry.Fk_LabourId = data.Fk_LabourId;
                            UpdateProductionEntry.Fk_LabourTypeId = MappingLabourType.Production;
                            UpdateProductionEntry.Quantity = data.Quantity;
                            UpdateProductionEntry.Rate = data.Rate;
                            UpdateProductionEntry.Narration = data.Narration;
                            UpdateProductionEntry.OTAmount = data.OTAmount;
                            UpdateProductionEntry.Amount = data.Amount;
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                        }
                    }
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                    _Result.IsSuccess = true;
                    transaction.Commit();
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/UpdateProductionEntry : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteProductionEntry(Guid Id, IDbContextTransaction transaction, bool IsCallback)
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
                        var DeleteProductionEntry = await (from s in _appDbContext.LabourOrders where s.LabourOrderId == Id && s.FK_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear select s).SingleOrDefaultAsync();
                        if (DeleteProductionEntry != null)
                        {
                            #region Production Entry Transaction 
                            var DeleteProductionEntryTransacion = await (from s in _appDbContext.LabourTransactions where s.Fk_LabourOdrId == Id && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear select s).ToListAsync();
                            if (DeleteProductionEntryTransacion != null)
                            {
                                foreach (var item in DeleteProductionEntryTransacion)
                                {
                                    #region Stock
                                    //Update RawMaterial Stocks (+)
                                    var UpdateRawMaterialStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateRawMaterialStock != null)
                                    {
                                        UpdateRawMaterialStock.AvilableStock += item.Quantity;
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    #endregion
                                }
                                _appDbContext.LabourTransactions.RemoveRange(DeleteProductionEntryTransacion);
                                await _appDbContext.SaveChangesAsync();
                            }
                            #endregion
                            #region Stock
                            //Update FinidhedGoog Stocks (-)
                            var UpdateFinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == DeleteProductionEntry.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (UpdateFinishedGoodStock != null)
                            {
                                UpdateFinishedGoodStock.AvilableStock -= DeleteProductionEntry.Quantity;
                                await _appDbContext.SaveChangesAsync();
                            }
                            #endregion
                            #region SubLedger & Ledger Balances
                            // @Labour A/c------Cr
                            var getLabourSubLedger = await _appDbContext.Labours.Where(s => s.LabourId == DeleteProductionEntry.Fk_LabourId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                            if (getLabourSubLedger != null)
                            {
                                var updateLabourSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == getLabourSubLedger.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (updateLabourSubledgerBalance != null)
                                {
                                    if (DeleteProductionEntry.Fk_LabourId == getLabourSubLedger.LabourId)
                                    {
                                        updateLabourSubledgerBalance.RunningBalance += DeleteProductionEntry.Amount;
                                        updateLabourSubledgerBalance.RunningBalanceType = (updateLabourSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        var updateLabourLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateLabourSubledgerBalance.Fk_LedgerBalanceId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (updateLabourLedgerBalance != null)
                                        {
                                            updateLabourLedgerBalance.RunningBalance += DeleteProductionEntry.Amount;
                                            updateLabourLedgerBalance.RunningBalanceType = (updateLabourLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        }
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                }
                            }
                            // @labourCharges A/c--------Dr
                            var updatelabourChargesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourCharges && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updatelabourChargesLedgerBalance != null)
                            {
                                updatelabourChargesLedgerBalance.RunningBalance -= DeleteProductionEntry.Amount;
                                updatelabourChargesLedgerBalance.RunningBalanceType = (updatelabourChargesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            #endregion
                            //Delete Production Entry
                            _appDbContext.LabourOrders.Remove(DeleteProductionEntry);
                            await _appDbContext.SaveChangesAsync();
                        }
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/DeleteProductionEntry : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Service Entry
        public async Task<Result<string>> GetLastServiceNo()
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var lastService = await _appDbContext.LabourOrders.Where(s => s.FK_BranchId == BranchId && s.Fk_LabourTypeId == MappingLabourType.Service && s.Fk_FinancialYearId == FinancialYear).OrderByDescending(s => s.TransactionNo).Select(s => new { s.TransactionNo }).FirstOrDefaultAsync();
                if (lastService != null)
                {
                    var lastServicnNo = lastService.TransactionNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastServicnNo.AsSpan(2), out int currentId))
                    {
                        currentId++;
                        var newServiceNo = $"SN{currentId:D6}";
                        _Result.SingleObjData = newServiceNo;
                    }
                }
                else
                {
                    _Result.SingleObjData = "SN000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/GetLastProductionNo : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LabourOrderModel>> GetServiceEntry()
        {
            Result<LabourOrderModel> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.LabourOrders.Where(s => s.Fk_FinancialYearId == FinancialYear && s.FK_BranchId == BranchId && s.Fk_LabourTypeId == MappingLabourType.Service).Select(s => new LabourOrderModel
                {
                    LabourOrderId = s.LabourOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    Product = s.Product != null ? new ProductModel { ProductName = s.Product.ProductName } : null,
                    Labour = s.Labour != null ? new LabourModel { LabourName = s.Labour.LabourName } : null,
                    LabourType = s.LabourType != null ? new LabourTypeModel { Labour_Type = s.LabourType.Labour_Type } : null,
                    Quantity = s.Quantity,
                    Rate = s.Rate,
                    OTAmount = s.OTAmount,
                    Amount = s.Amount,
                    Narration = s.Narration
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    var ProductEntryList = Query;
                    _Result.CollectionObjData = ProductEntryList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/GetProductionEntry : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateServiceEntry(ProductionEntryRequest data)
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
                    if (DateTime.TryParseExact(data.ProductionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedProductionDate))
                    {
                        foreach (var item in data.RowData)
                        {
                            #region Production Entry
                            var newProductionEntry = new LabourOrder
                            {
                                TransactionDate = convertedProductionDate,
                                TransactionNo = data.ProductionNo,
                                FK_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                Fk_ProductId = Guid.Parse(item[0]),
                                Fk_LabourId = data.Fk_LabourId,
                                Fk_LabourTypeId = MappingLabourType.Service,
                                Quantity = Convert.ToDecimal(item[1]),
                                Rate = Convert.ToDecimal(item[2]),
                                OTAmount = Convert.ToDecimal(item[3]),
                                Narration = item[4],
                                Amount = Convert.ToDecimal(item[5])
                            };
                            await _appDbContext.LabourOrders.AddAsync(newProductionEntry);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                            #region SubLedgerBalance & Ledger Balance
                            // @Labour A/c------Cr
                            var LedgerBalanceId = Guid.Empty;
                            var getLabourSubLedger = await _appDbContext.Labours.Where(s => s.LabourId == newProductionEntry.Fk_LabourId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                            if (getLabourSubLedger != null)
                            {
                                var updateLaourLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateLaourLedgerBalance != null)
                                {
                                    updateLaourLedgerBalance.RunningBalance -= newProductionEntry.Amount;
                                    updateLaourLedgerBalance.RunningBalanceType = (updateLaourLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                    LedgerBalanceId = updateLaourLedgerBalance.LedgerBalanceId;
                                }
                                else
                                {
                                    var newLedgerBalance = new LedgerBalance
                                    {
                                        Fk_LedgerId = MappingLedgers.LabourAccount,
                                        OpeningBalance = 0,
                                        OpeningBalanceType = "Cr",
                                        RunningBalance = -newProductionEntry.Amount,
                                        RunningBalanceType = "Cr",
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYear = FinancialYear
                                    };
                                    await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                    await _appDbContext.SaveChangesAsync();
                                    LedgerBalanceId = newLedgerBalance.LedgerBalanceId;
                                }
                                var updateLaourSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == getLabourSubLedger.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (updateLaourSubledgerBalance != null)
                                {
                                    updateLaourSubledgerBalance.RunningBalance -= newProductionEntry.Amount;
                                    updateLaourSubledgerBalance.RunningBalanceType = (updateLaourSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    var getSubledgerId = await _appDbContext.Labours.Where(s => s.LabourId == data.Fk_LabourId && s.Fk_BranchId == BranchId).Select(s => s.Fk_SubLedgerId).SingleOrDefaultAsync();
                                    var newSubLedgerBalance = new SubLedgerBalance
                                    {
                                        Fk_LedgerBalanceId = LedgerBalanceId,
                                        Fk_SubLedgerId = getSubledgerId,
                                        OpeningBalanceType = "Cr",
                                        OpeningBalance = 0,
                                        RunningBalanceType = "Cr",
                                        RunningBalance = -newProductionEntry.Amount,
                                        Fk_FinancialYearId = FinancialYear,
                                        Fk_BranchId = BranchId
                                    };
                                    await _appDbContext.SubLedgerBalances.AddAsync(newSubLedgerBalance);
                                    await _appDbContext.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                _Result.WarningMessage = "Labour Not Found";
                                return _Result;
                            }
                            // @labourCharges A/c--------Dr
                            var updatelabourChargesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourCharges && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updatelabourChargesLedgerBalance != null)
                            {
                                updatelabourChargesLedgerBalance.RunningBalance += newProductionEntry.Amount;
                                updatelabourChargesLedgerBalance.RunningBalanceType = (updatelabourChargesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.LabourCharges,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Dr",
                                    RunningBalance = newProductionEntry.Amount,
                                    RunningBalanceType = "Dr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/CreateProductionEntry : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateServiceEntry(LabourOrderModel data)
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
                    if (DateTime.TryParseExact(data.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedProductionDate))
                    {
                        var UpdateProductionEntry = await (from s in _appDbContext.LabourOrders where s.LabourOrderId == data.LabourOrderId && s.FK_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear select s).SingleOrDefaultAsync();
                        if (UpdateProductionEntry != null)
                        {
                            #region Ledger & SubLedger Balance
                            var difference = data.Amount - UpdateProductionEntry.Amount;
                            // @Labour A/c------Cr
                            var getLabourSubLedger = await _appDbContext.Labours.Where(s => s.LabourId == UpdateProductionEntry.Fk_LabourId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                            if (getLabourSubLedger != null)
                            {
                                var updateSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == getLabourSubLedger.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (updateSubledgerBalance != null)
                                {
                                    updateSubledgerBalance.RunningBalance -= difference;
                                    updateSubledgerBalance.RunningBalanceType = (updateSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (updateLedgerBalance != null)
                                    {
                                        updateLedgerBalance.RunningBalance -= difference;
                                        updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "SubLedger Balance Not Exist";
                                        return _Result;
                                    }
                                    await _appDbContext.SaveChangesAsync();

                                }
                                else
                                {
                                    _Result.WarningMessage = "Ledger Balance Not Exist";
                                    return _Result;
                                }
                            }
                            else
                            {
                                _Result.WarningMessage = "Labour Not Exist";
                                return _Result;
                            }
                            // @labourCharges A/c--------Dr
                            var updatelabourChargesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourCharges && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updatelabourChargesLedgerBalance != null)
                            {
                                updatelabourChargesLedgerBalance.RunningBalance += difference;
                                updatelabourChargesLedgerBalance.RunningBalanceType = (updatelabourChargesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                _Result.WarningMessage = "Ledger Balance Not Exist";
                                return _Result;
                            }
                            #endregion
                            #region Update Production Entry
                            UpdateProductionEntry.TransactionDate = convertedProductionDate;
                            UpdateProductionEntry.Fk_ProductId = data.Fk_ProductId;
                            UpdateProductionEntry.Fk_LabourId = data.Fk_LabourId;
                            UpdateProductionEntry.Fk_LabourTypeId = MappingLabourType.Service;
                            UpdateProductionEntry.Quantity = data.Quantity;
                            UpdateProductionEntry.Rate = data.Rate;
                            UpdateProductionEntry.Amount = data.Amount;
                            UpdateProductionEntry.OTAmount = data.OTAmount;
                            UpdateProductionEntry.Narration = data.Narration;
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                        }
                    }
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                    _Result.IsSuccess = true;
                    transaction.Commit();
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/UpdateProductionEntry : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteServiceEntry(Guid Id, IDbContextTransaction transaction, bool IsCallback)
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
                        var DeleteProductionEntry = await (from s in _appDbContext.LabourOrders where s.LabourOrderId == Id && s.FK_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear select s).SingleOrDefaultAsync();
                        if (DeleteProductionEntry != null)
                        {
                            #region Production Entry Transaction 
                            var DeleteProductionEntryTransacion = await (from s in _appDbContext.LabourTransactions where s.Fk_LabourOdrId == Id && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear select s).ToListAsync();
                            if (DeleteProductionEntryTransacion != null)
                            {
                                _appDbContext.LabourTransactions.RemoveRange(DeleteProductionEntryTransacion);
                                await _appDbContext.SaveChangesAsync();
                            }
                            #endregion                         
                            #region SubLedger & Ledger Balances
                            // @Labour A/c------Cr
                            var getLabourSubLedger = await _appDbContext.Labours.Where(s => s.LabourId == DeleteProductionEntry.Fk_LabourId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                            if (getLabourSubLedger != null)
                            {
                                var updateLabourSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == getLabourSubLedger.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (updateLabourSubledgerBalance != null)
                                {
                                    if (DeleteProductionEntry.Fk_LabourId == getLabourSubLedger.LabourId)
                                    {
                                        updateLabourSubledgerBalance.RunningBalance += DeleteProductionEntry.Amount;
                                        updateLabourSubledgerBalance.RunningBalanceType = (updateLabourSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        var updateLabourLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateLabourSubledgerBalance.Fk_LedgerBalanceId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (updateLabourLedgerBalance != null)
                                        {
                                            updateLabourLedgerBalance.RunningBalance += DeleteProductionEntry.Amount;
                                            updateLabourLedgerBalance.RunningBalanceType = (updateLabourLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        }
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                }
                            }
                            // @labourCharges A/c--------Dr
                            var updatelabourChargesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourCharges && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updatelabourChargesLedgerBalance != null)
                            {
                                updatelabourChargesLedgerBalance.RunningBalance -= DeleteProductionEntry.Amount;
                                updatelabourChargesLedgerBalance.RunningBalanceType = (updatelabourChargesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            #endregion
                            //Delete Production Entry
                            _appDbContext.LabourOrders.Remove(DeleteProductionEntry);
                            await _appDbContext.SaveChangesAsync();
                        }
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/DeleteProductionEntry : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Sales Transaction
        public async Task<Result<SubLedgerModel>> GetSundryDebtors(Guid PartyTypeId)
        {
            Result<SubLedgerModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                _Result.CollectionObjData = await (from s in _appDbContext.SubLedgers
                                                   where s.Fk_LedgerId == PartyTypeId
                                                   select new SubLedgerModel
                                                   {
                                                       SubLedgerId = s.SubLedgerId,
                                                       SubLedgerName = s.SubLedgerName,
                                                   }).ToListAsync();
                if (_Result.CollectionObjData.Count > 0)
                {
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    _Result.IsSuccess = true;
                }
                else
                {
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetSundryDebtors : {_Exception.Message}");
            }
            return _Result;
        }
        #region Sales
        public async Task<Result<string>> GetLastSalesTransaction()
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var lastSaleseOrder = await _appDbContext.SalesOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).OrderByDescending(s => s.TransactionNo).Select(s => new { s.TransactionNo }).FirstOrDefaultAsync();
                if (lastSaleseOrder != null)
                {
                    var lastTransactionId = lastSaleseOrder.TransactionNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastTransactionId.AsSpan(2), out int currentId))
                    {
                        currentId++;
                        var newTransactionId = $"SI{currentId:D6}";
                        _Result.SingleObjData = newTransactionId;
                    }
                }
                else
                {
                    _Result.SingleObjData = "SI000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetLastSalesTransaction : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<SalesOrderModel>> GetSales()
        {
            Result<SalesOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.SalesOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new SalesOrderModel
                {
                    SalesOrderId = s.SalesOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    OrderNo = s.OrderNo,
                    GrandTotal = s.GrandTotal,
                    SubLedger = s.SubLedger != null ? new SubLedgerModel { SubLedgerName = s.SubLedger.SubLedgerName } : null,
                    CustomerName = s.CustomerName,
                    Naration = s.Narration,
                    TransactionType = s.TransactionType
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    var OrderList = Query;
                    _Result.CollectionObjData = OrderList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetSales : {_Exception.Message}");
            }
            return _Result;
        }
        //public async Task<Result<SalesOrderModel>> GetSalesById(Guid Id)
        //{
        //    Result<SalesOrderModel> _Result = new();
        //    try
        //    {
        //        _Result.IsSuccess = false;
        //        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
        //        Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
        //        var Query = await _appDbContext.SalesOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.SalesOrderId == Id).Select(s => new SalesOrderModel
        //        {
        //            SalesOrderId = s.SalesOrderId,
        //            Fk_SubLedgerId = s.Fk_SubLedgerId,
        //            CustomerName = s.CustomerName,
        //            TransactionNo = s.TransactionNo,
        //            TransactionType = s.TransactionType,
        //            TransactionDate = s.TransactionDate,
        //            OrderNo = s.OrderNo,
        //            OrderDate = s.OrderDate,
        //            TranspoterName = s.TranspoterName,
        //            VehicleNo = s.VehicleNo,
        //            ReceivingPerson = s.ReceivingPerson,
        //            Naration = s.Narration,
        //            Discount = s.Discount,
        //            SubTotal = s.SubTotal,
        //            Gst = s.Gst,
        //            GrandTotal = s.GrandTotal,
        //           // _appDbContext.SalesTransaction.Where()
        //            SalesTransactions = _appDbContext.SalesTransaction.Where(x => x.Fk_SalesOrderId == s.SalesOrderId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(x => new SalesTransactionModel
        //            {
        //                SalesId = x.SalesId,
        //                AlternateQuantity = x.AlternateQuantity,
        //                UnitQuantity = x.UnitQuantity,
        //                UnitName = x.Product.Unit.UnitName,
        //                Fk_AlternateUnitId = x.Fk_AlternateUnitId,
        //                AlternateUnit = x.AlternateUnit != null ? new AlternateUnitModel { AlternateUnitName = x.AlternateUnit.AlternateUnitName } : null,
        //                Rate = x.Rate,
        //                Discount = x.Discount,
        //                DiscountAmount = x.DiscountAmount,
        //                Gst = x.Gst,
        //                GstAmount = x.GstAmount,
        //                Amount = x.Amount,
        //                Fk_ProductId = x.Fk_ProductId,
        //                Product = x.Product != null ? new ProductModel { ProductName = x.Product.ProductName, Unit = x.Product.Unit != null ? new UnitModel { UnitName = x.Product.Unit.UnitName } : null } : null,
        //            }).ToList(),
        //        }).FirstOrDefaultAsync();
        //        if (Query != null)
        //        {
        //            var Order = Query;
        //            _Result.SingleObjData = Order;
        //            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
        //        }
        //        _Result.IsSuccess = true;
        //    }
        //    catch (Exception _Exception)
        //    {
        //        _Result.Exception = _Exception;
        //        await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetSalesById : {_Exception.Message}");
        //    }
        //    return _Result;
        //}
        public async Task<Result<SalesOrderModel>> GetSalesById(Guid Id)
        {
            Result<SalesOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));

                var Query = await _appDbContext.SalesOrders
                    .Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.SalesOrderId == Id)
                    .Select(s => new SalesOrderModel
                    {
                        SalesOrderId = s.SalesOrderId,
                        Fk_SubLedgerId = s.Fk_SubLedgerId,
                        CustomerName = s.CustomerName,
                        TransactionNo = s.TransactionNo,
                        TransactionType = s.TransactionType,
                        TransactionDate = s.TransactionDate,
                        OrderNo = s.OrderNo,
                        OrderDate = s.OrderDate,
                        TranspoterName = s.TranspoterName,
                        VehicleNo = s.VehicleNo,
                        ReceivingPerson = s.ReceivingPerson,
                        Naration = s.Narration,
                        Discount = s.Discount,
                        SubTotal = s.SubTotal,
                        Gst = s.Gst,
                        GrandTotal = s.GrandTotal,
                        SalesTransactions = _appDbContext.SalesTransaction
    .Where(x => x.Fk_SalesOrderId == s.SalesOrderId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && (x.Fk_SubProductId == null || x.SalesId == _appDbContext.SalesTransaction.Where(y => y.Fk_SubProductId == x.Fk_SubProductId).Min(y => y.SalesId)))
    .Select(x => new SalesTransactionModel
    {
        SalesId = x.SalesId,
        AlternateQuantity = x.AlternateQuantity,
        UnitQuantity = x.UnitQuantity,
        UnitName = x.Fk_SubProductId == null ? _appDbContext.Products
                    .Where(p => p.ProductId == x.Fk_ProductId)
                    .Select(p => p.Unit.UnitName)
                    .FirstOrDefault() : _appDbContext.Products
                    .Where(p => p.ProductId == x.Fk_SubProductId)
                    .Select(p => p.Unit.UnitName)
                    .FirstOrDefault(),
        Fk_AlternateUnitId = x.Fk_AlternateUnitId,
        AlternateUnit = x.AlternateUnit != null ? new AlternateUnitModel { AlternateUnitName = x.AlternateUnit.AlternateUnitName } : null,
        Rate = x.Rate,
        Discount = x.Discount,
        DiscountAmount = x.DiscountAmount,
        Gst = x.Gst,
        GstAmount = x.GstAmount,
        Amount = x.Amount,
        Fk_ProductId = x.Fk_SubProductId ?? x.Fk_ProductId,
        Product = x.Fk_SubProductId == null ? _appDbContext.Products
                    .Where(p => p.ProductId == x.Fk_ProductId)
                    .Select(p => new ProductModel { ProductName = p.ProductName, Unit = new UnitModel { UnitName = p.Unit.UnitName } })
                    .FirstOrDefault() : _appDbContext.Products
                    .Where(p => p.ProductId == x.Fk_SubProductId)
                    .Select(p => new ProductModel { ProductName = p.ProductName, Unit = new UnitModel { UnitName = p.Unit.UnitName } })
                    .FirstOrDefault()
    })
    .ToList(),

                    })
                    .FirstOrDefaultAsync();

                if (Query != null)
                {
                    var Order = Query;
                    _Result.SingleObjData = Order;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetSalesById : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateSales(SalesDataRequest data)
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
                    if (DateTime.TryParseExact(data.OrderDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedOrderDate) && DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTransactionDate))
                    {
                        //************************************************************Sales Order*****************************************//
                        #region SalesOrder
                        var newSalesOrder = new SalesOrder
                        {
                            Fk_SubLedgerId = data.Fk_SubLedgerId,
                            CustomerName = data.CustomerName,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear,
                            TransactionDate = convertedTransactionDate,
                            TransactionNo = data.TransactionNo,
                            TransactionType = data.TransactionType,
                            PriceType = data.RateType,
                            OrderNo = data.OrderNo,
                            OrderDate = convertedOrderDate,
                            TranspoterName = data.TranspoterName,
                            VehicleNo = data.VehicleNo,
                            ReceivingPerson = data.ReceivingPerson,
                            SubTotal = data.SubTotal,
                            Gst = data.Gst,
                            Discount = data.Discount,
                            GrandTotal = data.GrandTotal,
                            Narration = data.Naration
                        };
                        await _appDbContext.SalesOrders.AddAsync(newSalesOrder);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        string Journalvoucher = "";
                        string ReciptVoucher = "";
                        if (data.TransactionType == "credit")
                        {
                            #region Ledger & SubLedger
                            /*********************************Update Ledger & SubLedger Balance********************************/
                            // @SundryDebitors A/c -----Dr
                            var LedgerBalanceId = Guid.Empty;
                            var updateSundryDebitorsLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SundryDebtors && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updateSundryDebitorsLedgerBalance != null)
                            {
                                updateSundryDebitorsLedgerBalance.RunningBalance += newSalesOrder.GrandTotal;
                                updateSundryDebitorsLedgerBalance.RunningBalanceType = (updateSundryDebitorsLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                                LedgerBalanceId = updateSundryDebitorsLedgerBalance.LedgerBalanceId;
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.PurchaseAccount,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Dr",
                                    RunningBalance = newSalesOrder.GrandTotal,
                                    RunningBalanceType = "Dr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                                LedgerBalanceId = newLedgerBalance.LedgerBalanceId;
                            }
                            var updateSundryDebitorsSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == newSalesOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                            if (updateSundryDebitorsSubledgerBalance != null)
                            {
                                updateSundryDebitorsSubledgerBalance.RunningBalance += newSalesOrder.GrandTotal;
                                updateSundryDebitorsSubledgerBalance.RunningBalanceType = (updateSundryDebitorsSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newSubLedgerBalance = new SubLedgerBalance
                                {
                                    Fk_LedgerBalanceId = LedgerBalanceId,
                                    Fk_SubLedgerId = data.Fk_SubLedgerId ?? Guid.Empty,
                                    OpeningBalanceType = "Dr",
                                    OpeningBalance = 0,
                                    RunningBalanceType = "Dr",
                                    RunningBalance = data.GrandTotal,
                                    Fk_FinancialYearId = FinancialYear,
                                    Fk_BranchId = BranchId
                                };
                                await _appDbContext.SubLedgerBalances.AddAsync(newSubLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                            }
                            // @Sales A/c ------Cr
                            var updateSalesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updateSalesLedgerBalance != null)
                            {
                                updateSalesLedgerBalance.RunningBalance -= newSalesOrder.GrandTotal;
                                updateSalesLedgerBalance.RunningBalanceType = (updateSalesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.SalesAccount,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Cr",
                                    RunningBalance = -newSalesOrder.GrandTotal,
                                    RunningBalanceType = "Cr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                            }
                            #endregion
                            #region Journal 
                            var JournalVoucherNo = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                            if (JournalVoucherNo != null)
                            {
                                if (int.TryParse(JournalVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                {
                                    currentId++;
                                    Journalvoucher = $"JN{currentId:D6}";
                                }
                            }
                            else
                            {
                                Journalvoucher = "JN000001";
                            }
                            var NewJournalSalesOrder = new Journal
                            {
                                VouvherNo = Journalvoucher,
                                VoucherDate = newSalesOrder.OrderDate,
                                Fk_LedgerGroupId = MappingLedgerGroup.CurrentAssets,
                                Fk_LedgerId = MappingLedgers.SundryDebtors,
                                Fk_SubLedgerId = newSalesOrder.Fk_SubLedgerId,
                                Fk_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                TransactionNo = newSalesOrder.TransactionNo,
                                TransactionId = newSalesOrder.SalesOrderId,
                                Narration = newSalesOrder.TransactionNo.ToString(),
                                Amount = newSalesOrder.GrandTotal,
                                DrCr = "Dr"
                            };
                            await _appDbContext.Journals.AddAsync(NewJournalSalesOrder);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                        }
                        else
                        {
                            #region Ledger & SubLedger
                            /***************************************Update Ledger Balance************************************/
                            // @cash A/c -----Dr
                            var updateCashLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updateCashLedgerBalance != null)
                            {
                                updateCashLedgerBalance.RunningBalance += newSalesOrder.GrandTotal;
                                updateCashLedgerBalance.RunningBalanceType = (updateCashLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.CashAccount,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Dr",
                                    RunningBalance = newSalesOrder.GrandTotal,
                                    RunningBalanceType = "Dr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                            }
                            //@sales A/c -----Cr
                            var updateSalesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updateSalesLedgerBalance != null)
                            {
                                updateSalesLedgerBalance.RunningBalance -= newSalesOrder.GrandTotal;
                                updateSalesLedgerBalance.RunningBalanceType = (updateSalesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.SalesAccount,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Cr",
                                    RunningBalance = -newSalesOrder.GrandTotal,
                                    RunningBalanceType = "Cr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                            }
                            #endregion
                            #region  Recipt 
                            var ReciptVoucherNo = await _appDbContext.Receipts.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                            if (ReciptVoucherNo != null)
                            {
                                if (int.TryParse(ReciptVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                {
                                    currentId++;
                                    ReciptVoucher = $"Cr{currentId:D6}";
                                }
                            }
                            else
                            {
                                ReciptVoucher = "CR000001";
                            }
                            var newRecipt = new Receipt
                            {
                                VoucherDate = newSalesOrder.TransactionDate,
                                VouvherNo = ReciptVoucher,
                                CashBank = data.TransactionType,
                                Fk_LedgerGroupId = MappingLedgerGroup.Sales,
                                Fk_LedgerId = MappingLedgers.SalesAccount,
                                Fk_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                TransactionNo = newSalesOrder.TransactionNo,
                                Amount = newSalesOrder.GrandTotal,
                                DrCr = "Cr",
                            };
                            await _appDbContext.Receipts.AddAsync(newRecipt);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                        }
                        //*********************************Sales Transaction**********************************************//
                        foreach (var item in data.RowData)
                        {
                            var getFinishedGoodSalesConfig1 = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                            if (getFinishedGoodSalesConfig1.Count > 0)
                            {
                                var newSalesTransaction1 = new SalesTransaction();
                                foreach (var item1 in getFinishedGoodSalesConfig1)
                                {
                                    newSalesTransaction1 = new SalesTransaction
                                    {
                                        Fk_SalesOrderId = newSalesOrder.SalesOrderId,
                                        TransactionNo = newSalesOrder.TransactionNo,
                                        TransactionDate = newSalesOrder.TransactionDate,
                                        TransactionType = data.TransactionType,
                                        Fk_SubProductId = Guid.Parse(item[1]),
                                        Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        AlternateQuantity = Convert.ToDecimal(item[2]),
                                        UnitQuantity = item1.Quantity * Convert.ToDecimal(item[2]),
                                        Rate = Convert.ToDecimal(item[5]),
                                        Discount = Convert.ToDecimal(item[6]),
                                        DiscountAmount = Convert.ToDecimal(item[7]),
                                        Gst = Convert.ToDecimal(item[8]),
                                        GstAmount = Convert.ToDecimal(item[9]),
                                        Amount = Convert.ToDecimal(item[10])
                                    };
                                    if (!string.IsNullOrEmpty(item[3]))
                                    {
                                        newSalesTransaction1.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                    }
                                    else
                                    {
                                        newSalesTransaction1.Fk_AlternateUnitId = null;
                                    }
                                    await _appDbContext.SalesTransaction.AddAsync(newSalesTransaction1);
                                    await _appDbContext.SaveChangesAsync();

                                }
                                #region Journal
                                if (data.TransactionType == "credit")
                                {
                                    var NewJournalSalesTransaction = new Journal
                                    {
                                        VouvherNo = Journalvoucher,
                                        VoucherDate = newSalesOrder.OrderDate,
                                        Fk_LedgerGroupId = MappingLedgerGroup.Sales,
                                        Fk_LedgerId = MappingLedgers.SalesAccount,
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        TransactionNo = newSalesOrder.TransactionNo,
                                        TransactionId = newSalesTransaction1.SalesId,
                                        Narration = newSalesOrder.TransactionNo.ToString(),
                                        Amount = newSalesTransaction1.Amount,
                                        DrCr = "Cr"
                                    };
                                    await _appDbContext.Journals.AddAsync(NewJournalSalesTransaction);
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                                #region Update Stock
                                //Start testing
                                var getFinishedGoodSalesConfig2 = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                if (getFinishedGoodSalesConfig2.Count > 0)
                                {
                                    foreach (var item2 in getFinishedGoodSalesConfig2)
                                    {
                                        var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item2.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateFhinishedGoodStock != null)
                                        {
                                            var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item2.Fk_SubFinishedGoodId).FirstOrDefault();
                                            UpdateFhinishedGoodStock.AvilableStock -= Unit != null ? (Convert.ToDecimal(item[2]) * Unit.UnitQuantity * item2.Quantity) : (Convert.ToDecimal(item[2]) * item2.Quantity);
                                            await _appDbContext.SaveChangesAsync();

                                        }
                                        else
                                        {
                                            var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item2.Fk_SubFinishedGoodId).FirstOrDefault();
                                            if (Unit != null)
                                            {
                                                var AddNewFhinishedGoodStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = item2.Fk_SubFinishedGoodId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = -(Convert.ToDecimal(item[2]) * Unit.UnitQuantity * item2.Quantity)
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                var AddNewFhinishedGoodStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = item2.Fk_SubFinishedGoodId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = -(Convert.ToDecimal(item[2]) * item2.Quantity)
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == Guid.Parse(item[1]) && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();

                                    if (UpdateStock != null)
                                    {
                                        UpdateStock.AvilableStock -= Convert.ToDecimal(item[4]);
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var AddNewStock = new Stock
                                        {
                                            Fk_BranchId = BranchId,
                                            Fk_ProductId = Guid.Parse(item[1]),
                                            Fk_FinancialYear = FinancialYear,
                                            AvilableStock = -Convert.ToDecimal(item[4])
                                        };
                                        await _appDbContext.Stocks.AddAsync(AddNewStock);
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                }
                                //end
                                #endregion
                            }
                            else
                            {
                                #region SalesTransaction  

                                var newSalesTransaction = new SalesTransaction
                                {
                                    Fk_SalesOrderId = newSalesOrder.SalesOrderId,
                                    TransactionNo = newSalesOrder.TransactionNo,
                                    TransactionDate = newSalesOrder.TransactionDate,
                                    TransactionType = data.TransactionType,
                                    Fk_ProductId = Guid.Parse(item[1]),
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYearId = FinancialYear,
                                    AlternateQuantity = Convert.ToDecimal(item[2]),
                                    //Fk_AlternateUnitId = Guid.Parse(item[3]),
                                    UnitQuantity = Convert.ToDecimal(item[4]),
                                    Rate = Convert.ToDecimal(item[5]),
                                    Discount = Convert.ToDecimal(item[6]),
                                    DiscountAmount = Convert.ToDecimal(item[7]),
                                    Gst = Convert.ToDecimal(item[8]),
                                    GstAmount = Convert.ToDecimal(item[9]),
                                    Amount = Convert.ToDecimal(item[10])
                                };
                                if (!string.IsNullOrEmpty(item[3]))
                                {
                                    newSalesTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                }
                                else
                                {
                                    newSalesTransaction.Fk_AlternateUnitId = null;
                                }
                                await _appDbContext.SalesTransaction.AddAsync(newSalesTransaction);
                                await _appDbContext.SaveChangesAsync();
                                #endregion
                                #region Journal
                                if (data.TransactionType == "credit")
                                {
                                    var NewJournalSalesTransaction = new Journal
                                    {
                                        VouvherNo = Journalvoucher,
                                        VoucherDate = newSalesOrder.OrderDate,
                                        Fk_LedgerGroupId = MappingLedgerGroup.Sales,
                                        Fk_LedgerId = MappingLedgers.SalesAccount,
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        TransactionNo = newSalesOrder.TransactionNo,
                                        TransactionId = newSalesTransaction.SalesId,
                                        Narration = newSalesOrder.TransactionNo.ToString(),
                                        Amount = newSalesTransaction.Amount,
                                        DrCr = "Cr"
                                    };
                                    await _appDbContext.Journals.AddAsync(NewJournalSalesTransaction);
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                                #region Update Stock
                                //Start testing
                                var getFinishedGoodSalesConfig = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                if (getFinishedGoodSalesConfig.Count > 0)
                                {
                                    foreach (var item1 in getFinishedGoodSalesConfig)
                                    {
                                        var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item1.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateFhinishedGoodStock != null)
                                        {
                                            var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                            UpdateFhinishedGoodStock.AvilableStock -= Unit != null ? (Convert.ToDecimal(item[2]) * Unit.UnitQuantity * item1.Quantity) : (Convert.ToDecimal(item[2]) * item1.Quantity);
                                            await _appDbContext.SaveChangesAsync();

                                        }
                                        else
                                        {
                                            var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                            if (Unit != null)
                                            {
                                                var AddNewFhinishedGoodStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = -(Convert.ToDecimal(item[2]) * Unit.UnitQuantity * item1.Quantity)
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                var AddNewFhinishedGoodStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = -(Convert.ToDecimal(item[2]) * item1.Quantity)
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == Guid.Parse(item[1]) && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();

                                    if (UpdateStock != null)
                                    {
                                        UpdateStock.AvilableStock -= Convert.ToDecimal(item[4]);
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var AddNewStock = new Stock
                                        {
                                            Fk_BranchId = BranchId,
                                            Fk_ProductId = Guid.Parse(item[1]),
                                            Fk_FinancialYear = FinancialYear,
                                            AvilableStock = -Convert.ToDecimal(item[4])
                                        };
                                        await _appDbContext.Stocks.AddAsync(AddNewStock);
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                }
                                //end
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/CreateSales : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateSales(SalesDataRequest data)
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
                    if (DateTime.TryParseExact(data.OrderDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedOrderDate) && DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTransactionDate))
                    {
                        var UpdateSalesOrder = await _appDbContext.SalesOrders.Where(s => s.SalesOrderId == data.SalesOrderId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                        if (UpdateSalesOrder != null)
                        {
                            string Journalvoucher = "";
                            string ReciptVoucher = "";
                            if (UpdateSalesOrder.TransactionType == data.TransactionType)
                            {
                                var difference = data.GrandTotal - UpdateSalesOrder.GrandTotal;
                                if (data.TransactionType == "credit")
                                {
                                    #region Update Leddger & SubLedger Balance
                                    if (UpdateSalesOrder.GrandTotal != data.GrandTotal)
                                    {
                                        // @SundryDebitors A/c ----- Dr
                                        var updateSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == UpdateSalesOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                        if (updateSubledgerBalance != null)
                                        {
                                            updateSubledgerBalance.RunningBalance += difference;
                                            updateSubledgerBalance.RunningBalanceType = (updateSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SundryDebtors && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (updateLedgerBalance != null)
                                            {
                                                updateLedgerBalance.RunningBalance += difference;
                                                updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            }
                                            else
                                            {
                                                _Result.WarningMessage = "Ledger Balance Not Exist";
                                                return _Result;
                                            }
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "SubLedger Balance Not Exist";
                                            return _Result;
                                        }
                                        // @Sales A/c -------- Cr
                                        var updateSalesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (updateSalesLedgerBalance != null)
                                        {
                                            updateSalesLedgerBalance.RunningBalance -= difference;
                                            updateSalesLedgerBalance.RunningBalanceType = (updateSalesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                    }
                                    #endregion
                                    #region Journal Table
                                    var UpdateOrderJournal = await _appDbContext.Journals.Where(s => s.TransactionId == UpdateSalesOrder.SalesOrderId && s.Fk_SubLedgerId == UpdateSalesOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateOrderJournal != null)
                                    {
                                        UpdateOrderJournal.Amount = data.GrandTotal;
                                        UpdateOrderJournal.VoucherDate = convertedTransactionDate;
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Update Leddger & SubLedger Balance
                                    if (UpdateSalesOrder.GrandTotal != data.GrandTotal)
                                    {
                                        // @cash A/c -----Dr
                                        var updateCashLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (updateCashLedgerBalance != null)
                                        {
                                            updateCashLedgerBalance.RunningBalance += difference;
                                            updateCashLedgerBalance.RunningBalanceType = (updateCashLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                        //@sales A/c -----Cr
                                        var updateSalesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (updateSalesLedgerBalance != null)
                                        {
                                            updateSalesLedgerBalance.RunningBalance -= difference;
                                            updateSalesLedgerBalance.RunningBalanceType = (updateSalesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                    }
                                    #endregion
                                    #region Update Recipt
                                    var UpdateOrderRecipt = await _appDbContext.Receipts.Where(s => s.TransactionNo == UpdateSalesOrder.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateOrderRecipt != null)
                                    {
                                        UpdateOrderRecipt.Amount = data.GrandTotal;
                                        UpdateOrderRecipt.VoucherDate = convertedTransactionDate;
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                if (data.TransactionType == "cash")
                                {
                                    #region Delete Journal Entries
                                    var DeleteJournal = await _appDbContext.Journals.Where(s => s.TransactionNo == UpdateSalesOrder.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                                    if (DeleteJournal.Count > 0)
                                    {
                                        _appDbContext.Journals.RemoveRange(DeleteJournal);
                                        await _appDbContext.SaveChangesAsync();
                                    }

                                    #endregion
                                    #region Revert LedgerBalance & Subledger Balance
                                    // @SundryDebitors A/c ----- Dr
                                    var RevertSundryDebitorsSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == UpdateSalesOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (RevertSundryDebitorsSubledgerBalance != null)
                                    {
                                        RevertSundryDebitorsSubledgerBalance.RunningBalance -= UpdateSalesOrder.GrandTotal;
                                        RevertSundryDebitorsSubledgerBalance.RunningBalanceType = (RevertSundryDebitorsSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        var RevertSundryDebitorsLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SundryDebtors && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (RevertSundryDebitorsLedgerBalance != null)
                                        {
                                            RevertSundryDebitorsLedgerBalance.RunningBalance -= UpdateSalesOrder.GrandTotal;
                                            RevertSundryDebitorsLedgerBalance.RunningBalanceType = (RevertSundryDebitorsLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "SubLedger Balance Not Exist";
                                        return _Result;
                                    }
                                    // @Sales A/c -------- Cr
                                    var RevertSalesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (RevertSalesLedgerBalance != null)
                                    {
                                        RevertSalesLedgerBalance.RunningBalance += UpdateSalesOrder.GrandTotal;
                                        RevertSalesLedgerBalance.RunningBalanceType = (RevertSalesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    #endregion
                                    #region Update LedgerBalance & Subledger Balance
                                    // @cash A/c -----Dr
                                    var updateCashLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (updateCashLedgerBalance != null)
                                    {
                                        updateCashLedgerBalance.RunningBalance += data.GrandTotal;
                                        updateCashLedgerBalance.RunningBalanceType = (updateCashLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    //@sales A/c -----Cr
                                    var updateSalesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSalesLedgerBalance != null)
                                    {
                                        updateSalesLedgerBalance.RunningBalance -= data.GrandTotal;
                                        updateSalesLedgerBalance.RunningBalanceType = (updateSalesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    #endregion
                                    #region  Insert Into Recipt           
                                    var ReciptVoucherNo = await _appDbContext.Receipts.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                                    if (ReciptVoucherNo != null)
                                    {
                                        if (int.TryParse(ReciptVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                        {
                                            currentId++;
                                            ReciptVoucher = $"Cr{currentId:D6}";
                                        }
                                    }
                                    else
                                    {
                                        ReciptVoucher = "CR000001";
                                    }
                                    var newRecipt = new Receipt
                                    {
                                        VoucherDate = convertedOrderDate,
                                        VouvherNo = ReciptVoucher,
                                        CashBank = data.TransactionType,
                                        Fk_LedgerGroupId = MappingLedgerGroup.Sales,
                                        Fk_LedgerId = MappingLedgers.SalesAccount,
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        TransactionNo = data.TransactionNo,
                                        Amount = data.GrandTotal,
                                        DrCr = "Cr",
                                    };
                                    await _appDbContext.Receipts.AddAsync(newRecipt);
                                    await _appDbContext.SaveChangesAsync();
                                    #endregion
                                }
                                else
                                {
                                    #region Delete Recipt Entries
                                    var DeleteRecipt = await _appDbContext.Receipts.Where(s => s.TransactionNo == UpdateSalesOrder.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                                    if (DeleteRecipt.Count > 0)
                                    {
                                        _appDbContext.Receipts.RemoveRange(DeleteRecipt);
                                        await _appDbContext.SaveChangesAsync();
                                    }

                                    #endregion
                                    #region  Revert LedgerBalance & Subledger Balance
                                    // @cash A/c -----Dr
                                    var RevertCashLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (RevertCashLedgerBalance != null)
                                    {
                                        RevertCashLedgerBalance.RunningBalance -= UpdateSalesOrder.GrandTotal;
                                        RevertCashLedgerBalance.RunningBalanceType = (RevertCashLedgerBalance.RunningBalance > 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    //@sales A/c -----Cr
                                    var RevertSalesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (RevertSalesLedgerBalance != null)
                                    {
                                        RevertSalesLedgerBalance.RunningBalance += UpdateSalesOrder.GrandTotal;
                                        RevertSalesLedgerBalance.RunningBalanceType = (RevertSalesLedgerBalance.RunningBalance > 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    #endregion
                                    #region Update Ledger & SubLedger Balance
                                    // @SundryDebitors A/c ----- Dr
                                    var updateSundryDebitorsSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == data.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSundryDebitorsSubledgerBalance != null)
                                    {
                                        updateSundryDebitorsSubledgerBalance.RunningBalance += data.GrandTotal;
                                        updateSundryDebitorsSubledgerBalance.RunningBalanceType = (updateSundryDebitorsSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        var updateSundryDebitorsLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SundryDebtors && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                        if (updateSundryDebitorsLedgerBalance != null)
                                        {
                                            updateSundryDebitorsLedgerBalance.RunningBalance += data.GrandTotal;
                                            updateSundryDebitorsLedgerBalance.RunningBalanceType = (updateSundryDebitorsLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "SubLedger Balance Not Exist";
                                        return _Result;
                                    }
                                    // @Sales A/c -------- Cr
                                    var updateSalesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSalesLedgerBalance != null)
                                    {
                                        updateSalesLedgerBalance.RunningBalance -= data.GrandTotal;
                                        updateSalesLedgerBalance.RunningBalanceType = (updateSalesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    #endregion
                                    #region Inset Into Journal
                                    var JournalVoucherNo = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).SingleOrDefaultAsync();
                                    if (JournalVoucherNo != null)
                                    {
                                        if (int.TryParse(JournalVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                        {
                                            currentId++;
                                            Journalvoucher = $"JN{currentId:D6}";
                                        }
                                    }
                                    else
                                    {
                                        Journalvoucher = "JN000001";
                                    }
                                    var NewJournalSalesOrder = new Journal
                                    {
                                        VouvherNo = Journalvoucher,
                                        VoucherDate = convertedOrderDate,
                                        Fk_LedgerGroupId = MappingLedgerGroup.CurrentAssets,
                                        Fk_LedgerId = MappingLedgers.SundryDebtors,
                                        Fk_SubLedgerId = data.Fk_SubLedgerId,
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        TransactionNo = data.TransactionNo,
                                        Narration = data.TransactionNo.ToString(),
                                        Amount = data.GrandTotal,
                                        DrCr = "Dr"
                                    };
                                    await _appDbContext.Journals.AddAsync(NewJournalSalesOrder);
                                    await _appDbContext.SaveChangesAsync();
                                    #endregion
                                }
                            }
                            #region Update Sales Odr
                            UpdateSalesOrder.TransactionDate = convertedTransactionDate;
                            UpdateSalesOrder.TransactionType = data.TransactionType;
                            UpdateSalesOrder.PriceType = data.RateType;
                            if (data.TransactionType == "cash")
                            {
                                UpdateSalesOrder.CustomerName = data.CustomerName;
                                UpdateSalesOrder.Fk_SubLedgerId = null;
                            }
                            else
                            {
                                UpdateSalesOrder.Fk_SubLedgerId = data.Fk_SubLedgerId;
                                UpdateSalesOrder.CustomerName = null;
                            }
                            UpdateSalesOrder.OrderNo = data.OrderNo;
                            UpdateSalesOrder.OrderDate = convertedOrderDate;
                            UpdateSalesOrder.TranspoterName = data.TranspoterName;
                            UpdateSalesOrder.ReceivingPerson = data.ReceivingPerson;
                            UpdateSalesOrder.VehicleNo = data.VehicleNo;
                            UpdateSalesOrder.SubTotal = Convert.ToDecimal(data.SubTotal);
                            UpdateSalesOrder.Discount = Convert.ToDecimal(data.Discount);
                            UpdateSalesOrder.Gst = Convert.ToDecimal(data.Gst);
                            UpdateSalesOrder.GrandTotal = Convert.ToDecimal(data.GrandTotal);
                            UpdateSalesOrder.Narration = data.Naration;
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                            //************************************************************Sales Transaction**************************************************//
                            foreach (var item in data.RowData)
                            {
                                Guid SalesId = Guid.TryParse(item[0], out var parsedGuid) ? parsedGuid : Guid.Empty;
                                var UpdateSalesTransaction = await _appDbContext.SalesTransaction.Where(s => s.SalesId == SalesId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (UpdateSalesTransaction != null)
                                {
                                    #region Update Stock
                                    //Start testing
                                    var getFinishedGoodSalesConfig = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                    if (getFinishedGoodSalesConfig.Count > 0)
                                    {
                                        foreach (var item1 in getFinishedGoodSalesConfig)
                                        {
                                            var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item1.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateFhinishedGoodStock != null)
                                            {
                                                var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                                if (Unit != null)
                                                {
                                                    if (UpdateSalesTransaction.UnitQuantity != Convert.ToDecimal(item[4]))
                                                    {
                                                        decimal quantityDifference = UpdateSalesTransaction.UnitQuantity - Convert.ToDecimal(item[2]);
                                                        UpdateFhinishedGoodStock.AvilableStock += quantityDifference * Unit.UnitQuantity * item1.Quantity;
                                                        await _appDbContext.SaveChangesAsync();
                                                    }
                                                }
                                                else
                                                {
                                                    if (UpdateSalesTransaction.UnitQuantity != Convert.ToDecimal(item[4]))
                                                    {
                                                        decimal quantityDifference = UpdateSalesTransaction.UnitQuantity - Convert.ToDecimal(item[2]);
                                                        UpdateFhinishedGoodStock.AvilableStock += quantityDifference * item1.Quantity;
                                                        await _appDbContext.SaveChangesAsync();

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                                var AddNewFhinishedGoodStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    //AvilableStock = +(UpdateSalesTransaction.AlternateQuantity * Unit.UnitQuantity * item1.Quantity)
                                                    AvilableStock = Unit != null ? +(UpdateSalesTransaction.AlternateQuantity * Unit.UnitQuantity * item1.Quantity) : +(UpdateSalesTransaction.AlternateQuantity * item1.Quantity)
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == UpdateSalesTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateSalesTransaction.Fk_ProductId != Guid.Parse(item[1]))
                                        {
                                            UpdateStock.AvilableStock += UpdateSalesTransaction.UnitQuantity;
                                            await _appDbContext.SaveChangesAsync();
                                            var UpdateNewStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == Guid.Parse(item[1]) && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateNewStock != null)
                                            {
                                                UpdateNewStock.AvilableStock -= Convert.ToDecimal(item[4]);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                _Result.WarningMessage = "Stock Not Avilable";
                                                return _Result;
                                            }
                                        }
                                        if (UpdateSalesTransaction.UnitQuantity != Convert.ToDecimal(item[4]))
                                        {
                                            decimal quantityDifference = UpdateSalesTransaction.UnitQuantity - Convert.ToDecimal(item[4]);
                                            UpdateStock.AvilableStock += quantityDifference;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                    }
                                    //end
                                    #endregion
                                    #region Update Journal
                                    if (data.TransactionType == "credit")
                                    {
                                        var UpdateTransactionJournal = await _appDbContext.Journals.Where(s => s.TransactionId == UpdateSalesTransaction.SalesId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateTransactionJournal != null)
                                        {
                                            UpdateTransactionJournal.Amount = Convert.ToDecimal(item[8]);
                                            UpdateTransactionJournal.VoucherDate = convertedTransactionDate;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            var NewJournalSalesTransaction = new Journal
                                            {
                                                VouvherNo = Journalvoucher,
                                                VoucherDate = UpdateSalesOrder.OrderDate,
                                                Fk_LedgerGroupId = MappingLedgerGroup.Sales,
                                                Fk_LedgerId = MappingLedgers.SalesAccount,
                                                Fk_BranchId = BranchId,
                                                Fk_FinancialYearId = FinancialYear,
                                                TransactionNo = UpdateSalesOrder.TransactionNo,
                                                TransactionId = UpdateSalesTransaction.SalesId,
                                                Narration = UpdateSalesOrder.TransactionNo.ToString(),
                                                Amount = Convert.ToDecimal(item[8]),
                                                DrCr = "Cr"
                                            };
                                            await _appDbContext.Journals.AddAsync(NewJournalSalesTransaction);
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                    }
                                    #endregion
                                    #region Update Sales Trn
                                    var getFinishedGoodSalesConfig1 = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                    if (getFinishedGoodSalesConfig1.Count > 0)
                                    {
                                        foreach (var item1 in getFinishedGoodSalesConfig1)
                                        {
                                            var NewUpdateSalesTransaction = await _appDbContext.SalesTransaction
                                                .Where(s => s.Fk_ProductId == item1.Fk_SubFinishedGoodId)
                                                .SingleOrDefaultAsync();
                                            if(NewUpdateSalesTransaction != null)
                                            {
                                                NewUpdateSalesTransaction.Fk_ProductId = item1.Fk_SubFinishedGoodId;
                                                NewUpdateSalesTransaction.Fk_SubProductId = Guid.Parse(item[1]);
                                                NewUpdateSalesTransaction.AlternateQuantity = Convert.ToDecimal(item[2]);
                                                //UpdateSalesTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                                NewUpdateSalesTransaction.UnitQuantity = item1.Quantity * Convert.ToDecimal(item[2]);
                                                NewUpdateSalesTransaction.Rate = Convert.ToDecimal(item[5]);
                                                NewUpdateSalesTransaction.Discount = Convert.ToDecimal(item[6]);
                                                NewUpdateSalesTransaction.DiscountAmount = Convert.ToDecimal(item[7]);
                                                NewUpdateSalesTransaction.Gst = Convert.ToDecimal(item[8]);
                                                NewUpdateSalesTransaction.GstAmount = Convert.ToDecimal(item[9]);
                                                NewUpdateSalesTransaction.Amount = Convert.ToDecimal(item[10]);
                                                NewUpdateSalesTransaction.TransactionType = UpdateSalesOrder.TransactionType;

                                                if (!string.IsNullOrEmpty(item[3]))
                                                {
                                                    NewUpdateSalesTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                                }
                                                else
                                                {
                                                    NewUpdateSalesTransaction.Fk_AlternateUnitId = null;
                                                }
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                           
                                        }
                                        
                                       
                                    }
                                    else
                                    {
                                        UpdateSalesTransaction.Fk_ProductId = Guid.Parse(item[1]);
                                        UpdateSalesTransaction.AlternateQuantity = Convert.ToDecimal(item[2]);
                                        //UpdateSalesTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                        UpdateSalesTransaction.UnitQuantity = Convert.ToDecimal(item[4]);
                                        UpdateSalesTransaction.Rate = Convert.ToDecimal(item[5]);
                                        UpdateSalesTransaction.Discount = Convert.ToDecimal(item[6]);
                                        UpdateSalesTransaction.DiscountAmount = Convert.ToDecimal(item[7]);
                                        UpdateSalesTransaction.Gst = Convert.ToDecimal(item[8]);
                                        UpdateSalesTransaction.GstAmount = Convert.ToDecimal(item[9]);
                                        UpdateSalesTransaction.Amount = Convert.ToDecimal(item[10]);
                                        UpdateSalesTransaction.TransactionType = UpdateSalesOrder.TransactionType;

                                        if (!string.IsNullOrEmpty(item[3]))
                                        {
                                            UpdateSalesTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                        }
                                        else
                                        {
                                            UpdateSalesTransaction.Fk_AlternateUnitId = null;
                                        }
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Add Sales Trn
                                    var getFinishedGoodSalesConfig1 = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                    if (getFinishedGoodSalesConfig1.Count > 0)
                                    {
                                        #region SalesTransaction
                                        var newSalesTransaction = new SalesTransaction();
                                        foreach (var item1 in getFinishedGoodSalesConfig1)
                                        {
                                            newSalesTransaction = new SalesTransaction
                                            {
                                                Fk_SalesOrderId = data.SalesOrderId,
                                                TransactionNo = data.TransactionNo,
                                                TransactionDate = convertedTransactionDate,
                                                TransactionType = data.TransactionType,
                                                Fk_SubProductId = Guid.Parse(item[1]),
                                                Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                Fk_BranchId = BranchId,
                                                Fk_FinancialYearId = FinancialYear,
                                                AlternateQuantity = Convert.ToDecimal(item[2]),
                                                UnitQuantity = item1.Quantity * Convert.ToDecimal(item[2]),
                                                Rate = Convert.ToDecimal(item[5]),
                                                Discount = Convert.ToDecimal(item[6]),
                                                DiscountAmount = Convert.ToDecimal(item[7]),
                                                Gst = Convert.ToDecimal(item[8]),
                                                GstAmount = Convert.ToDecimal(item[9]),
                                                Amount = Convert.ToDecimal(item[10])
                                            };
                                            if (!string.IsNullOrEmpty(item[3]))
                                            {
                                                newSalesTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                            }
                                            else
                                            {
                                                newSalesTransaction.Fk_AlternateUnitId = null;
                                            }
                                            await _appDbContext.SalesTransaction.AddAsync(newSalesTransaction);
                                            await _appDbContext.SaveChangesAsync();

                                        }
                                        #endregion
                                        #region Update Stock
                                        var getFinishedGoodSalesConfig = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                        if (getFinishedGoodSalesConfig.Count > 0)
                                        {
                                            foreach (var item2 in getFinishedGoodSalesConfig)
                                            {
                                                var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item2.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                                if (UpdateFhinishedGoodStock != null)
                                                {
                                                    var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item2.Fk_SubFinishedGoodId).FirstOrDefault();
                                                    if (Unit != null)
                                                    {
                                                        UpdateFhinishedGoodStock.AvilableStock -= (newSalesTransaction.AlternateQuantity * Unit.UnitQuantity * item2.Quantity);
                                                        await _appDbContext.SaveChangesAsync();
                                                    }
                                                    else
                                                    {
                                                        UpdateFhinishedGoodStock.AvilableStock -= (newSalesTransaction.AlternateQuantity);
                                                        await _appDbContext.SaveChangesAsync();
                                                    }
                                                }
                                                else
                                                {
                                                    var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item2.Fk_SubFinishedGoodId).FirstOrDefault();
                                                    var AddNewFhinishedGoodStock = new Stock
                                                    {
                                                        Fk_BranchId = BranchId,
                                                        Fk_ProductId = item2.Fk_SubFinishedGoodId,
                                                        Fk_FinancialYear = FinancialYear,
                                                        AvilableStock = Unit != null ? -(newSalesTransaction.AlternateQuantity * Unit.UnitQuantity * item2.Quantity) : -(newSalesTransaction.AlternateQuantity * item2.Quantity)
                                                    };
                                                    await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                    await _appDbContext.SaveChangesAsync();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newSalesTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateStock != null)
                                            {
                                                UpdateStock.AvilableStock -= newSalesTransaction.UnitQuantity;
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                var AddNewStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = newSalesTransaction.Fk_ProductId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = -newSalesTransaction.UnitQuantity
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                        #endregion
                                        #region Create Journal
                                        if (data.TransactionType == "credit")
                                        {
                                            var JournalVoucherNo = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                                            if (JournalVoucherNo != null)
                                            {
                                                if (int.TryParse(JournalVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                                {
                                                    currentId++;
                                                    Journalvoucher = $"JN{currentId:D6}";
                                                }
                                            }
                                            else
                                            {
                                                Journalvoucher = "JN000001";
                                            }
                                            var NewJournalTransaction = new Journal
                                            {
                                                VouvherNo = Journalvoucher,
                                                VoucherDate = UpdateSalesOrder.OrderDate,
                                                Fk_LedgerGroupId = MappingLedgerGroup.CurrentliabilitiesAndProvisions,
                                                Fk_LedgerId = MappingLedgers.SundryCreditors,
                                                Fk_SubLedgerId = UpdateSalesOrder.Fk_SubLedgerId,
                                                Fk_BranchId = BranchId,
                                                Fk_FinancialYearId = FinancialYear,
                                                TransactionNo = UpdateSalesOrder.TransactionNo,
                                                Narration = UpdateSalesOrder.TransactionNo.ToString(),
                                                Amount = newSalesTransaction.Amount,
                                                DrCr = "Cr"
                                            };
                                            await _appDbContext.Journals.AddAsync(NewJournalTransaction);
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        #endregion



                                    }
                                    else
                                    {
                                        #region SalesTransaction 
                                        var newSalesTransaction = new SalesTransaction
                                        {
                                            Fk_SalesOrderId = data.SalesOrderId,
                                            TransactionNo = data.TransactionNo,
                                            TransactionDate = convertedTransactionDate,
                                            TransactionType = data.TransactionType,
                                            Fk_ProductId = Guid.Parse(item[1]),
                                            Fk_BranchId = BranchId,
                                            Fk_FinancialYearId = FinancialYear,
                                            AlternateQuantity = Convert.ToDecimal(item[2]),
                                            //Fk_AlternateUnitId = Guid.Parse(item[3]),
                                            UnitQuantity = Convert.ToDecimal(item[4]),
                                            Rate = Convert.ToDecimal(item[5]),
                                            Discount = Convert.ToDecimal(item[6]),
                                            DiscountAmount = Convert.ToDecimal(item[7]),
                                            Gst = Convert.ToDecimal(item[8]),
                                            GstAmount = Convert.ToDecimal(item[9]),
                                            Amount = Convert.ToDecimal(item[10])
                                        };
                                        if (!string.IsNullOrEmpty(item[3]))
                                        {
                                            newSalesTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                        }
                                        else
                                        {
                                            newSalesTransaction.Fk_AlternateUnitId = null;
                                        }
                                        await _appDbContext.SalesTransaction.AddAsync(newSalesTransaction);
                                        await _appDbContext.SaveChangesAsync();
                                        #endregion
                                        #region Update Stock
                                        var getFinishedGoodSalesConfig = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                        if (getFinishedGoodSalesConfig.Count > 0)
                                        {
                                            foreach (var item2 in getFinishedGoodSalesConfig)
                                            {
                                                var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item2.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                                if (UpdateFhinishedGoodStock != null)
                                                {
                                                    var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item2.Fk_SubFinishedGoodId).FirstOrDefault();
                                                    if (Unit != null)
                                                    {
                                                        UpdateFhinishedGoodStock.AvilableStock -= (newSalesTransaction.AlternateQuantity * Unit.UnitQuantity * item2.Quantity);
                                                        await _appDbContext.SaveChangesAsync();
                                                    }
                                                    else
                                                    {
                                                        UpdateFhinishedGoodStock.AvilableStock -= (newSalesTransaction.AlternateQuantity);
                                                        await _appDbContext.SaveChangesAsync();
                                                    }
                                                }
                                                else
                                                {
                                                    var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item2.Fk_SubFinishedGoodId).FirstOrDefault();
                                                    var AddNewFhinishedGoodStock = new Stock
                                                    {
                                                        Fk_BranchId = BranchId,
                                                        Fk_ProductId = item2.Fk_SubFinishedGoodId,
                                                        Fk_FinancialYear = FinancialYear,
                                                        AvilableStock = Unit != null ? -(newSalesTransaction.AlternateQuantity * Unit.UnitQuantity * item2.Quantity) : -(newSalesTransaction.AlternateQuantity * item2.Quantity)
                                                    };
                                                    await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                    await _appDbContext.SaveChangesAsync();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newSalesTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateStock != null)
                                            {
                                                UpdateStock.AvilableStock -= newSalesTransaction.UnitQuantity;
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                var AddNewStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = newSalesTransaction.Fk_ProductId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = -newSalesTransaction.UnitQuantity
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                        #endregion
                                        #region Create Journal
                                        if (data.TransactionType == "credit")
                                        {
                                            var JournalVoucherNo = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                                            if (JournalVoucherNo != null)
                                            {
                                                if (int.TryParse(JournalVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                                {
                                                    currentId++;
                                                    Journalvoucher = $"JN{currentId:D6}";
                                                }
                                            }
                                            else
                                            {
                                                Journalvoucher = "JN000001";
                                            }
                                            var NewJournalTransaction = new Journal
                                            {
                                                VouvherNo = Journalvoucher,
                                                VoucherDate = UpdateSalesOrder.OrderDate,
                                                Fk_LedgerGroupId = MappingLedgerGroup.CurrentliabilitiesAndProvisions,
                                                Fk_LedgerId = MappingLedgers.SundryCreditors,
                                                Fk_SubLedgerId = UpdateSalesOrder.Fk_SubLedgerId,
                                                Fk_BranchId = BranchId,
                                                Fk_FinancialYearId = FinancialYear,
                                                TransactionNo = UpdateSalesOrder.TransactionNo,
                                                Narration = UpdateSalesOrder.TransactionNo.ToString(),
                                                Amount = newSalesTransaction.Amount,
                                                DrCr = "Cr"
                                            };
                                            await _appDbContext.Journals.AddAsync(NewJournalTransaction);
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        #endregion

                                    }

                                    #endregion

                                }
                            }
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                            transaction.Commit();
                            _Result.IsSuccess = true;
                        }

                    }
                }
                catch (DbUpdateException ex)
                {
                    _Result.Exception = ex;
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/UpdateSales : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteSales(Guid Id, IDbContextTransaction transaction, bool IsCallback)
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
                        //*****************************Delete Sales Transactions**************************************//
                        var deleteSalesTransaction = await _appDbContext.SalesTransaction.Where(s => s.Fk_SalesOrderId == Id && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                        if (deleteSalesTransaction.Count > 0)
                        {
                            foreach (var item in deleteSalesTransaction)
                            {
                                #region Update Stock
                                var getFinishedGoodSalesConfig = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == item.Fk_ProductId).ToListAsync();
                                if (getFinishedGoodSalesConfig.Count > 0)
                                {
                                    foreach (var item1 in getFinishedGoodSalesConfig)
                                    {
                                        var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item1.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateFhinishedGoodStock != null)
                                        {
                                            var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                            UpdateFhinishedGoodStock.AvilableStock += Unit != null ? (item.AlternateQuantity * Unit.UnitQuantity * item1.Quantity) : item.AlternateQuantity * item1.Quantity;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                    }
                                }
                                else
                                {
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateStock != null)
                                    {
                                        UpdateStock.AvilableStock += item.UnitQuantity;
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                }
                                #endregion
                                _appDbContext.SalesTransaction.Remove(item);
                                await _appDbContext.SaveChangesAsync();
                            }

                        }
                        //*******************************Delete Sales Orders***************************************//
                        var deleteSalesOrders = await _appDbContext.SalesOrders.SingleOrDefaultAsync(x => x.SalesOrderId == Id && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYear);
                        if (deleteSalesOrders != null)
                        {
                            //delete From Journal Or Recipt
                            if (deleteSalesOrders.TransactionType == "credit")
                            {
                                #region Delete Journal Entries
                                var DeleteJournal = await _appDbContext.Journals.Where(s => s.TransactionNo == deleteSalesOrders.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                                if (DeleteJournal.Count > 0)
                                {
                                    _appDbContext.Journals.RemoveRange(DeleteJournal);
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                                #region Update LedgerBalance & Subledger Balance
                                //@SundryDebtors A/c -----Dr
                                var updateSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == deleteSalesOrders.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (updateSubledgerBalance != null)
                                {
                                    updateSubledgerBalance.RunningBalance -= deleteSalesOrders.GrandTotal;
                                    updateSubledgerBalance.RunningBalanceType = (updateSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                    var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateSubledgerBalance.Fk_LedgerBalanceId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (updateLedgerBalance != null)
                                    {
                                        updateLedgerBalance.RunningBalance -= deleteSalesOrders.GrandTotal;
                                        updateSubledgerBalance.RunningBalanceType = (updateSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                }

                                // @Sales A/c ------Cr
                                var updateSalesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateSalesLedgerBalance != null)
                                {
                                    updateSalesLedgerBalance.RunningBalance += deleteSalesOrders.GrandTotal;
                                    updateSalesLedgerBalance.RunningBalanceType = (updateSalesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                            }
                            else
                            {
                                #region Delete Recipt Entries
                                var DeleteRecipt = await _appDbContext.Receipts.Where(s => s.TransactionNo == deleteSalesOrders.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                                if (DeleteRecipt.Count > 0)
                                {
                                    _appDbContext.Receipts.RemoveRange(DeleteRecipt);
                                    await _appDbContext.SaveChangesAsync();
                                }

                                #endregion
                                #region Update LedgerBalance & Subledger Balance
                                // @cash A/c -----Dr
                                var updateCashLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateCashLedgerBalance != null)
                                {
                                    updateCashLedgerBalance.RunningBalance -= deleteSalesOrders.GrandTotal;
                                    updateCashLedgerBalance.RunningBalanceType = (updateCashLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    _Result.WarningMessage = "Ledger Balance Not Exist";
                                    return _Result;
                                }
                                //@sales A/c -----Cr
                                var updateSalesLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (updateSalesLedgerBalance != null)
                                {
                                    updateSalesLedgerBalance.RunningBalance += deleteSalesOrders.GrandTotal;
                                    updateSalesLedgerBalance.RunningBalanceType = (updateSalesLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    _Result.WarningMessage = "Ledger Balance Not Exist";
                                    return _Result;
                                }
                                #endregion
                            }
                            _appDbContext.SalesOrders.Remove(deleteSalesOrders);
                            await _appDbContext.SaveChangesAsync();
                        }
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/DeleteSales : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Sales Return
        public async Task<Result<string>> GetLastSalesReturnTransaction()
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                _Result.IsSuccess = false;
                var lastSaleseReturnOrder = await _appDbContext.SalesReturnOrders.Where(s => s.Fk_BranchId == BranchId).OrderByDescending(s => s.TransactionNo).Select(s => new { s.TransactionNo }).FirstOrDefaultAsync();
                if (lastSaleseReturnOrder != null)
                {
                    var lastTransactionId = lastSaleseReturnOrder.TransactionNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastTransactionId.AsSpan(2), out int currentId))
                    {
                        currentId++;
                        var newTransactionId = $"SR{currentId:D6}";
                        _Result.SingleObjData = newTransactionId;
                    }
                }
                else
                {
                    _Result.SingleObjData = "SR000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetLastSalesReturnTransaction : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<SalesReturnOrderModel>> GetSalesReturns()
        {
            Result<SalesReturnOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                var Query = await _appDbContext.SalesReturnOrders.Where(s => s.Fk_BranchId == BranchId).Select(s => new SalesReturnOrderModel
                {
                    SalesReturnOrderId = s.SalesReturnOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    OrderNo = s.OrderNo,
                    GrandTotal = s.GrandTotal,
                    CustomerName = s.CustomerName,
                    Naration = s.Narration,
                    SubLedger = s.SubLedger != null ? new SubLedgerModel { SubLedgerName = s.SubLedger.SubLedgerName } : null,
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    var OrderList = Query;
                    _Result.CollectionObjData = OrderList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetSalesReturns : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<SalesReturnOrderModel>> GetSalesReturnById(Guid Id)
        {
            Result<SalesReturnOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.SalesReturnOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.SalesReturnOrderId == Id).Select(s => new SalesReturnOrderModel
                {
                    SalesReturnOrderId = s.SalesReturnOrderId,
                    Fk_SubLedgerId = s.Fk_SubLedgerId,
                    CustomerName = s.CustomerName,
                    TransactionNo = s.TransactionNo,
                    TransactionType = s.TransactionType,
                    TransactionDate = s.TransactionDate,
                    OrderNo = s.OrderNo,
                    OrderDate = s.OrderDate,
                    Discount = s.Discount,
                    Gst = s.Gst,
                    SubTotal = s.SubTotal,
                    GrandTotal = s.GrandTotal,
                    TranspoterName = s.TranspoterName,
                    VehicleNo = s.VehicleNo,
                    ReceivingPerson = s.ReceivingPerson,
                    Naration = s.Narration,
                    SalesReturnTransactions = _appDbContext.SalesReturnTransactions
                    //.Where(x => x.Fk_SalesReturnOrderId == s.SalesReturnOrderId)
                     .Where(x => x.Fk_SalesReturnOrderId == s.SalesReturnOrderId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && (x.Fk_SubProductId == null || x.SalesReturnId == _appDbContext.SalesReturnTransactions.Where(y => y.Fk_SubProductId == x.Fk_SubProductId).Min(y => y.SalesReturnId)))
                    .Select(x => new SalesReturnTransactionModel
                    {
                        SalesReturnId = x.SalesReturnId,
                        AlternateQuantity = x.AlternateQuantity,
                        UnitQuantity = x.UnitQuantity,
                        UnitName = x.Fk_SubProductId == null ? _appDbContext.Products
                                      .Where(p => p.ProductId == x.Fk_ProductId)
                                      .Select(p => p.Unit.UnitName)
                                      .FirstOrDefault() : _appDbContext.Products
                                      .Where(p => p.ProductId == x.Fk_SubProductId)
                                     .Select(p => p.Unit.UnitName)
                                     .FirstOrDefault(),
                        Fk_AlternateUnitId = x.Fk_AlternateUnitId,
                        AlternateUnit = x.AlternateUnit != null ? new AlternateUnitModel { AlternateUnitName = x.AlternateUnit.AlternateUnitName } : null,
                        Rate = x.Rate,
                        Discount = x.Discount,
                        DiscountAmount = x.DiscountAmount,
                        Gst = x.Gst,
                        GstAmount = x.GstAmount,
                        Amount = x.Amount,
                        Fk_ProductId = x.Fk_SubProductId ?? x.Fk_ProductId,
                        Product = x.Fk_SubProductId == null ? _appDbContext.Products
                                 .Where(p => p.ProductId == x.Fk_ProductId)
                                   .Select(p => new ProductModel { ProductName = p.ProductName, Unit = new UnitModel { UnitName = p.Unit.UnitName } })
                                   .FirstOrDefault() : _appDbContext.Products
                                    .Where(p => p.ProductId == x.Fk_SubProductId)
                                    .Select(p => new ProductModel { ProductName = p.ProductName, Unit = new UnitModel { UnitName = p.Unit.UnitName } })
                                    .FirstOrDefault()
                    }).ToList(),
                }).FirstOrDefaultAsync();
                if (Query != null)
                {
                    var Order = Query;
                    _Result.SingleObjData = Order;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetSalesReturnById : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateSalesReturn(SalesReturnDataRequest data)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                string Journalvoucher = "";
                string ReciptVoucher = "";
                using var transaction = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (DateTime.TryParseExact(data.OrderDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedOrderDate) && DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTransactionDate))
                    {
                        #region SalesReturn Order
                        var newSalesReturnOrder = new SalesReturnOrder
                        {
                            Fk_SubLedgerId = data.Fk_SubLedgerId,
                            CustomerName = data.CustomerName,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear,
                            TransactionDate = convertedTransactionDate,
                            TransactionNo = data.TransactionNo,
                            TransactionType = data.TransactionType,
                            PriceType = data.RateType,
                            OrderNo = data.OrderNo,
                            OrderDate = convertedOrderDate,
                            TranspoterName = data.TranspoterName,
                            VehicleNo = data.VehicleNo,
                            ReceivingPerson = data.ReceivingPerson,
                            Narration = data.Naration,
                            SubTotal = data.SubTotal,
                            Discount = data.Discount,
                            Gst = data.Gst,
                            GrandTotal = data.GrandTotal,
                        };
                        await _appDbContext.SalesReturnOrders.AddAsync(newSalesReturnOrder);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        if (data.TransactionType == "credit")
                        {
                            #region Ledger & Subledger
                            // @SalesReturn A/c ------Dr
                            var updateSalesReturnLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesReturnAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updateSalesReturnLedgerBalance != null)
                            {
                                updateSalesReturnLedgerBalance.RunningBalance += newSalesReturnOrder.GrandTotal;
                                updateSalesReturnLedgerBalance.RunningBalanceType = (updateSalesReturnLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.SalesReturnAccount,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Dr",
                                    RunningBalance = newSalesReturnOrder.GrandTotal,
                                    RunningBalanceType = "Dr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                            }
                            // @SundryDebitors A/c ---- Cr
                            var LedgerBalanceId = Guid.Empty;
                            var updateSundryDebitorsLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SundryDebtors && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updateSundryDebitorsLedgerBalance != null)
                            {
                                updateSundryDebitorsLedgerBalance.RunningBalance -= newSalesReturnOrder.GrandTotal;
                                updateSundryDebitorsLedgerBalance.RunningBalanceType = (updateSundryDebitorsLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                                LedgerBalanceId = updateSundryDebitorsLedgerBalance.LedgerBalanceId;
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.SundryDebtors,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Cr",
                                    RunningBalance = -newSalesReturnOrder.GrandTotal,
                                    RunningBalanceType = "Cr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                                LedgerBalanceId = newLedgerBalance.LedgerBalanceId;
                            }
                            var updateSundryDebitorsSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == newSalesReturnOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                            if (updateSundryDebitorsSubledgerBalance != null)
                            {
                                updateSundryDebitorsSubledgerBalance.RunningBalance -= newSalesReturnOrder.GrandTotal;
                                updateSundryDebitorsSubledgerBalance.RunningBalanceType = (updateSundryDebitorsSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newSubLedgerBalance = new SubLedgerBalance
                                {
                                    Fk_LedgerBalanceId = LedgerBalanceId,
                                    Fk_SubLedgerId = data.Fk_SubLedgerId ?? Guid.Empty,
                                    OpeningBalanceType = "Cr",
                                    OpeningBalance = 0,
                                    RunningBalanceType = "Cr",
                                    RunningBalance = -newSalesReturnOrder.GrandTotal,
                                    Fk_FinancialYearId = FinancialYear,
                                    Fk_BranchId = BranchId
                                };
                                await _appDbContext.SubLedgerBalances.AddAsync(newSubLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                            }
                            #endregion
                            #region Journal 
                            var JournalVoucherNo = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                            if (JournalVoucherNo != null)
                            {
                                if (int.TryParse(JournalVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                {
                                    currentId++;
                                    Journalvoucher = $"JN{currentId:D6}";
                                }
                            }
                            else
                            {
                                Journalvoucher = "JN000001";
                            }
                            var NewJournalSalesOrder = new Journal
                            {
                                VouvherNo = Journalvoucher,
                                VoucherDate = newSalesReturnOrder.OrderDate,
                                Fk_LedgerGroupId = MappingLedgerGroup.CurrentAssets,
                                Fk_LedgerId = MappingLedgers.SundryDebtors,
                                Fk_SubLedgerId = newSalesReturnOrder.Fk_SubLedgerId,
                                Fk_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                TransactionNo = newSalesReturnOrder.TransactionNo,
                                TransactionId = newSalesReturnOrder.SalesReturnOrderId,
                                Narration = newSalesReturnOrder.TransactionNo.ToString(),
                                Amount = newSalesReturnOrder.GrandTotal,
                                DrCr = "Cr"
                            };
                            await _appDbContext.Journals.AddAsync(NewJournalSalesOrder);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                        }
                        else
                        {
                            #region Ledger & SubLedger
                            //@salesReturn A/c -----Dr
                            var updateSalesReturnLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesReturnAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updateSalesReturnLedgerBalance != null)
                            {
                                updateSalesReturnLedgerBalance.RunningBalance += newSalesReturnOrder.GrandTotal;
                                updateSalesReturnLedgerBalance.RunningBalanceType = (updateSalesReturnLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.SalesReturnAccount,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Dr",
                                    RunningBalance = newSalesReturnOrder.GrandTotal,
                                    RunningBalanceType = "Dr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                            }
                            // @cash A/c -----Cr
                            var updateCashLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updateCashLedgerBalance != null)
                            {
                                updateCashLedgerBalance.RunningBalance -= newSalesReturnOrder.GrandTotal;
                                updateCashLedgerBalance.RunningBalanceType = (updateCashLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.CashAccount,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Cr",
                                    RunningBalance = -newSalesReturnOrder.GrandTotal,
                                    RunningBalanceType = "Cr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                            }
                            #endregion
                            #region Receipt
                            var ReciptVoucherNo = await _appDbContext.Receipts.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                            if (ReciptVoucherNo != null)
                            {
                                if (int.TryParse(ReciptVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                {
                                    currentId++;
                                    ReciptVoucher = $"Cr{currentId:D6}";
                                }
                            }
                            else
                            {
                                ReciptVoucher = "CR000001";
                            }
                            var newRecipt = new Receipt
                            {
                                VoucherDate = newSalesReturnOrder.TransactionDate,
                                VouvherNo = ReciptVoucher,
                                CashBank = data.TransactionType,
                                Fk_LedgerGroupId = MappingLedgerGroup.Sales,
                                Fk_LedgerId = MappingLedgers.SalesReturnAccount,
                                Fk_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                TransactionNo = newSalesReturnOrder.TransactionNo,
                                Amount = newSalesReturnOrder.GrandTotal,
                                DrCr = "Dr",
                            };
                            await _appDbContext.Receipts.AddAsync(newRecipt);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                        }

                        foreach (var item in data.RowData)
                        {
                            #region SalesReturn Transaction
                            var getFinishedGoodSalesConfig1 = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                            if (getFinishedGoodSalesConfig1.Count > 0)
                            {
                                var newSalesReturnTransaction1 = new SalesReturnTransaction();
                                foreach (var item1 in getFinishedGoodSalesConfig1)
                                {
                                    newSalesReturnTransaction1 = new SalesReturnTransaction
                                    {
                                        Fk_SalesReturnOrderId = newSalesReturnOrder.SalesReturnOrderId,
                                        TransactionNo = newSalesReturnOrder.TransactionNo,
                                        TransactionDate = newSalesReturnOrder.TransactionDate,
                                        TransactionType = data.TransactionType,
                                        Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                        Fk_SubProductId = Guid.Parse(item[1]),
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        AlternateQuantity = Convert.ToDecimal(item[2]),
                                        UnitQuantity = item1.Quantity * Convert.ToDecimal(item[2]),
                                        Rate = Convert.ToDecimal(item[5]),
                                        Discount = Convert.ToDecimal(item[6]),
                                        DiscountAmount = Convert.ToDecimal(item[7]),
                                        Gst = Convert.ToDecimal(item[8]),
                                        GstAmount = Convert.ToDecimal(item[9]),
                                        Amount = Convert.ToDecimal(item[10])
                                    };
                                    if (!string.IsNullOrEmpty(item[3]))
                                    {
                                        newSalesReturnTransaction1.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                    }
                                    else
                                    {
                                        newSalesReturnTransaction1.Fk_AlternateUnitId = null;
                                    }
                                    await _appDbContext.SalesReturnTransactions.AddAsync(newSalesReturnTransaction1);
                                    await _appDbContext.SaveChangesAsync();

                                }
                                #region Journal
                                if (data.TransactionType == "credit")
                                {
                                    var NewJournalSalesTransaction = new Journal
                                    {
                                        VouvherNo = Journalvoucher,
                                        VoucherDate = newSalesReturnOrder.OrderDate,
                                        Fk_LedgerGroupId = MappingLedgerGroup.Sales,
                                        Fk_LedgerId = MappingLedgers.SalesReturnAccount,
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        TransactionNo = newSalesReturnOrder.TransactionNo,
                                        TransactionId = newSalesReturnTransaction1.SalesReturnId,
                                        Narration = newSalesReturnOrder.TransactionNo.ToString(),
                                        Amount = newSalesReturnTransaction1.Amount,
                                        DrCr = "Dr"
                                    };
                                    await _appDbContext.Journals.AddAsync(NewJournalSalesTransaction);
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                                #region Update Stock

                                var getFinishedGoodSalesConfig = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                if (getFinishedGoodSalesConfig.Count > 0)
                                {
                                    foreach (var item1 in getFinishedGoodSalesConfig)
                                    {
                                        var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item1.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateFhinishedGoodStock != null)
                                        {
                                            var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                            UpdateFhinishedGoodStock.AvilableStock += Unit != null ? (newSalesReturnTransaction1.AlternateQuantity * Unit.UnitQuantity * item1.Quantity) : newSalesReturnTransaction1.AlternateQuantity * item1.Quantity;
                                            await _appDbContext.SaveChangesAsync();

                                        }
                                        else
                                        {
                                            var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                            if (Unit != null)
                                            {
                                                var AddNewFhinishedGoodStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = +(newSalesReturnTransaction1.AlternateQuantity * Unit.UnitQuantity * item1.Quantity)
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                var AddNewFhinishedGoodStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = +(newSalesReturnTransaction1.AlternateQuantity * item1.Quantity)
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newSalesReturnTransaction1.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateStock != null)
                                    {
                                        UpdateStock.AvilableStock += newSalesReturnTransaction1.UnitQuantity;
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var AddNewStock = new Stock
                                        {
                                            Fk_BranchId = BranchId,
                                            Fk_ProductId = newSalesReturnTransaction1.Fk_ProductId,
                                            Fk_FinancialYear = FinancialYear,
                                            AvilableStock = newSalesReturnTransaction1.UnitQuantity
                                        };
                                        await _appDbContext.Stocks.AddAsync(AddNewStock);
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                }
                                #endregion

                            }
                            else
                            {
                                var newSalesReturnTransaction = new SalesReturnTransaction
                                {
                                    Fk_SalesReturnOrderId = newSalesReturnOrder.SalesReturnOrderId,
                                    TransactionNo = newSalesReturnOrder.TransactionNo,
                                    TransactionDate = newSalesReturnOrder.TransactionDate,
                                    TransactionType = data.TransactionType,
                                    Fk_ProductId = Guid.Parse(item[1]),
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYearId = FinancialYear,
                                    AlternateQuantity = Convert.ToDecimal(item[2]),
                                    UnitQuantity = Convert.ToDecimal(item[4]),
                                    Rate = Convert.ToDecimal(item[5]),
                                    Discount = Convert.ToDecimal(item[6]),
                                    DiscountAmount = Convert.ToDecimal(item[7]),
                                    Gst = Convert.ToDecimal(item[8]),
                                    GstAmount = Convert.ToDecimal(item[9]),
                                    Amount = Convert.ToDecimal(item[10])
                                };
                                if (!string.IsNullOrEmpty(item[3]))
                                {
                                    newSalesReturnTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                }
                                else
                                {
                                    newSalesReturnTransaction.Fk_AlternateUnitId = null;
                                }
                                await _appDbContext.SalesReturnTransactions.AddAsync(newSalesReturnTransaction);
                                await _appDbContext.SaveChangesAsync();
                                #endregion
                                #region Journal
                                if (data.TransactionType == "credit")
                                {
                                    var NewJournalSalesTransaction = new Journal
                                    {
                                        VouvherNo = Journalvoucher,
                                        VoucherDate = newSalesReturnOrder.OrderDate,
                                        Fk_LedgerGroupId = MappingLedgerGroup.Sales,
                                        Fk_LedgerId = MappingLedgers.SalesReturnAccount,
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        TransactionNo = newSalesReturnOrder.TransactionNo,
                                        TransactionId = newSalesReturnTransaction.SalesReturnId,
                                        Narration = newSalesReturnOrder.TransactionNo.ToString(),
                                        Amount = newSalesReturnTransaction.Amount,
                                        DrCr = "Dr"
                                    };
                                    await _appDbContext.Journals.AddAsync(NewJournalSalesTransaction);
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                                #region Update Stock

                                var getFinishedGoodSalesConfig = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                if (getFinishedGoodSalesConfig.Count > 0)
                                {
                                    foreach (var item1 in getFinishedGoodSalesConfig)
                                    {
                                        var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item1.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateFhinishedGoodStock != null)
                                        {
                                            var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                            UpdateFhinishedGoodStock.AvilableStock += Unit != null ? (newSalesReturnTransaction.AlternateQuantity * Unit.UnitQuantity * item1.Quantity) : newSalesReturnTransaction.AlternateQuantity * item1.Quantity;
                                            await _appDbContext.SaveChangesAsync();

                                        }
                                        else
                                        {
                                            var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                            if (Unit != null)
                                            {
                                                var AddNewFhinishedGoodStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = +(newSalesReturnTransaction.AlternateQuantity * Unit.UnitQuantity * item1.Quantity)
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                var AddNewFhinishedGoodStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = +(newSalesReturnTransaction.AlternateQuantity * item1.Quantity)
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newSalesReturnTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateStock != null)
                                    {
                                        UpdateStock.AvilableStock += newSalesReturnTransaction.UnitQuantity;
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var AddNewStock = new Stock
                                        {
                                            Fk_BranchId = BranchId,
                                            Fk_ProductId = newSalesReturnTransaction.Fk_ProductId,
                                            Fk_FinancialYear = FinancialYear,
                                            AvilableStock = newSalesReturnTransaction.UnitQuantity
                                        };
                                        await _appDbContext.Stocks.AddAsync(AddNewStock);
                                        await _appDbContext.SaveChangesAsync();
                                    }
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/CreateSalesReturn : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateSalesReturn(SalesReturnDataRequest data)
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
                    if (DateTime.TryParseExact(data.OrderDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedOrderDate) && DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTransactionDate))
                    {
                        var UpdateSalesReturnOrder = await _appDbContext.SalesReturnOrders.Where(s => s.SalesReturnOrderId == data.SalesRetunOrderId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                        if (UpdateSalesReturnOrder != null)
                        {
                            string Journalvoucher = "";
                            string ReciptVoucher = "";
                            if (UpdateSalesReturnOrder.TransactionType == data.TransactionType)
                            {
                                var difference = data.GrandTotal - UpdateSalesReturnOrder.GrandTotal;
                                if (data.TransactionType == "credit")
                                {
                                    #region Update Ledger & SubLedger
                                    if (UpdateSalesReturnOrder.GrandTotal != data.GrandTotal)
                                    {
                                        // @ Sales Return A/c --------------- Dr
                                        var updateSalesReturnledgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesReturnAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (updateSalesReturnledgerBalance != null)
                                        {
                                            updateSalesReturnledgerBalance.RunningBalance += difference;
                                            updateSalesReturnledgerBalance.RunningBalanceType = (updateSalesReturnledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                        // @SundryDreditor A/c ------------ Cr
                                        var updateSundryDebtorsSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == UpdateSalesReturnOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                        if (updateSundryDebtorsSubledgerBalance != null)
                                        {
                                            updateSundryDebtorsSubledgerBalance.RunningBalance -= difference;
                                            updateSundryDebtorsSubledgerBalance.RunningBalanceType = (updateSundryDebtorsSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateSundryDebtorsSubledgerBalance.Fk_LedgerBalanceId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                            if (updateLedgerBalance != null)
                                            {
                                                updateLedgerBalance.RunningBalance -= difference;
                                                updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            }
                                            else
                                            {
                                                _Result.WarningMessage = "Ledger Balance Not Exist";
                                                return _Result;
                                            }
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "SubLedger Balance Not Exist";
                                            return _Result;
                                        }
                                    }
                                    #endregion
                                    #region Journal
                                    var UpdateOrderJournal = await _appDbContext.Journals.Where(s => s.TransactionId == UpdateSalesReturnOrder.SalesReturnOrderId && s.Fk_SubLedgerId == UpdateSalesReturnOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateOrderJournal != null)
                                    {
                                        UpdateOrderJournal.Amount = data.GrandTotal;
                                        UpdateOrderJournal.VoucherDate = convertedTransactionDate;
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Journal Entry Not Exist";
                                        return _Result;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Update Ledger & SubLedger
                                    if (UpdateSalesReturnOrder.GrandTotal != data.GrandTotal)
                                    {
                                        //@salesReturn A/c -----Dr
                                        var updateSalesReturnLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesReturnAccount && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                        if (updateSalesReturnLedgerBalance != null)
                                        {
                                            updateSalesReturnLedgerBalance.RunningBalance += difference;
                                            updateSalesReturnLedgerBalance.RunningBalanceType = (updateSalesReturnLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                        // @cash A/c -----Cr
                                        var updateCashLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                        if (updateCashLedgerBalance != null)
                                        {
                                            updateCashLedgerBalance.RunningBalance -= difference;
                                            updateCashLedgerBalance.RunningBalanceType = (updateCashLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                    }
                                    #endregion
                                    #region Update Recipt
                                    var UpdateOrderRecipt = await _appDbContext.Receipts.Where(s => s.TransactionNo == UpdateSalesReturnOrder.TransactionNo && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                    if (UpdateOrderRecipt != null)
                                    {
                                        UpdateOrderRecipt.Amount = data.GrandTotal;
                                        UpdateOrderRecipt.VoucherDate = convertedTransactionDate;
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                if (data.TransactionType == "cash")
                                {
                                    #region Delete Journal Entries
                                    var DeleteJournal = await _appDbContext.Journals.Where(s => s.TransactionNo == UpdateSalesReturnOrder.TransactionNo && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                                    if (DeleteJournal.Count > 0)
                                    {
                                        _appDbContext.Journals.RemoveRange(DeleteJournal);
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    #endregion
                                    #region Revert LedgerBalance & Subledger Balance
                                    // @ Sales Return A/c --------------- Dr
                                    var updateSalesReturnledgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesReturnAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSalesReturnledgerBalance != null)
                                    {
                                        updateSalesReturnledgerBalance.RunningBalance += UpdateSalesReturnOrder.GrandTotal;
                                        updateSalesReturnledgerBalance.RunningBalanceType = (updateSalesReturnledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    // @SundryDreditor A/c ------------ Cr
                                    var updateSundryDebtorsSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == UpdateSalesReturnOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSundryDebtorsSubledgerBalance != null)
                                    {
                                        updateSundryDebtorsSubledgerBalance.RunningBalance -= UpdateSalesReturnOrder.GrandTotal;
                                        updateSundryDebtorsSubledgerBalance.RunningBalanceType = (updateSundryDebtorsSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateSundryDebtorsSubledgerBalance.Fk_LedgerBalanceId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                        if (updateLedgerBalance != null)
                                        {
                                            updateLedgerBalance.RunningBalance -= UpdateSalesReturnOrder.GrandTotal;
                                            updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "SubLedger Balance Not Exist";
                                        return _Result;
                                    }
                                    #endregion
                                    #region Update LedgerBalance & Subledger Balance
                                    //@salesReturn A/c -----Dr
                                    var updateSalesReturnLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesReturnAccount && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                    if (updateSalesReturnLedgerBalance != null)
                                    {
                                        updateSalesReturnLedgerBalance.RunningBalance += data.GrandTotal;
                                        updateSalesReturnLedgerBalance.RunningBalanceType = (updateSalesReturnLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    // @cash A/c -----Cr
                                    var updateCashLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                    if (updateCashLedgerBalance != null)
                                    {
                                        updateCashLedgerBalance.RunningBalance -= data.GrandTotal;
                                        updateCashLedgerBalance.RunningBalanceType = (updateCashLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    #endregion
                                    #region  Insert Into Recipt  
                                    var ReciptVoucherNo = await _appDbContext.Receipts.Where(s => s.Fk_BranchId == BranchId).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                                    if (ReciptVoucherNo != null)
                                    {
                                        if (int.TryParse(ReciptVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                        {
                                            currentId++;
                                            ReciptVoucher = $"Cr{currentId:D6}";
                                        }
                                    }
                                    else
                                    {
                                        ReciptVoucher = "CR000001";
                                    }
                                    var newRecipt = new Receipt
                                    {
                                        VoucherDate = convertedOrderDate,
                                        VouvherNo = ReciptVoucher,
                                        CashBank = data.TransactionType,
                                        Fk_LedgerGroupId = MappingLedgerGroup.Sales,
                                        Fk_LedgerId = MappingLedgers.SalesAccount,
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        TransactionNo = data.TransactionNo,
                                        Amount = data.GrandTotal,
                                        DrCr = "Cr",
                                    };
                                    await _appDbContext.Receipts.AddAsync(newRecipt);
                                    await _appDbContext.SaveChangesAsync();
                                    #endregion
                                }
                                else
                                {
                                    #region Delete Recipt Entries
                                    var DeleteRecipt = await _appDbContext.Receipts.Where(s => s.TransactionNo == UpdateSalesReturnOrder.TransactionNo && s.Fk_BranchId == BranchId).ToListAsync();
                                    if (DeleteRecipt.Count > 0)
                                    {
                                        _appDbContext.Receipts.RemoveRange(DeleteRecipt);
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    #endregion
                                    #region  Revert LedgerBalance & Subledger Balance
                                    //@salesReturn A/c -----Dr
                                    var updateSalesReturnLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesReturnAccount && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                    if (updateSalesReturnLedgerBalance != null)
                                    {
                                        updateSalesReturnLedgerBalance.RunningBalance += UpdateSalesReturnOrder.GrandTotal;
                                        updateSalesReturnLedgerBalance.RunningBalanceType = (updateSalesReturnLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    // @cash A/c -----Cr
                                    var updateCashLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                    if (updateCashLedgerBalance != null)
                                    {
                                        updateCashLedgerBalance.RunningBalance -= UpdateSalesReturnOrder.GrandTotal;
                                        updateCashLedgerBalance.RunningBalanceType = (updateCashLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    #endregion
                                    #region Update Ledger & SubLedger Balance
                                    // @ Sales Return A/c --------------- Dr
                                    var updateSalesReturnledgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.SalesReturnAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSalesReturnledgerBalance != null)
                                    {
                                        updateSalesReturnledgerBalance.RunningBalance += data.GrandTotal;
                                        updateSalesReturnledgerBalance.RunningBalanceType = (updateSalesReturnledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                    // @SundryDreditor A/c ------------ Cr
                                    var updateSundryDebtorsSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == UpdateSalesReturnOrder.Fk_SubLedgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (updateSundryDebtorsSubledgerBalance != null)
                                    {
                                        updateSundryDebtorsSubledgerBalance.RunningBalance -= data.GrandTotal;
                                        updateSundryDebtorsSubledgerBalance.RunningBalanceType = (updateSundryDebtorsSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        var updateLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateSundryDebtorsSubledgerBalance.Fk_LedgerBalanceId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                        if (updateLedgerBalance != null)
                                        {
                                            updateLedgerBalance.RunningBalance -= data.GrandTotal;
                                            updateLedgerBalance.RunningBalanceType = (updateLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "SubLedger Balance Not Exist";
                                        return _Result;
                                    }
                                    #endregion
                                    #region Inset Into Journal
                                    var JournalVoucherNo = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                                    if (JournalVoucherNo != null)
                                    {
                                        if (int.TryParse(JournalVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                        {
                                            currentId++;
                                            Journalvoucher = $"JN{currentId:D6}";
                                        }
                                    }
                                    else
                                    {
                                        Journalvoucher = "JN000001";
                                    }
                                    var NewJournalSalesOrder = new Journal
                                    {
                                        VouvherNo = Journalvoucher,
                                        VoucherDate = convertedOrderDate,
                                        Fk_LedgerGroupId = MappingLedgerGroup.CurrentAssets,
                                        Fk_LedgerId = MappingLedgers.SundryDebtors,
                                        Fk_SubLedgerId = data.Fk_SubLedgerId,
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        TransactionNo = data.TransactionNo,
                                        Narration = data.TransactionNo.ToString(),
                                        Amount = data.GrandTotal,
                                        DrCr = "Dr"
                                    };
                                    await _appDbContext.Journals.AddAsync(NewJournalSalesOrder);
                                    await _appDbContext.SaveChangesAsync();
                                    #endregion
                                }
                            }
                            #region Update Sales Return Odr
                            #region Sales Return Odr
                            UpdateSalesReturnOrder.TransactionDate = convertedTransactionDate;
                            UpdateSalesReturnOrder.TransactionType = data.TransactionType;
                            UpdateSalesReturnOrder.PriceType = data.RateType;
                            if (data.TransactionType == "cash")
                            {
                                UpdateSalesReturnOrder.CustomerName = data.CustomerName;
                                UpdateSalesReturnOrder.Fk_SubLedgerId = null;
                            }
                            else
                            {
                                UpdateSalesReturnOrder.Fk_SubLedgerId = data.Fk_SubLedgerId;
                                UpdateSalesReturnOrder.CustomerName = null;
                            }
                            UpdateSalesReturnOrder.OrderNo = data.OrderNo;
                            UpdateSalesReturnOrder.OrderDate = convertedOrderDate;
                            UpdateSalesReturnOrder.TranspoterName = data.TranspoterName;
                            UpdateSalesReturnOrder.ReceivingPerson = data.ReceivingPerson;
                            UpdateSalesReturnOrder.Narration = data.Naration;
                            UpdateSalesReturnOrder.VehicleNo = data.VehicleNo;
                            UpdateSalesReturnOrder.SubTotal = Convert.ToDecimal(data.SubTotal);
                            UpdateSalesReturnOrder.Discount = Convert.ToDecimal(data.Discount);
                            UpdateSalesReturnOrder.Gst = Convert.ToDecimal(data.Gst);
                            UpdateSalesReturnOrder.GrandTotal = Convert.ToDecimal(data.GrandTotal);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                            #endregion
                            foreach (var item in data.RowData)
                            {
                                Guid SalesReturnId = Guid.TryParse(item[0], out var parsedGuid) ? parsedGuid : Guid.Empty;
                                var UpdateSalesReturnTransaction = await _appDbContext.SalesReturnTransactions.Where(s => s.SalesReturnId == SalesReturnId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                if (UpdateSalesReturnTransaction != null)
                                {
                                    #region Update Sales Trn
                                    var getFinishedGoodSalesConfig1 = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                    if (getFinishedGoodSalesConfig1.Count > 0)
                                    {
                                        foreach (var item1 in getFinishedGoodSalesConfig1)   
                                        {
                                            var newUpdateSalesReturnTransaction = await _appDbContext.SalesReturnTransactions
                                               .Where(s => s.Fk_ProductId == item1.Fk_SubFinishedGoodId)
                                               .SingleOrDefaultAsync();
                                            if(newUpdateSalesReturnTransaction != null)
                                            {
                                                newUpdateSalesReturnTransaction.Fk_ProductId = item1.Fk_SubFinishedGoodId;
                                                newUpdateSalesReturnTransaction.Fk_SubProductId = Guid.Parse(item[1]);
                                                newUpdateSalesReturnTransaction.AlternateQuantity = Convert.ToDecimal(item[2]);
                                                newUpdateSalesReturnTransaction.UnitQuantity = item1.Quantity * Convert.ToDecimal(item[2]);
                                                newUpdateSalesReturnTransaction.Rate = Convert.ToDecimal(item[5]);
                                                newUpdateSalesReturnTransaction.Discount = Convert.ToDecimal(item[6]);
                                                newUpdateSalesReturnTransaction.DiscountAmount = Convert.ToDecimal(item[7]);
                                                newUpdateSalesReturnTransaction.Gst = Convert.ToDecimal(item[8]);
                                                newUpdateSalesReturnTransaction.GstAmount = Convert.ToDecimal(item[9]);
                                                newUpdateSalesReturnTransaction.Amount = Convert.ToDecimal(item[10]);
                                                newUpdateSalesReturnTransaction.TransactionType = UpdateSalesReturnOrder.TransactionType;
                                                if (!string.IsNullOrEmpty(item[3]))
                                                {
                                                    newUpdateSalesReturnTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                                }
                                                else
                                                {
                                                    newUpdateSalesReturnTransaction.Fk_AlternateUnitId = null;
                                                }
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        UpdateSalesReturnTransaction.Fk_ProductId = Guid.Parse(item[1]);
                                        UpdateSalesReturnTransaction.AlternateQuantity = Convert.ToDecimal(item[2]);
                                        UpdateSalesReturnTransaction.UnitQuantity = Convert.ToDecimal(item[4]);
                                        UpdateSalesReturnTransaction.Rate = Convert.ToDecimal(item[5]);
                                        UpdateSalesReturnTransaction.Discount = Convert.ToDecimal(item[6]);
                                        UpdateSalesReturnTransaction.DiscountAmount = Convert.ToDecimal(item[7]);
                                        UpdateSalesReturnTransaction.Gst = Convert.ToDecimal(item[8]);
                                        UpdateSalesReturnTransaction.GstAmount = Convert.ToDecimal(item[9]);
                                        UpdateSalesReturnTransaction.Amount = Convert.ToDecimal(item[10]);
                                        UpdateSalesReturnTransaction.TransactionType = UpdateSalesReturnOrder.TransactionType;
                                        if (!string.IsNullOrEmpty(item[3]))
                                        {
                                            UpdateSalesReturnTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                        }
                                        else
                                        {
                                            UpdateSalesReturnTransaction.Fk_AlternateUnitId = null;
                                        }
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    #endregion
                                    #region Update Stock
                                    var getFinishedGoodSalesConfig = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                    if (getFinishedGoodSalesConfig.Count > 0)
                                    {
                                        foreach (var item1 in getFinishedGoodSalesConfig)
                                        {
                                            var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item1.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateFhinishedGoodStock != null)
                                            {
                                                var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                                if (Unit != null)
                                                {
                                                    if (UpdateSalesReturnTransaction.UnitQuantity != Convert.ToDecimal(item[4]))
                                                    {
                                                        decimal quantityDifference = UpdateSalesReturnTransaction.UnitQuantity - Convert.ToDecimal(item[2]);
                                                        UpdateFhinishedGoodStock.AvilableStock += quantityDifference * Unit.UnitQuantity * item1.Quantity;
                                                        await _appDbContext.SaveChangesAsync();
                                                    }
                                                }
                                                else
                                                {
                                                    if (UpdateSalesReturnTransaction.UnitQuantity != Convert.ToDecimal(item[4]))
                                                    {
                                                        decimal quantityDifference = UpdateSalesReturnTransaction.UnitQuantity - Convert.ToDecimal(item[2]);
                                                        UpdateFhinishedGoodStock.AvilableStock += quantityDifference * item1.Quantity;
                                                        await _appDbContext.SaveChangesAsync();

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                                var AddNewFhinishedGoodStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = +(UpdateSalesReturnTransaction.AlternateQuantity * Unit.UnitQuantity * item1.Quantity)
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == UpdateSalesReturnTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateSalesReturnTransaction.Fk_ProductId != Guid.Parse(item[1]))
                                        {
                                            UpdateStock.AvilableStock += UpdateSalesReturnTransaction.UnitQuantity;
                                            await _appDbContext.SaveChangesAsync();
                                            var UpdateNewStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == Guid.Parse(item[1]) && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateNewStock != null)
                                            {
                                                UpdateNewStock.AvilableStock -= Convert.ToDecimal(item[4]);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                _Result.WarningMessage = "Stock Not Avilable";
                                                return _Result;
                                            }
                                        }
                                        if (UpdateSalesReturnTransaction.UnitQuantity != Convert.ToDecimal(item[4]))
                                        {
                                            decimal quantityDifference = UpdateSalesReturnTransaction.UnitQuantity - Convert.ToDecimal(item[4]);
                                            UpdateStock.AvilableStock += quantityDifference;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                    }
                                    #endregion
                                    #region Update Journal
                                    if (data.TransactionType == "credit")
                                    {
                                        var UpdateTransactionJournal = await _appDbContext.Journals.Where(s => s.TransactionId == UpdateSalesReturnTransaction.SalesReturnId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                        if (UpdateTransactionJournal != null)
                                        {
                                            UpdateTransactionJournal.Amount = Convert.ToDecimal(item[8]);
                                            UpdateTransactionJournal.VoucherDate = convertedTransactionDate;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            var NewJournalSalesTransaction = new Journal
                                            {
                                                VouvherNo = Journalvoucher,
                                                VoucherDate = UpdateSalesReturnOrder.OrderDate,
                                                Fk_LedgerGroupId = MappingLedgerGroup.Sales,
                                                Fk_LedgerId = MappingLedgers.SalesAccount,
                                                Fk_BranchId = BranchId,
                                                Fk_FinancialYearId = FinancialYear,
                                                TransactionNo = UpdateSalesReturnOrder.TransactionNo,
                                                TransactionId = UpdateSalesReturnTransaction.SalesReturnId,
                                                Narration = UpdateSalesReturnOrder.TransactionNo.ToString(),
                                                Amount = Convert.ToDecimal(item[8]),
                                                DrCr = "Cr"
                                            };
                                            await _appDbContext.Journals.AddAsync(NewJournalSalesTransaction);
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                    }
                                    #endregion

                                }
                                else
                                {

                                    var getFinishedGoodSalesConfig1 = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                    if (getFinishedGoodSalesConfig1.Count > 0)
                                    {
                                        #region Add SalesreturnTransaction
                                        var newSalesReturnTransaction = new SalesReturnTransaction();
                                        foreach (var item1 in getFinishedGoodSalesConfig1)
                                        {
                                            newSalesReturnTransaction = new SalesReturnTransaction
                                            {
                                                Fk_SalesReturnOrderId = data.SalesRetunOrderId,
                                                TransactionNo = data.TransactionNo,
                                                TransactionType = data.TransactionType,
                                                Fk_SubProductId = Guid.Parse(item[1]),
                                                Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                Fk_BranchId = BranchId,
                                                Fk_FinancialYearId = FinancialYear,
                                                AlternateQuantity = Convert.ToDecimal(item[2]),
                                                UnitQuantity = item1.Quantity * Convert.ToDecimal(item[2]),
                                                Rate = Convert.ToDecimal(item[5]),
                                                Discount = Convert.ToDecimal(item[6]),
                                                DiscountAmount = Convert.ToDecimal(item[7]),
                                                Gst = Convert.ToDecimal(item[8]),
                                                GstAmount = Convert.ToDecimal(item[9]),
                                                Amount = Convert.ToDecimal(item[10])
                                            };
                                            if (!string.IsNullOrEmpty(item[3]))
                                            {
                                                newSalesReturnTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                            }
                                            else
                                            {
                                                newSalesReturnTransaction.Fk_AlternateUnitId = null;
                                            }
                                            await _appDbContext.SalesReturnTransactions.AddAsync(newSalesReturnTransaction);
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        #endregion
                                        #region Update Stock
                                        var getFinishedGoodSalesConfig = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                        if (getFinishedGoodSalesConfig.Count > 0)
                                        {
                                            foreach (var item1 in getFinishedGoodSalesConfig)
                                            {
                                                var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item1.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                                if (UpdateFhinishedGoodStock != null)
                                                {
                                                    var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                                    if (Unit != null)
                                                    {
                                                        UpdateFhinishedGoodStock.AvilableStock += (newSalesReturnTransaction.AlternateQuantity * Unit.UnitQuantity * item1.Quantity);
                                                        await _appDbContext.SaveChangesAsync();
                                                    }
                                                    else
                                                    {
                                                        UpdateFhinishedGoodStock.AvilableStock += (newSalesReturnTransaction.AlternateQuantity * item1.Quantity);
                                                        await _appDbContext.SaveChangesAsync();
                                                    }
                                                }
                                                else
                                                {
                                                    var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                                    var AddNewFhinishedGoodStock = new Stock
                                                    {
                                                        Fk_BranchId = BranchId,
                                                        Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                        Fk_FinancialYear = FinancialYear,
                                                        AvilableStock = Unit != null ? +(newSalesReturnTransaction.AlternateQuantity * Unit.UnitQuantity * item1.Quantity) : +(newSalesReturnTransaction.AlternateQuantity * item1.Quantity)
                                                    };
                                                    await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                    await _appDbContext.SaveChangesAsync();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newSalesReturnTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateStock != null)
                                            {
                                                UpdateStock.AvilableStock += newSalesReturnTransaction.UnitQuantity;
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                var AddNewStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = newSalesReturnTransaction.Fk_ProductId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = +newSalesReturnTransaction.UnitQuantity
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                        #endregion
                                        #region Create Journal
                                        if (data.TransactionType == "credit")
                                        {
                                            var JournalVoucherNo = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                                            if (JournalVoucherNo != null)
                                            {
                                                if (int.TryParse(JournalVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                                {
                                                    currentId++;
                                                    Journalvoucher = $"JN{currentId:D6}";
                                                }
                                            }
                                            else
                                            {
                                                Journalvoucher = "JN000001";
                                            }
                                            var NewJournalTransaction = new Journal
                                            {
                                                VouvherNo = Journalvoucher,
                                                VoucherDate = UpdateSalesReturnOrder.OrderDate,
                                                Fk_LedgerGroupId = MappingLedgerGroup.CurrentliabilitiesAndProvisions,
                                                Fk_LedgerId = MappingLedgers.SundryCreditors,
                                                Fk_SubLedgerId = UpdateSalesReturnOrder.Fk_SubLedgerId,
                                                Fk_BranchId = BranchId,
                                                Fk_FinancialYearId = FinancialYear,
                                                TransactionNo = UpdateSalesReturnOrder.TransactionNo,
                                                Narration = UpdateSalesReturnOrder.TransactionNo.ToString(),
                                                Amount = newSalesReturnTransaction.Amount,
                                                DrCr = "Cr"
                                            };
                                            await _appDbContext.Journals.AddAsync(NewJournalTransaction);
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Add Sales Trn
                                        var newSalesReturnTransaction = new SalesReturnTransaction
                                        {
                                            Fk_SalesReturnOrderId = data.SalesRetunOrderId,
                                            TransactionNo = data.TransactionNo,
                                            TransactionType = data.TransactionType,
                                            Fk_ProductId = Guid.Parse(item[1]),
                                            Fk_BranchId = BranchId,
                                            Fk_FinancialYearId = FinancialYear,
                                            AlternateQuantity = Convert.ToDecimal(item[2]),
                                            UnitQuantity = Convert.ToDecimal(item[4]),
                                            Rate = Convert.ToDecimal(item[5]),
                                            Discount = Convert.ToDecimal(item[6]),
                                            DiscountAmount = Convert.ToDecimal(item[7]),
                                            Gst = Convert.ToDecimal(item[8]),
                                            GstAmount = Convert.ToDecimal(item[9]),
                                            Amount = Convert.ToDecimal(item[10])
                                        };
                                        if (!string.IsNullOrEmpty(item[3]))
                                        {
                                            newSalesReturnTransaction.Fk_AlternateUnitId = Guid.Parse(item[3]);
                                        }
                                        else
                                        {
                                            newSalesReturnTransaction.Fk_AlternateUnitId = null;
                                        }
                                        await _appDbContext.SalesReturnTransactions.AddAsync(newSalesReturnTransaction);
                                        await _appDbContext.SaveChangesAsync();
                                        #endregion
                                        #region Update Stock
                                        var getFinishedGoodSalesConfig = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == Guid.Parse(item[1])).ToListAsync();
                                        if (getFinishedGoodSalesConfig.Count > 0)
                                        {
                                            foreach (var item1 in getFinishedGoodSalesConfig)
                                            {
                                                var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item1.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                                if (UpdateFhinishedGoodStock != null)
                                                {
                                                    var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                                    if (Unit != null)
                                                    {
                                                        UpdateFhinishedGoodStock.AvilableStock += (newSalesReturnTransaction.AlternateQuantity * Unit.UnitQuantity * item1.Quantity);
                                                        await _appDbContext.SaveChangesAsync();
                                                    }
                                                    else
                                                    {
                                                        UpdateFhinishedGoodStock.AvilableStock += (newSalesReturnTransaction.AlternateQuantity * item1.Quantity);
                                                        await _appDbContext.SaveChangesAsync();
                                                    }
                                                }
                                                else
                                                {
                                                    var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                                    var AddNewFhinishedGoodStock = new Stock
                                                    {
                                                        Fk_BranchId = BranchId,
                                                        Fk_ProductId = item1.Fk_SubFinishedGoodId,
                                                        Fk_FinancialYear = FinancialYear,
                                                        AvilableStock = Unit != null ? +(newSalesReturnTransaction.AlternateQuantity * Unit.UnitQuantity * item1.Quantity) : +(newSalesReturnTransaction.AlternateQuantity * item1.Quantity)
                                                    };
                                                    await _appDbContext.Stocks.AddAsync(AddNewFhinishedGoodStock);
                                                    await _appDbContext.SaveChangesAsync();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newSalesReturnTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateStock != null)
                                            {
                                                UpdateStock.AvilableStock += newSalesReturnTransaction.UnitQuantity;
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                var AddNewStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_ProductId = newSalesReturnTransaction.Fk_ProductId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    AvilableStock = +newSalesReturnTransaction.UnitQuantity
                                                };
                                                await _appDbContext.Stocks.AddAsync(AddNewStock);
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                        }
                                        #endregion
                                        #region Create Journal
                                        if (data.TransactionType == "credit")
                                        {
                                            var JournalVoucherNo = await _appDbContext.Journals.Where(s => s.Fk_BranchId == BranchId).Select(s => new { s.VouvherNo }).OrderByDescending(s => s.VouvherNo).FirstOrDefaultAsync();
                                            if (JournalVoucherNo != null)
                                            {
                                                if (int.TryParse(JournalVoucherNo.VouvherNo.AsSpan(2), out int currentId))
                                                {
                                                    currentId++;
                                                    Journalvoucher = $"JN{currentId:D6}";
                                                }
                                            }
                                            else
                                            {
                                                Journalvoucher = "JN000001";
                                            }
                                            var NewJournalTransaction = new Journal
                                            {
                                                VouvherNo = Journalvoucher,
                                                VoucherDate = UpdateSalesReturnOrder.OrderDate,
                                                Fk_LedgerGroupId = MappingLedgerGroup.CurrentliabilitiesAndProvisions,
                                                Fk_LedgerId = MappingLedgers.SundryCreditors,
                                                Fk_SubLedgerId = UpdateSalesReturnOrder.Fk_SubLedgerId,
                                                Fk_BranchId = BranchId,
                                                Fk_FinancialYearId = FinancialYear,
                                                TransactionNo = UpdateSalesReturnOrder.TransactionNo,
                                                Narration = UpdateSalesReturnOrder.TransactionNo.ToString(),
                                                Amount = newSalesReturnTransaction.Amount,
                                                DrCr = "Cr"
                                            };
                                            await _appDbContext.Journals.AddAsync(NewJournalTransaction);
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        #endregion

                                    }
                                }
                            }
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                            transaction.Commit();
                            _Result.IsSuccess = true;
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    _Result.Exception = ex;
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/UpdatePurchase : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteSalesReturn(Guid Id, IDbContextTransaction transaction, bool IsCallback)
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
                        //*****************************Delete SalesTransactions**************************************//
                        var deleteSalesReturnTransaction = await _appDbContext.SalesReturnTransactions.Where(s => s.Fk_SalesReturnOrderId == Id && s.Fk_BranchId == BranchId).ToListAsync();
                        if (deleteSalesReturnTransaction.Count > 0)
                        {
                            foreach (var item in deleteSalesReturnTransaction)
                            {
                                #region Stock

                                var getFinishedGoodSalesConfig = await _appDbContext.SalesConfigs.Where(s => s.Fk_FinishedGoodId == item.Fk_ProductId).ToListAsync();
                                if (getFinishedGoodSalesConfig.Count > 0)
                                {
                                    foreach (var item1 in getFinishedGoodSalesConfig)
                                    {
                                        var UpdateFhinishedGoodStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item1.Fk_SubFinishedGoodId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (UpdateFhinishedGoodStock != null)
                                        {
                                            var Unit = _appDbContext.AlternateUnits.Where(s => s.FK_ProductId == item1.Fk_SubFinishedGoodId).FirstOrDefault();
                                            UpdateFhinishedGoodStock.AvilableStock -= Unit != null ? (item.AlternateQuantity * Unit.UnitQuantity * item1.Quantity) : item.AlternateQuantity * item1.Quantity;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                    }
                                }
                                else
                                {
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateStock != null)
                                    {
                                        UpdateStock.AvilableStock -= item.UnitQuantity;
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                }
                                #endregion
                                _appDbContext.SalesReturnTransactions.Remove(item);
                                await _appDbContext.SaveChangesAsync();
                            }
                        }
                        //*******************************Delete PurchaseOrders***************************************//

                        var deleteSalesReturnsOrders = await _appDbContext.SalesReturnOrders.SingleOrDefaultAsync(x => x.SalesReturnOrderId == Id && x.Fk_BranchId == BranchId);
                        if (deleteSalesReturnsOrders != null)
                        {
                            if (deleteSalesReturnsOrders.TransactionType == "cash")
                            {
                                #region Receipts
                                var DeleteReceipts = await _appDbContext.Receipts.Where(s => s.TransactionNo == deleteSalesReturnsOrders.TransactionNo && s.Fk_BranchId == BranchId).ToListAsync();
                                _appDbContext.Receipts.RemoveRange(DeleteReceipts);
                                await _appDbContext.SaveChangesAsync();
                                #endregion
                                #region LedgerBalance & Subledger
                                //Cash A/c ---------------Cr
                                var updateCashldgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.CashAccount && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                if (updateCashldgerBalance != null)
                                {
                                    updateCashldgerBalance.RunningBalance -= deleteSalesReturnsOrders.GrandTotal;
                                    updateCashldgerBalance.RunningBalanceType = updateCashldgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                //SalesReturn A/c---------Dr
                                var updateSalesReturnldgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.PurchaseReturnAccount && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                if (updateSalesReturnldgerBalance != null)
                                {
                                    updateSalesReturnldgerBalance.RunningBalance += deleteSalesReturnsOrders.GrandTotal;
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                            }
                            else
                            {
                                #region Journal
                                var DeleteJournal = await _appDbContext.Journals.Where(s => s.TransactionNo == deleteSalesReturnsOrders.TransactionNo && s.Fk_BranchId == BranchId).ToListAsync();
                                _appDbContext.Journals.RemoveRange(DeleteJournal);
                                await _appDbContext.SaveChangesAsync();
                                #endregion
                                #region LedgerBalance & Subledger
                                //@`SundryDebtors A/c -----------Cr
                                var updateSundryDebtorsSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == deleteSalesReturnsOrders.Fk_SubLedgerId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                if (updateSundryDebtorsSubledgerBalance != null)
                                {
                                    updateSundryDebtorsSubledgerBalance.RunningBalance -= deleteSalesReturnsOrders.GrandTotal;
                                    updateSundryDebtorsSubledgerBalance.RunningBalanceType = updateSundryDebtorsSubledgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    var updateSundryDebtorsLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.LedgerBalanceId == updateSundryDebtorsSubledgerBalance.Fk_LedgerBalanceId && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                    if (updateSundryDebtorsLedgerBalance != null)
                                    {
                                        updateSundryDebtorsLedgerBalance.RunningBalance -= deleteSalesReturnsOrders.GrandTotal;
                                        updateSundryDebtorsSubledgerBalance.RunningBalanceType = updateSundryDebtorsLedgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    }
                                    await _appDbContext.SaveChangesAsync();
                                }
                                //@SalesReturn A/c ------------ Dr
                                var updateSalesReturnldgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.PurchaseReturnAccount && s.Fk_BranchId == BranchId).SingleOrDefaultAsync();
                                if (updateSalesReturnldgerBalance != null)
                                {
                                    updateSalesReturnldgerBalance.RunningBalance += deleteSalesReturnsOrders.GrandTotal;
                                    updateSalesReturnldgerBalance.RunningBalanceType = updateSalesReturnldgerBalance.RunningBalance > 0 ? "Dr" : "Cr";
                                    await _appDbContext.SaveChangesAsync();
                                }
                                #endregion
                            }

                            _appDbContext.SalesReturnOrders.Remove(deleteSalesReturnsOrders);
                            await _appDbContext.SaveChangesAsync();
                        }

                        _Result.IsSuccess = true;
                        if (IsCallback == false) localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/DeletetPurchaseReturn : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #endregion
        #region Damage Transaction
        public async Task<Result<string>> GetLastDamageEntry()
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var lastDamageEntry = await _appDbContext.DamageOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).OrderByDescending(s => s.TransactionNo).Select(s => new { s.TransactionNo }).FirstOrDefaultAsync();
                if (lastDamageEntry != null)
                {
                    var lastTransactionNo = lastDamageEntry.TransactionNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastTransactionNo.AsSpan(3), out int currentId))
                    {
                        currentId++;
                        var newTransactionNo = $"DM{currentId:D6}";
                        _Result.SingleObjData = newTransactionNo;
                    }
                }
                else
                {
                    _Result.SingleObjData = "DM000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetLastDamageEntry : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<DamageOrderModel>> GetDamages()
        {
            Result<DamageOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.DamageOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new DamageOrderModel
                {
                    DamageOrderId = s.DamageOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    TotalAmount = s.TotalAmount,
                    Reason = s.Reason,
                    ProductType = s.ProductType != null ? new ProductTypeModel { Product_Type = s.ProductType.Product_Type } : null,
                    Labour = s.Labour != null ? new LabourModel { LabourName = s.Labour.LabourName } : null
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    var OrderList = Query;
                    _Result.CollectionObjData = OrderList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetDamages : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<DamageOrderModel>> GetDamageById(Guid Id)
        {
            Result<DamageOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.DamageOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.DamageOrderId == Id).Select(s => new DamageOrderModel
                {
                    DamageOrderId = s.DamageOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    Fk_ProductTypeId = s.Fk_ProductTypeId,
                    Fk_LabourId = s.Fk_LabourId,
                    Reason = s.Reason,
                    TotalAmount = s.TotalAmount,
                    ProductTypeName = _appDbContext.ProductTypes.Where(p => p.ProductTypeId == s.Fk_ProductTypeId).Select(b => b.Product_Type).FirstOrDefault(),
                    DamageTransactions = s.DamageTransactions != null ?
                    s.DamageTransactions.Where(x => x.Fk_FinancialYearId == FinancialYear && s.Fk_BranchId == BranchId).Select(t => new DamageTransactionModel
                    {
                        DamageTransactionId = t.DamageTransactionId,
                        Fk_ProductId = t.Fk_ProductId,
                        Quantity = t.Quantity,
                        Rate = t.Rate,
                        Amount = t.Amount,
                    }).ToList() : null,
                }).SingleOrDefaultAsync();
                if (Query != null)
                {
                    var Order = Query;
                    _Result.SingleObjData = Order;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetDamageById : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateDamage(DamageRequestData data)
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
                    if (DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTrnsactionDate))
                    {
                        #region Damage Order
                        var newDamageOrder = new DamageOrder
                        {
                            TransactionDate = convertedTrnsactionDate,
                            TransactionNo = data.TransactionNo,
                            Fk_ProductTypeId = data.Fk_ProductTypeId,
                            TotalAmount = data.TotalAmount,
                            Fk_LabourId = data.Fk_LabourId,
                            Reason = data.Reason,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear,
                        };
                        await _appDbContext.DamageOrders.AddAsync(newDamageOrder);
                        await _appDbContext.SaveChangesAsync();
                        #endregion
                        #region Update Ledger and SubLedger Balance
                        if (data.Fk_LabourId != null)
                        {
                            // @Labour A/c ------------ Dr
                            var LedgerBalanceId = Guid.Empty;
                            var updateLabourLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updateLabourLedgerBalance != null)
                            {
                                updateLabourLedgerBalance.RunningBalance += data.TotalAmount;
                                updateLabourLedgerBalance.RunningBalanceType = (updateLabourLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                                LedgerBalanceId = updateLabourLedgerBalance.LedgerBalanceId;
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.LabourAccount,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Dr",
                                    RunningBalance = data.TotalAmount,
                                    RunningBalanceType = "Dr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                                LedgerBalanceId = newLedgerBalance.LedgerBalanceId;
                            }
                            var SubledgerId = await _appDbContext.Labours.Where(s => s.LabourId == data.Fk_LabourId && s.Fk_BranchId == BranchId).Select(s => s.Fk_SubLedgerId).SingleOrDefaultAsync();
                            var updateLabourSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == SubledgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                            if (updateLabourSubledgerBalance != null)
                            {
                                updateLabourSubledgerBalance.RunningBalance += data.TotalAmount;
                                updateLabourSubledgerBalance.RunningBalanceType = (updateLabourSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newSubLedgerBalance = new SubLedgerBalance
                                {
                                    Fk_LedgerBalanceId = LedgerBalanceId,
                                    Fk_SubLedgerId = SubledgerId,
                                    OpeningBalanceType = "Dr",
                                    OpeningBalance = 0,
                                    RunningBalanceType = "Dr",
                                    RunningBalance = data.TotalAmount,
                                    Fk_FinancialYearId = FinancialYear,
                                    Fk_BranchId = BranchId
                                };
                                await _appDbContext.SubLedgerBalances.AddAsync(newSubLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                            }
                            // @ LabourCharges A/c --------- Cr
                            var updateLabourChargesledgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourCharges && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (updateLabourChargesledgerBalance != null)
                            {
                                updateLabourChargesledgerBalance.RunningBalance -= data.TotalAmount;
                                updateLabourChargesledgerBalance.RunningBalanceType = (updateLabourChargesledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var newLedgerBalance = new LedgerBalance
                                {
                                    Fk_LedgerId = MappingLedgers.LabourCharges,
                                    OpeningBalance = 0,
                                    OpeningBalanceType = "Cr",
                                    RunningBalance = -data.TotalAmount,
                                    RunningBalanceType = "Cr",
                                    Fk_BranchId = BranchId,
                                    Fk_FinancialYear = FinancialYear
                                };
                                await _appDbContext.LedgerBalances.AddAsync(newLedgerBalance);
                                await _appDbContext.SaveChangesAsync();
                            }
                        }
                        #endregion
                        foreach (var item in data.RowData)
                        {
                            # region Damage Transaction
                            var newDamageTransaction = new DamageTransaction
                            {
                                Fk_DamageOrderId = newDamageOrder.DamageOrderId,
                                TransactionDate = convertedTrnsactionDate,
                                TransactionNo = data.TransactionNo,
                                Fk_ProductId = Guid.Parse(item[1]),
                                Fk_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                Quantity = Convert.ToDecimal(item[2]),
                                Rate = Convert.ToDecimal(item[3]),
                                Amount = Convert.ToDecimal(item[4])
                            };
                            await _appDbContext.DamageTransactions.AddAsync(newDamageTransaction);
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                            #region update Stock
                            var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newDamageTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (UpdateStock != null)
                            {
                                UpdateStock.AvilableStock -= newDamageTransaction.Quantity;
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var AddNewStock = new Stock
                                {
                                    Fk_BranchId = BranchId,
                                    Fk_ProductId = newDamageTransaction.Fk_ProductId,
                                    Fk_FinancialYear = FinancialYear,
                                    AvilableStock = -newDamageTransaction.Quantity,
                                };
                                await _appDbContext.Stocks.AddAsync(AddNewStock);
                                await _appDbContext.SaveChangesAsync();
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/CreateDamage : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateDamage(DamageRequestData data)
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
                    if (DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTrnsactionDate))
                    {

                        var UpdateDamageOrder = await _appDbContext.DamageOrders.Where(s => s.DamageOrderId == data.DamageId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                        if (UpdateDamageOrder != null)
                        {
                            #region Damage Order
                            UpdateDamageOrder.TransactionDate = convertedTrnsactionDate;
                            UpdateDamageOrder.Fk_LabourId = data.Fk_LabourId;
                            UpdateDamageOrder.Fk_ProductTypeId = data.Fk_ProductTypeId;
                            UpdateDamageOrder.Reason = data.Reason;
                            UpdateDamageOrder.TotalAmount = data.TotalAmount;
                            await _appDbContext.SaveChangesAsync();
                            #endregion
                            #region Update Ledger and SubLedger Balance
                            if (UpdateDamageOrder.Fk_LabourId != null)
                            {
                                if (UpdateDamageOrder.Fk_LabourId == data.Fk_LabourId)
                                {
                                    var difference = data.TotalAmount - UpdateDamageOrder.TotalAmount;
                                    // @Labour A/c ------------ Dr

                                    var SubledgerId = await _appDbContext.Labours.Where(s => s.LabourId == data.Fk_LabourId && s.Fk_BranchId == BranchId).Select(s => s.Fk_SubLedgerId).SingleOrDefaultAsync();
                                    var updateLabourSubledgerBalance = await _appDbContext.SubLedgerBalances.Where(s => s.Fk_SubLedgerId == SubledgerId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                    if (updateLabourSubledgerBalance != null)
                                    {
                                        updateLabourSubledgerBalance.RunningBalance += difference;
                                        updateLabourSubledgerBalance.RunningBalanceType = (updateLabourSubledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        var updateLabourLedgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourAccount && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                        if (updateLabourLedgerBalance != null)
                                        {
                                            updateLabourLedgerBalance.RunningBalance += difference;
                                            updateLabourLedgerBalance.RunningBalanceType = (updateLabourLedgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        }
                                        else
                                        {
                                            _Result.WarningMessage = "Ledger Balance Not Exist";
                                            return _Result;
                                        }
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "SubLedger Balance Not Exist";
                                        return _Result;
                                    }
                                    // @ LabourCharges A/c --------- Cr
                                    var updateLabourChargesledgerBalance = await _appDbContext.LedgerBalances.Where(s => s.Fk_LedgerId == MappingLedgers.LabourCharges && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (updateLabourChargesledgerBalance != null)
                                    {
                                        updateLabourChargesledgerBalance.RunningBalance -= difference;
                                        updateLabourChargesledgerBalance.RunningBalanceType = (updateLabourChargesledgerBalance.RunningBalance >= 0) ? "Dr" : "Cr";
                                        await _appDbContext.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Ledger Balance Not Exist";
                                        return _Result;
                                    }
                                }
                                if (UpdateDamageOrder.Fk_LabourId != data.Fk_LabourId)
                                {

                                }
                            }
                            else
                            {
                                if (data.Fk_LabourId != null) { }

                            }

                            #endregion

                            foreach (var item in data.RowData)
                            {
                                Guid DamageId = Guid.TryParse(item[0], out var parsedGuid) ? parsedGuid : Guid.Empty;
                                var UpdateDamageTransaction = await _appDbContext.DamageTransactions.Where(s => s.DamageTransactionId == DamageId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (UpdateDamageTransaction != null)
                                {
                                    #region Update Stock
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == UpdateDamageTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateStock != null)
                                    {
                                        if (UpdateDamageTransaction.Fk_ProductId != Guid.Parse(item[1]))
                                        {
                                            UpdateStock.AvilableStock += UpdateDamageTransaction.Quantity;
                                            await _appDbContext.SaveChangesAsync();
                                            var UpdateNewStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == Guid.Parse(item[1]) && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateNewStock != null)
                                            {
                                                UpdateNewStock.AvilableStock -= UpdateDamageTransaction.Quantity;
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                _Result.WarningMessage = "Stock Not Avilable";
                                                return _Result;
                                            }
                                        }
                                        if (UpdateDamageTransaction.Quantity != Convert.ToDecimal(item[2]))
                                        {
                                            var quantityDifference = UpdateDamageTransaction.Quantity - Convert.ToDecimal(item[2]);
                                            UpdateStock.AvilableStock += quantityDifference;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                    }
                                    #endregion
                                    #region Update Damage Transaction
                                    UpdateDamageTransaction.Fk_ProductId = Guid.Parse(item[1]);
                                    UpdateDamageTransaction.Quantity = Convert.ToDecimal(item[2]);
                                    UpdateDamageTransaction.Rate = Convert.ToDecimal(item[3]);
                                    UpdateDamageTransaction.Amount = Convert.ToDecimal(item[4]);
                                    await _appDbContext.SaveChangesAsync();
                                    #endregion
                                }
                                else
                                {
                                    #region Damage Transaction
                                    var newDamageTransaction = new DamageTransaction
                                    {
                                        Fk_DamageOrderId = UpdateDamageOrder.DamageOrderId,
                                        TransactionNo = data.TransactionNo,
                                        TransactionDate = convertedTrnsactionDate,
                                        Fk_ProductId = Guid.Parse(item[1]),
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        Quantity = Convert.ToDecimal(item[2]),
                                        Rate = Convert.ToDecimal(item[3]),
                                        Amount = Convert.ToDecimal(item[4]),
                                    };
                                    await _appDbContext.DamageTransactions.AddAsync(newDamageTransaction);
                                    await _appDbContext.SaveChangesAsync();
                                    #endregion
                                    #region Update Stock
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newDamageTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateStock != null)
                                    {
                                        UpdateStock.AvilableStock -= newDamageTransaction.Quantity;
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Stock Not Avilable";
                                        return _Result;
                                    }
                                    await _appDbContext.SaveChangesAsync();
                                    #endregion
                                }
                            }
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                            transaction.Commit();
                            _Result.IsSuccess = true;
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    _Result.Exception = ex;
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/UpdateDamage : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteDamage(Guid Id, IDbContextTransaction transaction, bool IsCallback)
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
                        //*****************************Delete Damage Transaction**************************************//
                        var deleteDamageTransaction = await _appDbContext.DamageTransactions.Where(s => s.Fk_DamageOrderId == Id && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                        if (deleteDamageTransaction.Count > 0)
                        {
                            foreach (var item in deleteDamageTransaction)
                            {
                                var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (UpdateStock != null)
                                {
                                    UpdateStock.AvilableStock += item.Quantity;
                                    await _appDbContext.SaveChangesAsync();
                                }
                                _appDbContext.DamageTransactions.Remove(item);
                                await _appDbContext.SaveChangesAsync();

                            }
                        }
                        //*******************************Delete Damage Orders***************************************//
                        var deleteDamageOrders = await _appDbContext.DamageOrders.SingleOrDefaultAsync(x => x.DamageOrderId == Id && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYear);
                        if (deleteDamageOrders != null)
                        {
                            _appDbContext.DamageOrders.Remove(deleteDamageOrders);
                            await _appDbContext.SaveChangesAsync();
                        }
                        _Result.IsSuccess = true;
                        if (IsCallback == false) localTransaction.Commit();
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/DeleteDamage : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Inward Supply Transaction
        public async Task<Result<string>> GetLastInwardSupply()
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var lastInwardSupply = await _appDbContext.InwardSupplyOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).OrderByDescending(s => s.TransactionNo).Select(s => new { s.TransactionNo }).FirstOrDefaultAsync();
                if (lastInwardSupply != null)
                {
                    var lastTransactionNo = lastInwardSupply.TransactionNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastTransactionNo.AsSpan(3), out int currentId))
                    {
                        currentId++;
                        var newTransactionNo = $"STI{currentId:D6}";
                        _Result.SingleObjData = newTransactionNo;
                    }
                }
                else
                {
                    _Result.SingleObjData = "STI000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetLastInwardSupply : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<InwardSupplyOrderModel>> GetInwardSupply()
        {
            Result<InwardSupplyOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.InwardSupplyOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new InwardSupplyOrderModel
                {
                    InwardSupplyOrderId = s.InwardSupplyOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    TotalAmount = s.TotalAmount,
                    ProductType = s.ProductType != null ? new ProductTypeModel { Product_Type = s.ProductType.Product_Type } : null,
                    BranchName = _appDbContext.Branches.Where(b => b.BranchId == s.FromBranch).Select(b => b.BranchName).FirstOrDefault()
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    var OrderList = Query;
                    _Result.CollectionObjData = OrderList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetInwardSupply : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<InwardSupplyOrderModel>> GetInwardSupplyById(Guid Id)
        {
            Result<InwardSupplyOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.InwardSupplyOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.InwardSupplyOrderId == Id).Select(s => new InwardSupplyOrderModel
                {
                    InwardSupplyOrderId = s.InwardSupplyOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    FromBranch = s.FromBranch,
                    Fk_ProductTypeId = s.Fk_ProductTypeId,
                    ProductTypeName = _appDbContext.ProductTypes.Where(p => p.ProductTypeId == s.Fk_ProductTypeId).Select(b => b.Product_Type).FirstOrDefault(),
                    TotalAmount = s.TotalAmount,
                    InwardSupplyTransactions = s.InwardSupplyTransactions != null ?
                    s.InwardSupplyTransactions.Where(x => x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYear).Select(t => new InwardSupplyTransactionModel
                    {
                        InwardSupplyTransactionId = t.InwardSupplyTransactionId,
                        Fk_ProductId = t.Fk_ProductId,
                        Quantity = t.Quantity,
                        Rate = t.Rate,
                        Amount = t.Amount,
                    }).ToList() : null,
                }).SingleOrDefaultAsync();
                if (Query != null)
                {
                    var Order = Query;
                    _Result.SingleObjData = Order;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetInwardSupplyById : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateInwardSupply(SupplyDataRequest data)
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
                    if (DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTrnsactionDate))
                    {
                        //************************************************************InwardSupply Order*****************************************//
                        var newInwardSupplyOdr = new InwardSupplyOrder
                        {
                            FromBranch = data.Branch,
                            Fk_ProductTypeId = data.Fk_ProductTypeId,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear,
                            TransactionDate = convertedTrnsactionDate,
                            TransactionNo = data.TransactionNo,
                            TotalAmount = data.TotalAmount
                        };
                        await _appDbContext.InwardSupplyOrders.AddAsync(newInwardSupplyOdr);
                        await _appDbContext.SaveChangesAsync();
                        //*********************************InwardSupply Transaction**********************************************//
                        foreach (var item in data.RowData)
                        {
                            var newInwardSupplyTransaction = new InwardSupplyTransaction
                            {
                                Fk_InwardSupplyOrderId = newInwardSupplyOdr.InwardSupplyOrderId,
                                TransactionDate = convertedTrnsactionDate,
                                TransactionNo = data.TransactionNo,
                                Fk_ProductId = Guid.Parse(item[1]),
                                Fk_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                Quantity = Convert.ToDecimal(item[2]),
                                Rate = Convert.ToDecimal(item[3]),
                                Amount = Convert.ToDecimal(item[4])
                            };
                            await _appDbContext.InwardSupplyTransactions.AddAsync(newInwardSupplyTransaction);
                            await _appDbContext.SaveChangesAsync();

                            //update Stock
                            var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newInwardSupplyTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (UpdateStock != null)
                            {
                                UpdateStock.AvilableStock += newInwardSupplyTransaction.Quantity;
                            }
                            else
                            {
                                var AddNewStock = new Stock
                                {
                                    Fk_BranchId = BranchId,
                                    Fk_ProductId = newInwardSupplyTransaction.Fk_ProductId,
                                    Fk_FinancialYear = FinancialYear,
                                    AvilableStock = newInwardSupplyTransaction.Quantity
                                };
                                await _appDbContext.Stocks.AddAsync(AddNewStock);
                            }
                            await _appDbContext.SaveChangesAsync();
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/CreateInwardSupply : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateInwardSupply(SupplyDataRequest data)
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
                    if (DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTrnsactionDate))
                    {
                        //************************************************************Inward Supply Order*************************************************************//
                        var UpdateInwardSupplyOrder = await _appDbContext.InwardSupplyOrders.Where(s => s.InwardSupplyOrderId == data.SupplyId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                        if (UpdateInwardSupplyOrder != null)
                        {

                            UpdateInwardSupplyOrder.TransactionDate = convertedTrnsactionDate;
                            UpdateInwardSupplyOrder.FromBranch = data.Branch;
                            UpdateInwardSupplyOrder.Fk_ProductTypeId = data.Fk_ProductTypeId;
                            UpdateInwardSupplyOrder.TotalAmount = data.TotalAmount;
                            await _appDbContext.SaveChangesAsync();
                            //************************************************************Inward Supply Transaction**************************************************//
                            foreach (var item in data.RowData)
                            {
                                Guid InwardSupplyId = Guid.TryParse(item[0], out var parsedGuid) ? parsedGuid : Guid.Empty;
                                var UpdateInwardSupplyTransaction = await _appDbContext.InwardSupplyTransactions.Where(s => s.InwardSupplyTransactionId == InwardSupplyId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (UpdateInwardSupplyTransaction != null)
                                {
                                    //Update Stock
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == UpdateInwardSupplyTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateStock != null)
                                    {
                                        if (UpdateInwardSupplyTransaction.Fk_ProductId != Guid.Parse(item[1]))
                                        {
                                            UpdateStock.AvilableStock -= UpdateInwardSupplyTransaction.Quantity;
                                            await _appDbContext.SaveChangesAsync();
                                            var UpdateNewStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == Guid.Parse(item[1]) && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateNewStock != null)
                                            {
                                                UpdateNewStock.AvilableStock += UpdateInwardSupplyTransaction.Quantity;
                                            }
                                            else
                                            {
                                                var newStock = new Stock
                                                {
                                                    Fk_BranchId = BranchId,
                                                    Fk_FinancialYear = FinancialYear,
                                                    Fk_ProductId = Guid.Parse(item[1]),
                                                    AvilableStock = Convert.ToDecimal(item[2])
                                                };
                                                await _appDbContext.Stocks.AddAsync(newStock);
                                            }
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                        if (UpdateInwardSupplyTransaction.Quantity != Convert.ToDecimal(item[2]))
                                        {
                                            var quantityDifference = UpdateInwardSupplyTransaction.Quantity - Convert.ToDecimal(item[2]);
                                            UpdateStock.AvilableStock -= quantityDifference;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                    }
                                    UpdateInwardSupplyTransaction.Fk_ProductId = Guid.Parse(item[1]);
                                    UpdateInwardSupplyTransaction.Quantity = Convert.ToDecimal(item[2]);
                                    UpdateInwardSupplyTransaction.Rate = Convert.ToDecimal(item[3]);
                                    UpdateInwardSupplyTransaction.Amount = Convert.ToDecimal(item[4]);
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    var newInwardSupplyTransaction = new InwardSupplyTransaction
                                    {
                                        Fk_InwardSupplyOrderId = data.SupplyId,
                                        TransactionNo = data.TransactionNo,
                                        TransactionDate = convertedTrnsactionDate,
                                        Fk_ProductId = Guid.Parse(item[1]),
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        Quantity = Convert.ToDecimal(item[2]),
                                        Rate = Convert.ToDecimal(item[3]),
                                        Amount = Convert.ToDecimal(item[4]),
                                    };
                                    await _appDbContext.InwardSupplyTransactions.AddAsync(newInwardSupplyTransaction);
                                    await _appDbContext.SaveChangesAsync();

                                    //Update Stock
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newInwardSupplyTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateStock != null)
                                    {
                                        UpdateStock.AvilableStock += newInwardSupplyTransaction.Quantity;
                                    }
                                    else
                                    {
                                        var AddNewStock = new Stock
                                        {
                                            Fk_BranchId = BranchId,
                                            Fk_ProductId = newInwardSupplyTransaction.Fk_ProductId,
                                            Fk_FinancialYear = FinancialYear,
                                            AvilableStock = newInwardSupplyTransaction.Quantity
                                        };
                                        await _appDbContext.Stocks.AddAsync(AddNewStock);
                                    }
                                    await _appDbContext.SaveChangesAsync();
                                }
                            }
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                            transaction.Commit();
                            _Result.IsSuccess = true;
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    _Result.Exception = ex;
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/UpdateInwardSupply : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteInwardSupply(Guid Id, IDbContextTransaction transaction, bool IsCallback)
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
                        //*****************************Delete InwardSupply Transaction**************************************//
                        var deleteInwardSupplyTransaction = await _appDbContext.InwardSupplyTransactions.Where(s => s.Fk_InwardSupplyOrderId == Id && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                        if (deleteInwardSupplyTransaction.Count > 0)
                        {
                            foreach (var item in deleteInwardSupplyTransaction)
                            {
                                //Update Stock
                                var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (UpdateStock != null)
                                {
                                    UpdateStock.AvilableStock -= item.Quantity;
                                    await _appDbContext.SaveChangesAsync();
                                }
                                _appDbContext.InwardSupplyTransactions.Remove(item);
                                await _appDbContext.SaveChangesAsync();
                            }
                        }
                        //*******************************Delete InwardSupply Orders***************************************//
                        var deleteInwardSupplyOrders = await _appDbContext.InwardSupplyOrders.SingleOrDefaultAsync(x => x.InwardSupplyOrderId == Id && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYear);
                        if (deleteInwardSupplyOrders != null)
                        {
                            _appDbContext.InwardSupplyOrders.Remove(deleteInwardSupplyOrders);
                            await _appDbContext.SaveChangesAsync();
                        }
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/DeleteInwardSupply : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Outward Supply Transaction
        public async Task<Result<string>> GetLastOutwardSupply()
        {
            Result<string> _Result = new();
            try
            {
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                _Result.IsSuccess = false;
                var lastOutwardSupply = await _appDbContext.OutwardSupplyOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).OrderByDescending(s => s.TransactionNo).Select(s => new { s.TransactionNo }).FirstOrDefaultAsync();
                if (lastOutwardSupply != null)
                {
                    var lastTransactionNo = lastOutwardSupply.TransactionNo;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    if (int.TryParse(lastTransactionNo.AsSpan(3), out int currentId))
                    {
                        currentId++;
                        var newTransactionNo = $"STO{currentId:D6}";
                        _Result.SingleObjData = newTransactionNo;
                    }
                }
                else
                {
                    _Result.SingleObjData = "STO000001";
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetLastOutwardSupply : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<OutwardSupplyOrderModel>> GetOutwardSupply()
        {
            Result<OutwardSupplyOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.OutwardSupplyOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s => new OutwardSupplyOrderModel
                {
                    OutwardSupplyOrderId = s.OutwardSupplyOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    TotalAmount = s.TotalAmount,
                    ProductType = s.ProductType != null ? new ProductTypeModel { Product_Type = s.ProductType.Product_Type } : null,
                    BranchName = _appDbContext.Branches.Where(b => b.BranchId == s.ToBranch).Select(b => b.BranchName).FirstOrDefault()
                }).ToListAsync();
                if (Query.Count > 0)
                {
                    var OrderList = Query;
                    _Result.CollectionObjData = OrderList;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetOutwardSupply : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<OutwardSupplyOrderModel>> GetOutwardSupplyById(Guid Id)
        {
            Result<OutwardSupplyOrderModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                var Query = await _appDbContext.OutwardSupplyOrders.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear && s.OutwardSupplyOrderId == Id).Select(s => new OutwardSupplyOrderModel
                {
                    OutwardSupplyOrderId = s.OutwardSupplyOrderId,
                    TransactionNo = s.TransactionNo,
                    TransactionDate = s.TransactionDate,
                    ToBranch = s.ToBranch,
                    Fk_ProductTypeId = s.Fk_ProductTypeId,
                    ProductTypeName = _appDbContext.ProductTypes.Where(p => p.ProductTypeId == s.Fk_ProductTypeId).Select(b => b.Product_Type).SingleOrDefault(),
                    TotalAmount = s.TotalAmount,
                    OutwardSupplyTransactions = s.OutwardSupplyTransactions != null ?
                    s.OutwardSupplyTransactions.Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(t => new OutwardSupplyTransactionModel
                    {
                        OutwardSupplyTransactionId = t.OutwardSupplyTransactionId,
                        Fk_ProductId = t.Fk_ProductId,
                        Quantity = t.Quantity,
                        Rate = t.Rate,
                        Amount = t.Amount,
                    }).ToList() : null,
                }).SingleOrDefaultAsync();
                if (Query != null)
                {
                    var Order = Query;
                    _Result.SingleObjData = Order;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/GetOutwardSupplyById : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> CreateOutwardSupply(SupplyDataRequest data)
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
                    if (DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTrnsactionDate))
                    {
                        //************************************************************InwardSupply Order*****************************************//
                        var newOutwardSupplyOrder = new OutwardSupplyOrder
                        {
                            ToBranch = data.Branch,
                            Fk_ProductTypeId = data.Fk_ProductTypeId,
                            Fk_BranchId = BranchId,
                            Fk_FinancialYearId = FinancialYear,
                            TransactionDate = convertedTrnsactionDate,
                            TransactionNo = data.TransactionNo,
                            TotalAmount = data.TotalAmount
                        };
                        await _appDbContext.OutwardSupplyOrders.AddAsync(newOutwardSupplyOrder);
                        await _appDbContext.SaveChangesAsync();
                        //*********************************InwardSupply Transaction**********************************************//
                        foreach (var item in data.RowData)
                        {
                            var newOutwardSupplyTransactionn = new OutwardSupplyTransaction
                            {
                                Fk_OutwardSupplyOrderId = newOutwardSupplyOrder.OutwardSupplyOrderId,
                                TransactionDate = convertedTrnsactionDate,
                                TransactionNo = data.TransactionNo,
                                Fk_ProductId = Guid.Parse(item[1]),
                                Fk_BranchId = BranchId,
                                Fk_FinancialYearId = FinancialYear,
                                Quantity = Convert.ToDecimal(item[2]),
                                Rate = Convert.ToDecimal(item[3]),
                                Amount = Convert.ToDecimal(item[4])
                            };
                            await _appDbContext.OutwardSupplyTransactions.AddAsync(newOutwardSupplyTransactionn);
                            await _appDbContext.SaveChangesAsync();

                            //update Stock
                            var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newOutwardSupplyTransactionn.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                            if (UpdateStock != null)
                            {
                                UpdateStock.AvilableStock -= newOutwardSupplyTransactionn.Quantity;
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                _Result.WarningMessage = "Stock Not Avilable";
                                return _Result;
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/CreateOutwardSupply : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> UpdateOutwardSupply(SupplyDataRequest data)
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
                    if (DateTime.TryParseExact(data.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedTrnsactionDate))
                    {
                        //************************************************************Inward Supply Order*************************************************************//
                        var UpdateOutwardSupplyOrder = await _appDbContext.OutwardSupplyOrders.Where(s => s.OutwardSupplyOrderId == data.SupplyId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                        if (UpdateOutwardSupplyOrder != null)
                        {
                            UpdateOutwardSupplyOrder.TransactionDate = convertedTrnsactionDate;
                            UpdateOutwardSupplyOrder.ToBranch = data.Branch;
                            UpdateOutwardSupplyOrder.Fk_ProductTypeId = data.Fk_ProductTypeId;
                            UpdateOutwardSupplyOrder.TotalAmount = data.TotalAmount;
                            await _appDbContext.SaveChangesAsync();
                            //************************************************************Inward Supply Transaction**************************************************//
                            foreach (var item in data.RowData)
                            {
                                Guid OutwardSupplyId = Guid.TryParse(item[0], out var parsedGuid) ? parsedGuid : Guid.Empty;
                                var UpdateOutwardSupplyTransaction = await _appDbContext.OutwardSupplyTransactions.Where(s => s.OutwardSupplyTransactionId == OutwardSupplyId && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).SingleOrDefaultAsync();
                                if (UpdateOutwardSupplyTransaction != null)
                                {
                                    //Update Stock
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == UpdateOutwardSupplyTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateStock != null)
                                    {
                                        if (UpdateOutwardSupplyTransaction.Fk_ProductId != Guid.Parse(item[1]))
                                        {
                                            UpdateStock.AvilableStock += UpdateOutwardSupplyTransaction.Quantity;
                                            await _appDbContext.SaveChangesAsync();
                                            var UpdateNewStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == Guid.Parse(item[1]) && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                            if (UpdateNewStock != null)
                                            {
                                                UpdateNewStock.AvilableStock -= UpdateOutwardSupplyTransaction.Quantity;
                                                await _appDbContext.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                _Result.WarningMessage = "Stock Not Avilable";
                                                return _Result;
                                            }
                                        }
                                        if (UpdateOutwardSupplyTransaction.Quantity != Convert.ToDecimal(item[2]))
                                        {
                                            var quantityDifference = UpdateOutwardSupplyTransaction.Quantity - Convert.ToDecimal(item[2]);
                                            UpdateStock.AvilableStock += quantityDifference;
                                            await _appDbContext.SaveChangesAsync();
                                        }
                                    }
                                    UpdateOutwardSupplyTransaction.Fk_ProductId = Guid.Parse(item[1]);
                                    UpdateOutwardSupplyTransaction.Quantity = Convert.ToDecimal(item[2]);
                                    UpdateOutwardSupplyTransaction.Rate = Convert.ToDecimal(item[3]);
                                    UpdateOutwardSupplyTransaction.Amount = Convert.ToDecimal(item[4]);
                                    await _appDbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    var newOutwardSupplyTransaction = new OutwardSupplyTransaction
                                    {
                                        Fk_OutwardSupplyOrderId = data.SupplyId,
                                        TransactionNo = data.TransactionNo,
                                        TransactionDate = convertedTrnsactionDate,
                                        Fk_ProductId = Guid.Parse(item[1]),
                                        Fk_BranchId = BranchId,
                                        Fk_FinancialYearId = FinancialYear,
                                        Quantity = Convert.ToDecimal(item[2]),
                                        Rate = Convert.ToDecimal(item[3]),
                                        Amount = Convert.ToDecimal(item[4]),
                                    };
                                    await _appDbContext.OutwardSupplyTransactions.AddAsync(newOutwardSupplyTransaction);
                                    await _appDbContext.SaveChangesAsync();

                                    //Update Stock
                                    var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == newOutwardSupplyTransaction.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                    if (UpdateStock != null)
                                    {
                                        UpdateStock.AvilableStock -= newOutwardSupplyTransaction.Quantity;
                                    }
                                    else
                                    {
                                        _Result.WarningMessage = "Stock Not Avilable";
                                        return _Result;
                                    }
                                    await _appDbContext.SaveChangesAsync();
                                }
                            }
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Modified);
                            transaction.Commit();
                            _Result.IsSuccess = true;
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    _Result.Exception = ex;
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/UpdateOutwardSupply : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<bool>> DeleteOutwardSupply(Guid Id, IDbContextTransaction transaction, bool IsCallback)
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
                        //*****************************Delete OutwardSupply Transaction**************************************//
                        var deleteOutwardSupplyTransaction = await _appDbContext.OutwardSupplyTransactions.Where(s => s.Fk_OutwardSupplyOrderId == Id && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).ToListAsync();
                        if (deleteOutwardSupplyTransaction.Count > 0)
                        {
                            foreach (var item in deleteOutwardSupplyTransaction)
                            {
                                //Update Stock
                                var UpdateStock = await _appDbContext.Stocks.Where(s => s.Fk_ProductId == item.Fk_ProductId && s.Fk_BranchId == BranchId && s.Fk_FinancialYear == FinancialYear).SingleOrDefaultAsync();
                                if (UpdateStock != null)
                                {
                                    UpdateStock.AvilableStock += item.Quantity;
                                    await _appDbContext.SaveChangesAsync();
                                }
                                _appDbContext.OutwardSupplyTransactions.Remove(item);
                                await _appDbContext.SaveChangesAsync();
                            }
                        }
                        //*******************************Delete OutwardSupply Orders***************************************//
                        var deleteOutwardSupplyOrders = await _appDbContext.OutwardSupplyOrders.SingleOrDefaultAsync(x => x.OutwardSupplyOrderId == Id && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYear);
                        if (deleteOutwardSupplyOrders != null)
                        {
                            _appDbContext.OutwardSupplyOrders.Remove(deleteOutwardSupplyOrders);
                            await _appDbContext.SaveChangesAsync();
                        }
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"TransactionRepo/DeleteOutwardSupply : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
    }
}
