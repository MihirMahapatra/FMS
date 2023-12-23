using FMS.Model.CommonModel;
using FMS.Service.Admin;
using FMS.Service.Devloper;
using FMS.Service.Master;
using FMS.Service.Transaction;
using FMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Controllers.Transaction
{
    public class TransactionController : Controller
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public readonly ITransactionSvcs _transactionSvcs;
        public readonly IDevloperSvcs _devloperSvcs;
        private readonly IAdminSvcs _adminSvcs;
        private readonly IMasterSvcs _masterSvcs;
        #region Constructor
        public TransactionController(IHttpContextAccessor httpContextAccessor, ITransactionSvcs transactionSvcs, IDevloperSvcs devloperSvcs, IAdminSvcs adminSvcs, IMasterSvcs masterSvcs) : base()
        {
            _HttpContextAccessor = httpContextAccessor;
            _transactionSvcs = transactionSvcs;
            _devloperSvcs = devloperSvcs;
            _adminSvcs = adminSvcs;
            _masterSvcs = masterSvcs;
        }
        #endregion
        #region Purchase Transaction
        [HttpGet]
        public IActionResult PurchaseTransaction()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetSundryCreditors()
        {
            Guid PartyTypeId = MappingLedgers.SundryCreditors;
            var result = await _transactionSvcs.GetSundryCreditors(PartyTypeId);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductRawMaterial()
        {
            Guid ProductType = MappingProductType.RawMaterial;
            var result = await _adminSvcs.GetProductByTypeId(ProductType);
            return new JsonResult(result);
        }
        #region Purchase
        [HttpGet]
        public async Task<IActionResult> GetLastPurchaseTransaction()
        {
            var result = await _transactionSvcs.GetLastPurchaseTransaction();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPurchases()
        {
            var result = await _transactionSvcs.GetPurchases();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPurchaseById([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.GetPurchaseById(Id);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductGstWithRate([FromQuery] Guid id)
        {
            var result = await _adminSvcs.GetProductGstWithRate(id);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreatePurchase([FromBody] PurchaseDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.CreatePurchase(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdatePurchase([FromBody] PurchaseDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.UpdatePurchase(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeletePurchase([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.DeletePurchase(Id);
            return new JsonResult(result);
        }
        #endregion
        #region Purchase Return
        [HttpGet]
        public async Task<IActionResult> GetLastPurchaseReturnTransaction()
        {
            var result = await _transactionSvcs.GetLastPurchaseReturnTransaction();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPurchaseReturns()
        {
            var result = await _transactionSvcs.GetPurchaseReturns();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPurchaseReturnById([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.GetPurchaseReturnById(Id);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreatetPurchaseReturn([FromBody] PurchaseDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.CreatetPurchaseReturn(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdatetPurchaseReturn([FromBody] PurchaseDataRequest requestData)
        {

            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.UpdatetPurchaseReturn(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeletetPurchaseReturn([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.DeletetPurchaseReturn(Id);
            return new JsonResult(result);
        }
        #endregion
        #endregion
        #region Production & Service Entry
        [HttpGet]
        public async Task<IActionResult> GetProductLabourRate([FromQuery] Guid ProductId)
        {
            var result = await _adminSvcs.GetLabourRateByProductId(ProductId);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetLabourDetailById([FromQuery] Guid LabourId)
        {
            var result = await _masterSvcs.GetLabourDetailById(LabourId);
            return new JsonResult(result);
        }
        #region Production
        [HttpGet]
        public IActionResult Production()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetLastProductionNo()
        {
            var result = await _transactionSvcs.GetLastProductionNo();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductionLabours()
        {
            var result = await _masterSvcs.GetLaboursByLabourTypeId(MappingLabourType.Production);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductionEntry()
        {
            var result = await _transactionSvcs.GetProductionEntry();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductionConfig([FromQuery] Guid ProductId)
        {
            var result = await _transactionSvcs.GetProductionConfig(ProductId);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateProductionEntry([FromBody] ProductionEntryRequest data)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.CreateProductionEntry(data);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateProductionEntry([FromBody] ProductionEntryModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.UpdateProductionEntry(model);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteProductionEntry([FromQuery] string id)
        {
            Guid ProductionEntryId = Guid.Parse(id);
            var result = await _transactionSvcs.DeleteProductionEntry(ProductionEntryId);
            return new JsonResult(result);
        }
        #endregion
        #region Service

        #endregion
        [HttpGet]
        public IActionResult Service()
        {
            return PartialView();
        }
        #endregion
        #region Sales Transaction
        [HttpGet]
        public IActionResult SalesTransaction()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetSundryDebtors()
        {
            Guid PartyTypeId = MappingLedgers.SundryDebtors;
            var result = await _transactionSvcs.GetSundryDebtors(PartyTypeId);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductFinishedGood()
        {
            Guid ProductType = MappingProductType.FinishedGood;
            var result = await _adminSvcs.GetProductByTypeId(ProductType);
            return new JsonResult(result);
        }
        #region Sales
        [HttpGet]
        public async Task<IActionResult> GetLastSalesTransaction()
        {
            var result = await _transactionSvcs.GetLastSalesTransaction();
            return new JsonResult(result);
        }
        [HttpGet]
        public IActionResult GetSalesType()
        {
            Mapping s1 = Mapping.GetInstance();
            var result = s1.GetSalesType();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSales()
        {
            var result = await _transactionSvcs.GetSales();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSalesById([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.GetSalesById(Id);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateSale([FromBody] SalesDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.CreateSale(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateSales([FromBody] SalesDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.UpdatSales(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteSales([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.DeleteSales(Id);
            return new JsonResult(result);
        }
        #endregion
        #region Sales Return
        [HttpGet]
        public async Task<IActionResult> GetLastSalesReturnTransaction()
        {
            var result = await _transactionSvcs.GetLastSalesReturnTransaction();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSalesReturns()
        {
            var result = await _transactionSvcs.GetSalesReturns();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSalesReturnById([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.GetSalesReturnById(Id);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateSalesReturn([FromBody] SalesDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.CreateSalesReturn(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateSalesReturn([FromBody] SalesDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.UpdateSalesReturn(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteSalesReturn([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.DeleteSalesReturn(Id);
            return new JsonResult(result);
        }
        #endregion
        #endregion   
        #region Supply
        [HttpGet]
        public async Task<IActionResult> GetAllProductTypes()
        {
            var result = await _adminSvcs.GetProductTypes();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBranch()
        {
            var Branches = await _devloperSvcs.GetAllBranch();
            return new JsonResult(Branches);
        }
        #region Inward Supply Transaction
        [HttpGet]
        public IActionResult InwardSupplyTransaction()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetLastInwardSupply()
        {
            var result = await _transactionSvcs.GetLastInwardSupply();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetInwardSupply()
        {
            var result = await _transactionSvcs.GetInwardSupply();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetInwardSupplyById([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.GetInwardSupplyById(Id);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateInwardSupply([FromBody] SupplyDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.CreateInwardSupply(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateInwardSupply([FromBody] SupplyDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.UpdateInwardSupply(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteInwardSupply([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.DeleteInwardSupply(Id);
            return new JsonResult(result);
        }
        #endregion
        #region Outward Supply Transaction
        [HttpGet]
        public IActionResult OutwardSupplyTransaction()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetLastOutwardSupply()
        {
            var result = await _transactionSvcs.GetLastOutwardSupply();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetOutwardSupply()
        {
            var result = await _transactionSvcs.GetOutwardSupply();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetOutwardSupplyById([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.GetOutwardSupplyById(Id);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateOutwardSupply([FromBody] SupplyDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.CreateOutwardSupply(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateOutwardSupply([FromBody] SupplyDataRequest requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.UpdateOutwardSupply(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteOutwardSupply([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.DeleteOutwardSupply(Id);
            return new JsonResult(result);
        }
        #endregion
        #endregion
        #region Damage
        [HttpGet]
        public async Task<IActionResult> GetAllLabours()
        {
            var result = await _masterSvcs.GetAllLabourDetails();
            return new JsonResult(result);
        }
        [HttpGet]
        public IActionResult DamageTransaction()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> GetLastDamageEntry()
        {
            var result = await _transactionSvcs.GetLastDamageEntry();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetDamages()
        {
            var result = await _transactionSvcs.GetDamages();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetDamageById([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.GetDamageById(Id);
            return new JsonResult(result);
        }
        [HttpPost, Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateDamage([FromBody] DamageRequestData requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.CreateDamage(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Edit")]
        public async Task<IActionResult> UpdateDamage([FromBody] DamageRequestData requestData)
        {
            if (ModelState.IsValid)
            {
                var result = await _transactionSvcs.UpdateDamage(requestData);
                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost, Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteDamage([FromQuery] Guid Id)
        {
            var result = await _transactionSvcs.DeleteDamage(Id);
            return new JsonResult(result);
        }
        #endregion
    }
}
