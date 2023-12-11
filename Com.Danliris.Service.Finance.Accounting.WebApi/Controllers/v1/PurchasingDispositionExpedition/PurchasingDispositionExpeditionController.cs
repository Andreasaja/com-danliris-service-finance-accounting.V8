using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Com.Danliris.Service.Production.WebApi.Utilities;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.PurchasingDispositionExpedition.PurchasingDispositionExpeditionService;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.PurchasingDispositionExpedition
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/purchasing-disposition-expeditions")]
    [Authorize]
    public class PurchasingDispositionExpeditionController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IPurchasingDispositionExpeditionService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;
        public PurchasingDispositionExpeditionController(IIdentityService identityService, IValidateService validateService, IMapper mapper, IPurchasingDispositionExpeditionService service)
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
        public IActionResult Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                ReadResponse<PurchasingDispositionExpeditionModel> read = Service.Read(page, size, order, select, keyword, filter);

                List<PurchasingDispositionExpeditionViewModel> dataVM = Mapper.Map<List<PurchasingDispositionExpeditionViewModel>>(read.Data);

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
        public async Task<ActionResult> Post([FromBody] List<PurchasingDispositionExpeditionViewModel> viewModel)
        {
            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel);
                foreach (var vm in viewModel)
                {
                    ValidateService.Validate(viewModel);
                    PurchasingDispositionExpeditionModel model = Mapper.Map<PurchasingDispositionExpeditionModel>(vm);
                    await Service.CreateAsync(model);
                }
                
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
                PurchasingDispositionExpeditionModel model = await Service.ReadByIdAsync(id);

                if (model == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    PurchasingDispositionExpeditionViewModel viewModel = Mapper.Map<PurchasingDispositionExpeditionViewModel>(model);
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                        .Ok<PurchasingDispositionExpeditionViewModel>(Mapper, viewModel);
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

		[HttpGet("loaderBankExpenditureNo")]
		public IActionResult GetLoaderBankExpenditureNoteNo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
		{
			try
			{
				ReadResponse<PurchasingDispositionExpeditionModel> read = Service.ReadBankExpenditureNoteNo(page, size, order, select, keyword, filter);

				List<PurchasingDispositionExpeditionViewModel> dataVM = Mapper.Map<List<PurchasingDispositionExpeditionViewModel>>(read.Data);

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

	}
}
