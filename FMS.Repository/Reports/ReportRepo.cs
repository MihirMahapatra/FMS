using AutoMapper;
using FMS.Api.Email.EmailService;
using FMS.Db.Context;
using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

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
        #region GrapData
        public async Task<Result<GraphDataModel>> GetGraphData()
        {
            Result<GraphDataModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                GraphDataModel Model = new GraphDataModel();

                if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                {
                    Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    List<decimal> salesAmount = new List<decimal>(new decimal[12]); // Initialize with 12 elements, all 0
                    List<decimal> purchaseAmount = new List<decimal>(new decimal[12]);
                    List<decimal> productionAmount = new List<decimal>(new decimal[12]);
                    List<decimal> receivedAmount = new List<decimal>(new decimal[12]);
                    #region SaleData
                    var salesData = _appDbContext.SalesOrders
                    .Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId)
                   .GroupBy(s => s.OrderDate.Month)
                   .Select(g => new
                   {
                       Month = g.Key,
                       SaleAmount = g.Sum(s => s.GrandTotal)
                   });
                    foreach (var item in salesData)
                    {
                        salesAmount[item.Month - 1] = item.SaleAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    #region PurchaseData
                    var PurchaseData = _appDbContext.PurchaseOrders
                   .Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId)
                  .GroupBy(s => s.TransactionDate.Month)
                  .Select(g => new
                  {
                      Month = g.Key,
                      PurchaseAmount = g.Sum(s => s.GrandTotal)
                  });
                    foreach (var item in PurchaseData)
                    {
                        purchaseAmount[item.Month - 1] = item.PurchaseAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    #region ProductionData
                    var ProductionData = _appDbContext.LabourOrders
                  .Where(s => s.FK_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId)
                   .GroupBy(s => s.TransactionDate.Month)
                   .Select(g => new
                   {
                       Month = g.Key,
                       ProductionAmount = g.Sum(s => s.Amount)
                   });
                    foreach (var item in ProductionData)
                    {
                        productionAmount[item.Month - 1] = item.ProductionAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    #region ReceiptData
                    var ReceivedAmountdata = _appDbContext.Receipts
                  .Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId)
                   .GroupBy(s => s.VoucherDate.Month)
                   .Select(g => new
                   {
                       Month = g.Key,
                       ReceivedAmount = g.Sum(s => s.Amount)
                   });
                    foreach (var item in ReceivedAmountdata)
                    {
                        receivedAmount[item.Month - 1] = item.ReceivedAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    Model.SalesAmount = salesAmount;
                    Model.PurchaseAmount = purchaseAmount;
                    Model.ProductionAmount = productionAmount;
                    Model.ReceivedAmount = receivedAmount;
                }
                else
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    List<decimal> salesAmount = new List<decimal>(new decimal[12]); // Initialize with 12 elements, all 0
                    List<decimal> purchaseAmount = new List<decimal>(new decimal[12]);
                    List<decimal> productionAmount = new List<decimal>(new decimal[12]);
                    List<decimal> receivedAmount = new List<decimal>(new decimal[12]);
                    #region SaleData
                    var salesData = _appDbContext.SalesOrders
                    .Where(s => s.Fk_FinancialYearId == FinancialYearId)
                   .GroupBy(s => s.OrderDate.Month)
                   .Select(g => new
                   {
                       Month = g.Key,
                       SaleAmount = g.Sum(s => s.GrandTotal)
                   });
                    foreach (var item in salesData)
                    {
                        salesAmount[item.Month - 1] = item.SaleAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    #region PurchaseData
                    var PurchaseData = _appDbContext.PurchaseOrders
                   .Where(s => s.Fk_FinancialYearId == FinancialYearId)
                  .GroupBy(s => s.TransactionDate.Month)
                  .Select(g => new
                  {
                      Month = g.Key,
                      PurchaseAmount = g.Sum(s => s.GrandTotal)
                  });
                    foreach (var item in PurchaseData)
                    {
                        purchaseAmount[item.Month - 1] = item.PurchaseAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    #region ProductionData
                    var ProductionData = _appDbContext.LabourOrders
                  .Where(s => s.Fk_FinancialYearId == FinancialYearId)
                   .GroupBy(s => s.TransactionDate.Month)
                   .Select(g => new
                   {
                       Month = g.Key,
                       ProductionAmount = g.Sum(s => s.Amount)
                   });
                    foreach (var item in ProductionData)
                    {
                        productionAmount[item.Month - 1] = item.ProductionAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    #region ReceiptData
                    var ReceivedAmountdata = _appDbContext.Receipts
                  .Where(s => s.Fk_FinancialYearId == FinancialYearId)
                   .GroupBy(s => s.VoucherDate.Month)
                   .Select(g => new
                   {
                       Month = g.Key,
                       ReceivedAmount = g.Sum(s => s.Amount)
                   });
                    foreach (var item in ReceivedAmountdata)
                    {
                        receivedAmount[item.Month - 1] = item.ReceivedAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    Model.SalesAmount = salesAmount;
                    Model.PurchaseAmount = purchaseAmount;
                    Model.ProductionAmount = productionAmount;
                    Model.ReceivedAmount = receivedAmount;
                }

                if (Model != null)
                {
                    _Result.SingleObjData = Model;
                    _Result.IsSuccess = true;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetDaySheet : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<GraphDataModel>> GetGraphDataforProductWise(StockReportDataRequest requestData)
        {
            Result<GraphDataModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                GraphDataModel Model = new GraphDataModel();

                if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                {
                    Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    List<decimal> salesAmount = new List<decimal>(new decimal[12]); // Initialize with 12 elements, all 0
                    List<decimal> productionAmount = new List<decimal>(new decimal[12]);
                    #region SaleData
                    var salesData = _appDbContext.SalesTransaction
                    .Where(s => s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_ProductId == requestData.ProductId)
                   .GroupBy(s => s.TransactionDate.Month)
                   .Select(g => new
                   {
                       Month = g.Key,
                       SaleAmount = g.Sum(s => s.UnitQuantity)
                   });
                    foreach (var item in salesData)
                    {
                        salesAmount[item.Month - 1] = item.SaleAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                
                    #region ProductionData
                    var ProductionData = _appDbContext.LabourOrders
                  .Where(s => s.FK_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_ProductId == requestData.ProductId)
                   .GroupBy(s => s.TransactionDate.Month)
                   .Select(g => new
                   {
                       Month = g.Key,
                       ProductionAmount = g.Sum(s => s.Quantity)
                   });
                    foreach (var item in ProductionData)
                    {
                        productionAmount[item.Month - 1] = item.ProductionAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    Model.SalesAmount = salesAmount;
                    Model.ProductionAmount = productionAmount;
                }
                else
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    List<decimal> salesAmount = new List<decimal>(new decimal[12]); // Initialize with 12 elements, all 0
                    List<decimal> purchaseAmount = new List<decimal>(new decimal[12]);
                    List<decimal> productionAmount = new List<decimal>(new decimal[12]);
                    List<decimal> receivedAmount = new List<decimal>(new decimal[12]);
                    #region SaleData
                    var salesData = _appDbContext.SalesOrders
                    .Where(s => s.Fk_FinancialYearId == FinancialYearId)
                   .GroupBy(s => s.OrderDate.Month)
                   .Select(g => new
                   {
                       Month = g.Key,
                       SaleAmount = g.Sum(s => s.GrandTotal)
                   });
                    foreach (var item in salesData)
                    {
                        salesAmount[item.Month - 1] = item.SaleAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    #region PurchaseData
                    var PurchaseData = _appDbContext.PurchaseOrders
                   .Where(s => s.Fk_FinancialYearId == FinancialYearId)
                  .GroupBy(s => s.TransactionDate.Month)
                  .Select(g => new
                  {
                      Month = g.Key,
                      PurchaseAmount = g.Sum(s => s.GrandTotal)
                  });
                    foreach (var item in PurchaseData)
                    {
                        purchaseAmount[item.Month - 1] = item.PurchaseAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    #region ProductionData
                    var ProductionData = _appDbContext.LabourOrders
                  .Where(s => s.Fk_FinancialYearId == FinancialYearId)
                   .GroupBy(s => s.TransactionDate.Month)
                   .Select(g => new
                   {
                       Month = g.Key,
                       ProductionAmount = g.Sum(s => s.Amount)
                   });
                    foreach (var item in ProductionData)
                    {
                        productionAmount[item.Month - 1] = item.ProductionAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    #region ReceiptData
                    var ReceivedAmountdata = _appDbContext.Receipts
                  .Where(s => s.Fk_FinancialYearId == FinancialYearId)
                   .GroupBy(s => s.VoucherDate.Month)
                   .Select(g => new
                   {
                       Month = g.Key,
                       ReceivedAmount = g.Sum(s => s.Amount)
                   });
                    foreach (var item in ReceivedAmountdata)
                    {
                        receivedAmount[item.Month - 1] = item.ReceivedAmount; // Month - 1 to match zero-based index
                    }
                    #endregion
                    Model.SalesAmount = salesAmount;
                    Model.PurchaseAmount = purchaseAmount;
                    Model.ProductionAmount = productionAmount;
                    Model.ReceivedAmount = receivedAmount;
                }

                if (Model != null)
                {
                    _Result.SingleObjData = Model;
                    _Result.IsSuccess = true;
                    _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetDaySheet : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Stock Report
        public async Task<Result<StockReportSummerizedModel>> GetSummerizedStockReports(StockReportDataRequest requestData)
        {
            Result<StockReportSummerizedModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<StockReportSummerizedModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Models = await _appDbContext.Products.Where(s => s.Fk_ProductTypeId == requestData.ProductTypeId).Select(
                              s => new StockReportSummerizedModel
                              {
                                  ProductId = s.ProductId,
                                  ProductName = s.ProductName,
                                  UnitName = s.Unit.UnitName,
                                  DamageQty = s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                                  OutwardQty = s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                                  InwardQty = s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                                  SalesQty = s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.UnitQuantity).Sum(),
                                  SalesReturnQty = s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.UnitQuantity).Sum(),
                                  PurchaseQty = s.PurchaseTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity).Sum(),
                                  PurchaseReturnQty = s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity).Sum(),
                                  ProductionEntryQty = s.LabourTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                                  ProductionQty = s.LabourOrders.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.FK_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                                  OpeningQty =
                                  s.Stocks.Where(d => d.Fk_FinancialYear == FinancialYearId && d.Fk_BranchId == BranchId).Sum(i => i.OpeningStock)
                                  + s.PurchaseTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(p => p.AlternateQuantity * p.AlternateUnit.UnitQuantity)
                                  + s.LabourOrders.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.FK_BranchId == BranchId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(pe => pe.Quantity)
                                  + s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.UnitQuantity)
                                  + s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.Quantity)
                                  - s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity)
                                  - s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.UnitQuantity)
                                  - s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.Quantity)
                                  - s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.Quantity)
                                  - s.LabourTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.Quantity)
                              }).OrderBy(s => s.ProductName).ToListAsync();
                    }
                    else
                    {
                        Models = await _appDbContext.Products.Where(s => s.Fk_ProductTypeId == requestData.ProductTypeId)
                            .Select(s => new StockReportSummerizedModel
                            {
                                ProductId = s.ProductId,
                                ProductName = s.ProductName,
                                UnitName = s.Unit.UnitName,
                                DamageQty = s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                                OutwardQty = s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                                InwardQty = s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                                SalesQty = s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.UnitQuantity).Sum(),
                                SalesReturnQty = s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.UnitQuantity).Sum(),
                                PurchaseQty = s.PurchaseTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity).Sum(),
                                PurchaseReturnQty = s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity).Sum(),
                                ProductionEntryQty = s.LabourTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                                ProductionQty = s.LabourOrders.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                                OpeningQty =
                                  s.Stocks.Where(d => d.Fk_FinancialYear == FinancialYearId).Sum(i => i.OpeningStock)
                                  + s.PurchaseTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(p => p.AlternateQuantity * p.AlternateUnit.UnitQuantity)
                                  + s.LabourOrders.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(pe => pe.Quantity)
                                  + s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.UnitQuantity)
                                  + s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.Quantity)
                                  - s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity)
                                  - s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.UnitQuantity)
                                  - s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.Quantity)
                                  - s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.Quantity)
                                  - s.LabourTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate <= convertedFromDate && d.TransactionDate < convertedToDate).Sum(i => i.Quantity)
                            }).OrderBy(s => s.ProductName).ToListAsync();
                    }
                    if (Models.Count > 0)
                    {
                        if (requestData.ZeroValued == "No")
                        {
                            _Result.CollectionObjData = Models.Where(s => s.OpeningQty > 0 || s.DamageQty > 0 || s.OutwardQty > 0 || s.InwardQty > 0 || s.SalesQty > 0 || s.SalesReturnQty > 0 || s.PurchaseQty > 0 || s.PurchaseReturnQty > 0 || s.ProductionEntryQty > 0 || s.ProductionQty > 0).ToList();
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedStockReports : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<StockReportSummerizedInfoModel>> GetBranchWiseStockInfo(StockReportDataRequest requestData)
        {
            Result<StockReportSummerizedInfoModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<StockReportSummerizedInfoModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        string BranchName = await _appDbContext.Branches.Where(s => s.BranchId == BranchId).Select(s => s.BranchName).SingleOrDefaultAsync();
                        var Stocks = await _appDbContext.Products.Where(s => s.ProductId == requestData.ProductId).Select(
                              s => new StockReportSummerizedInfoModel
                              {
                                  BranchName = BranchName,
                                  RunningStock =
                                 -s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum()
                                  - s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum()
                                 + s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum()
                                  - s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.UnitQuantity).Sum()
                                 + s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.UnitQuantity).Sum()
                                  + s.PurchaseTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity).Sum()
                                 - s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity).Sum()
                                  - s.LabourTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum()
                                  + s.LabourOrders.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.FK_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                                  OpeningStock =
                                  s.Stocks.Where(d => d.Fk_FinancialYear == FinancialYearId && d.Fk_BranchId == BranchId).Sum(i => i.OpeningStock)
                                  + s.PurchaseTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate).Sum(p => p.AlternateQuantity * p.AlternateUnit.UnitQuantity)
                                  + s.LabourOrders.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.FK_BranchId == BranchId && d.TransactionDate <= convertedFromDate).Sum(pe => pe.Quantity)
                                  + s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.UnitQuantity)
                                  + s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.Quantity)
                                  - s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity)
                                  - s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.UnitQuantity)
                                  - s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.Quantity)
                                  - s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.Quantity)
                                  - s.LabourTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.Quantity)
                              }).ToListAsync();
                        Models.AddRange(Stocks);
                    }
                    else
                    {
                        var Branches = await _appDbContext.Branches.Select(s => new BranchModel { BranchId = s.BranchId, BranchName = s.BranchName }).ToListAsync();
                        foreach (var item in Branches)
                        {
                            var Stocks = await _appDbContext.Products.Where(s => s.ProductId == requestData.ProductId).Select(
                           s => new StockReportSummerizedInfoModel
                           {
                               BranchName = item.BranchName,
                               RunningStock =
                              -s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum()
                               - s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum()
                              + s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum()
                               - s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.UnitQuantity).Sum()
                              + s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.UnitQuantity).Sum()
                               + s.PurchaseTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity).Sum()
                              - s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity).Sum()
                               - s.LabourTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum()
                               + s.LabourOrders.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.FK_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(i => i.Quantity).Sum(),
                               OpeningStock =
                               s.Stocks.Where(d => d.Fk_FinancialYear == FinancialYearId && d.Fk_BranchId == item.BranchId).Sum(i => i.OpeningStock)
                               + s.PurchaseTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate <= convertedFromDate).Sum(p => p.AlternateQuantity * p.AlternateUnit.UnitQuantity)
                               + s.LabourOrders.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.FK_BranchId == item.BranchId && d.TransactionDate <= convertedFromDate).Sum(pe => pe.Quantity)
                               + s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.UnitQuantity)
                               + s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.Quantity)
                               - s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity)
                               - s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.UnitQuantity)
                               - s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.Quantity)
                               - s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.Quantity)
                               - s.LabourTransactions.Where(d => d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == item.BranchId && d.TransactionDate <= convertedFromDate).Sum(i => i.Quantity)
                           }).ToListAsync();
                            Models.AddRange(Stocks);
                        }
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedStockReports : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<StockReportDetailedModel2>> GetDetailedStockReport(StockReportDataRequest requestData)
        {
            Result<StockReportDetailedModel2> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<StockReportDetailedModel2> ListItems = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    Guid FinancialYear = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        StockReportDetailedModel2 Result = new();
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        string BranchName = await _appDbContext.Branches.Where(s => s.BranchId == BranchId).Select(s => s.BranchName).SingleOrDefaultAsync();
                        Result.BranchName = BranchName;
                        #region Opening Quantity
                        Result.ProductName = _appDbContext.Products.Where(x => x.ProductId == requestData.ProductId).Select(s => s.ProductName).FirstOrDefault();
                        Result.OpeningQty = await _appDbContext.Products
                            .Where(p => p.Fk_ProductTypeId == requestData.ProductTypeId && p.ProductId == requestData.ProductId)
                             .Select(s =>
                                 s.Stocks.Where(x => x.Fk_ProductId == s.ProductId && x.Fk_BranchId == BranchId && x.Fk_FinancialYear == FinancialYear).Sum(i => i.OpeningStock)
                                 + s.PurchaseTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Sum(p => p.AlternateQuantity * p.AlternateUnit.UnitQuantity)
                                 + s.LabourOrders.Where(d => d.Fk_FinancialYearId == FinancialYear && d.FK_BranchId == BranchId && d.TransactionDate < convertedFromDate).Sum(pe => pe.Quantity)
                                 + s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.UnitQuantity)
                                 + s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.Quantity)
                                 - s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity)
                                 - s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.UnitQuantity)
                                 - s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.Quantity)
                                 - s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.Quantity)
                                 - s.LabourTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.Quantity)
                             ).SingleOrDefaultAsync();
                        #endregion
                        #region Damage Transaction
                        Result.Stocks.AddRange(await _appDbContext.DamageTransactions
                            .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(d => new StockReportDetailedModel
                            {
                                TransactionDate = d.TransactionDate,
                                TransactionNo = d.TransactionNo,
                                BranchName = d.Branch.BranchName,
                                Quantity = d.Quantity,
                                Particular = "Damage",
                                IncrementStock = false,
                            }).ToListAsync());

                        #endregion
                        #region OutwardSupplyTransactions
                        Result.Stocks.AddRange(await _appDbContext.OutwardSupplyTransactions
                            .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(o => new StockReportDetailedModel
                            {
                                TransactionDate = o.TransactionDate,
                                TransactionNo = o.TransactionNo,
                                BranchName = o.Branch.BranchName,
                                Quantity = o.Quantity,
                                Particular = "Outward Supply",
                                IncrementStock = false,
                            }).ToListAsync());
                        #endregion
                        #region InwardSupplyTransactions
                        Result.Stocks.AddRange(await _appDbContext.InwardSupplyTransactions
                            .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(i => new StockReportDetailedModel
                            {
                                TransactionDate = i.TransactionDate,
                                TransactionNo = i.TransactionNo,
                                BranchName = i.Branch.BranchName,
                                Quantity = i.Quantity,
                                Particular = "Inward Supply",
                                IncrementStock = true
                            }).ToListAsync());
                        #endregion
                        #region SalesTransactions
                        Result.Stocks.AddRange(await _appDbContext.SalesTransaction
                            .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(s => new StockReportDetailedModel
                            {
                                TransactionDate = s.TransactionDate,
                                TransactionNo = s.TransactionNo,
                                BranchName = s.Branch.BranchName,
                                Quantity = s.UnitQuantity,
                                Particular = "Sales",
                                IncrementStock = false
                            }).ToListAsync());
                        #endregion
                        #region SalesReturnTransactions
                        Result.Stocks.AddRange(await _appDbContext.SalesReturnTransactions
                            .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(sr => new StockReportDetailedModel
                            {
                                TransactionDate = sr.TransactionDate,
                                TransactionNo = sr.TransactionNo,
                                BranchName = sr.Branch.BranchName,
                                Quantity = sr.UnitQuantity,
                                Particular = "Sales Return",
                                IncrementStock = true
                            }).ToListAsync());
                        #endregion
                        #region PurchaseTransactions
                        Result.Stocks.AddRange(await _appDbContext.PurchaseTransactions
                            .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(p => new StockReportDetailedModel
                            {
                                TransactionDate = p.TransactionDate,
                                TransactionNo = p.TransactionNo,
                                BranchName = p.Branch.BranchName,
                                Quantity = p.AlternateQuantity * p.AlternateUnit.UnitQuantity,
                                Particular = "Purchase",
                                IncrementStock = true
                            }).ToListAsync());
                        #endregion
                        #region PurchaseReturnTransactions
                        Result.Stocks.AddRange(await _appDbContext.PurchaseReturnTransactions
                            .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(pr => new StockReportDetailedModel
                            {
                                TransactionDate = pr.TransactionDate,
                                TransactionNo = pr.TransactionNo,
                                BranchName = pr.Branch.BranchName,
                                Quantity = pr.AlternateQuantity * pr.AlternateUnit.UnitQuantity,
                                Particular = "Purchase Return",
                                IncrementStock = false
                            }).ToListAsync());
                        #endregion
                        #region LabourTransactions
                        Result.Stocks.AddRange(await _appDbContext.LabourTransactions
                            .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(pet => new StockReportDetailedModel
                            {
                                TransactionDate = pet.TransactionDate,
                                TransactionNo = pet.TransactionNo,
                                BranchName = pet.Branch.BranchName,
                                Quantity = pet.Quantity,
                                Particular = "Production",
                                IncrementStock = false
                            }).ToListAsync());
                        #endregion
                        #region LabourOrders
                        Result.Stocks.AddRange(await _appDbContext.LabourOrders
                            .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.FK_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(pe => new StockReportDetailedModel
                            {
                                TransactionDate = pe.TransactionDate,
                                TransactionNo = pe.TransactionNo,
                                BranchName = pe.Branch.BranchName,
                                Quantity = pe.Quantity,
                                Particular = "Production",
                                IncrementStock = true
                            }).ToListAsync());
                        #endregion
                        Result.Stocks = Result.Stocks.OrderBy(s => s.TransactionDate).ToList();
                        ListItems.Add(Result);
                        if (ListItems.Count > 0)
                        {
                            _Result.CollectionObjData = ListItems;
                            _Result.IsSuccess = true;
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                        }
                    }
                    else
                    {
                        var Branches = await _appDbContext.Branches.Select(s => new BranchModel { BranchId = s.BranchId, BranchName = s.BranchName }).ToListAsync();
                        foreach (var item in Branches)
                        {
                            StockReportDetailedModel2 Result = new();
                            #region Branch
                            Result.BranchName = item.BranchName;
                            #endregion
                            #region Opening Quantity
                            Result.ProductName = _appDbContext.Products.Where(x => x.ProductId == requestData.ProductId).Select(s => s.ProductName).FirstOrDefault();
                            Result.OpeningQty = await _appDbContext.Products
                                .Where(p => p.Fk_ProductTypeId == requestData.ProductTypeId && p.ProductId == requestData.ProductId)
                                 .Select(s =>
                                     s.Stocks.Where(x => x.Fk_ProductId == s.ProductId && x.Fk_BranchId == item.BranchId && x.Fk_FinancialYear == FinancialYear).Sum(i => i.OpeningStock)
                                     + s.PurchaseTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate < convertedFromDate).Sum(p => p.AlternateQuantity * p.AlternateUnit.UnitQuantity)
                                     + s.LabourOrders.Where(d => d.Fk_FinancialYearId == FinancialYear && d.FK_BranchId == item.BranchId && d.TransactionDate < convertedFromDate).Sum(pe => pe.Quantity)
                                     + s.SalesReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.UnitQuantity)
                                     + s.InwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.Quantity)
                                     - s.PurchaseReturnTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.AlternateQuantity * i.AlternateUnit.UnitQuantity)
                                     - s.SalesTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.UnitQuantity)
                                     - s.DamageTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.Quantity)
                                     - s.OutwardSupplyTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.Quantity)
                                     - s.LabourTransactions.Where(d => d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate < convertedFromDate).Sum(i => i.Quantity)
                                 ).SingleOrDefaultAsync();
                            #endregion
                            #region Stock
                            #region Damage Transaction
                            var damageTransactions = (await _appDbContext.DamageTransactions
                                .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                                .Select(d => new StockReportDetailedModel
                                {
                                    TransactionDate = d.TransactionDate,
                                    TransactionNo = d.TransactionNo,
                                    BranchName = d.Branch.BranchName,
                                    Quantity = d.Quantity,
                                    Particular = "Damage",
                                    IncrementStock = false,
                                }).ToListAsync());
                            Result.Stocks.AddRange(damageTransactions);
                            #endregion
                            #region OutwardSupplyTransactions
                            var outwardSupplyTransactions = await _appDbContext.OutwardSupplyTransactions
                                .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                                .Select(o => new StockReportDetailedModel
                                {
                                    TransactionDate = o.TransactionDate,
                                    TransactionNo = o.TransactionNo,
                                    BranchName = o.Branch.BranchName,
                                    Quantity = o.Quantity,
                                    Particular = "Outward Supply",
                                    IncrementStock = false,
                                }).ToListAsync();
                            Result.Stocks.AddRange(outwardSupplyTransactions);
                            #endregion
                            #region InwardSupplyTransactions
                            var inwardSupplyTransactions = await _appDbContext.InwardSupplyTransactions
                                 .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                                 .Select(i => new StockReportDetailedModel
                                 {
                                     TransactionDate = i.TransactionDate,
                                     TransactionNo = i.TransactionNo,
                                     BranchName = i.Branch.BranchName,
                                     Quantity = i.Quantity,
                                     Particular = "Inward Supply",
                                     IncrementStock = true
                                 }).ToListAsync();
                            Result.Stocks.AddRange(inwardSupplyTransactions);
                            #endregion
                            #region SalesTransactions
                            var salesTransactions = await _appDbContext.SalesTransaction
                                .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                                .Select(s => new StockReportDetailedModel
                                {
                                    TransactionDate = s.TransactionDate,
                                    TransactionNo = s.TransactionNo,
                                    BranchName = s.Branch.BranchName,
                                    Quantity = s.UnitQuantity,
                                    Particular = "Sales",
                                    IncrementStock = false
                                }).ToListAsync();
                            Result.Stocks.AddRange(salesTransactions);
                            #endregion
                            #region SalesReturnTransactions
                            var salesReturnTransactions = await _appDbContext.SalesReturnTransactions
                                .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                                .Select(sr => new StockReportDetailedModel
                                {
                                    TransactionDate = sr.TransactionDate,
                                    TransactionNo = sr.TransactionNo,
                                    BranchName = sr.Branch.BranchName,
                                    Quantity = sr.UnitQuantity,
                                    Particular = "Sales Return",
                                    IncrementStock = true
                                }).ToListAsync();
                            Result.Stocks.AddRange(salesReturnTransactions);
                            #endregion
                            #region PurchaseTransactions
                            var purchaseTransactions = await _appDbContext.PurchaseTransactions
                                 .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                                 .Select(p => new StockReportDetailedModel
                                 {
                                     TransactionDate = p.TransactionDate,
                                     TransactionNo = p.TransactionNo,
                                     BranchName = p.Branch.BranchName,
                                     Quantity = p.AlternateQuantity * p.AlternateUnit.UnitQuantity,
                                     Particular = "Purchase",
                                     IncrementStock = true
                                 }).ToListAsync();
                            Result.Stocks.AddRange(purchaseTransactions);
                            #endregion
                            #region PurchaseReturnTransactions
                            var purchaseReturnTransactions = await _appDbContext.PurchaseReturnTransactions
                                 .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                                 .Select(pr => new StockReportDetailedModel
                                 {
                                     TransactionDate = pr.TransactionDate,
                                     TransactionNo = pr.TransactionNo,
                                     BranchName = pr.Branch.BranchName,
                                     Quantity = pr.AlternateQuantity * pr.AlternateUnit.UnitQuantity,
                                     Particular = "Purchase Return",
                                     IncrementStock = false
                                 }).ToListAsync();
                            Result.Stocks.AddRange(purchaseReturnTransactions);
                            #endregion
                            #region LabourTransactions
                            var labourTransactions = await _appDbContext.LabourTransactions
                                .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.Fk_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                                .Select(pet => new StockReportDetailedModel
                                {
                                    TransactionDate = pet.TransactionDate,
                                    TransactionNo = pet.TransactionNo,
                                    BranchName = pet.Branch.BranchName,
                                    Quantity = pet.Quantity,
                                    Particular = "Production",
                                    IncrementStock = false
                                }).ToListAsync();
                            Result.Stocks.AddRange(labourTransactions);
                            #endregion
                            #region LabourOrders
                            var labourOrders = await _appDbContext.LabourOrders
                                 .Where(d => d.Fk_ProductId == requestData.ProductId && d.Fk_FinancialYearId == FinancialYear && d.FK_BranchId == item.BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                                 .Select(pe => new StockReportDetailedModel
                                 {
                                     TransactionDate = pe.TransactionDate,
                                     TransactionNo = pe.TransactionNo,
                                     BranchName = pe.Branch.BranchName,
                                     Quantity = pe.Quantity,
                                     Particular = "Production",
                                     IncrementStock = true
                                 }).ToListAsync();
                            Result.Stocks.AddRange(labourOrders);
                            #endregion
                            Result.Stocks = Result.Stocks.OrderBy(s => s.TransactionDate).ToList();
                            #endregion
                            ListItems.Add(Result);
                        }
                        if (ListItems.Count > 0)
                        {
                            _Result.CollectionObjData = ListItems;
                            _Result.IsSuccess = true;
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedStockReport : {_Exception.Message}");
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
                        Models = await _appDbContext.Labours.Where(l => l.Fk_BranchId == BranchId && l.Fk_Labour_TypeId == requestData.LabourTypeId).Select(s => new LaborReportModel
                        {
                            LabourName = s.LabourName,
                            BillingAmt = s.LabourOrders.Where(l => l.Fk_LabourId == s.LabourId && l.Fk_FinancialYearId == FinancialYearId && l.FK_BranchId == BranchId && l.TransactionDate >= convertedFromDate && l.TransactionDate <= convertedToDate).Select(t => t.Amount).Sum(),
                            PaymentAmt = _appDbContext.Payments.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && l.Fk_FinancialYearId == FinancialYearId && l.Fk_BranchId == BranchId && l.VoucherDate >= convertedFromDate && l.VoucherDate <= convertedToDate).Select(t => t.Amount).Sum(),
                            DamageAmt = _appDbContext.DamageOrders.Where(l => l.Fk_LabourId == s.LabourId && l.Fk_FinancialYearId == FinancialYearId && l.Fk_BranchId == BranchId && l.TransactionDate >= convertedFromDate && l.TransactionDate <= convertedToDate).Select(t => t.TotalAmount).Sum(),
                            OpeningBal =
                                _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && l.Fk_FinancialYearId == FinancialYearId && l.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).SingleOrDefault()
                              + s.LabourOrders.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.FK_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.Amount)
                              - _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                              - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.VoucherDate < convertedFromDate).Sum(x => x.Amount),
                            OpeningBalType = (_appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && s.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).SingleOrDefault()
                            + s.LabourOrders.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.FK_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.Amount)
                             - _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                            - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.VoucherDate < convertedFromDate).Sum(x => x.Amount)) > 0 ? "Cr" : "Dr",
                        }).ToListAsync();
                    }
                    else
                    {
                        var ListFinancialYearId = await _appDbContext.FinancialYears.Where(x => x.Financial_Year == _HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId")).Select(x => x.FinancialYearId).ToListAsync();
                        Models = await _appDbContext.Labours.Where(s => s.Fk_Labour_TypeId == requestData.LabourTypeId).Select(s => new LaborReportModel
                        {
                            LabourName = s.LabourName,
                            BillingAmt = _appDbContext.LabourOrders.Where(l => l.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(l.Fk_FinancialYearId) && l.TransactionDate >= convertedFromDate && l.TransactionDate <= convertedToDate).Select(t => t.Amount).Sum(),
                            PaymentAmt = _appDbContext.Payments.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId) && l.VoucherDate >= convertedFromDate && l.VoucherDate <= convertedToDate).Select(t => t.Amount).Sum(),
                            DamageAmt = _appDbContext.DamageOrders.Where(l => l.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(l.Fk_FinancialYearId) && l.TransactionDate >= convertedFromDate && l.TransactionDate <= convertedToDate).Select(t => t.TotalAmount).Sum(),
                            OpeningBal =
                                _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId)).Select(t => t.OpeningBalance).SingleOrDefault()
                              + s.LabourOrders.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.TransactionDate < convertedFromDate).Sum(x => x.Amount)
                              - _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                              - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.VoucherDate < convertedFromDate).Sum(x => x.Amount),
                            OpeningBalType = (
                            _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId)).Select(t => t.OpeningBalance).SingleOrDefault()
                              + s.LabourOrders.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.TransactionDate < convertedFromDate).Sum(x => x.Amount)
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedLabourReport : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<LabourReportDetailedModel2>> GetDetailedLabourReport(LabourReportDataRequest requestData)
        {
            Result<LabourReportDetailedModel2> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<LabourReportDetailedModel2> ListItems = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        LabourReportDetailedModel2 Result = new();
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        string BranchName = await _appDbContext.Branches.Where(s => s.BranchId == BranchId).Select(s => s.BranchName).SingleOrDefaultAsync();
                        Guid LabourSubLadgerId = _appDbContext.Labours.Where(x => x.LabourId == requestData.LabourId).Select(s => s.Fk_SubLedgerId).FirstOrDefault();
                        Result.BranchName = BranchName;
                        Result.LabourName = _appDbContext.Labours.Where(x => x.LabourId == requestData.LabourId).Select(s => s.LabourName).FirstOrDefault();
                        #region OpeningBalance
                        Result.OpeningBalance =
                               _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == LabourSubLadgerId && l.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).SingleOrDefault()
                             + _appDbContext.LabourOrders.Where(x => x.Fk_LabourId == requestData.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.FK_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.Amount)
                             - _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == requestData.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                             - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == LabourSubLadgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.VoucherDate < convertedFromDate).Sum(x => x.Amount);
                        #endregion
                        #region OpeningBalType
                        Result.BalanceType =
                            (
                            _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == LabourSubLadgerId && l.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).SingleOrDefault()
                            + _appDbContext.LabourOrders.Where(x => x.Fk_LabourId == requestData.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.FK_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.Amount)
                             - _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == requestData.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                            - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == LabourSubLadgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId && x.VoucherDate < convertedFromDate).Sum(x => x.Amount)) > 0 ? "Cr" : "Dr";
                        #endregion
                        #region LabourOrders
                        Result.Orders.AddRange(await _appDbContext.LabourOrders
                            .Where(d => d.Fk_LabourTypeId == requestData.LabourTypeId && d.Fk_LabourId == requestData.LabourId && d.Fk_FinancialYearId == FinancialYearId && d.FK_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(pe => new LobourReportDetailedModel
                            {
                                TransactionDate = pe.TransactionDate,
                                TransactionNo = pe.TransactionNo,
                                BranchName = pe.Branch.BranchName,
                                Quantity = pe.Quantity,
                                OTAmount = pe.OTAmount,
                                Rate = pe.Rate,
                                Amount = pe.Amount,
                                Narration = pe.Narration,
                                Product = new ProductModel { ProductName = pe.Product.ProductName },
                                Particular = "LabourOrders",
                                IncrementStock = true
                            }).ToListAsync());
                        #endregion
                        #region Payments
                        Result.Orders.AddRange(await _appDbContext.Payments
                            .Where(d => d.Fk_SubLedgerId == LabourSubLadgerId && d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.VoucherDate >= convertedFromDate && d.VoucherDate <= convertedToDate)
                            .Select(pe => new LobourReportDetailedModel
                            {
                                TransactionDate = pe.VoucherDate,
                                TransactionNo = pe.VouvherNo,
                                BranchName = pe.Branch.BranchName,
                                Narration = pe.Narration,
                                Amount = pe.Amount,
                                Particular = "Payments",
                                IncrementStock = true
                            }).ToListAsync());
                        #endregion
                        #region DamageOrders 
                        Result.Orders.AddRange(await _appDbContext.DamageOrders
                            .Where(d => d.Fk_LabourId == requestData.LabourId && d.Fk_FinancialYearId == FinancialYearId && d.Fk_BranchId == BranchId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(pe => new LobourReportDetailedModel
                            {
                                TransactionDate = pe.TransactionDate,
                                TransactionNo = pe.TransactionNo,
                                BranchName = pe.Branch.BranchName,
                                Amount = pe.TotalAmount,
                                Particular = "DamageOrders",
                                IncrementStock = true
                            }).ToListAsync());
                        #endregion   
                        Result.Orders = Result.Orders.OrderBy(s => s.TransactionDate).ToList();
                        ListItems.Add(Result);
                    }
                    else
                    {

                        LabourReportDetailedModel2 Result = new();
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        Result.LabourName = _appDbContext.Labours.Where(x => x.LabourId == requestData.LabourId).Select(s => s.LabourName).FirstOrDefault();
                        Guid LabourSubLadgerId = _appDbContext.Labours.Where(x => x.LabourId == requestData.LabourId).Select(s => s.Fk_SubLedgerId).FirstOrDefault();
                        #region OpeningBalance
                        Result.OpeningBalance =
                               _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == LabourSubLadgerId && l.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).SingleOrDefault()
                             + _appDbContext.LabourOrders.Where(x => x.Fk_LabourId == requestData.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.TransactionDate < convertedFromDate).Sum(x => x.Amount)
                             - _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == requestData.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                             - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == LabourSubLadgerId && x.Fk_FinancialYearId == FinancialYearId && x.VoucherDate < convertedFromDate).Sum(x => x.Amount);
                        #endregion
                        #region OpeningBalType
                        Result.BalanceType =
                            (_appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == LabourSubLadgerId).Select(t => t.OpeningBalance).SingleOrDefault()
                            + _appDbContext.LabourOrders.Where(x => x.Fk_LabourId == requestData.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.TransactionDate < convertedFromDate).Sum(x => x.Amount)
                             - _appDbContext.DamageOrders.Where(x => x.Fk_LabourId == requestData.LabourId && x.Fk_FinancialYearId == FinancialYearId && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                            - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == LabourSubLadgerId && x.Fk_FinancialYearId == FinancialYearId && x.VoucherDate < convertedFromDate).Sum(x => x.Amount)) > 0 ? "Cr" : "Dr";
                        #endregion
                        #region LabourOrders
                        Result.Orders.AddRange(await _appDbContext.LabourOrders
                            .Where(d => d.Fk_LabourTypeId == requestData.LabourTypeId && d.Fk_LabourId == requestData.LabourId && d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(pe => new LobourReportDetailedModel
                            {
                                TransactionDate = pe.TransactionDate,
                                TransactionNo = pe.TransactionNo,
                                BranchName = pe.Branch.BranchName,
                                Rate = pe.Rate,
                                Quantity = pe.Quantity,
                                OTAmount = pe.OTAmount,
                                Amount = pe.Amount,
                                Product = new ProductModel { ProductName = pe.Product.ProductName },
                                Particular = "LabourOrders",
                                IncrementStock = true
                            }).ToListAsync());
                        #endregion
                        #region Payments
                        Result.Orders.AddRange(await _appDbContext.Payments
                            .Where(d => d.Fk_SubLedgerId == LabourSubLadgerId && d.Fk_FinancialYearId == FinancialYearId && d.VoucherDate >= convertedFromDate && d.VoucherDate <= convertedToDate)
                            .Select(pe => new LobourReportDetailedModel
                            {
                                TransactionDate = pe.VoucherDate,
                                TransactionNo = pe.VouvherNo,
                                BranchName = pe.Branch.BranchName,
                                Amount = pe.Amount,
                                Particular = "Payments",
                                IncrementStock = true
                            }).ToListAsync());
                        #endregion
                        #region DamageOrders 
                        Result.Orders.AddRange(await _appDbContext.DamageOrders
                            .Where(d => d.Fk_LabourId == requestData.LabourId && d.Fk_FinancialYearId == FinancialYearId && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate)
                            .Select(pe => new LobourReportDetailedModel
                            {
                                TransactionDate = pe.TransactionDate,
                                TransactionNo = pe.TransactionNo,
                                BranchName = pe.Branch.BranchName,
                                Amount = pe.TotalAmount,
                                Particular = "DamageOrders",
                                IncrementStock = true
                            }).ToListAsync());
                        #endregion   
                        Result.Orders = Result.Orders.OrderBy(s => s.TransactionDate).ToList();
                        ListItems.Add(Result);
                        //var ListFinancialYearId = await _appDbContext.FinancialYears.Where(x => x.Financial_Year == _HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId")).Select(x => x.FinancialYearId).ToListAsync();
                        //Models = await _appDbContext.Labours
                        //    .Where(s => s.LabourId == requestData.LabourId && s.Fk_Labour_TypeId == requestData.LabourTypeId)
                        //    .Select(s => new LabourModel
                        //    {

                        //        LabourName = s.LabourName,
                        //        LabourOrders = s.LabourOrders.Where(d => d.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(d.Fk_FinancialYearId) && d.TransactionDate >= convertedFromDate && d.TransactionDate <= convertedToDate).Select(d => new LabourOrderModel { TransactionDate = d.TransactionDate, TransactionNo = d.TransactionNo, Quantity = d.Quantity, Rate = d.Rate, OTAmount = d.OTAmount, Amount = d.Amount, Product = new ProductModel { ProductName = d.Product.ProductName } }).ToList(),
                        //        Payment = _appDbContext.Payments.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId) && l.VoucherDate >= convertedFromDate && l.VoucherDate <= convertedToDate).Select(t => new PaymentModel { VoucherDate = t.VoucherDate, VouvherNo = t.VouvherNo, Amount = t.Amount }).ToList(),
                        //        DamageOrders = s.DamageOrders.Where(l => l.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(l.Fk_FinancialYearId) && l.TransactionDate >= convertedFromDate && l.TransactionDate <= convertedToDate).Select(d => new DamageOrderModel { TransactionDate = d.TransactionDate, TransactionNo = d.TransactionNo, TotalAmount = d.TotalAmount }).ToList(),
                        //        OpeningBalance =
                        //        _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId)).Select(t => t.OpeningBalance).SingleOrDefault()
                        //      + s.LabourOrders.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.TransactionDate < convertedFromDate).Sum(x => x.Amount)
                        //      - s.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                        //      - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.VoucherDate < convertedFromDate).Sum(x => x.Amount),
                        //        BalanceType = (
                        //    _appDbContext.SubLedgerBalances.Where(l => l.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(l.Fk_FinancialYearId)).Select(t => t.OpeningBalance).SingleOrDefault()
                        //      + s.LabourOrders.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.TransactionDate < convertedFromDate).Sum(x => x.Amount)
                        //      - s.DamageOrders.Where(x => x.Fk_LabourId == s.LabourId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.TransactionDate < convertedFromDate).Sum(x => x.TotalAmount)
                        //      - _appDbContext.Payments.Where(x => x.Fk_SubLedgerId == s.Fk_SubLedgerId && ListFinancialYearId.Contains(x.Fk_FinancialYearId) && x.VoucherDate < convertedFromDate).Sum(x => x.Amount)
                        //      ) > 0 ? "Cr" : "Dr",
                        //    }).ToListAsync();
                    }
                    if (ListItems.Count > 0)
                    {
                        _Result.CollectionObjData = ListItems;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                }

                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedLabourReport : {_Exception.Message}");
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
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        if (requestData.Fk_PartyGroupId != Guid.Empty)
                        {
                            var data = await _appDbContext.SubLedgers
                            .Where(s => s.Fk_LedgerId == MappingLedgers.SundryDebtors && s.Parties.Any(p => p.Fk_PartyGroupId == requestData.Fk_PartyGroupId))
                            .Select(s => new PartyReportModel
                            {
                                Fk_SubledgerId = s.SubLedgerId,
                                PartyName = s.SubLedgerName,
                                Adress = _appDbContext.Parties.Where(a => a.Fk_SubledgerId == s.SubLedgerId).Select(s => s.Address).FirstOrDefault(),
                                OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                                DrAmt = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum(),
                                CrAmt = _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            + _appDbContext.Receipts.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                Balance = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                BalanceType = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                            }).OrderBy(p => p.PartyName).ToListAsync();
                            Models.AddRange(data);
                        }
                        else
                        {
                            var data = await _appDbContext.SubLedgers
                            .Where(s => s.Fk_LedgerId == MappingLedgers.SundryDebtors)
                            .Select(s => new PartyReportModel
                            {
                                Fk_SubledgerId = s.SubLedgerId,
                                PartyName = s.SubLedgerName,
                                Adress = _appDbContext.Parties.Where(a => a.Fk_SubledgerId == s.SubLedgerId).Select(s => s.Address).FirstOrDefault(),
                                OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                                DrAmt = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum(),
                                CrAmt = _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            + _appDbContext.Receipts.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                Balance = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                BalanceType = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                            }).OrderBy(p => p.PartyName).ToListAsync();
                            Models.AddRange(data);
                        }
                    }
                    else
                    {
                        var data = await _appDbContext.SubLedgers
                            .Where(s => s.Fk_LedgerId == MappingLedgers.SundryDebtors)
                            .Select(s => new PartyReportModel
                            {
                                Fk_SubledgerId = s.SubLedgerId,
                                PartyName = s.SubLedgerName,
                                OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                                DrAmt = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum(),
                                CrAmt = _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            + _appDbContext.Receipts.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                Balance = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                BalanceType = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                            }).OrderBy(p => p.PartyName).ToListAsync();
                        Models.AddRange(data);
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedCustomerReport : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<PartyReportInfoModel>> GetBranchWiseCustomerInfo(PartyReportDataRequest requestData)
        {
            Result<PartyReportInfoModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<PartyReportInfoModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        string BranchName = await _appDbContext.Branches.Where(s => s.BranchId == BranchId).Select(s => s.BranchName).SingleOrDefaultAsync();
                        var Customers = await _appDbContext.SubLedgers
                            .Where(s => s.SubLedgerId == requestData.Fk_SubledgerId)
                            .Select(s => new PartyReportInfoModel
                            {
                                BranchName = BranchName,
                                OpeningBalance = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                OpeningBalanceType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                             > 0 ? "Dr" : "Cr",
                                RunningBalance = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                RunningBalanceType = (_appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum())
                             > 0 ? "Dr" : "Cr",
                            }).ToListAsync();
                        Models.AddRange(Customers);
                    }
                    else
                    {
                        var Branches = await _appDbContext.Branches.Select(s => new BranchModel { BranchId = s.BranchId, BranchName = s.BranchName }).ToListAsync();
                        foreach (var item in Branches)
                        {
                            var Customers = await _appDbContext.SubLedgers
                                .Where(s => s.SubLedgerId == requestData.Fk_SubledgerId)
                                .Select(s => new PartyReportInfoModel
                                {
                                    BranchName = item.BranchName,
                                    OpeningBalance = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == item.BranchId).Select(t => t.OpeningBalance).Sum()
                                + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == item.BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == item.BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == item.BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                    OpeningBalanceType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == item.BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == item.BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == item.BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                                    RunningBalance = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == item.BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == item.BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == item.BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                    RunningBalanceType = (_appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == item.BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == item.BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == item.BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum())
                            > 0 ? "Dr" : "Cr",
                                }).ToListAsync();
                            Models.AddRange(Customers);
                        }
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedCustomerReport : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<PartyReportModel2>> GetDetailedCustomerReport(PartyReportDataRequest requestData)
        {
            Result<PartyReportModel2> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                PartyReportModel2 PartyInfos = new PartyReportModel2();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        PartyInfos.PartyName = _appDbContext.SubLedgers.Where(x => x.SubLedgerId == requestData.PartyId).Select(x => x.SubLedgerName).FirstOrDefault();
                        PartyInfos.Adress = _appDbContext.Parties.Where(a => a.Fk_SubledgerId == requestData.PartyId).Select(s => s.Address).FirstOrDefault();
                        PartyInfos.OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                                              + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                              - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                              - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum();
                        PartyInfos.OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum()
                                            > 0 ? "Dr" : "Cr";
                        PartyInfos.Orders.AddRange(_appDbContext.SalesOrders
                                               .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                               .OrderBy(t => t.TransactionDate)
                                               .Select(t => new PartyReportOrderModel
                                               {
                                                   TransactionDate = t.TransactionDate,
                                                   TransactionNo = t.TransactionNo,
                                                   GrandTotal = t.GrandTotal,
                                                   SiteAdress = t.SiteAdress,
                                                   TransportCharges = t.TransportationCharges,
                                                   HandlingCharges = t.HandlingCharges,
                                                   ChallanNo = t.OrderNo,
                                                   Naration = t.Narration,
                                                   BranchName = t.Branch.BranchName,
                                                   DrCr = "Dr",
                                                   Transactions = t.SalesTransactions.Where(s => s.Fk_SalesOrderId == t.SalesOrderId)
                                                   .Select(s => new PartyReportTransactionModel
                                                   {
                                                       ProductName = _appDbContext.Products.Where(p => p.ProductId == s.Fk_ProductId).Select(p => p.ProductName).FirstOrDefault(),
                                                       SubProductName = _appDbContext.Products.Where(p => p.ProductId == s.Fk_SubProductId).Select(p => p.ProductName).FirstOrDefault(),
                                                       Quantity = s.UnitQuantity,
                                                       AlternateQuantity = s.AlternateQuantity,
                                                       AlternateUnit = _appDbContext.AlternateUnits.Where(a => a.FK_ProductId == s.Fk_ProductId).Select(a => a.AlternateUnitName).FirstOrDefault(),
                                                       Rate = s.Rate,
                                                       Unit = (from p in _appDbContext.Products
                                                               join u in _appDbContext.Units on p.Fk_UnitId equals u.UnitId
                                                               where p.ProductId == s.Fk_ProductId
                                                               select u.UnitName).FirstOrDefault(),
                                                       SubProductUnit = (from p in _appDbContext.Products
                                                                         join u in _appDbContext.Units on p.Fk_UnitId equals u.UnitId
                                                                         where p.ProductId == s.Fk_SubProductId
                                                                         select u.UnitName).FirstOrDefault(),
                                                       Amount = s.Amount
                                                   }).ToList()
                                               }).ToList());
                        PartyInfos.Orders.AddRange(_appDbContext.SalesReturnOrders
                                          .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                          .OrderBy(t => t.TransactionDate)
                                          .Select(t => new PartyReportOrderModel
                                          {
                                              TransactionDate = t.TransactionDate,
                                              TransactionNo = t.TransactionNo,
                                              GrandTotal = t.GrandTotal,
                                              SiteAdress = t.SiteAdress,
                                              ChallanNo = t.OrderNo,
                                              Naration = t.Narration,
                                              DrCr = "Cr",
                                              BranchName = t.Branch.BranchName,
                                              Transactions = t.SalesReturnTransactions.Where(s => s.Fk_SalesReturnOrderId == t.SalesReturnOrderId)
                                          .Select(s => new PartyReportTransactionModel
                                          {
                                              ProductName = _appDbContext.Products.Where(p => p.ProductId == s.Fk_ProductId).Select(p => p.ProductName).FirstOrDefault(),
                                              Quantity = s.UnitQuantity,
                                              AlternateUnit = _appDbContext.AlternateUnits.Where(a => a.FK_ProductId == s.Fk_ProductId).Select(a => a.AlternateUnitName).FirstOrDefault(),
                                              Rate = s.Rate,
                                              Unit = (from p in _appDbContext.Products
                                                      join u in _appDbContext.Units on p.Fk_UnitId equals u.UnitId
                                                      where p.ProductId == s.Fk_ProductId
                                                      select u.UnitName).FirstOrDefault(),
                                              Amount = s.Amount,
                                          }).ToList()
                                          }).ToList());
                        PartyInfos.Orders.AddRange(_appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == requestData.PartyId)
                            .OrderBy(t => t.VoucherDate)
                            .Select(t => new PartyReportOrderModel
                            {
                                TransactionDate = t.VoucherDate,
                                TransactionNo = t.VouvherNo,
                                ChallanNo = t.VouvherNo,
                                Naration = t.Narration,
                                SiteAdress = "_",
                                GrandTotal = t.Amount,
                                BranchName = t.Branch.BranchName,
                                DrCr = "Cr",
                            }).ToList());
                        PartyInfos.Orders = PartyInfos.Orders.OrderBy(t => t.TransactionDate).ToList();
                    }
                    else
                    {
                        PartyInfos.OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                         + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                         - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                         - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum();
                        PartyInfos.OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum()
                                            > 0 ? "Dr" : "Cr";
                        PartyInfos.Orders.AddRange(_appDbContext.SalesOrders
                                           .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                           .OrderBy(t => t.TransactionDate)
                                           .Select(t => new PartyReportOrderModel
                                           {
                                               TransactionDate = t.TransactionDate,
                                               TransactionNo = t.TransactionNo,
                                               TransactionType = t.TransactionType,
                                               GrandTotal = t.GrandTotal,
                                               SiteAdress = t.SiteAdress,
                                               Naration = t.Narration,
                                               DrCr = "Dr",
                                               BranchName = t.Branch.BranchName,
                                               Transactions = t.SalesTransactions.Where(s => s.Fk_SalesOrderId == t.SalesOrderId)
                                               .Select(s => new PartyReportTransactionModel
                                               {
                                                   ProductName = _appDbContext.Products.Where(p => p.ProductId == s.Fk_ProductId).Select(p => p.ProductName).FirstOrDefault(),
                                                   Quantity = s.UnitQuantity,
                                                   AlternateUnit = _appDbContext.AlternateUnits.Where(a => a.FK_ProductId == s.Fk_ProductId).Select(a => a.AlternateUnitName).FirstOrDefault(),
                                                   Unit = (from p in _appDbContext.Products
                                                           join u in _appDbContext.Units on p.Fk_UnitId equals u.UnitId
                                                           where p.ProductId == s.Fk_ProductId
                                                           select u.UnitName).FirstOrDefault(),
                                                   Rate = s.Rate,
                                                   Amount = s.Amount
                                               }).ToList()
                                           }).ToList());
                        PartyInfos.Orders.AddRange(_appDbContext.SalesReturnOrders
                                          .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                          .OrderBy(t => t.TransactionDate)
                                          .Select(t => new PartyReportOrderModel
                                          {
                                              TransactionDate = t.TransactionDate,
                                              TransactionNo = t.TransactionNo,
                                              TransactionType = t.TransactionType,
                                              GrandTotal = t.GrandTotal,
                                              SiteAdress = t.SiteAdress,
                                              Naration = t.Narration,
                                              DrCr = "Cr",
                                              BranchName = t.Branch.BranchName,
                                              Transactions = t.SalesReturnTransactions.Where(s => s.Fk_SalesReturnOrderId == t.SalesReturnOrderId)
                                          .Select(s => new PartyReportTransactionModel
                                          {
                                              ProductName = _appDbContext.Products.Where(p => p.ProductId == s.Fk_ProductId).Select(p => p.ProductName).FirstOrDefault(),
                                              Quantity = s.UnitQuantity,
                                              AlternateUnit = _appDbContext.AlternateUnits.Where(a => a.FK_ProductId == s.Fk_ProductId).Select(a => a.AlternateUnitName).FirstOrDefault(),
                                              Rate = s.Rate,
                                              Unit = (from p in _appDbContext.Products
                                                      join u in _appDbContext.Units on p.Fk_UnitId equals u.UnitId
                                                      where p.ProductId == s.Fk_ProductId
                                                      select u.UnitName).FirstOrDefault(),
                                              Amount = s.Amount,
                                          }).ToList()
                                          }).ToList());
                        PartyInfos.Orders.AddRange(_appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == requestData.PartyId)
                            .OrderBy(t => t.VoucherDate)
                            .Select(t => new PartyReportOrderModel
                            {
                                TransactionDate = t.VoucherDate,
                                TransactionNo = t.VouvherNo,
                                Naration = t.Narration,
                                SiteAdress = "-",
                                GrandTotal = t.Amount,
                                DrCr = "Cr",
                                BranchName = t.Branch.BranchName,
                            }).ToList());
                        PartyInfos.Orders = PartyInfos.Orders.OrderBy(t => t.TransactionDate).ToList();
                    }
                    if (PartyInfos != null)
                    {
                        _Result.SingleObjData = PartyInfos;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                        _Result.IsSuccess = true;
                    }
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedCustomerReport : {_Exception.Message}");
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
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        var data = await _appDbContext.SubLedgers
                            .Where(s => s.Fk_LedgerId == MappingLedgers.SundryCreditors)
                            .Select(s => new PartyReportModel
                            {
                                Fk_SubledgerId = s.SubLedgerId,
                                PartyName = s.SubLedgerName,
                                OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                                DrAmt = _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum(),
                                CrAmt = _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            + _appDbContext.Payments.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                Balance = _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                BalanceType = _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                            }).OrderBy(p => p.PartyName).ToListAsync();
                        Models.AddRange(data);
                    }

                    else
                    {
                        var data = await _appDbContext.SubLedgers
                            .Where(s => s.Fk_LedgerId == MappingLedgers.SundryCreditors)
                            .Select(s => new PartyReportModel
                            {
                                Fk_SubledgerId = s.SubLedgerId,
                                PartyName = s.SubLedgerName,
                                OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                                DrAmt = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum(),
                                CrAmt = _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            + _appDbContext.Receipts.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                Balance = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                BalanceType = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                            }).OrderBy(p => p.PartyName).ToListAsync();
                        Models.AddRange(data);
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedSupplyerReport : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<PartyReportInfoModel>> GetBranchWiseSupllayerInfo(PartyReportDataRequest requestData)
        {
            Result<PartyReportInfoModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<PartyReportInfoModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        string BranchName = await _appDbContext.Branches.Where(s => s.BranchId == BranchId).Select(s => s.BranchName).SingleOrDefaultAsync();
                        var Supllyers = await _appDbContext.SubLedgers
                            .Where(s => s.SubLedgerId == requestData.Fk_SubledgerId)
                            .Select(s => new PartyReportInfoModel
                            {
                                BranchName = BranchName,
                                OpeningBalance = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                OpeningBalanceType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                             > 0 ? "Dr" : "Cr",
                                RunningBalance = _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                RunningBalanceType = (_appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum())
                             > 0 ? "Dr" : "Cr",
                            }).ToListAsync();
                        Models.AddRange(Supllyers);
                    }
                    else
                    {
                        var Branches = await _appDbContext.Branches.Select(s => new BranchModel { BranchId = s.BranchId, BranchName = s.BranchName }).ToListAsync();
                        foreach (var item in Branches)
                        {
                            var Supllyers = await _appDbContext.SubLedgers
                                .Where(s => s.SubLedgerId == requestData.Fk_SubledgerId)
                                .Select(s => new PartyReportInfoModel
                                {
                                    BranchName = item.BranchName,
                                    OpeningBalance = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == item.BranchId).Select(t => t.OpeningBalance).Sum()
                                + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == item.BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == item.BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == item.BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                    OpeningBalanceType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == item.BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == item.BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == item.BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                                    RunningBalance = _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == item.BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == item.BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == item.BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                    RunningBalanceType = (_appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == item.BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == item.BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == item.BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum())
                            > 0 ? "Dr" : "Cr",
                                }).ToListAsync();
                            Models.AddRange(Supllyers);
                        }
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedCustomerReport : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<PartyReportModel2>> GetDetailedSupplyerReport(PartyReportDataRequest requestData)
        {
            Result<PartyReportModel2> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                PartyReportModel2 SupllyerInfos = new PartyReportModel2();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        SupllyerInfos.PartyName = _appDbContext.SubLedgers.Where(x => x.SubLedgerId == requestData.PartyId).Select(x => x.SubLedgerName).FirstOrDefault();
                        SupllyerInfos.OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                                              + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                              - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                              - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum();
                        SupllyerInfos.OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                                              + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                              - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                              - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum()
                                              > 0 ? "Dr" : "Cr";
                        SupllyerInfos.Orders.AddRange(_appDbContext.PurchaseOrders
                                                 .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                                 .OrderBy(t => t.TransactionDate)
                                                 .Select(t => new PartyReportOrderModel
                                                 {
                                                     TransactionDate = t.TransactionDate,
                                                     TransactionNo = t.TransactionNo,
                                                     MaterialReceiptNo = t.InvoiceNo,
                                                     GrandTotal = t.GrandTotal,
                                                     Naration = t.Narration,
                                                     BranchName = t.Branch.BranchName,
                                                     DrCr = "Dr",
                                                     Transactions = t.PurchaseTransactions.Where(s => s.Fk_PurchaseOrderId == t.PurchaseOrderId)
                                                     .Select(s => new PartyReportTransactionModel
                                                     {
                                                         ProductName = s.Product.ProductName,
                                                         Quantity = s.UnitQuantity,
                                                         AlternateQuantity = s.AlternateQuantity,
                                                         Rate = s.Rate,
                                                         Amount = s.Amount
                                                     }).ToList()
                                                 }).ToList());
                        SupllyerInfos.Orders.AddRange(_appDbContext.PurchaseReturnOrders
                                             .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                             .OrderBy(t => t.TransactionDate)
                                             .Select(t => new PartyReportOrderModel
                                             {
                                                 TransactionDate = t.TransactionDate,
                                                 TransactionNo = t.TransactionNo,
                                                 GrandTotal = t.GrandTotal,
                                                 Naration = t.Narration,
                                                 DrCr = "Cr",
                                                 BranchName = t.Branch.BranchName,
                                                 Transactions = t.PurchaseReturnTransactions.Where(s => s.Fk_PurchaseReturnOrderId == t.PurchaseReturnOrderId)
                                             .Select(s => new PartyReportTransactionModel
                                             {
                                                 ProductName = s.Product.ProductName,
                                                 Quantity = s.UnitQuantity,
                                                 AlternateQuantity = s.AlternateQuantity,
                                                 Rate = s.Rate,
                                                 Amount = s.Amount,
                                             }).ToList()
                                             }).ToList());
                        SupllyerInfos.Orders.AddRange(_appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == requestData.PartyId)
                               .OrderBy(t => t.VoucherDate)
                               .Select(t => new PartyReportOrderModel
                               {
                                   TransactionDate = t.VoucherDate,
                                   TransactionNo = t.VouvherNo,
                                   Naration = t.Narration,
                                   GrandTotal = t.Amount,
                                   BranchName = t.Branch.BranchName,
                                   DrCr = "Cr",
                               }).ToList());
                        SupllyerInfos.Orders = SupllyerInfos.Orders.OrderBy(t => t.TransactionDate).ToList();
                    }
                    else
                    {
                        SupllyerInfos.PartyName = _appDbContext.SubLedgers.Where(x => x.SubLedgerId == requestData.PartyId).Select(x => x.SubLedgerName).FirstOrDefault();
                        SupllyerInfos.OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                          + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                          - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                          - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum();
                        SupllyerInfos.OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum()
                                            > 0 ? "Dr" : "Cr";
                        SupllyerInfos.Orders.AddRange(_appDbContext.PurchaseOrders
                                           .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                           .OrderBy(t => t.TransactionDate)
                                           .Select(t => new PartyReportOrderModel
                                           {
                                               TransactionDate = t.TransactionDate,
                                               TransactionNo = t.TransactionNo,
                                               //TransactionType = t.TransactionType,
                                               GrandTotal = t.GrandTotal,
                                               Naration = t.Narration,
                                               DrCr = "Dr",
                                               BranchName = t.Branch.BranchName,
                                               Transactions = t.PurchaseTransactions.Where(s => s.Fk_PurchaseOrderId == t.PurchaseOrderId)
                                               .Select(s => new PartyReportTransactionModel
                                               {
                                                   ProductName = s.Product.ProductName,
                                                   Quantity = s.UnitQuantity,
                                                   AlternateQuantity = s.AlternateQuantity,
                                                   Rate = s.Rate,
                                                   Amount = s.Amount
                                               }).ToList()
                                           }).ToList());
                        SupllyerInfos.Orders.AddRange(_appDbContext.PurchaseReturnOrders
                                          .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                          .OrderBy(t => t.TransactionDate)
                                          .Select(t => new PartyReportOrderModel
                                          {
                                              TransactionDate = t.TransactionDate,
                                              TransactionNo = t.TransactionNo,
                                              //TransactionType = t.TransactionType,
                                              GrandTotal = t.GrandTotal,
                                              Naration = t.Narration,
                                              DrCr = "Cr",
                                              BranchName = t.Branch.BranchName,
                                              Transactions = t.PurchaseReturnTransactions.Where(s => s.Fk_PurchaseReturnOrderId == t.PurchaseReturnOrderId)
                                          .Select(s => new PartyReportTransactionModel
                                          {
                                              ProductName = s.Product.ProductName,
                                              Quantity = s.UnitQuantity,
                                              AlternateQuantity = s.AlternateQuantity,
                                              Rate = s.Rate,
                                              Amount = s.Amount,
                                          }).ToList()
                                          }).ToList());
                        SupllyerInfos.Orders.AddRange(_appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == requestData.PartyId)
                            .OrderBy(t => t.VoucherDate)
                            .Select(t => new PartyReportOrderModel
                            {
                                TransactionDate = t.VoucherDate,
                                TransactionNo = t.VouvherNo,
                                Naration = t.Narration,
                                GrandTotal = t.Amount,
                                DrCr = "Cr",
                                BranchName = t.Branch.BranchName,
                            }).ToList());
                        SupllyerInfos.Orders = SupllyerInfos.Orders.OrderBy(t => t.TransactionDate).ToList();
                    }
                    if (SupllyerInfos != null)
                    {
                        _Result.SingleObjData = SupllyerInfos;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedSupplyerReport : {_Exception.Message}");
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
                                AlternateQuantity = x.AlternateQuantity,
                                UnitQuantity = x.UnitQuantity,
                                AlternateUnit = x.AlternateUnit != null ? new AlternateUnitModel { AlternateUnitName = x.AlternateUnit.AlternateUnitName } : null,
                                UnitName = x.Product.Unit.UnitName,
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
                                Quantity = x.UnitQuantity,
                                UnitName = x.Product.Unit.UnitName,
                                AlternateUnit = x.AlternateUnit != null ? new AlternateUnitModel { AlternateUnitName = x.AlternateUnit.AlternateUnitName } : null,
                                AlternateQuantity = x.AlternateQuantity,
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
                                ProductName = x.Product != null ? x.Product.ProductName : _appDbContext.Products.Where(p => p.ProductId == x.Fk_ProductId).Select(p => p.ProductName).FirstOrDefault(),
                                UnitName = x.Product.Unit.UnitName,
                                Quantity = x.UnitQuantity,
                                AlternateQuantity = x.AlternateQuantity,
                                AlternateUnit =  x.AlternateUnit != null ? new AlternateUnitModel { AlternateUnitName = x.AlternateUnit.AlternateUnitName } : null,
                                Rate = x.Rate,
                                Amount = x.Amount,
                            }).ToList(),
                        }).ToListAsync();
                        var CreditSale = _appDbContext.SalesOrders.Where(s => s.TransactionDate == convertedDate && s.TransactionType == "credit" && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId).Select(t => t.GrandTotal).Sum();
                        var Receipts = await _appDbContext.Receipts.Where(s => s.VoucherDate == convertedDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId).Select(s => new ReceiptModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            Narration = s.Narration,
                            LedgerDevName = _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                            LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                            SubLedgerName = s.SubLedger != null ? s.SubLedger.SubLedgerName : "-",
                            FromAcc = s.SubLedger != null ? s.SubLedger.SubLedgerName : s.Ledger != null ? _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault() : _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                            //FromAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        var Payments = await _appDbContext.Payments.Where(s => s.VoucherDate == convertedDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId).Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            Narration = s.Narration,
                            LedgerDevName = _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                            LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                            SubLedgerName = s.SubLedger != null ? s.SubLedger.SubLedgerName : "-",
                            ToAcc = s.SubLedger != null ? s.SubLedger.SubLedgerName : s.Ledger != null ? _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault() : _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        decimal OpeningCashBal = await _appDbContext.LedgerBalances.Where(x => x.Fk_LedgerId == MappingLedgers.CashAccount && x.Fk_BranchId == BranchId && x.Fk_FinancialYear == FinancialYearId).Select(x => x.OpeningBalance).SingleOrDefaultAsync() + await _appDbContext.Receipts.Where(x => x.VoucherDate < convertedDate && x.CashBank == "cash" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate < convertedDate && x.CashBank == "cash" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount);
                        decimal ClosingCashBal = OpeningCashBal + await _appDbContext.Receipts.Where(x => x.VoucherDate == convertedDate && x.CashBank == "cash" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate == convertedDate && x.CashBank == "cash" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount);
                        var ListBankLedgers = await _appDbContext.Ledgers.Where(x => x.LedgerType == "bank").Select(x => x.LedgerId).ToListAsync();
                        decimal OpeningBankBal = await _appDbContext.LedgerBalances.Where(x => ListBankLedgers.Contains(x.Fk_LedgerId) && x.Fk_BranchId == BranchId && x.Fk_FinancialYear == FinancialYearId).SumAsync(x => x.OpeningBalance) + await _appDbContext.Receipts.Where(x => x.VoucherDate < convertedDate && x.CashBank == "bank" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate < convertedDate && x.CashBank == "bank" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount);
                        decimal ClosingBankBal = OpeningBankBal + await _appDbContext.Receipts.Where(x => x.VoucherDate == convertedDate && x.CashBank == "bank" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount) - await _appDbContext.Payments.Where(x => x.VoucherDate == convertedDate && x.CashBank == "bank" && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId).SumAsync(x => x.Amount);
                        var ProductionAmount = _appDbContext.LabourOrders.Where(s => s.TransactionDate == convertedDate && s.FK_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId).Select(t => t.Amount).Sum();
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
                            ClosingBankBal = ClosingBankBal,
                            ProductionAmount = ProductionAmount,
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
                                AlternateQuantity = x.AlternateQuantity,
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
                                Quantity = x.UnitQuantity,
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
                                Quantity = x.UnitQuantity,
                                Rate = x.Rate,
                                Amount = x.Amount,
                            }).ToList(),
                        }).ToListAsync();
                        var CreditSale = _appDbContext.SalesOrders.Where(s => s.TransactionDate == convertedDate && s.TransactionType == "credit" && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(t => t.GrandTotal).Sum();
                        var Receipts = await _appDbContext.Receipts.Where(s => s.VoucherDate == convertedDate && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(s => new ReceiptModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            Narration = s.Narration,
                            FromAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        var Payments = await _appDbContext.Payments.Where(s => s.VoucherDate == convertedDate && ListFinancialYearId.Contains(s.Fk_FinancialYearId)).Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            Narration = s.Narration,
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetDaySheet : {_Exception.Message}");
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
                            Narration = s.Narration,
                            FromAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount

                        }).ToListAsync();
                        var Payments = await _appDbContext.Payments.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.CashBank == "Cash").Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            VoucherDate = s.VoucherDate,
                            CashBank = s.CashBank,
                            Narration = s.Narration,
                            DrCr = s.DrCr,
                            ToAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        var Journals = await _appDbContext.Journals.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == MappingLedgers.CashAccount).Select(s => new JournalModel
                        {
                            VouvherNo = s.VouvherNo,
                            VoucherDate = s.VoucherDate,
                            Narration = s.Narration,
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedLabourReport : {_Exception.Message}");
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

                        Guid Fk_BankID = requestData.BankId;
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        var Receipts = await _appDbContext.Receipts.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == requestData.BankId).Select(s => new ReceiptModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            Narration = s.Narration,
                            TransactionNo = s.TransactionNo,
                            FromAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();
                        var Payments = await _appDbContext.Payments.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == requestData.BankId).Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            CashBank = s.CashBank,
                            Narration = s.Narration,
                            ToAcc = _appDbContext.Parties.Where(p => p.Fk_SubledgerId == s.Fk_SubLedgerId).Select(p => p.PartyName).SingleOrDefault(),
                            Amount = s.Amount
                        }).ToListAsync();

                        var Journals = await _appDbContext.Journals.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == requestData.BankId).Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            Narration = s.Narration,
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedLabourReport : {_Exception.Message}");
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
                            Narration = s.Narration,
                            Amount = s.Amount

                        }).ToListAsync();
                        var Payments = await _appDbContext.Payments.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == requestData.LedgerId || s.CashBankLedgerId == requestData.LedgerId).Select(s => new PaymentModel
                        {
                            VouvherNo = s.VouvherNo,
                            VoucherDate = s.VoucherDate,
                            LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault() ?? _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                            CashBank = s.CashBank,
                            Narration = s.Narration,
                            DrCr = s.DrCr,
                            Amount = s.Amount
                        }).ToListAsync();
                        var Journals = await _appDbContext.Journals.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == requestData.LedgerId).Select(s => new JournalModel
                        {
                            VouvherNo = s.VouvherNo,
                            VoucherDate = s.VoucherDate,
                            Narration = s.Narration,
                            LedgerName = _appDbContext.Ledgers.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault() ?? _appDbContext.LedgersDev.Where(l => l.LedgerId == s.Fk_LedgerId).Select(l => l.LedgerName).SingleOrDefault(),
                            DrCr = s.DrCr,
                            Amount = s.Amount
                        }).ToListAsync();
                        decimal OpeningBal = (await _appDbContext.Receipts.Where(x => x.VoucherDate < convertedFromDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_LedgerId == requestData.LedgerId || x.CashBankLedgerId == requestData.LedgerId).SumAsync(x => x.Amount) + await _appDbContext.Journals.Where(x => x.VoucherDate < convertedFromDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_LedgerId == requestData.LedgerId && x.DrCr == "Cr").SumAsync(x => x.Amount))
                            - (await _appDbContext.Payments.Where(x => x.VoucherDate < convertedToDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_LedgerId == requestData.LedgerId || x.CashBankLedgerId == requestData.LedgerId).SumAsync(x => x.Amount) + await _appDbContext.Journals.Where(x => x.VoucherDate < convertedFromDate && x.Fk_BranchId == BranchId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_LedgerId == requestData.LedgerId && x.DrCr == "Dr").SumAsync(x => x.Amount));
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedLabourReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region SubLadgerBook
        public async Task<Result<SubLedgerModel>> GetSubLadgers()
        {
            Result<SubLedgerModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                _Result.CollectionObjData = await (from s in _appDbContext.SubLedgers
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
        public async Task<Result<PartyReportModel>> GetSummerizedSubLadgerReport(LedgerbookDataRequest requestData)
        {
            Result<PartyReportModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<PartyReportModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                        var data = await _appDbContext.SubLedgers
                            .Where(s => s.Fk_LedgerId == requestData.LedgerId && s.Fk_BranchId == BranchId)
                            .Select(s => new PartyReportModel
                            {
                                Fk_SubledgerId = s.SubLedgerId,
                                PartyName = s.SubLedgerName,
                                OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                             + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                              - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                              - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                                DrAmt = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                              + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                                + _appDbContext.Payments.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                CrAmt = _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            + _appDbContext.Receipts.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            + _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum(),

                                Balance = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            + _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            + _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                             - _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                                - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                                - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                BalanceType = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                          + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                                - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                                - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                            }).ToListAsync();
                        Models.AddRange(data);
                    }
                    else
                    {
                        var data = await _appDbContext.SubLedgers
                            .Where(s => s.Fk_LedgerId == requestData.LedgerId)
                            .Select(s => new PartyReportModel
                            {
                                Fk_SubledgerId = s.SubLedgerId,
                                PartyName = s.SubLedgerName,
                                OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                             + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                             - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                              - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                              - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                                DrAmt = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                                + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum(),
                                CrAmt = _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            + _appDbContext.Receipts.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                                + _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            + _appDbContext.Payments.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                Balance = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                                + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                                - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                                - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                BalanceType = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                           + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                                - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                                - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                            }).ToListAsync();
                        Models.AddRange(data);
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedCustomerReport : {_Exception.Message}");
            }
            return _Result;
        }
        public async Task<Result<PartyReportModel2>> SubLadgerDetailedBookReport(LedgerbookDataRequest requestData)
        {
            Result<PartyReportModel2> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                PartyReportModel2 SubLadegerInfos = new PartyReportModel2();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {

                        Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                        if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                        {
                            Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));
                            SubLadegerInfos.PartyName = _appDbContext.SubLedgers.Where(x => x.SubLedgerId == requestData.LedgerId).Select(x => x.SubLedgerName).FirstOrDefault();
                            SubLadegerInfos.OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.LedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                                                  + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                  - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                  - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.Amount).Sum()
                                                   + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                  - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                  + _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.Amount).Sum();
                            var Sum = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.LedgerId && x.Fk_FinancialYearId == FinancialYearId && x.Fk_BranchId == BranchId).Select(t => t.OpeningBalance).Sum()
                                                + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.Amount).Sum()
                                                 + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.Amount).Sum();
                            SubLadegerInfos.OpeningBalType = Sum > 0 ? "Dr" : (Sum < 0 ? "Cr" : "-");
                            SubLadegerInfos.Orders.AddRange(_appDbContext.SalesOrders
                                                   .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.LedgerId)
                                                   .OrderBy(t => t.TransactionDate)
                                                   .Select(t => new PartyReportOrderModel
                                                   {
                                                       TransactionDate = t.TransactionDate,
                                                       TransactionNo = t.TransactionNo,
                                                       GrandTotal = t.GrandTotal,
                                                       Naration = t.Narration,
                                                       BranchName = t.Branch.BranchName,
                                                       DrCr = "Dr",
                                                       Transactions = t.SalesTransactions.Where(s => s.Fk_SalesOrderId == t.SalesOrderId)
                                                       .Select(s => new PartyReportTransactionModel
                                                       {
                                                           ProductName = s.Product.ProductName,
                                                           Quantity = s.UnitQuantity,
                                                           Rate = s.Rate,
                                                           Amount = s.Amount
                                                       }).ToList()
                                                   }).ToList());
                            SubLadegerInfos.Orders.AddRange(_appDbContext.SalesReturnOrders
                                              .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.LedgerId)
                                              .OrderBy(t => t.TransactionDate)
                                              .Select(t => new PartyReportOrderModel
                                              {
                                                  TransactionDate = t.TransactionDate,
                                                  TransactionNo = t.TransactionNo,
                                                  GrandTotal = t.GrandTotal,
                                                  Naration = t.Narration,
                                                  DrCr = "Cr",
                                                  BranchName = t.Branch.BranchName,
                                                  Transactions = t.SalesReturnTransactions.Where(s => s.Fk_SalesReturnOrderId == t.SalesReturnOrderId)
                                              .Select(s => new PartyReportTransactionModel
                                              {
                                                  ProductName = s.Product.ProductName,
                                                  Quantity = s.UnitQuantity,
                                                  Rate = s.Rate,
                                                  Amount = s.Amount,
                                              }).ToList()
                                              }).ToList());
                            SubLadegerInfos.Orders.AddRange(_appDbContext.PurchaseOrders
                                                   .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.LedgerId)
                                                   .OrderBy(t => t.TransactionDate)
                                                   .Select(t => new PartyReportOrderModel
                                                   {
                                                       TransactionDate = t.TransactionDate,
                                                       TransactionNo = t.TransactionNo,
                                                       GrandTotal = t.GrandTotal,
                                                       Naration = t.Narration,
                                                       BranchName = t.Branch.BranchName,
                                                       DrCr = "Dr",
                                                       Transactions = t.PurchaseTransactions.Where(s => s.Fk_PurchaseOrderId == t.PurchaseOrderId)
                                                       .Select(s => new PartyReportTransactionModel
                                                       {
                                                           ProductName = s.Product.ProductName,
                                                           Quantity = s.UnitQuantity,
                                                           Rate = s.Rate,
                                                           Amount = s.Amount
                                                       }).ToList()
                                                   }).ToList());
                            SubLadegerInfos.Orders.AddRange(_appDbContext.PurchaseReturnOrders
                                                   .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.Fk_BranchId == BranchId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.LedgerId)
                                                   .OrderBy(t => t.TransactionDate)
                                                   .Select(t => new PartyReportOrderModel
                                                   {
                                                       TransactionDate = t.TransactionDate,
                                                       TransactionNo = t.TransactionNo,
                                                       GrandTotal = t.GrandTotal,
                                                       Naration = t.Narration,
                                                       BranchName = t.Branch.BranchName,
                                                       DrCr = "Dr",
                                                       Transactions = t.PurchaseReturnTransactions.Where(s => s.Fk_PurchaseReturnOrderId == t.PurchaseReturnOrderId)
                                                       .Select(s => new PartyReportTransactionModel
                                                       {
                                                           ProductName = s.Product.ProductName,
                                                           Quantity = s.UnitQuantity,
                                                           Rate = s.Rate,
                                                           Amount = s.Amount
                                                       }).ToList()
                                                   }).ToList());
                            SubLadegerInfos.Orders.AddRange(_appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == requestData.LedgerId)
                                 .OrderBy(t => t.VoucherDate)
                                 .Select(t => new PartyReportOrderModel
                                 {
                                     TransactionDate = t.VoucherDate,
                                     TransactionNo = t.VouvherNo,
                                     Naration = t.Narration,
                                     GrandTotal = t.Amount,
                                     BranchName = t.Branch.BranchName,
                                     DrCr = "Dr",
                                 }).ToList());
                            SubLadegerInfos.Orders.AddRange(_appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == requestData.LedgerId)
                             .OrderBy(t => t.VoucherDate)
                             .Select(t => new PartyReportOrderModel
                             {
                                 TransactionDate = t.VoucherDate,
                                 TransactionNo = t.VouvherNo,
                                 Naration = t.Narration,
                                 GrandTotal = t.Amount,
                                 BranchName = t.Branch.BranchName,
                                 DrCr = "Cr",
                             }).ToList());

                            SubLadegerInfos.Orders = SubLadegerInfos.Orders.OrderBy(t => t.TransactionDate).ToList();
                        }
                        else
                        {
                            SubLadegerInfos.OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.LedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                             + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                             - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                             - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.Amount).Sum()
                                              + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                             - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                             - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.Amount).Sum();
                            SubLadegerInfos.OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.LedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                                + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.Amount).Sum()
                                                 + _appDbContext.PurchaseOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                - _appDbContext.PurchaseReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.GrandTotal).Sum()
                                                - _appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.LedgerId).Select(t => t.Amount).Sum()
                                                > 0 ? "Dr" : "Cr";
                            SubLadegerInfos.Orders.AddRange(_appDbContext.SalesOrders
                                               .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.LedgerId)
                                               .OrderBy(t => t.TransactionDate)
                                               .Select(t => new PartyReportOrderModel
                                               {
                                                   TransactionDate = t.TransactionDate,
                                                   TransactionNo = t.TransactionNo,
                                                   TransactionType = t.TransactionType,
                                                   GrandTotal = t.GrandTotal,
                                                   Naration = t.Narration,
                                                   DrCr = "Dr",
                                                   BranchName = t.Branch.BranchName,
                                                   Transactions = t.SalesTransactions.Where(s => s.Fk_SalesOrderId == t.SalesOrderId)
                                                   .Select(s => new PartyReportTransactionModel
                                                   {
                                                       ProductName = s.Product.ProductName,
                                                       Quantity = s.UnitQuantity,
                                                       Rate = s.Rate,
                                                       Amount = s.Amount
                                                   }).ToList()
                                               }).ToList());
                            SubLadegerInfos.Orders.AddRange(_appDbContext.SalesReturnOrders
                                              .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.LedgerId)
                                              .OrderBy(t => t.TransactionDate)
                                              .Select(t => new PartyReportOrderModel
                                              {
                                                  TransactionDate = t.TransactionDate,
                                                  TransactionNo = t.TransactionNo,
                                                  TransactionType = t.TransactionType,
                                                  GrandTotal = t.GrandTotal,
                                                  Naration = t.Narration,
                                                  DrCr = "Cr",
                                                  BranchName = t.Branch.BranchName,
                                                  Transactions = t.SalesReturnTransactions.Where(s => s.Fk_SalesReturnOrderId == t.SalesReturnOrderId)
                                              .Select(s => new PartyReportTransactionModel
                                              {
                                                  ProductName = s.Product.ProductName,
                                                  Quantity = s.UnitQuantity,
                                                  Rate = s.Rate,
                                                  Amount = s.Amount,
                                              }).ToList()
                                              }).ToList());
                            SubLadegerInfos.Orders.AddRange(_appDbContext.PurchaseOrders
                                               .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.LedgerId)
                                               .OrderBy(t => t.TransactionDate)
                                               .Select(t => new PartyReportOrderModel
                                               {
                                                   TransactionDate = t.TransactionDate,
                                                   TransactionNo = t.TransactionNo,
                                                   //TransactionType = t.TransactionType,
                                                   GrandTotal = t.GrandTotal,
                                                   Naration = t.Narration,
                                                   DrCr = "Dr",
                                                   BranchName = t.Branch.BranchName,
                                                   Transactions = t.PurchaseTransactions.Where(s => s.Fk_PurchaseOrderId == t.PurchaseOrderId)
                                                   .Select(s => new PartyReportTransactionModel
                                                   {
                                                       ProductName = s.Product.ProductName,
                                                       Quantity = s.UnitQuantity,
                                                       Rate = s.Rate,
                                                       Amount = s.Amount
                                                   }).ToList()
                                               }).ToList());
                            SubLadegerInfos.Orders.AddRange(_appDbContext.PurchaseReturnOrders
                                              .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.LedgerId)
                                              .OrderBy(t => t.TransactionDate)
                                              .Select(t => new PartyReportOrderModel
                                              {
                                                  TransactionDate = t.TransactionDate,
                                                  TransactionNo = t.TransactionNo,
                                                  //TransactionType = t.TransactionType,
                                                  GrandTotal = t.GrandTotal,
                                                  Naration = t.Narration,
                                                  DrCr = "Cr",
                                                  BranchName = t.Branch.BranchName,
                                                  Transactions = t.PurchaseReturnTransactions.Where(s => s.Fk_PurchaseReturnOrderId == t.PurchaseReturnOrderId)
                                              .Select(s => new PartyReportTransactionModel
                                              {
                                                  ProductName = s.Product.ProductName,
                                                  Quantity = s.UnitQuantity,
                                                  Rate = s.Rate,
                                                  Amount = s.Amount,
                                              }).ToList()
                                              }).ToList());
                            SubLadegerInfos.Orders.AddRange(_appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == requestData.LedgerId)
                                .OrderBy(t => t.VoucherDate)
                                .Select(t => new PartyReportOrderModel
                                {
                                    TransactionDate = t.VoucherDate,
                                    TransactionNo = t.VouvherNo,
                                    Naration = t.Narration,
                                    GrandTotal = t.Amount,
                                    DrCr = "Cr",
                                    BranchName = t.Branch.BranchName,
                                }).ToList());
                            SubLadegerInfos.Orders.AddRange(_appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == requestData.LedgerId)
                                .OrderBy(t => t.VoucherDate)
                                .Select(t => new PartyReportOrderModel
                                {
                                    TransactionDate = t.VoucherDate,
                                    TransactionNo = t.VouvherNo,
                                    Naration = t.Narration,
                                    GrandTotal = t.Amount,
                                    DrCr = "Cr",
                                    BranchName = t.Branch.BranchName,
                                }).ToList());
                            SubLadegerInfos.Orders = SubLadegerInfos.Orders.OrderBy(t => t.TransactionDate).ToList();
                        }
                        if (SubLadegerInfos != null)
                        {
                            _Result.SingleObjData = SubLadegerInfos;
                            _Result.IsSuccess = true;
                            _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                        }
                    }

                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedLabourReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region TrailBalance
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
                        var allLedgerIds = ledgerIdsFromLedger.Union(ledgerIdsFromLedgerDev).Distinct().ToList();
                        var Data = new List<LedgerTrialBalanceModel>();
                        foreach (var LedgerId in allLedgerIds)
                        {
                            var PaymentDebittotal = await _appDbContext.Payments.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId || s.CashBankLedgerId == LedgerId)).SumAsync(s => s.Amount);
                            var ReceiptsCredittotal = await _appDbContext.Receipts.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId || s.CashBankLedgerId == LedgerId)).SumAsync(s => s.Amount);
                            var journalDebitTotal = await _appDbContext.Journals.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId && s.DrCr == "Dr")).SumAsync(s => s.Amount);
                            var journalCreditTotal = await _appDbContext.Journals.Where(s => s.VoucherDate >= convertedFromDate && s.VoucherDate <= convertedToDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId && s.DrCr == "Cr")).SumAsync(s => s.Amount);
                            var Credittotal = ReceiptsCredittotal + journalCreditTotal;
                            var Debittotal = PaymentDebittotal + journalDebitTotal;
                            var LagedrOpeningbal = _appDbContext.LedgerBalances.Where(l => l.Fk_LedgerId == LedgerId && l.Fk_BranchId == BranchId && l.Fk_FinancialYear == FinancialYearId).Select(l => l.RunningBalance).SingleOrDefault();
                            var OpeningBalance = LagedrOpeningbal + (await _appDbContext.Payments.Where(s => s.VoucherDate < convertedFromDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId || s.CashBankLedgerId == LedgerId)).SumAsync(s => s.Amount) + await _appDbContext.Journals.Where(s => s.VoucherDate < convertedFromDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == LedgerId && s.DrCr == "Dr").SumAsync(s => s.Amount))
                            + (await _appDbContext.Receipts.Where(s => s.VoucherDate < convertedFromDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && (s.Fk_LedgerId == LedgerId || s.CashBankLedgerId == LedgerId)).SumAsync(s => s.Amount) + await _appDbContext.Journals.Where(s => s.VoucherDate < convertedFromDate && s.Fk_BranchId == BranchId && s.Fk_FinancialYearId == FinancialYearId && s.Fk_LedgerId == LedgerId && s.DrCr == "Cr").SumAsync(s => s.Amount));
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedLabourReport : {_Exception.Message}");
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
                                       Narration = s.Narration,
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"MasterRepo/GetAllProducts : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region RefaranceReport
        public async Task<Result<PartyReportModel>> GetCustomerRefranceReport(PartyReportDataRequest requestData)
        {
            Result<PartyReportModel> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                List<PartyReportModel> Models = new();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    if (_HttpContextAccessor.HttpContext.Session.GetString("BranchId") != "All")
                    {
                        Guid BranchId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("BranchId"));

                        var data = await _appDbContext.Parties
    .Where(p => p.Fk_RefarenceId == requestData.Fk_RefaranceId)
    .Select(p => new PartyReportModel
    {
        Fk_SubledgerId = p.Fk_SubledgerId,
        RefarenceName = p.Referance.ReferanceName,
        PartyName = p.PartyName,
        OpeningBal = _appDbContext.SubLedgerBalances
            .Where(sb => sb.Fk_SubLedgerId == p.Fk_SubledgerId && sb.Fk_FinancialYearId == FinancialYearId && sb.Fk_BranchId == BranchId)
            .Sum(sb => sb.OpeningBalance) +
            _appDbContext.SalesOrders
                .Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == p.Fk_SubledgerId)
                .Sum(so => so.GrandTotal) -
            _appDbContext.SalesReturnOrders
                .Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == p.Fk_SubledgerId)
                .Sum(so => so.GrandTotal) -
            _appDbContext.Receipts
                .Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == p.Fk_SubledgerId)
                .Sum(r => r.Amount),
        OpeningBalType = _appDbContext.SubLedgerBalances
            .Where(sb => sb.Fk_SubLedgerId == p.Fk_SubledgerId && sb.Fk_FinancialYearId == FinancialYearId && sb.Fk_BranchId == BranchId)
            .Sum(sb => sb.OpeningBalance) +
            _appDbContext.SalesOrders
                .Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == p.Fk_SubledgerId)
                .Sum(so => so.GrandTotal) -
            _appDbContext.SalesReturnOrders
                .Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == p.Fk_SubledgerId)
                .Sum(so => so.GrandTotal) -
            _appDbContext.Receipts
                .Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == p.Fk_SubledgerId)
                .Sum(r => r.Amount) > 0 ? "Dr" : "Cr",
        Balance = _appDbContext.SalesOrders
            .Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate >= convertedFromDate && so.TransactionDate <= convertedToDate && so.Fk_SubLedgerId == p.Fk_SubledgerId)
            .Sum(so => so.GrandTotal) -
            _appDbContext.SalesReturnOrders
                .Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == p.Fk_SubledgerId)
                .Sum(so => so.GrandTotal) -
            _appDbContext.Receipts
                .Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == p.Fk_SubledgerId)
                .Sum(r => r.Amount),
        BalanceType = _appDbContext.SalesOrders
            .Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate >= convertedFromDate && so.TransactionDate <= convertedToDate && so.Fk_SubLedgerId == p.Fk_SubledgerId)
            .Sum(so => so.GrandTotal) -
            _appDbContext.SalesReturnOrders
                .Where(so => so.Fk_FinancialYearId == FinancialYearId && so.Fk_BranchId == BranchId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == p.Fk_SubledgerId)
                .Sum(so => so.GrandTotal) -
            _appDbContext.Receipts
                .Where(r => r.Fk_FinancialYearId == FinancialYearId && r.Fk_BranchId == BranchId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == p.Fk_SubledgerId)
                .Sum(r => r.Amount) > 0 ? "Dr" : "Cr",
    })
    .OrderBy(p => p.PartyName)
    .ToListAsync();

                        Models.AddRange(data);


                    }
                    else
                    {
                        var data = await _appDbContext.SubLedgers
                            .Where(s => s.Fk_LedgerId == MappingLedgers.SundryDebtors)
                            .Select(s => new PartyReportModel
                            {
                                Fk_SubledgerId = s.SubLedgerId,
                                PartyName = s.SubLedgerName,
                                OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == s.SubLedgerId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                                DrAmt = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum(),
                                CrAmt = _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            + _appDbContext.Receipts.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.VoucherDate >= convertedFromDate && p.VoucherDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                Balance = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum(),
                                BalanceType = _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.GrandTotal).Sum()
                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == s.SubLedgerId).Select(t => t.Amount).Sum()
                            > 0 ? "Dr" : "Cr",
                            }).OrderBy(p => p.PartyName).ToListAsync();
                        Models.AddRange(data);
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
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetSummerizedCustomerReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        //For All Branches Reports Supplyer And Customer
        #region Supplyer Report All Branches
        public async Task<Result<PartyReportModel2>> GetDetailedSupplyerReportForAll(PartyReportDataRequest requestData)
        {
            Result<PartyReportModel2> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                PartyReportModel2 SupllyerInfos = new PartyReportModel2();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                    SupllyerInfos.PartyName = _appDbContext.SubLedgers.Where(x => x.SubLedgerId == requestData.PartyId).Select(x => x.SubLedgerName).FirstOrDefault();
                    SupllyerInfos.OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                      + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                      - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                      - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum();
                    SupllyerInfos.OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                        + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                        - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                        - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum()
                                        > 0 ? "Dr" : "Cr";
                    SupllyerInfos.Orders.AddRange(_appDbContext.PurchaseOrders
                                       .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                       .OrderBy(t => t.TransactionDate)
                                       .Select(t => new PartyReportOrderModel
                                       {
                                           TransactionDate = t.TransactionDate,
                                           TransactionNo = t.TransactionNo,
                                           //TransactionType = t.TransactionType,
                                           GrandTotal = t.GrandTotal,
                                           Naration = t.Narration,
                                           DrCr = "Dr",
                                           BranchName = t.Branch.BranchName,
                                           Transactions = t.PurchaseTransactions.Where(s => s.Fk_PurchaseOrderId == t.PurchaseOrderId)
                                           .Select(s => new PartyReportTransactionModel
                                           {
                                               ProductName = s.Product.ProductName,
                                               Quantity = s.UnitQuantity,
                                               AlternateQuantity = s.AlternateQuantity,
                                               Rate = s.Rate,
                                               Amount = s.Amount
                                           }).ToList()
                                       }).ToList());
                    SupllyerInfos.Orders.AddRange(_appDbContext.PurchaseReturnOrders
                                      .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                      .OrderBy(t => t.TransactionDate)
                                      .Select(t => new PartyReportOrderModel
                                      {
                                          TransactionDate = t.TransactionDate,
                                          TransactionNo = t.TransactionNo,
                                          //TransactionType = t.TransactionType,
                                          GrandTotal = t.GrandTotal,
                                          Naration = t.Narration,
                                          DrCr = "Cr",
                                          BranchName = t.Branch.BranchName,
                                          Transactions = t.PurchaseReturnTransactions.Where(s => s.Fk_PurchaseReturnOrderId == t.PurchaseReturnOrderId)
                                      .Select(s => new PartyReportTransactionModel
                                      {
                                          ProductName = s.Product.ProductName,
                                          Quantity = s.UnitQuantity,
                                          AlternateQuantity = s.AlternateQuantity,
                                          Rate = s.Rate,
                                          Amount = s.Amount,
                                      }).ToList()
                                      }).ToList());
                    SupllyerInfos.Orders.AddRange(_appDbContext.Payments.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == requestData.PartyId)
                        .OrderBy(t => t.VoucherDate)
                        .Select(t => new PartyReportOrderModel
                        {
                            TransactionDate = t.VoucherDate,
                            TransactionNo = t.VouvherNo,
                            Naration = t.Narration,
                            GrandTotal = t.Amount,
                            DrCr = "Cr",
                            BranchName = t.Branch.BranchName,
                        }).ToList());
                    SupllyerInfos.Orders = SupllyerInfos.Orders.OrderBy(t => t.TransactionDate).ToList();

                    if (SupllyerInfos != null)
                    {
                        _Result.SingleObjData = SupllyerInfos;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedSupplyerReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
        #region Customer Report For All Branches
        public async Task<Result<PartyReportModel2>> GetDetailedCustomerReportForAll(PartyReportDataRequest requestData)
        {
            Result<PartyReportModel2> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                PartyReportModel2 PartyInfos = new PartyReportModel2();
                if (DateTime.TryParseExact(requestData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedFromDate) && DateTime.TryParseExact(requestData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedToDate))
                {
                    Guid FinancialYearId = Guid.Parse(_HttpContextAccessor.HttpContext.Session.GetString("FinancialYearId"));
                   
                        PartyInfos.OpeningBal = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                         + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                         - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                         - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum();
                        PartyInfos.OpeningBalType = _appDbContext.SubLedgerBalances.Where(x => x.Fk_SubLedgerId == requestData.PartyId && x.Fk_FinancialYearId == FinancialYearId).Select(t => t.OpeningBalance).Sum()
                                            + _appDbContext.SalesOrders.Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate < convertedFromDate && p.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                            - _appDbContext.SalesReturnOrders.Where(so => so.Fk_FinancialYearId == FinancialYearId && so.TransactionDate < convertedFromDate && so.Fk_SubLedgerId == requestData.PartyId).Select(t => t.GrandTotal).Sum()
                                            - _appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate < convertedFromDate && r.Fk_SubLedgerId == requestData.PartyId).Select(t => t.Amount).Sum()
                                            > 0 ? "Dr" : "Cr";
                        PartyInfos.Orders.AddRange(_appDbContext.SalesOrders
                                           .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                           .OrderBy(t => t.TransactionDate)
                                           .Select(t => new PartyReportOrderModel
                                           {
                                               TransactionDate = t.TransactionDate,
                                               TransactionNo = t.TransactionNo,
                                               TransactionType = t.TransactionType,
                                               GrandTotal = t.GrandTotal,
                                               SiteAdress = t.SiteAdress,
                                               Naration = t.Narration,
                                               DrCr = "Dr",
                                               BranchName = t.Branch.BranchName,
                                               Transactions = t.SalesTransactions.Where(s => s.Fk_SalesOrderId == t.SalesOrderId)
                                               .Select(s => new PartyReportTransactionModel
                                               {
                                                   ProductName = _appDbContext.Products.Where(p => p.ProductId == s.Fk_ProductId).Select(p => p.ProductName).FirstOrDefault(),
                                                   Quantity = s.UnitQuantity,
                                                   AlternateUnit = _appDbContext.AlternateUnits.Where(a => a.FK_ProductId == s.Fk_ProductId).Select(a => a.AlternateUnitName).FirstOrDefault(),
                                                   Unit = (from p in _appDbContext.Products
                                                           join u in _appDbContext.Units on p.Fk_UnitId equals u.UnitId
                                                           where p.ProductId == s.Fk_ProductId
                                                           select u.UnitName).FirstOrDefault(),
                                                   Rate = s.Rate,
                                                   Amount = s.Amount
                                               }).ToList()
                                           }).ToList());
                        PartyInfos.Orders.AddRange(_appDbContext.SalesReturnOrders
                                          .Where(p => p.Fk_FinancialYearId == FinancialYearId && p.TransactionDate >= convertedFromDate && p.TransactionDate <= convertedToDate && p.Fk_SubLedgerId == requestData.PartyId)
                                          .OrderBy(t => t.TransactionDate)
                                          .Select(t => new PartyReportOrderModel
                                          {
                                              TransactionDate = t.TransactionDate,
                                              TransactionNo = t.TransactionNo,
                                              TransactionType = t.TransactionType,
                                              GrandTotal = t.GrandTotal,
                                              SiteAdress = t.SiteAdress,
                                              Naration = t.Narration,
                                              DrCr = "Cr",
                                              BranchName = t.Branch.BranchName,
                                              Transactions = t.SalesReturnTransactions.Where(s => s.Fk_SalesReturnOrderId == t.SalesReturnOrderId)
                                          .Select(s => new PartyReportTransactionModel
                                          {
                                              ProductName = _appDbContext.Products.Where(p => p.ProductId == s.Fk_ProductId).Select(p => p.ProductName).FirstOrDefault(),
                                              Quantity = s.UnitQuantity,
                                              AlternateUnit = _appDbContext.AlternateUnits.Where(a => a.FK_ProductId == s.Fk_ProductId).Select(a => a.AlternateUnitName).FirstOrDefault(),
                                              Rate = s.Rate,
                                              Unit = (from p in _appDbContext.Products
                                                      join u in _appDbContext.Units on p.Fk_UnitId equals u.UnitId
                                                      where p.ProductId == s.Fk_ProductId
                                                      select u.UnitName).FirstOrDefault(),
                                              Amount = s.Amount,
                                          }).ToList()
                                          }).ToList());
                        PartyInfos.Orders.AddRange(_appDbContext.Receipts.Where(r => r.Fk_FinancialYearId == FinancialYearId && r.VoucherDate >= convertedFromDate && r.VoucherDate <= convertedToDate && r.Fk_SubLedgerId == requestData.PartyId)
                            .OrderBy(t => t.VoucherDate)
                            .Select(t => new PartyReportOrderModel
                            {
                                TransactionDate = t.VoucherDate,
                                TransactionNo = t.VouvherNo,
                                Naration = t.Narration,
                                SiteAdress = "-",
                                GrandTotal = t.Amount,
                                DrCr = "Cr",
                                BranchName = t.Branch.BranchName,
                            }).ToList());
                        PartyInfos.Orders = PartyInfos.Orders.OrderBy(t => t.TransactionDate).ToList();
                    
                    if (PartyInfos != null)
                    {
                        _Result.SingleObjData = PartyInfos;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                        _Result.IsSuccess = true;
                    }
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
                await _emailService.SendExceptionEmail("horizonexception@gmail.com", "FMS Excepion", $"ReportRepo/GetDetailedCustomerReport : {_Exception.Message}");
            }
            return _Result;
        }
        #endregion
    }
}

