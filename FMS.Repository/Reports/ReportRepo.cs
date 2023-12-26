using AutoMapper;
using FMS.Api.Email.EmailService;
using FMS.Db.Context;
using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FMS.Repository.Reports
{
    public class ReportRepo : IReportRepo
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<ReportRepo> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IEmailService _emailService;
        public ReportRepo(ILogger<ReportRepo> logger, AppDbContext appDbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _mapper = mapper;
            _HttpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }
        #region Stock Report
        public async Task<Result<StockReportModel>> GetSummerizedStockReports(StockReportDataRequest requestData)
        {
            Result<StockReportModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<StockReportModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        Models = await _appDbContext.Products.Where(s => s.Fk_ProductTypeId == requestData.ProductTypeId).Select(s => new StockReportModel
                        {
                            ProductName = s.ProductName,
                            DamageQty = s.DamageTransactions != null ? s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum() : 0,
                            OutwardQty = s.OutwardSupplyTransactions != null ? s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum() : 0,
                            InwardQty = s.InwardSupplyTransactions != null ? s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum() : 0,
                            SalesQty = s.SalesTransactions != null ? s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum() : 0,
                            SalesReturnQty = s.SalesReturnTransactions != null ? s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum() : 0,
                            PurchaseQty = s.PurchaseTransactions != null ? s.PurchaseTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum() : 0,
                            PurchaseReturnQty = s.PurchaseReturnTransactions != null ? s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum() : 0,
                            ProductionEntryQty = s.ProductionEntryTransactions != null ? s.ProductionEntryTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum() : 0,
                            ProductionQty = s.ProductionEntries != null ? s.ProductionEntries.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.FK_BranchId == BranchId && d.ProductionDate >= convertedFromDate && d.ProductionDate <= convertedToDate).Select(i => i.Quantity).Sum() : 0,
                            OpeningQty =
                         (s.PurchaseTransactions != null ? s.PurchaseTransactions.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate).Sum(p => p.Quantity) : 0)
                       + (s.ProductionEntries != null ? s.ProductionEntries.Where(pe => pe.Fk_FinancialYearId == FinancialYearId && pe.FK_BranchId == BranchId && pe.ProductionDate < convertedFromDate).Sum(pe => pe.Quantity) : 0)
                       + (s.SalesReturnTransactions != null ? s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       + (s.InwardSupplyTransactions != null ? s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       - (s.PurchaseReturnTransactions != null ? s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       - (s.SalesTransactions != null ? s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       - (s.DamageTransactions != null ? s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       - (s.OutwardSupplyTransactions != null ? s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       - (s.ProductionEntryTransactions != null ? s.ProductionEntryTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                        }).ToListAsync();
                    }
                    else
                    {
                        var ListFinancialYearId = await _appDbContext.FinancialYears.Where(x => x.Financial_Year == _HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId")).Select(x => x.FinancialYearId).ToListAsync();
                        Models = await _appDbContext.Products.Where(s => s.Fk_ProductTypeId == requestData.ProductTypeId).Select(s => new StockReportModel
                        {
                            ProductName = s.ProductName,
                            DamageQty = s.DamageTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                            OutwardQty = s.OutwardSupplyTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                            InwardQty = s.InwardSupplyTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                            SalesQty = s.SalesTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                            SalesReturnQty = s.SalesReturnTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                            PurchaseQty = s.PurchaseTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                            PurchaseReturnQty = s.PurchaseReturnTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                            ProductionEntryQty = s.ProductionEntryTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                            ProductionQty = s.ProductionEntries.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.ProductionDate >= convertedFromDate && d.ProductionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                            OpeningQty =
                         s.PurchaseTransactions.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate).Select(p => p.Quantity).Sum()
                      + s.ProductionEntries.Where(pe => ListFinancialYearId.Contains(pe.Fk_FinancialYearId) && pe.ProductionDate < convertedFromDate).Select(pe => pe.Quantity).Sum()
                      + s.SalesReturnTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                      + s.InwardSupplyTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                      - s.PurchaseReturnTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                      - s.SalesTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                      - s.DamageTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                      - s.OutwardSupplyTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                      - s.ProductionEntryTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                        }).ToListAsync();
                    }
                    if (Models.Count > 0)
                    {
                        if (requestData.ZeroValued == "No")
                        {
                            _Result.CollectionObjData = Models.Where(s => s.DamageQty > 0 || s.OutwardQty > 0 || s.InwardQty > 0 || s.SalesQty > 0 || s.SalesReturnQty > 0 || s.PurchaseQty > 0 || s.PurchaseReturnQty > 0 || s.ProductionEntryQty > 0 || s.ProductionQty > 0).ToList();
                        }
                        else
                        {
                            _Result.CollectionObjData = Models;
                        }
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedStockReports : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<ProductModel>> GetDetailedStockReport(StockReportDataRequest requestData)
        {
            Result<ProductModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<ProductModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    var Query = _appDbContext.Products.Include(p => p.DamageTransactions)
                        .Include(p => p.OutwardSupplyTransactions)
                        .Include(p => p.InwardSupplyTransactions)
                        .Include(p => p.SalesTransactions)
                        .Include(p => p.SalesReturnTransactions)
                        .Include(p => p.PurchaseTransactions)
                        .Include(p => p.PurchaseReturnTransactions)
                        .Include(p => p.ProductionEntryTransactions)
                        .Include(p => p.ProductionEntries);

                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        Models = await Query.Where(p => p.Fk_ProductTypeId == requestData.ProductTypeId && p.ProductId == requestData.ProductId).Select(s => new ProductModel
                        {

                            ProductName = s.ProductName,
                            DamageTransactions = s.DamageTransactions != null ? s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(d => new DamageTransactionModel { TransactionDate = d.TransactionDate, TransactionNo = d.TransactionNo, Quantity = d.Quantity }).ToList() : null,
                            OutwardSupplyTransactions = s.OutwardSupplyTransactions != null ? s.OutwardSupplyTransactions.Where(o => o.Fk_FinancialYearId == FinancialYear && o.Fk_BranchId == BranchId && o.TransactionDate >= convertedFromDate && o.TransactionDate <= convertedToDate).Select(o => new OutwardSupplyTransactionModel { TransactionDate = o.TransactionDate, TransactionNo = o.TransactionNo, Quantity = o.Quantity }).ToList() : null,
                            InwardSupplyTransactions = s.InwardSupplyTransactions != null ? s.InwardSupplyTransactions.Where(i => i.Fk_FinancialYearId == FinancialYear && i.Fk_BranchId == BranchId && i.TransactionDate >= convertedFromDate && i.TransactionDate <= convertedToDate).Select(i => new InwardSupplyTransactionModel { TransactionDate = i.TransactionDate, TransactionNo = i.TransactionNo, Quantity = i.Quantity }).ToList() : null,
                            SalesTransactions = s.SalesTransactions != null ? s.SalesTransactions.Where(s => s.Fk_FinancialYearId == FinancialYear && s.Fk_BranchId == BranchId && s.TransactionDate >= convertedFromDate && s.TransactionDate <= convertedToDate).Select(s => new SalesTransactionModel { TransactionDate = s.TransactionDate, TransactionNo = s.TransactionNo, Quantity = s.Quantity }).ToList() : null,
                            SalesReturnTransactions = s.SalesReturnTransactions != null ? s.SalesReturnTransactions.Where(sr => sr.Fk_FinancialYearId == FinancialYear && sr.Fk_BranchId == BranchId && sr.TransactionDate >= convertedFromDate && sr.TransactionDate <= convertedToDate).Select(sr => new SalesReturnTransactionModel { TransactionDate = sr.TransactionDate, TransactionNo = sr.TransactionNo, Quantity = sr.Quantity }).ToList() : null,
                            PurchaseTransactions = s.PurchaseTransactions != null ? s.PurchaseTransactions.Where(p => p.Fk_FinancialYearId == FinancialYear && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate).Select(p => new PurchaseTransactionModel { TransactionDate = p.TransactionDate, TransactionNo = p.TransactionNo, Quantity = p.Quantity }).ToList() : null,
                            PurchaseReturnTransactions = s.PurchaseReturnTransactions != null ? s.PurchaseReturnTransactions.Where(pr => pr.Fk_FinancialYearId == FinancialYear && pr.Fk_BranchId == BranchId && pr.TransactionDate >= convertedFromDate && pr.TransactionDate <= convertedToDate).Select(pr => new PurchaseReturnTransactionModel { TransactionDate = pr.TransactionDate, TransactionNo = pr.TransactionNo, Quantity = pr.Quantity }).ToList() : null,
                            ProductionEntryTransactions = s.ProductionEntryTransactions != null ? s.ProductionEntryTransactions.Where(pet => pet.Fk_FinancialYearId == FinancialYear && pet.Fk_BranchId == BranchId && pet.TransactionDate >= convertedFromDate && pet.TransactionDate <= convertedToDate).Select(pet => new ProductionEntryTransactionModel { TransactionDate = pet.TransactionDate, TransactionNo = pet.TransactionNo, Quantity = pet.Quantity }).ToList() : null,
                            ProductionEntries = s.ProductionEntries != null ? s.ProductionEntries.Where(pe => pe.Fk_FinancialYearId == FinancialYear && pe.FK_BranchId == BranchId && pe.ProductionDate >= convertedFromDate && pe.ProductionDate <= convertedToDate).Select(pe => new ProductionEntryModel { ProductionDate = pe.ProductionDate, ProductionNo = pe.ProductionNo, Quantity = pe.Quantity }).ToList() : null,
                            OpeningQty =
                         (s.PurchaseTransactions != null ? s.PurchaseTransactions.Where(p => p.Fk_FinancialYearId == FinancialYear && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate).Sum(p => p.Quantity) : 0)
                       + (s.ProductionEntries != null ? s.ProductionEntries.Where(pe => pe.Fk_FinancialYearId == FinancialYear && pe.FK_BranchId == BranchId && pe.ProductionDate < convertedFromDate).Sum(pe => pe.Quantity) : 0)
                       + (s.SalesReturnTransactions != null ? s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       + (s.InwardSupplyTransactions != null ? s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       - (s.PurchaseReturnTransactions != null ? s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       - (s.SalesTransactions != null ? s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       - (s.DamageTransactions != null ? s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       - (s.OutwardSupplyTransactions != null ? s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                       - (s.ProductionEntryTransactions != null ? s.ProductionEntryTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum() : 0)
                        }).ToListAsync();
                    }
                    else
                    {
                        var ListFinancialYearId = await _appDbContext.FinancialYears.Where(x => x.Financial_Year == _HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId")).Select(x => x.FinancialYearId).ToListAsync();
                        Models = await Query.Where(p => p.Fk_ProductTypeId == requestData.ProductTypeId && p.ProductId == requestData.ProductId).Select(s => new ProductModel
                        {
                            ProductName = s.ProductName,

                            DamageTransactions = s.DamageTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(d => new DamageTransactionModel { TransactionDate = d.TransactionDate, TransactionNo = d.TransactionNo, Quantity = d.Quantity }).ToList(),
                            OutwardSupplyTransactions = s.OutwardSupplyTransactions.Where(o => ListFinancialYearId.Contains(o.Fk_FinancialYearId) && o.TransactionDate >= convertedFromDate && o.TransactionDate <= convertedToDate).Select(o => new OutwardSupplyTransactionModel { TransactionDate = o.TransactionDate, TransactionNo = o.TransactionNo, Quantity = o.Quantity }).ToList(),
                            InwardSupplyTransactions = s.InwardSupplyTransactions.Where(i => ListFinancialYearId.Contains(i.Fk_FinancialYearId) && i.TransactionDate >= convertedFromDate && i.TransactionDate <= convertedToDate).Select(i => new InwardSupplyTransactionModel { TransactionDate = i.TransactionDate, TransactionNo = i.TransactionNo, Quantity = i.Quantity }).ToList(),
                            SalesTransactions = s.SalesTransactions.Where(s => ListFinancialYearId.Contains(s.Fk_FinancialYearId) && s.TransactionDate >= convertedFromDate && s.TransactionDate <= convertedToDate).Select(s => new SalesTransactionModel { TransactionDate = s.TransactionDate, TransactionNo = s.TransactionNo, Quantity = s.Quantity }).ToList(),
                            SalesReturnTransactions = s.SalesReturnTransactions.Where(sr => ListFinancialYearId.Contains(sr.Fk_FinancialYearId) && sr.TransactionDate >= convertedFromDate && sr.TransactionDate <= convertedToDate).Select(sr => new SalesReturnTransactionModel { TransactionDate = sr.TransactionDate, TransactionNo = sr.TransactionNo, Quantity = sr.Quantity }).ToList(),
                            PurchaseTransactions = s.PurchaseTransactions.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate).Select(p => new PurchaseTransactionModel { TransactionDate = p.TransactionDate, TransactionNo = p.TransactionNo, Quantity = p.Quantity }).ToList(),
                            PurchaseReturnTransactions = s.PurchaseReturnTransactions.Where(pr => ListFinancialYearId.Contains(pr.Fk_FinancialYearId) && pr.TransactionDate >= convertedFromDate && pr.TransactionDate <= convertedToDate).Select(pr => new PurchaseReturnTransactionModel { TransactionDate = pr.TransactionDate, TransactionNo = pr.TransactionNo, Quantity = pr.Quantity }).ToList(),
                            ProductionEntryTransactions = s.ProductionEntryTransactions.Where(pet => ListFinancialYearId.Contains(pet.Fk_FinancialYearId) && pet.TransactionDate >= convertedFromDate && pet.TransactionDate <= convertedToDate).Select(pet => new ProductionEntryTransactionModel { TransactionDate = pet.TransactionDate, TransactionNo = pet.TransactionNo, Quantity = pet.Quantity }).ToList(),
                            ProductionEntries = s.ProductionEntries.Where(pe => ListFinancialYearId.Contains(pe.Fk_FinancialYearId) && pe.ProductionDate >= convertedFromDate && pe.ProductionDate <= convertedToDate).Select(pe => new ProductionEntryModel { ProductionDate = pe.ProductionDate, ProductionNo = pe.ProductionNo, Quantity = pe.Quantity }).ToList(),
                            OpeningQty =
                                s.PurchaseTransactions.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate).Sum(p => p.Quantity)
                             + s.ProductionEntries.Where(pe => ListFinancialYearId.Contains(pe.Fk_FinancialYearId) && pe.ProductionDate < convertedFromDate).Sum(pe => pe.Quantity)
                             + s.SalesReturnTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                             + s.InwardSupplyTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                             - s.PurchaseReturnTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                             - s.SalesTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                             - s.DamageTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                             - s.OutwardSupplyTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                             - s.ProductionEntryTransactions.Where(d => ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate < convertedFromDate).Select(i => i.Quantity).Sum()
                        }).ToListAsync();
                    }
                    if (Models.Count > 0)
                    {
                        if (requestData.ZeroValued == "No")
                        {
                            _Result.CollectionObjData = Models.Where(s => s.DamageTransactions.Count > 0 || s.OutwardSupplyTransactions.Count > 0 || s.InwardSupplyTransactions.Count > 0 || s.SalesTransactions.Count > 0 || s.SalesReturnTransactions.Count > 0 || s.PurchaseTransactions.Count > 0 || s.PurchaseReturnTransactions.Count > 0 || s.ProductionEntries.Count > 0).ToList();
                        }
                        else
                        {
                            _Result.CollectionObjData = Models;
                        }
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedStockReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Labour Report
        public async Task<Result<LaborReportModel>> GetSummerizedLabourReport(LabourReportDataRequest requestData)
        {
            Result<LaborReportModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<LaborReportModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        Models = await _appDbContext.Labours.Where(l => l.Fk_BranchId == BranchId).Select(s => new LaborReportModel
                        {
                            LabourName = s.LabourName,
                            BillingAmt = s.ProductionEntries.Where(l => l.Fk_LabourId == s.LabourId && l.Fk_FinancialYearId == FinancialYearId && l.FK_BranchId == BranchId && l.ProductionDate >= convertedFromDate && l.ProductionDate <= convertedToDate).Select(t => t.Amount).Sum(),
                            PaymentAmt = _appDbContext.Payments.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && l.Fk_FinancialYearId == FinancialYearId && l.Fk_BranchId == BranchId && l.VoucherDate >= convertedFromDate && l.VoucherDate <= convertedToDate).Select(t => t.Amount).Sum(),
                            DamageAmt = _appDbContext.DamageOrders.Where(l => l.Fk_LabourId == s.LabourId && l.Fk_FinancialYearId == FinancialYearId && l.Fk_BranchId == BranchId && l.TransactionDate >= convertedFromDate && l.TransactionDate <= convertedToDate).Select(t => t.TotalAmount).Sum(),
                            OpeningBal =
                                _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && l.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).SingleOrDefault()
                              + s.ProductionEntries.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.FK_BranchId == BranchId && x.ProductionDate < convertedFromDate).Sum(x => x.Amount)
                              - _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                              - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.VoucherDate < convertedFromDate).Sum(x => x.Amount),
                            OpeningBalType = (_appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId).Select(t => t.OpeningBalance).SingleOrDefault()
                            + s.ProductionEntries.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.FK_BranchId == BranchId && x.ProductionDate < convertedFromDate).Sum(x => x.Amount)
                             - _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                            - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.VoucherDate < convertedFromDate).Sum(x => x.Amount)) > 0 ? "Cr" : "Dr",
                        }).ToListAsync();
                    }
                    else
                    {
                        var ListFinancialYearId = await _appDbContext.FinancialYears.Where(x => x.Financial_Year == _HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId")).Select(x => x.FinancialYearId).ToListAsync();
                        Models = await _appDbContext.Labours.Select(s => new LaborReportModel
                        {
                            LabourName = s.LabourName,
                            BillingAmt = _appDbContext.ProductionEntries.Where(l => l.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(l.Fk_FinancialYearId) && l.ProductionDate >= convertedFromDate && l.ProductionDate <= convertedToDate).Select(t => t.Amount).Sum(),
                            PaymentAmt = _appDbContext.Payments.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId) && l.VoucherDate >= convertedFromDate && l.VoucherDate <= convertedToDate).Select(t => t.Amount).Sum(),
                            DamageAmt = _appDbContext.DamageOrders.Where(l => l.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(l.Fk_FinancialYearId) && l.TransactionDate >= convertedFromDate && l.TransactionDate <= convertedToDate).Select(t => t.TotalAmount).Sum(),
                            OpeningBal =
                                _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId)).Select(t => t.OpeningBalance).SingleOrDefault()
                              + s.ProductionEntries.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.ProductionDate < convertedFromDate).Sum(x => x.Amount)
                              - _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                              - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.VoucherDate < convertedFromDate).Sum(x => x.Amount),
                            OpeningBalType = (
                            _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId)).Select(t => t.OpeningBalance).SingleOrDefault()
                              + s.ProductionEntries.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.ProductionDate < convertedFromDate).Sum(x => x.Amount)
                              - _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                              - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.VoucherDate < convertedFromDate).Sum(x => x.Amount)
                              ) > 0 ? "Cr" : "Dr",
                        }).ToListAsync();
                    }
                    if (Models.Count > 0)
                    {
                        _Result.CollectionObjData = Models;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedLabourReport : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LabourModel>> GetDetailedLabourReport(LabourReportDataRequest requestData)
        {
            Result<LabourModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<LabourModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        Models = await _appDbContext.Labours.Where(s => s.LabourId == requestData.LabourId).Select(s => new LabourModel
                        {
                            LabourName = s.LabourName,
                            ProductionEntries = s.ProductionEntries.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.FK_BranchId == BranchId && d.ProductionDate >= convertedFromDate && d.ProductionDate < convertedToDate).Select(d => new ProductionEntryModel { ProductionDate = d.ProductionDate, ProductionNo = d.ProductionNo, Quantity = d.Quantity, Rate = d.Rate, Amount = d.Amount, Product = new ProductModel { ProductName = d.Product.ProductName } }).ToList(),
                            Payment = _appDbContext.Payments.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && l.Fk_FinancialYearId == FinancialYearId && l.Fk_BranchId == BranchId && l.VoucherDate >= convertedFromDate && l.VoucherDate <= convertedToDate).Select(t => new PaymentModel { VoucherDate = t.VoucherDate, VouvherNo = t.VouvherNo, Amount = t.Amount }).ToList(),
                            DamageOrders = s.DamageOrders.Where(l => l.Fk_LabourId == s.LabourId && l.Fk_FinancialYearId == FinancialYearId && l.Fk_BranchId == BranchId && l.TransactionDate >= convertedFromDate && l.TransactionDate <= convertedToDate).Select(d => new DamageOrderModel { TransactionDate = d.TransactionDate, TransactionNo = d.TransactionNo, TotalAmount = d.TotalAmount }).ToList(),
                            OpeningBalance =
                                _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && l.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).SingleOrDefault()
                              + s.ProductionEntries.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.FK_BranchId == BranchId && x.ProductionDate < convertedFromDate).Sum(x => x.Amount)
                              - s.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                              - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.VoucherDate < convertedFromDate).Sum(x => x.Amount),
                            BalanceType = (_appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId).Select(t => t.OpeningBalance).SingleOrDefault()
                            + s.ProductionEntries.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.FK_BranchId == BranchId && x.ProductionDate < convertedFromDate).Sum(x => x.Amount)
                             - s.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                            - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.VoucherDate < convertedFromDate).Sum(x => x.Amount)) > 0 ? "Cr" : "Dr",
                        }).ToListAsync();
                    }
                    else
                    {
                        var ListFinancialYearId = await _appDbContext.FinancialYears.Where(x => x.Financial_Year == _HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId")).Select(x => x.FinancialYearId).ToListAsync();
                        Models = await _appDbContext.Labours.Where(s => s.LabourId == requestData.LabourId).Select(s => new LabourModel
                        {
                            LabourName = s.LabourName,
                            ProductionEntries = s.ProductionEntries.Where(d => d.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.ProductionDate >= convertedFromDate && d.ProductionDate <= convertedToDate).Select(d => new ProductionEntryModel { ProductionDate = d.ProductionDate, ProductionNo = d.ProductionNo, Quantity = d.Quantity, Rate = d.Rate, Amount = d.Amount, Product = new ProductModel { ProductName = d.Product.ProductName } }).ToList(),
                            Payment = _appDbContext.Payments.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId) && l.VoucherDate >= convertedFromDate && l.VoucherDate <= convertedToDate).Select(t => new PaymentModel { VoucherDate = t.VoucherDate, VouvherNo = t.VouvherNo, Amount = t.Amount }).ToList(),
                            DamageOrders = s.DamageOrders.Where(l => l.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(l.Fk_FinancialYearId) && l.TransactionDate >= convertedFromDate && l.TransactionDate <= convertedToDate).Select(d => new DamageOrderModel { TransactionDate = d.TransactionDate, TransactionNo = d.TransactionNo, TotalAmount = d.TotalAmount }).ToList(),
                            OpeningBalance =
                                _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId)).Select(t => t.OpeningBalance).SingleOrDefault()
                              + s.ProductionEntries.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.ProductionDate < convertedFromDate).Sum(x => x.Amount)
                              - s.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                              - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.VoucherDate < convertedFromDate).Sum(x => x.Amount),
                            BalanceType = (
                            _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId)).Select(t => t.OpeningBalance).SingleOrDefault()
                              + s.ProductionEntries.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.ProductionDate < convertedFromDate).Sum(x => x.Amount)
                              - s.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                              - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.VoucherDate < convertedFromDate).Sum(x => x.Amount)
                              ) > 0 ? "Cr" : "Dr",
                        }).ToListAsync();
                    }
                    if (Models.Count > 0)
                    {
                        _Result.CollectionObjData = Models;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                }

                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedLabourReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Customer Report
        public async Task<Result<PartyReportModel>> GetSummerizedCustomerReport(PartyReportDataRequest requestData)
        {
            Result<PartyReportModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<PartyReportModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        Models = await _appDbContext.Parties.Where(s => s.Fk_PartyType == MappingLedgers.SundryDebtors && s.Fk_BranchId == BranchId).Select(s => new PartyReportModel
                        {
                            PartyName = s.PartyName,
                            OpeningBal = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            OpeningBalType = (_appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) > 0 ? "DR" : "CR",
                            DrAmt = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum(),
                            CrAmt = _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() + _appDbContext.Receipts.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            Balance = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            BalanceType = (_appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) > 0 ? "DR" : "CR",
                        }).ToListAsync();
                    }
                    else
                    {
                        var ListFinancialYearId = await _appDbContext.FinancialYears.Where(x => x.Financial_Year == _HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId")).Select(x => x.FinancialYearId).ToListAsync();
                        Models = await _appDbContext.Parties.Where(s => s.Fk_PartyType == MappingLedgers.SundryDebtors).Select(s => new PartyReportModel
                        {
                            PartyName = s.PartyName,
                            OpeningBal = _appDbContext.SalesOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => ListFinancialYearId.Contains(so.Fk_FinancialYearId) && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            OpeningBalType = (_appDbContext.SalesOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => ListFinancialYearId.Contains(so.Fk_FinancialYearId) && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) > 0 ? "DR" : "CR",
                            DrAmt = _appDbContext.SalesOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum(),
                            CrAmt = _appDbContext.SalesReturnOrders.Where(so => ListFinancialYearId.Contains(so.Fk_FinancialYearId) && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() + _appDbContext.Receipts.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            Balance = _appDbContext.SalesOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => ListFinancialYearId.Contains(so.Fk_FinancialYearId) && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            BalanceType = (_appDbContext.SalesOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => ListFinancialYearId.Contains(so.Fk_FinancialYearId) && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) > 0 ? "DR" : "CR",
                        }).ToListAsync();
                    }
                    if (Models.Count > 0)
                    {
                        _Result.CollectionObjData = Models;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedCustomerReport : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<PartyModel>> GetDetailedCustomerReport(PartyReportDataRequest requestData)
        {
            Result<PartyModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                PartyModel Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        Models = await _appDbContext.Parties.Where(s => s.Fk_PartyType == MappingLedgers.SundryDebtors && s.Fk_SubledgerId == requestData.PartyId && s.Fk_BranchId == BranchId).Select(s => new PartyModel
                        {
                            PartyName = s.PartyName,
                            OpeningBal = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            OpeningBalType = (_appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) > 0 ? "DR" : "CR",
                            SalesOrders = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.TransactionNo).Select(t => new SalesOrderModel { TransactionDate = t.TransactionDate, TransactionNo = t.TransactionNo, TransactionType = t.TransactionType, GrandTotal = t.GrandTotal }).ToList(),
                            SalesReturns = _appDbContext.SalesReturnOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.TransactionNo).Select(t => new SalesReturnOrderModel { TransactionDate = t.TransactionDate, TransactionNo = t.TransactionNo, TransactionType = t.TransactionType, GrandTotal = t.GrandTotal }).ToList(),
                            Receipts = _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.VouvherNo).Select(t => new ReceiptModel { VoucherDate = t.VoucherDate, VouvherNo = t.VouvherNo, narration = t.narration, Amount = t.Amount }).ToList(),
                        }).SingleOrDefaultAsync();
                    }
                    else
                    {
                        var ListFinancialYearId = await _appDbContext.FinancialYears.Where(x => x.Financial_Year == _HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId")).Select(x => x.FinancialYearId).ToListAsync();
                        Models = await _appDbContext.Parties.Where(s => s.Fk_PartyType == MappingLedgers.SundryDebtors && s.Fk_SubledgerId == requestData.PartyId).Select(s => new PartyModel
                        {
                            PartyName = s.PartyName,
                            OpeningBal = _appDbContext.SalesOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => ListFinancialYearId.Contains(so.Fk_FinancialYearId) && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            OpeningBalType = (_appDbContext.SalesOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.SalesReturnOrders.Where(so => ListFinancialYearId.Contains(so.Fk_FinancialYearId) && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Receipts.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) > 0 ? "DR" : "CR",
                            SalesOrders = _appDbContext.SalesOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.TransactionNo).Select(t => new SalesOrderModel { TransactionDate = t.TransactionDate, TransactionNo = t.TransactionNo, TransactionType = t.TransactionType, GrandTotal = t.GrandTotal }).ToList(),
                            SalesReturns = _appDbContext.SalesReturnOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.TransactionNo).Select(t => new SalesReturnOrderModel { TransactionDate = t.TransactionDate, TransactionNo = t.TransactionNo, TransactionType = t.TransactionType, GrandTotal = t.GrandTotal }).ToList(),
                            Receipts = _appDbContext.Receipts.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.VouvherNo).Select(t => new ReceiptModel { VoucherDate = t.VoucherDate, VouvherNo = t.VouvherNo, narration = t.narration, Amount = t.Amount }).ToList(),
                        }).SingleOrDefaultAsync();
                    }
                    if (Models != null)
                    {
                        _Result.SingleObjData = Models;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedCustomerReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Supplyer Report
        public async Task<Result<PartyReportModel>> GetSummerizedSupplyerReport(PartyReportDataRequest requestData)
        {
            Result<PartyReportModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<PartyReportModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        Models = await _appDbContext.Parties.Where(s => s.Fk_PartyType == MappingLedgers.SundryCreditors && s.Fk_BranchId == BranchId).Select(s => new PartyReportModel
                        {
                            PartyName = s.PartyName,
                            OpeningBal = _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            OpeningBalType = (_appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) >= 0 ? "Cr" : "Dr",
                            DrAmt = _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum(),
                            CrAmt = _appDbContext.PurchaseReturnOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() + _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            Balance = _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            BalanceType = (_appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) > 0 ? "Cr" : "Dr",
                        }).ToListAsync();
                    }
                    else
                    {
                        var ListFinancialYearId = await _appDbContext.FinancialYears.Where(x => x.Financial_Year == _HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId")).Select(x => x.FinancialYearId).ToListAsync();
                        Models = await _appDbContext.Parties.Where(s => s.Fk_PartyType == MappingLedgers.SundryCreditors).Select(s => new PartyReportModel
                        {
                            PartyName = s.PartyName,
                            OpeningBal = _appDbContext.PurchaseOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            OpeningBalType = (_appDbContext.PurchaseOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) >= 0 ? "Cr" : "Dr",
                            DrAmt = _appDbContext.PurchaseOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum(),
                            CrAmt = _appDbContext.PurchaseReturnOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() + _appDbContext.Payments.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            Balance = _appDbContext.PurchaseOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            BalanceType = (_appDbContext.PurchaseOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) > 0 ? "Cr" : "Dr",
                        }).ToListAsync();
                    }
                    if (Models.Count > 0)
                    {
                        _Result.CollectionObjData = Models;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedSupplyerReport : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<PartyModel>> GetDetailedSupplyerReport(PartyReportDataRequest requestData)
        {
            Result<PartyModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                PartyModel Model = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        Model = await _appDbContext.Parties.Where(s => s.Fk_PartyType == MappingLedgers.SundryCreditors && s.Fk_SubledgerId == requestData.PartyId && s.Fk_BranchId == BranchId).Select(s => new PartyModel
                        {
                            OpeningBal = _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            OpeningBalType = (_appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) >= 0 ? "Cr" : "Dr",
                            PurchaseOrders = _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.TransactionNo).Select(t => new PurchaseOrderModel { TransactionDate = t.TransactionDate, TransactionNo = t.TransactionNo, GrandTotal = t.GrandTotal }).ToList(),
                            PurchaseReturns = _appDbContext.PurchaseReturnOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.TransactionNo).Select(t => new PurchaseReturnOrderModel { TransactionDate = t.TransactionDate, TransactionNo = t.TransactionNo, GrandTotal = t.GrandTotal }).ToList(),
                            payments = _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.VouvherNo).Select(t => new PaymentModel { VoucherDate = t.VoucherDate, VouvherNo = t.VouvherNo, narration = t.narration, Amount = t.Amount }).ToList(),
                        }).SingleOrDefaultAsync();
                    }
                    else
                    {
                        var ListFinancialYearId = await _appDbContext.FinancialYears.Where(x => x.Financial_Year == _HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId")).Select(x => x.FinancialYearId).ToListAsync();
                        Model = await _appDbContext.Parties.Where(s => s.Fk_PartyType == MappingLedgers.SundryCreditors && s.Fk_SubledgerId == requestData.PartyId).Select(s => new PartyModel
                        {
                            OpeningBal = _appDbContext.PurchaseOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum(),
                            OpeningBalType = (_appDbContext.PurchaseOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.PurchaseReturnOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.GrandTotal).Sum() - _appDbContext.Payments.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).Select(t => t.Amount).Sum()) >= 0 ? "Cr" : "Dr",
                            PurchaseOrders = _appDbContext.PurchaseOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.TransactionNo).Select(t => new PurchaseOrderModel { TransactionDate = t.TransactionDate, TransactionNo = t.TransactionNo, GrandTotal = t.GrandTotal }).ToList(),
                            PurchaseReturns = _appDbContext.PurchaseReturnOrders.Where(p => ListFinancialYearId.Contains(p.Fk_FinancialYearId) && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.TransactionNo).Select(t => new PurchaseReturnOrderModel { TransactionDate = t.TransactionDate, TransactionNo = t.TransactionNo, GrandTotal = t.GrandTotal }).ToList(),
                            payments = _appDbContext.Payments.Where(r => ListFinancialYearId.Contains(r.Fk_FinancialYearId) && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.Fk_SubledgerId).OrderBy(t => t.VouvherNo).Select(t => new PaymentModel { VoucherDate = t.VoucherDate, VouvherNo = t.VouvherNo, narration = t.narration, Amount = t.Amount }).ToList(),
                        }).SingleOrDefaultAsync();
                    }
                    if (Model != null)
                    {
                        _Result.SingleObjData = Model;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedSupplyerReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region DaySheet
        public async Task<Result<DaySheetModel>> GetDaySheet(string Date)
        {
            Result<DaySheetModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                DaySheetModel Model = new();
                if (DateTime.TryParseExact(Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        var Purchases = await _appDbContext.PurchaseOrders.Where(s => s.TransactionDate == convertedDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId).Select(s => new PurchaseOrderModel
                        {
                            TransactionNo = s.TransactionNo,
                            PartyName = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            PurchaseTransactions = _appDbContext.PurchaseTransactions.Where(x => x.Fk_PurchaseOrderId == s.PurchaseOrderId && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).Select(x => new PurchaseTransactionModel
                            {
                                ProductName = x.Product != null ? x.Product.ProductName : null,
                                Quantity = x.Quantity,
                                Rate = x.Rate,
                                Amount = x.Amount,
                            }).ToList(),
                        }).ToListAsync();
                        var CashSales = await _appDbContext.SalesOrders.Where(s => s.TransactionDate == convertedDate && s.TransactionType == "cash" && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId).Select(s => new SalesOrderModel
                        {
                            TransactionNo = s.TransactionNo,
                            CustomerName = s.CustomerName,
                            SalesTransactions = _appDbContext.SalesTransaction.Where(x => x.Fk_SalesOrderId == s.SalesOrderId && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).Select(x => new SalesTransactionModel
                            {
                                ProductName = x.Product != null ? x.Product.ProductName : null,
                                Quantity = x.Quantity,
                                Rate = x.Rate,
                                Amount = x.Amount,
                            }).ToList(),
                        }).ToListAsync();
                        var CashSale = _appDbContext.SalesOrders.Where(s => s.TransactionDate == convertedDate && s.TransactionType == "cash" && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId).Select(t => t.GrandTotal).Sum();
                        var CreditSales = await _appDbContext.SalesOrders.Where(s => s.TransactionDate == convertedDate && s.TransactionType == "credit" && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId).Select(s => new SalesOrderModel
                        {
                            TransactionNo = s.TransactionNo,
                            CustomerName = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            SalesTransactions = _appDbContext.SalesTransaction.Where(x => x.Fk_SalesOrderId == s.SalesOrderId && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).Select(x => new SalesTransactionModel
                            {
                                ProductName = x.Product != null ? x.Product.ProductName : null,
                                Quantity = x.Quantity,
                                Rate = x.Rate,
                                Amount = x.Amount,
                            }).ToList(),
                        }).ToListAsync();
                        var CreditSale = _appDbContext.SalesOrders.Where(s => s.TransactionDate == convertedDate && s.TransactionType == "credit" && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId).Select(t => t.GrandTotal).Sum();
                        var Receipts = await _appDbContext.Receipts.Where(s => s.VoucherDate == convertedDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId).Select(s => new ReceiptModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            narration = s.narration,
                            FromAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        var Payments = await _appDbContext.Payments.Where(s => s.VoucherDate == convertedDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId).Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            narration = s.narration,
                            ToAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        decimal OpeningCashBal = await _appDbContext.LedgerBalances.Where(x => x.Fk_LedgerId == MappingLedgers.CashAccount && x.Fk_BranchId == BranchId && x.Fk_FinancialYear == FinancialYearId).Select(x => x.OpeningBalance).SingleOrDefaultAsync() + await _appDbContext.Receipts.Where(x => x.VoucherDate < convertedDate && x.CashBank == "cash" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate < convertedDate && x.CashBank == "cash" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount);
                        decimal ClosingCashBal = OpeningCashBal + await _appDbContext.Receipts.Where(x => x.VoucherDate == convertedDate && x.CashBank == "cash" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate == convertedDate && x.CashBank == "cash" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount);
                        var ListBankLedgers = await _appDbContext.Ledgers.Where(x => x.LedgerType == "bank").Select(x => x.LedgerId).ToListAsync();
                        decimal OpeningBankBal = await _appDbContext.LedgerBalances.Where(x => ListBankLedgers.Contains(x.Fk_LedgerId) && x.Fk_BranchId == BranchId && x.Fk_FinancialYear == FinancialYearId).SumAsync(x => x.OpeningBalance) + await _appDbContext.Receipts.Where(x => x.VoucherDate < convertedDate && x.CashBank == "bank" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate < convertedDate && x.CashBank == "bank" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount);
                        decimal ClosingBankBal = OpeningBankBal + await _appDbContext.Receipts.Where(x => x.VoucherDate == convertedDate && x.CashBank == "bank" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate == convertedDate && x.CashBank == "bank" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount);
                        Model = new DaySheetModel
                        {
                            Purchases = Purchases,
                            CashSales = CashSales,
                            CreditSales = CreditSales,
                            CashSale = CashSale,
                            CreditSale = CreditSale,
                            Receipts = Receipts,
                            Payments = Payments,
                            OpeningCashBal = OpeningCashBal,
                            OpeningBankBal = OpeningBankBal,
                            ClosingCashBal = ClosingCashBal,
                            ClosingBankBal = ClosingBankBal
                        };
                    }
                    else
                    {
                        var ListFinancialYearId = await _appDbContext.FinancialYears.Where(x => x.Financial_Year == _HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId")).Select(x => x.FinancialYearId).ToListAsync();
                        var Purchases = await _appDbContext.PurchaseOrders.Where(s => s.TransactionDate == convertedDate && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(s => new PurchaseOrderModel
                        {
                            TransactionNo = s.TransactionNo,
                            PartyName = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            PurchaseTransactions = _appDbContext.PurchaseTransactions.Where(x => x.Fk_PurchaseOrderId == s.PurchaseOrderId && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(x => new PurchaseTransactionModel
                            {
                                ProductName = x.Product != null ? x.Product.ProductName : null,
                                Quantity = x.Quantity,
                                Rate = x.Rate,
                                Amount = x.Amount,
                            }).ToList(),
                        }).ToListAsync();
                        var CashSales = await _appDbContext.SalesOrders.Where(s => s.TransactionDate == convertedDate && s.TransactionType == "cash" && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(s => new SalesOrderModel
                        {
                            TransactionNo = s.TransactionNo,
                            CustomerName = s.CustomerName,
                            SalesTransactions = _appDbContext.SalesTransaction.Where(x => x.Fk_SalesOrderId == s.SalesOrderId && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(x => new SalesTransactionModel
                            {
                                ProductName = x.Product != null ? x.Product.ProductName : null,
                                Quantity = x.Quantity,
                                Rate = x.Rate,
                                Amount = x.Amount,
                            }).ToList(),
                        }).ToListAsync();
                        var CashSale = _appDbContext.SalesOrders.Where(s => s.TransactionDate == convertedDate && s.TransactionType == "cash" && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(t => t.GrandTotal).Sum();
                        var CreditSales = await _appDbContext.SalesOrders.Where(s => s.TransactionDate == convertedDate && s.TransactionType == "credit" && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(s => new SalesOrderModel
                        {
                            TransactionNo = s.TransactionNo,
                            CustomerName = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            SalesTransactions = _appDbContext.SalesTransaction.Where(x => x.Fk_SalesOrderId == s.SalesOrderId && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(x => new SalesTransactionModel
                            {
                                ProductName = x.Product != null ? x.Product.ProductName : null,
                                Quantity = x.Quantity,
                                Rate = x.Rate,
                                Amount = x.Amount,
                            }).ToList(),
                        }).ToListAsync();
                        var CreditSale = _appDbContext.SalesOrders.Where(s => s.TransactionDate == convertedDate && s.TransactionType == "credit" && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(t => t.GrandTotal).Sum();
                        var Receipts = await _appDbContext.Receipts.Where(s => s.VoucherDate == convertedDate && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(s => new ReceiptModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            narration = s.narration,
                            FromAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        var Payments = await _appDbContext.Payments.Where(s => s.VoucherDate == convertedDate && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            narration = s.narration,
                            ToAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        decimal OpeningCashBal = await _appDbContext.LedgerBalances.Where(x => x.Fk_LedgerId == MappingLedgers.CashAccount && ListFinancialYearId.Contains(x.Fk_FinancialYear)).Select(x => x.OpeningBalance).SingleOrDefaultAsync() + await _appDbContext.Receipts.Where(x => x.VoucherDate < convertedDate && x.CashBank == "cash" && ListFinancialYearId.Contains(x.Fk_FinancialYearId)).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate < convertedDate && x.CashBank == "cash" && ListFinancialYearId.Contains(x.Fk_FinancialYearId)).SumAsync(x => x.Amount);
                        decimal ClosingCashBal = OpeningCashBal + await _appDbContext.Receipts.Where(x => x.VoucherDate == convertedDate && x.CashBank == "cash" && ListFinancialYearId.Contains(x.Fk_FinancialYearId)).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate == convertedDate && x.CashBank == "cash" && ListFinancialYearId.Contains(x.Fk_FinancialYearId)).SumAsync(x => x.Amount);
                        var ListBankLedgers = await _appDbContext.Ledgers.Where(x => x.LedgerType == "bank").Select(x => x.LedgerId).ToListAsync();
                        decimal OpeningBankBal = await _appDbContext.LedgerBalances.Where(x => ListBankLedgers.Contains(x.Fk_LedgerId) && ListFinancialYearId.Contains(x.Fk_FinancialYear)).SumAsync(x => x.OpeningBalance) + await _appDbContext.Receipts.Where(x => x.VoucherDate < convertedDate && x.CashBank == "bank" && ListFinancialYearId.Contains(x.Fk_FinancialYearId)).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate < convertedDate && x.CashBank == "bank" && ListFinancialYearId.Contains(x.Fk_FinancialYearId)).SumAsync(x => x.Amount);
                        decimal ClosingBankBal = OpeningBankBal + await _appDbContext.Receipts.Where(x => x.VoucherDate == convertedDate && x.CashBank == "bank" && ListFinancialYearId.Contains(x.Fk_FinancialYearId)).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate == convertedDate && x.CashBank == "bank" && ListFinancialYearId.Contains(x.Fk_FinancialYearId)).SumAsync(x => x.Amount);
                        Model = new DaySheetModel
                        {
                            Purchases = Purchases,
                            CashSales = CashSales,
                            CreditSales = CreditSales,
                            CashSale = CashSale,
                            CreditSale = CreditSale,
                            Receipts = Receipts,
                            Payments = Payments,
                            OpeningCashBal = OpeningCashBal,
                            OpeningBankBal = OpeningBankBal,
                            ClosingCashBal = ClosingCashBal,
                            ClosingBankBal = ClosingBankBal
                        };
                    }
                    if (Model != null)
                    {
                        _Result.SingleObjData = Model;
                        _Result.IsSuccess = true;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetDaySheet : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region CashBook
        public async Task<Result<CashBookModal>> CashBookReport(CashBookDataRequest requestData)
        {
            Result<CashBookModal> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        var Receipts = await _appDbContext.Receipts.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.CashBank == "Cash").Select(s => new ReceiptModel
                        {
                            VouvherNo = s.VouvherNo,
                            VoucherDate = s.VoucherDate,
                            CashBank = s.CashBank,
                            DrCr = s.DrCr,
                            narration = s.narration,
                            FromAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount

                        }).ToListAsync();
                        var Payments = await _appDbContext.Payments.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.CashBank == "Cash").Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            VoucherDate = s.VoucherDate,
                            CashBank = s.CashBank,
                            narration = s.narration,
                            DrCr = s.DrCr,
                            ToAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        var Journals = await _appDbContext.Journals.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == MappingLedgers.CashAccount).Select(s => new JournalModel
                        {
                            VouvherNo = s.VouvherNo,
                            VoucherDate = s.VoucherDate,
                            narration = s.Narration,
                            DrCr = s.DrCr,
                            Amount = s.Amount
                        }).ToListAsync();
                        decimal OpeningBal = await _appDbContext.Receipts.Where(x => x.VoucherDate < convertedFromDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.CashBank == "Cash").SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate < convertedToDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.CashBank == "Cash").SumAsync(x => x.Amount);
                        decimal ClosingBal = OpeningBal + (await _appDbContext.Receipts.Where(x => x.VoucherDate >= convertedFromDate && x.VoucherDate <= convertedToDate && x.CashBank == "Cash" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate >= convertedFromDate && x.VoucherDate <= convertedToDate && x.CashBank == "Cash" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount));
                        var Data = new CashBookModal
                        {
                            OpeningBal = OpeningBal,
                            ClosingBal = ClosingBal,
                            Receipts = Receipts,
                            Payments = Payments,
                            Journals = Journals
                        };
                        if (Data != null)
                        {
                            _Result.SingleObjData = Data;
                            _Result.IsSuccess = true;
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                        }
                    }

                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedLabourReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region BankBook
        public async Task<Result<BankBookModal>> BankBookReport(BankBookDataRequest requestData)
        {
            Result<BankBookModal> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {

                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        var Receipts = await _appDbContext.Receipts.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == requestData.BankId).Select(s => new ReceiptModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            narration = s.narration,
                            TransactionNo = s.TransactionNo,
                            FromAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        var Payments = await _appDbContext.Payments.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == requestData.BankId).Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            narration = s.narration,
                            ToAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        var Journals = await _appDbContext.Journals.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == requestData.BankId).Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            narration = s.Narration,
                            ToAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        string BankName = _appDbContext.Ledgers.Where(l => l.LedgerId == requestData.BankId).Select(l => l.LedgerName).SingleOrDefault();
                        decimal OpeningBal = await _appDbContext.Receipts.Where(x => x.VoucherDate < convertedToDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.CashBank == "Bank").SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate < convertedToDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.CashBank == "Bank").SumAsync(x => x.Amount);
                        decimal ClosingBal = OpeningBal + (await _appDbContext.Receipts.Where(x => x.VoucherDate >= convertedFromDate && x.VoucherDate <= convertedToDate && x.CashBank == "Bank" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate >= convertedFromDate && x.VoucherDate <= convertedToDate && x.CashBank == "Bank" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount));
                        var Data = new BankBookModal
                        {
                            OpeningBal = OpeningBal,
                            ClosingBal = ClosingBal,
                            Receipts = Receipts,
                            Payments = Payments,
                            BankName = BankName,

                        };
                        if (Data != null)
                        {
                            _Result.SingleObjData = Data;
                            _Result.IsSuccess = true;
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                        }
                    }

                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedLabourReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region LadgerBook
        public async Task<Result<LedgerBookModel>> LedgerBookReport(LedgerbookDataRequest requestData)
        {
            Result<LedgerBookModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        var Receipts = await _appDbContext.Receipts.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == requestData.LedgerId || s.CashBankLedgerId == requestData.LedgerId).Select(s => new ReceiptModel
                        {
                            VouvherNo = s.VouvherNo,
                            VoucherDate = s.VoucherDate,
                            CashBank = s.CashBank,
                            LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault() ?? _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                            DrCr = s.DrCr,
                            narration = s.narration,
                            Amount = s.Amount

                        }).ToListAsync();
                        var Payments = await _appDbContext.Payments.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == requestData.LedgerId || s.CashBankLedgerId == requestData.LedgerId).Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            VoucherDate = s.VoucherDate,
                            LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault() ?? _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                            CashBank = s.CashBank,
                            narration = s.narration,
                            DrCr = s.DrCr,
                            Amount = s.Amount
                        }).ToListAsync();
                        var Journals = await _appDbContext.Journals.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == requestData.LedgerId).Select(s => new JournalModel
                        {
                            VouvherNo = s.VouvherNo,
                            VoucherDate = s.VoucherDate,
                            narration = s.Narration,
                            LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault() ?? _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                            DrCr = s.DrCr,
                            Amount = s.Amount
                        }).ToListAsync();
                        decimal OpeningBal = (await _appDbContext.Receipts.Where(x => x.VoucherDate < convertedFromDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_LedgerId == requestData.LedgerId || x.CashBankLedgerId == requestData.LedgerId).SumAsync(x => x.Amount) + await _appDbContext.Journals.Where(x => x.VoucherDate < convertedFromDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_LedgerId == requestData.LedgerId && x.DrCr == "CR").SumAsync(x => x.Amount))
                            - (await _appDbContext.Payments.Where(x => x.VoucherDate < convertedToDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_LedgerId == requestData.LedgerId || x.CashBankLedgerId == requestData.LedgerId).SumAsync(x => x.Amount) + await _appDbContext.Journals.Where(x => x.VoucherDate < convertedFromDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_LedgerId == requestData.LedgerId && x.DrCr == "DR").SumAsync(x => x.Amount));
                        decimal ClosingBal = OpeningBal + (await _appDbContext.Receipts.Where(x => x.VoucherDate >= convertedFromDate && x.VoucherDate <= convertedToDate && x.Fk_LedgerId == requestData.LedgerId || x.CashBankLedgerId == requestData.LedgerId && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate >= convertedFromDate && x.VoucherDate <= convertedToDate && x.Fk_LedgerId == requestData.LedgerId || x.CashBankLedgerId == requestData.LedgerId && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount));
                        var Data = new LedgerBookModel
                        {
                            OpeningBal = OpeningBal,
                            ClosingBal = ClosingBal,
                            Receipts = Receipts,
                            Payments = Payments,
                            Journals = Journals,
                            LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == requestData.LedgerId).Select(l => l.LedgerName).SingleOrDefault() ?? _appDbContext.LedgersDev.Where(l => l.LedgerId == requestData.LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                        };
                        if (Data != null)
                        {
                            _Result.SingleObjData = Data;
                            _Result.IsSuccess = true;
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                        }
                    }

                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedLabourReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region
        public async Task<Result<LedgerTrialBalanceModel>> TrialbalanceReport(LedgerbookDataRequest requestData)
        {
            Result<LedgerTrialBalanceModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        var ledgerIdsFromLedger = await _appDbContext.Ledgers.Select(l => l.LedgerId).Distinct().ToListAsync();
                        var ledgerIdsFromLedgerDev = await _appDbContext.LedgersDev.Select(ld => ld.LedgerId).Distinct().ToListAsync();
                        var allLedgerIds = ledgerIdsFromLedger
                            .Union(ledgerIdsFromLedgerDev)
                            .Distinct()
                            .ToList();
                        var Data =  new List<LedgerTrialBalanceModel>();
                        foreach (var LedgerId in allLedgerIds)
                        {
                            var PaymentDebittotal = await _appDbContext.Payments.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId || s.CashBankLedgerId == LedgerId)).SumAsync(s => s.Amount);
                            var ReceiptsCredittotal = await _appDbContext.Receipts.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId || s.CashBankLedgerId == LedgerId)).SumAsync(s => s.Amount);
                            var journalDebitTotal = await _appDbContext.Journals.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId && s.DrCr == "DR")).SumAsync(s => s.Amount);
                            var journalCreditTotal = await _appDbContext.Journals.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId && s.DrCr == "CR")).SumAsync(s => s.Amount);
                            var Credittotal = ReceiptsCredittotal + journalCreditTotal;
                            var Debittotal = PaymentDebittotal + journalDebitTotal;
                            var LagedrOpeningbal = _appDbContext.LedgerBalances.Where(l => l.Fk_LedgerId == LedgerId && l.Fk_BranchId == BranchId && l.Fk_FinancialYear == FinancialYearId).Select(l => l.RunningBalance).SingleOrDefault();
                            var OpeningBalance = LagedrOpeningbal + (await _appDbContext.Payments .Where(s => s.VoucherDate < convertedFromDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId || s.CashBankLedgerId == LedgerId)).SumAsync(s => s.Amount)  + await _appDbContext.Journals.Where(s => s.VoucherDate < convertedFromDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == LedgerId && s.DrCr == "DR") .SumAsync(s => s.Amount)) 
                            + (await _appDbContext.Receipts .Where(s => s.VoucherDate < convertedFromDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId || s.CashBankLedgerId == LedgerId)) .SumAsync(s => s.Amount) + await _appDbContext.Journals.Where(s => s.VoucherDate < convertedFromDate && s.Fk_BranchId == BranchId &&  s.Fk_FinancialYearId == FinancialYearId &&  s.Fk_LedgerId == LedgerId && s.DrCr == "CR").SumAsync(s => s.Amount));
                            var ClosingBalance = OpeningBalance + Credittotal + Debittotal;
                            var LadgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == LedgerId).Select(l => l.LedgerName).SingleOrDefault() ?? _appDbContext.LedgersDev.Where(l => l.LedgerId == LedgerId).Select(l => l.LedgerName).SingleOrDefault();
                          var LedgerBalances = new LedgerTrialBalanceModel
                          {
                              OpeningBal = OpeningBalance,
                              ClosingBal = ClosingBalance,
                              LedgerName = LadgerName,
                              CreditTotal = Credittotal,
                              DebitTotal = Debittotal
                          };
                            Data.Add(LedgerBalances);
                        }
                        if (Data.Any())
                        {
                            _Result.CollectionObjData = Data;
                            _Result.IsSuccess = true;
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedLabourReport : {_Exception.Message}");
            }
            return _Result;
        }


        #endregion
        #region JournalBook
        public async Task<Result<GroupedJournalModel>> JournalBookreport(LedgerbookDataRequest requestData)
        {
            Result<GroupedJournalModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        var Query = await _appDbContext.Journals.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYear).Select(s =>
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
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("Exception2345@gmail.com", "FMS Excepion", $"MasterRepo/GetAllProducts : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
    }
}

