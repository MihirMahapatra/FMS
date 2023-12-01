using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Repository.Transaction;
using FMS.Utility;

namespace FMS.Service.Transaction
{
    public class TransactionSvcs : ITransactionSvcs
    {
        private readonly ITransactionRepo _transactionRepo;
        public TransactionSvcs(ITransactionRepo transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }
        #region Purchase Transaction
        public async Task<SubLedgerViewModel> GetSundryCreditors(Guid PartyTypeId)
        {
            SubLedgerViewModel Obj;
            var Result = await _transactionRepo.GetSundryCreditors(PartyTypeId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        SubLedgers = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        #region Purchase
        public async Task<Base> GetLastPurchaseTransaction()
        {
            var Result = await _transactionRepo.GetLastPurchaseTransaction();
            return new Base()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                Data = Result.SingleObjData,
            };
        }
        public async Task<PurchaseOrderViewModel> GetPurchases()
        {
            PurchaseOrderViewModel Obj;
            var Result = await _transactionRepo.GetPurchases();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        purchaseOrders = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<PurchaseOrderViewModel> GetPurchaseById(Guid Id)
        {
            PurchaseOrderViewModel Obj;
            var Result = await _transactionRepo.GetPurchaseById(Id);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        purchaseOrder = Result.SingleObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> CreatePurchase(PurchaseDataRequest data)
        {
            Base Obj;
            var Result = await _transactionRepo.CreatePurchase(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Save Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> UpdatePurchase(PurchaseDataRequest data)
        {
            Base Obj;
            var Result = await _transactionRepo.UpdatePurchase(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> DeletePurchase(Guid Id)
        {
            var Result = await _transactionRepo.DeletePurchase(Id, null, false);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        #endregion
        #region Purchase return
        public async Task<Base> GetLastPurchaseReturnTransaction()
        {
            var Result = await _transactionRepo.GetLastPurchaseReturnTransaction();
            return new Base()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                Data = Result.SingleObjData,
            };
        }
        public async Task<PurchaseReturnOrderViewModel> GetPurchaseReturns()
        {
            PurchaseReturnOrderViewModel Obj;
            var Result = await _transactionRepo.GetPurchaseReturns();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        purchaseReturnOrders = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<PurchaseReturnOrderViewModel> GetPurchaseReturnById(Guid Id)
        {
            PurchaseReturnOrderViewModel Obj = new PurchaseReturnOrderViewModel();
            var Result = await _transactionRepo.GetPurchaseReturnById(Id);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new PurchaseReturnOrderViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        purchaseReturnOrder = Result.SingleObjData,
                    };
                }
                else
                {
                    Obj = new PurchaseReturnOrderViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new PurchaseReturnOrderViewModel()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> CreatetPurchaseReturn(PurchaseDataRequest data)
        {
            Base Obj;
            var Result = await _transactionRepo.CreatetPurchaseReturn(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Save Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> UpdatetPurchaseReturn(PurchaseDataRequest data)
        {
            Base Obj = new();
            var Result = await _transactionRepo.UpdatetPurchaseReturn(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new Base
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new Base
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new Base
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> DeletetPurchaseReturn(Guid Id)
        {
            var Result = await _transactionRepo.DeletetPurchaseReturn(Id, null, false);
            Base Obj = new();
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new Base
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new Base
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new Base
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        #endregion
        #endregion
        #region Production 
        public async Task<Base> GetLastProductionNo()
        {
            var Result = await _transactionRepo.GetLastProductionNo();
            return new Base()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                Data = Result.SingleObjData,
            };

        }
        public async Task<ProductionViewModel> GetProductionConfig(Guid ProductId)
        {
            ProductionViewModel Obj;
            var Result = await _transactionRepo.GetProductionConfig(ProductId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Productions = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<ProductionEntryViewModel> GetProductionEntry()
        {
            ProductionEntryViewModel Obj;
            var Result = await _transactionRepo.GetProductionEntry();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        ProductionEntries = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateProductionEntry(ProductionEntryRequest data)
        {
            Base Obj;
            var Result = await _transactionRepo.CreateProductionEntry(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    ErrorMsg = Result.WarningMessage
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateProductionEntry(ProductionEntryModel data)
        {
            var Result = await _transactionRepo.UpdateProductionEntry(data);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteProductionEntry(Guid Id)
        {
            var Result = await _transactionRepo.DeleteProductionEntry(Id, null, false);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        #endregion
        #region Sales Transaction
        public async Task<SubLedgerViewModel> GetSundryDebtors(Guid PartyTypeId)
        {
            SubLedgerViewModel Obj;
            var Result = await _transactionRepo.GetSundryDebtors(PartyTypeId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        SubLedgers = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        #region Sales
        public async Task<Base> GetLastSalesTransaction()
        {
            var Result = await _transactionRepo.GetLastSalesTransaction();
            return new Base()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                Data = Result.SingleObjData,
            };
        }
        public async Task<Base> CreateSale(SalesDataRequest data)
        {
            Base Obj;
            var Result = await _transactionRepo.CreateSales(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Save Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<SalesOrderViewModel> GetSales()
        {
            SalesOrderViewModel Obj;
            var Result = await _transactionRepo.GetSales();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        salesOrders = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<SalesOrderViewModel> GetSalesById(Guid Id)
        {
            SalesOrderViewModel Obj;
            var Result = await _transactionRepo.GetSalesById(Id);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        salesOrder = Result.SingleObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> UpdatSales(SalesDataRequest data)
        {
            Base Obj;
            var Result = await _transactionRepo.UpdateSales(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteSales(Guid Id)
        {
            var Result = await _transactionRepo.DeleteSales(Id, null, false);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        #endregion
        #region Sales Return
        public async Task<Base> GetLastSalesReturnTransaction()
        {
            var Result = await _transactionRepo.GetLastSalesReturnTransaction();
            return new Base()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                Data = Result.SingleObjData,
            };
        }
        public async Task<Base> CreateSalesReturn(SalesDataRequest data)
        {
            Base Obj = new();
            var Result = await _transactionRepo.CreateSalesReturn(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new Base
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new Base
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Save Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new Base
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<SalesReturnOrderViewModel> GetSalesReturns()
        {
            SalesReturnOrderViewModel Obj = new SalesReturnOrderViewModel();
            var Result = await _transactionRepo.GetSalesReturns();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new SalesReturnOrderViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        SalesReturnOrders = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new SalesReturnOrderViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new SalesReturnOrderViewModel()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<SalesReturnOrderViewModel> GetSalesReturnById(Guid Id)
        {
            SalesReturnOrderViewModel Obj = new SalesReturnOrderViewModel();
            var Result = await _transactionRepo.GetSalesReturnById(Id);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new SalesReturnOrderViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        SalesReturnOrder = Result.SingleObjData,
                    };
                }
                else
                {
                    Obj = new SalesReturnOrderViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new SalesReturnOrderViewModel()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateSalesReturn(SalesDataRequest data)
        {
            Base Obj = new();
            var Result = await _transactionRepo.UpdateSalesReturn(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new Base
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new Base
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }

            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new Base
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteSalesReturn(Guid Id)
        {
            var Result = await _transactionRepo.DeleteSalesReturn(Id, null, false);
            Base Obj = new();
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new Base
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new Base
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new Base
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        #endregion
        #endregion
        #region Inward Supply Transaction
        public async Task<Base> GetLastInwardSupply()
        {
            var Result = await _transactionRepo.GetLastInwardSupply();
            return new Base()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                Data = Result.SingleObjData,
            };
        }
        public async Task<InwardSupplyViewModel> GetInwardSupply()
        {
            InwardSupplyViewModel Obj;
            var Result = await _transactionRepo.GetInwardSupply();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        InwardSupplies = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<InwardSupplyViewModel> GetInwardSupplyById(Guid Id)
        {
            InwardSupplyViewModel Obj;
            var Result = await _transactionRepo.GetInwardSupplyById(Id);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new InwardSupplyViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        InwardSupply = Result.SingleObjData,
                    };
                }
                else
                {
                    Obj = new InwardSupplyViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new InwardSupplyViewModel()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> CreateInwardSupply(SupplyDataRequest data)
        {
            Base Obj;
            var Result = await _transactionRepo.CreateInwardSupply(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Save Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateInwardSupply(SupplyDataRequest data)
        {
            Base Obj;
            var Result = await _transactionRepo.UpdateInwardSupply(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteInwardSupply(Guid Id)
        {
            var Result = await _transactionRepo.DeleteInwardSupply(Id, null, false);
            Base Obj;
             if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }

        #endregion
        #region Outward Supply Transaction
        public async Task<Base> GetLastOutwardSupply()
        {
            var Result = await _transactionRepo.GetLastOutwardSupply();
            return new Base()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                Data = Result.SingleObjData,
            };
        }
        public async Task<OutwardSupplyViewModel> GetOutwardSupply()
        {
            OutwardSupplyViewModel Obj;
            var Result = await _transactionRepo.GetOutwardSupply();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        OutwardSupplies = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<OutwardSupplyViewModel> GetOutwardSupplyById(Guid Id)
        {
            OutwardSupplyViewModel Obj;
            var Result = await _transactionRepo.GetOutwardSupplyById(Id);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new OutwardSupplyViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        OutwardSupply = Result.SingleObjData,
                    };
                }
                else
                {
                    Obj = new OutwardSupplyViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new OutwardSupplyViewModel()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> CreateOutwardSupply(SupplyDataRequest data)
        {
            Base Obj;
            var Result = await _transactionRepo.CreateOutwardSupply(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Save Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async  Task<Base> UpdateOutwardSupply(SupplyDataRequest data)
        {
            Base Obj;
            var Result = await _transactionRepo.UpdateOutwardSupply(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteOutwardSupply(Guid Id)
        {
            var Result = await _transactionRepo.DeleteOutwardSupply(Id, null, false);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        #endregion
        #region Damage Transaction
        public async Task<Base> GetLastDamageEntry()
        {
            var Result = await _transactionRepo.GetLastDamageEntry();
            return new Base()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                Data = Result.SingleObjData,
            };
        }
        public async Task<DamageViewModel> GetDamages()
        {
            DamageViewModel Obj;
            var Result = await _transactionRepo.GetDamages();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        DamageOrders = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<DamageViewModel> GetDamageById(Guid Id)
        {
            DamageViewModel Obj;
            var Result = await _transactionRepo.GetDamageById(Id);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        DamageOrder = Result.SingleObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> CreateDamage(DamageRequestData data)
        {
            Base Obj;
            var Result = await _transactionRepo.CreateDamage(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Save Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateDamage(DamageRequestData data)
        {
            Base Obj;
            var Result = await _transactionRepo.UpdateDamage(data);
            if (Result.IsSuccess)
            {
                if (Result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (Result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteDamage(Guid Id)
        {
            var Result = await _transactionRepo.DeleteDamage(Id, null, false);
            Base Obj;
            if (Result.IsSuccess)
            {
                if (Result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    ErrorMsg = Result.Exception.InnerException.Message
                };
            }
            return Obj;
        }
        #endregion
    }
}
