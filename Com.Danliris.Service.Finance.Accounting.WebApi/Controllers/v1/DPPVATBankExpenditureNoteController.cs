using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote.Excel;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote.PDF;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/dpp-vat-bank-expenditure-notes")]
    [Authorize]

    public class DPPVATBankExpenditureNoteController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IDPPVATBankExpenditureNoteService _service;
        private readonly IValidateService _validateService;
        private const string ApiVersion = "1.0";

        public DPPVATBankExpenditureNoteController(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            _service = serviceProvider.GetService<IDPPVATBankExpenditureNoteService>();
            _validateService = serviceProvider.GetService<IValidateService>();
        }

        private void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = await _service.Create(form);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return Created(string.Concat(Request.Path, "/", id), result);
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

        [HttpGet]
        public IActionResult Get([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string order = "{}")
        {
            //proses
            try
            {
                var result = _service.Read(keyword, page, size, order);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    statusCode = General.OK_STATUS_CODE,
                    message = General.OK_MESSAGE,
                    data = result.Data,
                    info = new
                    {
                        total = result.Count,
                        page,
                        size,
                        count = result.Data.Count
                    }
                });
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] FormDto form)
        {
            try
            {
                VerifyUser();

                var note = _service.Read(id);
                if (note == null)
                    return NotFound();

                _validateService.Validate(form);

                await _service.Update(id, form);

                return NoContent();
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

        [HttpPut("posting")]
        public async Task<IActionResult> Posting([FromBody] List<int> ids)
        {
            try
            {
                VerifyUser();

                var note = await _service.Posting(ids);

                return NoContent();
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                var note = _service.Read(id);
                if (note == null)
                    return NotFound();

                await _service.Delete(id);

                return NoContent();
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
        public IActionResult Get(int id)
        {
            try
            {
                var result = _service.Read(id);

                if (result == null)
                    return NotFound();

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    statusCode = General.OK_STATUS_CODE,
                    message = General.OK_MESSAGE,
                    data = result
                });
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }

        [HttpGet("download/pdf/{id}")]
        public IActionResult GeneratePdf([FromRoute] int id)
        {

            try
            {
                VerifyUser();
                var data = _service.Read(id);
                var stream = DPPVATBankExpenditureNotePDFGenerator.Generate(data, _identityService.TimezoneOffset);
                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "Bukti Pengeluaran Bank DPP+PPN.pdf"
                };
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("report")]
        public IActionResult GetReport([FromQuery] int expenditureId, [FromQuery] int internalNoteId, [FromQuery] int invoiceId, [FromQuery] int supplierId, [FromQuery] DateTimeOffset? startDate, [FromQuery] DateTimeOffset? endDate)
        {
            try
            {
                startDate = startDate.HasValue ? startDate : DateTimeOffset.MinValue;
                endDate = endDate.HasValue ? endDate : DateTimeOffset.MaxValue;
                //var result = _service.ExpenditureReport(expenditureId, internalNoteId, invoiceId, supplierId, startDate.GetValueOrDefault(), endDate.GetValueOrDefault());
                var result = _service.ExpenditureReportDetailDO(expenditureId, internalNoteId, invoiceId, supplierId, startDate.GetValueOrDefault(), endDate.GetValueOrDefault());


                return Ok(new
                {
                    apiVersion = ApiVersion,
                    statusCode = General.OK_STATUS_CODE,
                    message = General.OK_MESSAGE,
                    data = result
                });
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }

        [HttpGet("report/download/xls")]
        public IActionResult GetReportXls([FromQuery] int expenditureId, [FromQuery] int internalNoteId, [FromQuery] int invoiceId, [FromQuery] int supplierId, [FromQuery] DateTimeOffset? startDate, [FromQuery] DateTimeOffset? endDate)
        {
            try
            {
                VerifyUser();
                startDate = startDate.HasValue ? startDate : DateTimeOffset.MinValue;
                endDate = endDate.HasValue ? endDate : DateTimeOffset.MaxValue;
                //var result = _service.ExpenditureReport(expenditureId, internalNoteId, invoiceId, supplierId, startDate.GetValueOrDefault(), endDate.GetValueOrDefault());
                var result = _service.ExpenditureReportDetailDO(expenditureId, internalNoteId, invoiceId, supplierId, startDate.GetValueOrDefault(), endDate.GetValueOrDefault());

                var stream = DPPVATBankExpenditureNoteExcelGenerator.Generate(result, startDate.GetValueOrDefault(), endDate.GetValueOrDefault(), _identityService.TimezoneOffset);

                var bytes = stream.ToArray();

                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Pengeluaran Bank DPP+PPN.xlsx");
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }


        [HttpGet("invoice/{InvoiceId}")]
        public IActionResult GetFromInvoice([FromRoute] long InvoiceId)
        {
            try
            {

                var result = _service.ExpenditureFromInvoice(InvoiceId);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    statusCode = General.OK_STATUS_CODE,
                    message = General.OK_MESSAGE,
                    data = result
                });
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }
    }
}
