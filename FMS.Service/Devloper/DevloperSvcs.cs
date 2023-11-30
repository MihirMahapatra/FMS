using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Model.ViewModel;
using FMS.Repository.Devloper;
using FMS.Utility;

namespace FMS.Service.Devloper
{
    public class DevloperSvcs : IDevloperSvcs
    {
        private readonly IDevloperRepo _devloperRepo;
        public DevloperSvcs(IDevloperRepo devloperRepo)
        {
            _devloperRepo = devloperRepo;
        }
        #region Branch
        public async Task<BranchViewModel> GetAllBranch()
        {
            BranchViewModel Obj;
            var Result = await _devloperRepo.GetAllBranch();
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Branches = Result.CollectionObjData,
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
        public async Task<BranchViewModel> GetBranchAccordingToUser(string UserId)
        {
            BranchViewModel Obj;
            var Result = await _devloperRepo.GetBranchAccordingToUser(UserId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    List<BranchModel> data = new()
                    {
                        Result.SingleObjData
                    };

                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        Branches = data,
                        Branch =Result.SingleObjData
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
        public async Task<Base> CreateBranch(BranchModel data)
        {
            Base Obj;
            var Result = await _devloperRepo.CreateBranch(data);

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
        public async Task<Base> UpdateBranch(BranchModel data)
        {
            var Result = await _devloperRepo.UpdateBranch(data);
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
        public async Task<Base> DeleteBranch(Guid Id)
        {
            var Result = await _devloperRepo.DeleteBranch(Id, null);
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
        #region Financial Year
        public async Task<FinancialYearViewModel> GetFinancialYears(Guid BranchId)
        {
            FinancialYearViewModel Obj;
            var Result = await _devloperRepo.GetFinancialYears(BranchId);

            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        FinancialYears = Result.CollectionObjData,
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
        public async Task<FinancialYearViewModel> GetFinancialYears()
        {
            FinancialYearViewModel Obj;
            var Result = await _devloperRepo.GetFinancialYears();

            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        FinancialYears = Result.CollectionObjData,
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
        public async Task<Base> CreateFinancialYear(FinancialYearModel data)
        {
            Base Obj;
            var Result = await _devloperRepo.CreateFinancialYear(data);
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
        public async Task<Base> UpdateFinancialYear(FinancialYearModel data)
        {
            var Result = await _devloperRepo.UpdateFinancialYear(data);
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
        public async Task<Base> DeleteFinancialYear(Guid Id)
        {
            var Result = await _devloperRepo.DeleteFinancialYear(Id, null);
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
        #region Accounting Setup
        #region LedgerGroup
        public async Task<LedgerGroupViewModel> GetLedgerGroups()
        {
            LedgerGroupViewModel Obj;
            var Result = await _devloperRepo.GetLedgerGroups();

            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        LedgerGroups = Result.CollectionObjData,
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
        public async Task<Base> CreateLedgerGroup(LedgerGroupModel data)
        {
            Base Obj;
            var Result = await _devloperRepo.CreateLedgerGroup(data);
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
        public async Task<Base> UpdateLedgerGroup(LedgerGroupModel data)
        {
            var Result = await _devloperRepo.UpdateLedgerGroup(data);
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
        public async Task<Base> DeleteLedgerGroup(Guid Id)
        {
            var Result = await _devloperRepo.DeleteLedgerGroup(Id, null);
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
        #region LedgerSubGroup
        public async Task<LedgerSubGroupViewModel> GetLedgerSubGroups(Guid BranchId, Guid GroupId)
        {
            LedgerSubGroupViewModel Obj;
            var Result = await _devloperRepo.GetLedgerSubGroups(BranchId, GroupId);
            if (Result.IsSuccess)
            {
                if (Result.Response == "success")
                {
                    Obj = new LedgerSubGroupViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                        LedgerSubGroups = Result.CollectionObjData,
                    };
                }
                else
                {
                    Obj = new LedgerSubGroupViewModel()
                    {
                        ResponseStatus = Result.Response,
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                        Message = "No Record Found"
                    };
                }
            }
            else
            {
                Obj = new LedgerSubGroupViewModel()
                {
                    ResponseStatus = Result.Response,
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    Exception = Result.Exception,
                    Message = "Some Eroor Occoured"
                };
            }
            return Obj;
        }
        public async Task<Base> CreateLedgerSubGroup(LedgerSubGroupModel data)
        {
            Base Obj;
            var Result = await _devloperRepo.CreateLedgerSubGroup(data);
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
        public async Task<Base> UpdateLedgerSubGroup(LedgerSubGroupModel data)
        {
            var Result = await _devloperRepo.UpdateLedgerSubGroup(data);
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
        public async Task<Base> DeleteLedgerSubGroup(Guid BranchId, Guid Id)
        {
            var Result = await _devloperRepo.DeleteLedgerSubGroup(BranchId, Id, null);
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
        #region Ledger
        public async Task<LedgerViewModel> GetLedgers()
        {
            LedgerViewModel Obj;
            var Result = await _devloperRepo.GetLedgers();
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
        public async Task<Base> CreateLedger(List<LedgerModel> listData)
        {
            Base Obj;
            var Result = await _devloperRepo.CreateLedger(listData);
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
        public async Task<Base> UpdateLedger(LedgerModel data)
        {
            var Result = await _devloperRepo.UpdateLedger(data);
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
        public async Task<Base> DeleteLedger(Guid Id)
        {
            var Result = await _devloperRepo.DeleteLedger(Id, null);
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
