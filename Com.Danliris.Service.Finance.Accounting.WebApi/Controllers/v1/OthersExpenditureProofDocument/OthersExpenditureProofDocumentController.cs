using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.OthersExpenditureProofDocument.ExcelGenerator;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.OthersExpenditureProofDocument
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/others-expenditure-proof-documents")]
    [Authorize]
    public class OthersExpenditureProofDocumentController : Controller
    {
        private const string _apiVersion = "1.0";
        private readonly IIdentityService _identityService;
        private readonly IValidateService _validateService;
        private readonly IOthersExpenditureProofDocumentService _service;

        public OthersExpenditureProofDocumentController(IIdentityService identityService, IValidateService validateService, IOthersExpenditureProofDocumentService service)
        {
            _identityService = identityService;
            _validateService = validateService;
            _service = service;
        }

        protected void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                VerifyUser();

                var result = await _service.GetPagedListAsync(page, size, order, keyword, filter);

                return Ok(new
                {
                    apiVersion = _apiVersion,
                    data = result.Data,
                    info = new
                    {
                        total = result.Total,
                        count = result.Count,
                        page = result.Page,
                        size = result.Size,
                        order = order
                    },
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("loader")]
        public async Task<IActionResult> GetLoader(string keyword = null, string filter = "{}")
        {
            try
            {
                VerifyUser();

                var result = await _service.GetLoaderAsync(keyword, filter);

                return Ok(new
                {
                    apiVersion = _apiVersion,
                    data = result.Data,
                    info = new
                    {
                        total = result.Total,
                        count = result.Count,
                        page = result.Page,
                        size = result.Size
                    },
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OthersExpenditureProofDocumentCreateUpdateViewModel viewModel)
        {
            try
            {
                VerifyUser();

                _validateService.Validate(viewModel);

                await _service.CreateAsync(viewModel);

                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(String.Concat(Request.Path, "/", 0), Result);
            }
            catch (ServiceValidationException e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                var result = await _service.GetSingleByIdAsync(id);

                if (result == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(_apiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    return Ok(new
                    {
                        apiVersion = _apiVersion,
                        data = result,
                        message = General.OK_MESSAGE,
                        statusCode = General.OK_STATUS_CODE
                    });
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] OthersExpenditureProofDocumentCreateUpdateViewModel viewModel)
        {
            try
            {
                VerifyUser();

                _validateService.Validate(viewModel);

                await _service.UpdateAsync(id, viewModel);

                return NoContent();
            }
            catch (ServiceValidationException e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
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

                await _service.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("pdf/{Id}")]
        public async Task<IActionResult> GetPDFById([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                int timeoffsset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var result = await _service.GetPDFByIdAsync(id);

                if (result == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(_apiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    OthersExpenditureDocumentPDFTemplate PdfTemplate = new OthersExpenditureDocumentPDFTemplate();
                    MemoryStream stream = PdfTemplate.GeneratePdfTemplate(result, timeoffsset);
                    return new FileStreamResult(stream, "application/pdf")
                    {
                        FileDownloadName = $"Bukti Pengeluaran Bank (Lain-lain) - {result.DocumentNo}.pdf"
                    };
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }

        }

        [HttpPut("posting")]
        public async Task<IActionResult> Posting([FromBody] List<int> ids)
        {
            try
            {
                VerifyUser();
                var result = await _service.Posting(ids);

                return Ok(new
                {
                    apiVersion = _apiVersion,
                    data = result,
                    message = General.OK_MESSAGE,
                    statusCode = General.CREATED_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport(DateTimeOffset? startDate, DateTimeOffset? endDate, DateTimeOffset? dateExpenditure, string bankExpenditureNo, string division, int page = 1, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            try
            {
                VerifyUser();

                var result = await _service.GetReportList(startDate,endDate,dateExpenditure,bankExpenditureNo,division, page, size, order, keyword, filter);

                return Ok(new
                {
                    apiVersion = _apiVersion,
                    data = result.Data,
                    info = new
                    {
                        total = result.Total,
                        count = result.Count,
                        page = result.Page,
                        size = result.Size,
                        order = order
                    },
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("report/xls")]
        public async Task<IActionResult> GetReportXls(DateTimeOffset? startDate, DateTimeOffset? endDate, DateTimeOffset? dateExpenditure, string bankExpenditureNo, string division,  int page = 1, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            try
            {
                VerifyUser();

                var result = await _service.GetReportList(startDate, endDate, dateExpenditure, bankExpenditureNo, division, 1, int.MaxValue, order, keyword, filter);

                var filename = "Laporan Bukti Pengeluaran Bank Lain - Lain";
                var xls = OthersExpenditureProofDocumentExcelGenerator.Create(filename, result.Data, bankExpenditureNo, dateExpenditure.GetValueOrDefault(), division, startDate.GetValueOrDefault(), endDate.GetValueOrDefault(), _identityService.TimezoneOffset);

                var xlsInBytes = xls.ToArray();

                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(_apiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}
