using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.GarmentFinance.BankCashReceiptDetailLocal
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/bank-cash-receipt-detail-locals")]
    [Authorize]
    public class GarmentFinanceBankCashReceiptDetailLocalController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IGarmentFinanceBankCashReceiptDetailLocalService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public GarmentFinanceBankCashReceiptDetailLocalController(IIdentityService identityService, IValidateService validateService, IMapper mapper, IGarmentFinanceBankCashReceiptDetailLocalService service)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            Mapper = mapper;
            ApiVersion = "1.0.0";
        }

        protected void VerifyUser()
        {
            IdentityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            IdentityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            IdentityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                ReadResponse<GarmentFinanceBankCashReceiptDetailLocalModel> read = Service.Read(page, size, order, select, keyword, filter);

                List<GarmentFinanceBankCashReceiptDetailLocalViewModel> dataVM = Mapper.Map<List<GarmentFinanceBankCashReceiptDetailLocalViewModel>>(read.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Mapper, dataVM, page, size, read.Count, dataVM.Count, read.Order, read.Selected);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GarmentFinanceBankCashReceiptDetailLocalViewModel viewModel)
        {
            try
            {
                VerifyUser();

                ValidateService.Validate(viewModel);
                GarmentFinanceBankCashReceiptDetailLocalModel model = Mapper.Map<GarmentFinanceBankCashReceiptDetailLocalModel>(viewModel);
                await Service.CreateAsync(model);

                Dictionary<string, object> Result =
                       new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                       .Ok();
                return Created(String.Concat(Request.Path, "/", 0), Result);
            }
            catch (ServiceValidationException e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                var model = await Service.ReadByIdAsync(id);
                GarmentFinanceBankCashReceiptDetailLocalViewModel viewModel = Mapper.Map<GarmentFinanceBankCashReceiptDetailLocalViewModel>(model);

                if (model == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = viewModel,
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });

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

                await Service.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] GarmentFinanceBankCashReceiptDetailLocalViewModel viewModel)
        {
            try
            {
                VerifyUser();

                ValidateService.Validate(viewModel);
                GarmentFinanceBankCashReceiptDetailLocalModel model = Mapper.Map<GarmentFinanceBankCashReceiptDetailLocalModel>(viewModel);
                await Service.UpdateAsync(id, model);

                return NoContent();
            }
            catch (ServiceValidationException e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
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
