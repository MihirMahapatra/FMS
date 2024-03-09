using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Repository.Accounting;
using FMS.Utility;

namespace FMS.Service.Accounting
{
    public class AccountingSvcs : IAccountingSvcs
    {
        private readonly IAccountingRepo _accountingRepo;
        public AccountingSvcs(IAccountingRepo accountingRepo)
        {
            _accountingRepo = accountingRepo;
        }
        #region Journal
        public async Task<Base> GetJournalVoucherNo()
        {

            var Result = await _accountingRepo.GetJournalVoucherNo();
            return new Base()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                Data = Result.SingleObjData,
            };
        }
        public async Task<Base> CreateJournal(JournalDataRequest requestData)
        {
            Base Obj;
            var Result = await _accountingRepo.CreateJournal(requestData);
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
        public async Task<JournalViewModel> GetJournals()
        {
            JournalViewModel Obj;
            var Result = await _accountingRepo.GetJournals();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        GroupedJournals = Result.CollectionObjData,
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
        public async Task<JournalViewModel> GetJournalById(string Id)
        {
            JournalViewModel Obj;
            var Result = await _accountingRepo.GetJournalById(Id);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        GroupedJournals = Result.CollectionObjData,
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
        public async Task<Base> DeleteJournal(string Id)
        {
            var Result = await _accountingRepo.DeleteJournal(Id, null);
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
        #region Payment
        public async Task<Base> GetPaymentVoucherNo(string CashBank)
        {

            var Result = await _accountingRepo.GetPaymentVoucherNo(CashBank);
            return new Base()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                Data = Result.SingleObjData,
            };
        }
        public async Task<LedgerViewModel> GetBankLedgers()
        {
            LedgerViewModel Obj;
            var Result = await _accountingRepo.GetBankLedgers();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Ledgers = Result.CollectionObjData,
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
        public async Task<Base> CreatePayment(PaymentDataRequest requestData)
        {
            Base Obj;
            var Result = await _accountingRepo.CreatePayment(requestData);
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
        public async Task<PaymentViewModel> GetPayments()
        {
            PaymentViewModel Obj;
            var Result = await _accountingRepo.GetPayments();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        GroupedPayments = Result.CollectionObjData,
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
        public async Task<PaymentViewModel> GetPaymentById(string Id)
        {
            PaymentViewModel Obj;
            var Result = await _accountingRepo.GetPaymentById(Id);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        GroupedLederwisePayments = Result.CollectionObjData,
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
        public async Task<Base> DeletePayment(string Id)
        {
            var Result = await _accountingRepo.DeletePayment(Id, null);
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
        #region Receipt
        public async Task<Base> GetReceiptVoucherNo(string CashBank)
        {

            var Result = await _accountingRepo.GetReceiptVoucherNo(CashBank);
            return new Base()
            {
                ResponseStatus = Result.Response,
                ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                Data = Result.SingleObjData,
            };
        }
        public async Task<Base> CreateRecipt(ReciptsDataRequest requestData)
        {
            Base Obj;
            var Result = await _accountingRepo.CreateRecipt(requestData);
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
        public async Task<ReceiptViewModel> GetReceipts()
        {
            ReceiptViewModel Obj;
            var Result = await _accountingRepo.GetReceipts();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        GroupedReceipts = Result.CollectionObjData,
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
        public async Task<ReceiptViewModel> GetReceiptById(string Id)
        {
            ReceiptViewModel Obj;
            var Result = await _accountingRepo.GetReceipts();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        GroupedReceipts = Result.CollectionObjData,
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
        public async Task<Base> DeleteReceipt(string Id)
        {
            var Result = await _accountingRepo.DeleteRecipt(Id, null);
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
    }
}
