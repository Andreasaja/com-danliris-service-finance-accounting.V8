using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingPphBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels;
using Com.Danliris.Service.Production.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingPphBankExpenditureNote.PdfGenerator;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.GarmentPurchasingPphBankExpenditureNote
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/garment-purchasing-pph-bank-expenditure-note")]
    [Authorize]
    public class GarmentPurchasingPphBankExpenditureNoteController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IGarmentPurchasingPphBankExpenditureNoteService Service;
        private readonly IMapper Mapper;
        private readonly string ApiVersion;


        public GarmentPurchasingPphBankExpenditureNoteController(IIdentityService identityService, IValidateService validateService, IGarmentPurchasingPphBankExpenditureNoteService service, IMapper mapper)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            Mapper = mapper;
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
                ReadResponse<GarmentPurchasingPphBankExpenditureNoteDataViewModel> read = Service.Read(page, size, order, select, keyword, filter);

                List<GarmentPurchasingPphBankExpenditureNoteDataViewModel> dataVM = Mapper.Map<List<GarmentPurchasingPphBankExpenditureNoteDataViewModel>>(read.Data);

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
        public async Task<ActionResult> Post([FromBody] FormInsert viewModel)
        {
            try
            {
                VerifyUser();

                ValidateService.Validate(viewModel);
                //FormInsert model = Mapper.Map<FormInsert>(viewModel);
                await Service.CreateAsync(viewModel);

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

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] FormInsert viewModel)
        {
            try
            {
                VerifyUser();

                ValidateService.Validate(viewModel);
                //FormInsert model = Mapper.Map<FormInsert>(viewModel);
                await Service.UpdateAsync(viewModel);

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

        [HttpGet("pdf/{Id}")]
        public async Task<IActionResult> DownloadPdfById([FromRoute] int id)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                var model = await Service.ReadByIdAsync(id);
                //GarmentPurchasingPphBankExpenditureNoteDataViewModel viewModel = Mapper.Map<GarmentPurchasingPphBankExpenditureNoteDataViewModel>(model);

                var stream = GarmentPurchasingPphBankExpenditureNotePdfGenerator.GeneratePdfTemplate(model, IdentityService.TimezoneOffset);
                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "Garment Purchasing PPH Bank.pdf"
                };
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport([FromQuery] GarmentPurchasingPphBankExpenditureNoteFilterReportDto filter, int page = 1, int size = 25, string order = "{}")
        {
            try
            {
                VerifyUser();
                var model = Service.GetReportView(page, size, order, filter);

                //List<GarmentPurchasingPphBankExpenditureNoteReportViewDto> dataVM = Mapper.Map<List<GarmentPurchasingPphBankExpenditureNoteReportViewDto>>(model.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Mapper, model.Data, page, size, model.Count, model.Count, new Dictionary<string, string>(), new List<string>());
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
        [HttpGet("report-group")]
        public async Task<IActionResult> GetReportGroup([FromQuery] GarmentPurchasingPphBankExpenditureNoteFilterReportDto filter, int page = 1, int size = 25, string order = "{}")
        {
            try
            {
                VerifyUser();
                var model = Service.GetReportGroupView(page, size, order, filter);

                //List<GarmentPurchasingPphBankExpenditureNoteReportViewDto> dataVM = Mapper.Map<List<GarmentPurchasingPphBankExpenditureNoteReportViewDto>>(model.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Mapper, model.Data, page, size, model.Count, model.Count, new Dictionary<string, string>(), new List<string>());
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
        [HttpGet("report/xls")]
        public async Task<IActionResult> DownloadReportXls([FromQuery] GarmentPurchasingPphBankExpenditureNoteFilterReportDto filter, int page = 1, int size = 25, string order = "{}")
        {
            try
            {
                VerifyUser();
                var stream = Service.DownloadReportXls(filter);

                return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "Garment Purchasing Laporan PPH Bank.xls"
                };
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
        [HttpGet("loader-pph-intern-note")]
        public async Task<IActionResult> GetLoaderInternNote([FromQuery] string keyword= "")
        {
            try
            {
                VerifyUser();
                var model = Service.GetLoaderInterNotePPH(keyword);

                //List<GarmentPurchasingPphBankExpenditureNoteReportViewDto> dataVM = Mapper.Map<List<GarmentPurchasingPphBankExpenditureNoteReportViewDto>>(model.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Mapper, model, 1, 10, model.Count, model.Count, new Dictionary<string, string>(), new List<string>());
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

        [HttpGet("loader-pph-supplier")]
        public async Task<IActionResult> GetLoaderSupplier([FromQuery] string keyword = "")
        {
            try
            {
                VerifyUser();
                var model = Service.GetLoaderSupplier(keyword);

                //List<GarmentPurchasingPphBankExpenditureNoteReportViewDto> dataVM = Mapper.Map<List<GarmentPurchasingPphBankExpenditureNoteReportViewDto>>(model.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Mapper, model, 1, 10, model.Count, model.Count, new Dictionary<string, string>(), new List<string>());
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
        [HttpGet("loader-pph-invoice")]
        public async Task<IActionResult> GetLoaderInvoice([FromQuery] string keyword = "")
        {
            try
            {
                VerifyUser();
                var model = Service.GetLoaderInvoice(keyword);

                //List<GarmentPurchasingPphBankExpenditureNoteReportViewDto> dataVM = Mapper.Map<List<GarmentPurchasingPphBankExpenditureNoteReportViewDto>>(model.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Mapper, model, 1, 10, model.Count, model.Count, new Dictionary<string, string>(), new List<string>());
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
        [HttpGet("loader-pph-invoice-out")]
        public async Task<IActionResult> GetLoaderInvoiceOut([FromQuery] string keyword = "")
        {
            try
            {
                VerifyUser();
                var model = Service.GetLoaderInvoiceOut(keyword);

                //List<GarmentPurchasingPphBankExpenditureNoteReportViewDto> dataVM = Mapper.Map<List<GarmentPurchasingPphBankExpenditureNoteReportViewDto>>(model.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Mapper, model, 1, 10, model.Count, model.Count, new Dictionary<string, string>(), new List<string>());
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

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {

                var model = await Service.ReadByIdAsync(id);


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
                    data = model,
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

        [HttpPost("posting")]
        public async Task<IActionResult> PostingDocument([FromBody] List<int> id)
        {
            try
            {
                VerifyUser();

                await Service.PostingDocument(id);

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


        [HttpPost("print-invoice/{id}")]
        public async Task<IActionResult> PrintInvoice([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                //var dataReport = await Service.PrintInvoice(id);

                Dictionary<string, object> Result =
                       new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
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

    }
}
