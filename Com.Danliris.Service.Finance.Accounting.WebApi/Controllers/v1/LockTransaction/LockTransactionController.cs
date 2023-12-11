using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Com.Danliris.Service.Production.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.LockTransaction
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/lock-transactions")]
    [Authorize]
    public class LockTransactionController : BaseController<LockTransactionModel, LockTransactionViewModel, ILockTransactionService>
    {
        public LockTransactionController(IIdentityService identityService, IValidateService validateService, ILockTransactionService service, IMapper mapper) : base(identityService, validateService, service, mapper, "1.0.0")
        {
        }

        [HttpGet("active-lock-date/by-type/{type}")]
        public async Task<IActionResult> GetLatestActiveLockTransactionByType([FromRoute] string type)
        {
            try
            {
                var model = await Service.GetLastActiveLockTransaction(type);

                if (model == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    var viewModel = Mapper.Map<LockTransactionViewModel>(model);
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                        .Ok(Mapper, viewModel);
                    return Ok(Result);
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("options/lock-type")]
        public IActionResult GetLockTypeOptions()
        {
            var typeOptions = new List<string>()
            {
                "Pembelian",
                "Penjualan",
                "Keuangan"
            };
            Dictionary<string, object> Result =
                new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                .Ok(Mapper, typeOptions);
            return Ok(Result);
        }
    }
}
