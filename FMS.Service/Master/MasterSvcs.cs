using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Repository.Master;
using FMS.Utility;


namespace FMS.Service.Master
{
    public class MasterSvcs : IMasterSvcs
    {
        private readonly IMasterRepo _masterRepo;
        public MasterSvcs(IMasterRepo masterRepo)
        {
            _masterRepo = masterRepo;
        }
        #region Stock Master
   
        public async Task<StockViewModel> GetStocks()
        {
            StockViewModel Obj;
            var Result = await _masterRepo.GetStocks();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Stocks = Result.CollectionObjData,
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
        public async Task<ProductViewModel> GetProductsWhichNotInStock(Guid GroupId, Guid SubGroupId)
        {
            ProductViewModel Obj;
            var Result = await _masterRepo.GetProductsWhichNotInStock(GroupId, SubGroupId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        products = Result.CollectionObjData,
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
        public async Task<Base> CreateStock(StockModel data)
        {
            Base Obj;
            var Result = await _masterRepo.CreateStock(data);
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
        public async Task<Base> UpdateStock(StockModel data)
        {
            var Result = await _masterRepo.UpdateStock(data);
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
        public async Task<Base> DeleteStock(Guid Id)
        {
            var Result = await _masterRepo.DeleteStock(Id, null, false);
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
        #region labour Master
        #region Labour Type
        public async Task<LabourTypeViewModel> GetAllLabourTypes()
        {
            LabourTypeViewModel Obj;
            var Result = await _masterRepo.GetAllLabourTypes();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        LabourTypes = Result.CollectionObjData,
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
        #endregion
        #region Labour Details
        public async Task<LabourViewModel> GetAllLabourDetails()
        {
            LabourViewModel Obj;
            var Result = await _masterRepo.GetAllLabourDetails();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Labours = Result.CollectionObjData,
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
        public async Task<LabourViewModel> GetLabourDetailById(Guid LabourId)
        {
            LabourViewModel Obj;
            var Result = await _masterRepo.GetLabourDetailById(LabourId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Labour = Result.SingleObjData,
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
        public async Task<Base> CreateLabourDetail(LabourModel data)
        {
            Base Obj;
            var Result = await _masterRepo.CreateLabourDetail(data);
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
        public async Task<Base> UpdateLabourDetail(LabourModel data)
        {
            var Result = await _masterRepo.UpdateLabourDetail(data);
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
        public async Task<Base> DeleteLabourDetail(Guid Id)
        {
            var Result = await _masterRepo.DeleteLabourDetail(Id, null, false);
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
        #endregion
        #region Party Master
        #region Party
        public async Task<PartyViewModel> GetParties()
        {
            PartyViewModel Obj;
            var Result = await _masterRepo.GetParties();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Parties = Result.CollectionObjData,
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
        public async Task<Base> CreateParty(PartyModel listData)
        {
            Base Obj;
            var Result = await _masterRepo.CreateParty(listData);
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
        public async Task<Base> UpdateParty(PartyModel data)
        {
            var Result = await _masterRepo.UpdateParty(data);
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
        public async Task<Base> DeleteParty(Guid Id)
        {
            var Result = await _masterRepo.DeleteParty(Id, null, false);
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
        #region State
        public async Task<StateViewModel> GetStates()
        {
            StateViewModel Obj;
            var Result = await _masterRepo.GetStates();

            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        States = Result.CollectionObjData,
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
        public async Task<Base> CreateState(StateModel data)
        {
            Base Obj;
            var Result = await _masterRepo.CreateState(data);
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
        public async Task<Base> UpdateState(StateModel data)
        {
            var Result = await _masterRepo.UpdateState(data);
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
        public async Task<Base> DeleteState(Guid Id)
        {
            var Result = await _masterRepo.DeleteState(Id, null, false);
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
        #region City
        public async Task<CityViewModel> GetCities(Guid Id)
        {
            CityViewModel Obj;
            var Result = await _masterRepo.GetCities(Id);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Cities = Result.CollectionObjData,
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
        public async Task<Base> CreateCity(CityModel data)
        {
            Base Obj;
            var Result = await _masterRepo.CreateCity(data);
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
                        ErrorMsg = "\r\nData Already Exist"
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
        public async Task<Base> UpdateCity(CityModel data)
        {
            var Result = await _masterRepo.UpdateCity(data);
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
        public async Task<Base> DeleteCity(Guid Id)
        {
            var Result = await _masterRepo.DeleteCity(Id, null, false);
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
        #endregion
        #region Account Master
        #region LedgerBalance
        public async Task<LedgerBalanceViewModel> GetLedgerBalances()
        {
            LedgerBalanceViewModel Obj;
            var Result = await _masterRepo.GetLedgerBalances();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        LedgerBalances = Result.CollectionObjData,
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
        public async Task<SubLedgerViewModel> GetSubLedgersByBranch(Guid LedgerId)
        {
            SubLedgerViewModel Obj;
            var Result = await _masterRepo.GetSubLedgersByBranch(LedgerId);
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
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateLedgerBalance(LedgerBalanceRequest data)
        {
            Base Obj;
            var Result = await _masterRepo.CreateLedgerBalance(data);
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
        public async Task<Base> UpdateLedgerBalance(LedgerBalanceModel data)
        {
            var Result = await _masterRepo.UpdateLedgerBalance(data);
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
        public async Task<Base> DeleteLedgerBalance(Guid Id)
        {
            var Result = await _masterRepo.DeleteLedgerBalance(Id, null, false);
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
        #region SubLedger
        public async Task<SubLedgerViewModel> GetSubLedgers()
        {
            SubLedgerViewModel Obj;
            var result = await _masterRepo.GetSubLedgers();
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        SubLedgers = result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<SubLedgerViewModel> GetSubLedgersById(Guid LedgerId)
        {
            SubLedgerViewModel Obj;
            var result = await _masterRepo.GetSubLedgersById(LedgerId);
            if (result.IsSuccess)
            {
                if (result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        SubLedgers = result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateSubLedger(SubLedgerDataRequest Data)
        {
            Base Obj;
            var result = await _masterRepo.CreateSubLedger(Data);
            if (result.IsSuccess)
            {
                if (result.Response == "created")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Created),
                        SuccessMsg = "Data Saved SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Data Already Exist"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> UpdateSubLedger(SubLedgerModel data)
        {
            var result = await _masterRepo.UpdateSubLedger(data);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "modified")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Updated SuccessFully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Update Data"
                    };
                }
            }
            else if (result.WarningMessage != null)
            {
                Obj = new()
                {
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = result.WarningMessage,
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<Base> DeleteSubLedger(Guid Id)
        {
            var result = await _masterRepo.DeleteSubLedger(Id, null, false);
            Base Obj;
            if (result.IsSuccess)
            {
                if (result.Response == "deleted")
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        SuccessMsg = "Data Deleted Successfully"
                    };
                }
                else
                {
                    Obj = new()
                    {
                        ResponseStatus = result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        ErrorMsg = "Failed To Delete Data"
                    };
                }
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = result.Exception,
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        #endregion
        #region SubLedger Balance
        public async Task<SubLedgerBalanceViewModel> GetSubLedgerBalances()
        {
            SubLedgerBalanceViewModel Obj;
            var Result = await _masterRepo.GetSubLedgerBalances();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        SubLedgerBalances = Result.CollectionObjData,
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
        public async Task<Base> UpdateSubLedgerBalance(SubLedgerBalanceModel data)
        {
            var Result = await _masterRepo.UpdateSubLedgerBalance(data);
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
        public async Task<Base> DeleteSubLedgerBalance(Guid Id)
        {
            var Result = await _masterRepo.DeleteSubLedgerBalance(Id, null, false);
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
        #endregion       
    }
}
