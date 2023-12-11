using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.CashierApproval;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionAcceptance;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.PurchasingDispositionAcceptance
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/purchasing-disposition-acceptance")]
    [Authorize]
    public class PurchasingDispositionAcceptanceController : Controller
    {
        private IIdentityService IdentityService;
        private IIdentityService object1;
        private IValidateService object2;
        private IMapper object3;
        private ICashierAprovalService object4;
        private readonly IValidateService ValidateService;
        private readonly IPurchasingDispositionExpeditionService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public PurchasingDispositionAcceptanceController(IIdentityService identityService, IValidateService validateService, IMapper mapper, IPurchasingDispositionExpeditionService service)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            Mapper = mapper;
            ApiVersion = "1.0.0";
        }

        //public PurchasingDispositionAcceptanceController(IIdentityService object1, IValidateService object2, IMapper object3, ICashierAprovalService object4)
        //{
        //    this.object1 = object1;
        //    this.object2 = object2;
        //    this.object3 = object3;
        //    this.object4 = object4;
        //}

        protected void VerifyUser()
        {
            IdentityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            IdentityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            IdentityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PurchasingDispositionAcceptanceViewModel viewModel)
        {
            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel);

                await Service.PurchasingDispositionAcceptance(viewModel);

                return NoContent();
            }
            catch (ServiceValidationException e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (AggregateException ex)
            {
                string message = string.Join(',', ex.InnerExceptions.Select(x => x.Message));
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                await Service.DeletePurchasingDispositionAcceptance(id);

                return NoContent();
            }
            catch (AggregateException ex)
            {
                string message = string.Join(',', ex.InnerExceptions.Select(x => x.Message));
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}
