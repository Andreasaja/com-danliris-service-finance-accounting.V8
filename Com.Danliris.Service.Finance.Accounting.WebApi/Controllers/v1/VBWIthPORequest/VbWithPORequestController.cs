using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.VBWIthPORequest
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/vb-with-po-request")]
    [Authorize]

    public class VbWithPORequestController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IValidateService _validateService;
        private readonly IVbWithPORequestService _service;
        private readonly IMapper _mapper;
        private const string ApiVersion = "1.0";

        public VbWithPORequestController(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            _validateService = serviceProvider.GetService<IValidateService>();
            _service = serviceProvider.GetService<IVbWithPORequestService>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet("pdf/{Id}")]
        public async Task<IActionResult> GetVbNonPORequestPDF([FromRoute] int Id)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                int timeoffsset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                //VbRequestModel model = await _service.ReadByIdAsync(Id);
                VbWithPORequestViewModel viewModel = await _service.ReadByIdAsync2(Id);
                if (viewModel == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    /*_mapper.Map<VbWithPORequestViewModel>(model);*/

                    VbWithPORequestPDFTemplate PdfTemplate = new VbWithPORequestPDFTemplate();
                    MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel, timeoffsset);
                    return new FileStreamResult(stream, "application/pdf")
                    {
                        FileDownloadName = "Permohonan VB dengan PO - " + viewModel.VBNo + ".pdf"
                    };
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

        [HttpGet]
        public IActionResult Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                var queryResult = _service.Read(page, size, order, select, keyword, filter);

                var result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(_mapper, queryResult.Data, page, size, queryResult.Count, queryResult.Data.Count, queryResult.Order, select);
                return Ok(result);
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("with-date-filter")]
        public IActionResult GetWithDateFilter(DateTimeOffset? dateFilter, [FromHeader(Name = "x-timezone-offset")] string timezone, int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {

            int offset = Convert.ToInt32(timezone);
            try
            {
                var queryResult = _service.ReadWithDateFilter(dateFilter, offset, page, size, order, select, keyword, filter);

                var result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(_mapper, queryResult.Data, page, size, queryResult.Count, queryResult.Data.Count, queryResult.Order, select);
                return Ok(result);
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VbWithPORequestViewModel viewModel)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(viewModel);

                var model = _mapper.Map<VbRequestModel>(viewModel);

                await _service.CreateAsync(model, viewModel);

                await _service.MappingData(viewModel);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return Created(String.Concat(Request.Path, "/", 0), result);
            }
            catch (ServiceValidationException e)
            {
                var result = new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE).Fail(e);
                return BadRequest(result);
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var model = await _service.ReadByIdAsync2(id);

                if (model == null)
                {
                    var result = new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE).Fail();
                    return NotFound(result);
                }
                else
                {
                    var viewModel = _mapper.Map<VbWithPORequestViewModel>(model);
                    var result = new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE).Ok<VbWithPORequestViewModel>(_mapper, viewModel);
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] VbWithPORequestViewModel viewModel)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(viewModel);

                if (id != viewModel.Id)
                {
                    var result = new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE).Fail();
                    return BadRequest(result);
                }

                var model = _mapper.Map<VbRequestModel>(viewModel);

                await _service.UpdateAsync(id, viewModel);

                return NoContent();
            }
            catch (ServiceValidationException e)
            {
                var result = new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE).Fail(e);
                return BadRequest(result);
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                await _service.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }
    }
}
