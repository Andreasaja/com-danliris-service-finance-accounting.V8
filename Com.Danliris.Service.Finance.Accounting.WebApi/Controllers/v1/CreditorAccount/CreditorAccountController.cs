using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.CreditorAccount
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/creditor-account")]
    public class CreditorAccountController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly ICreditorAccountService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public CreditorAccountController(IIdentityService identityService, IValidateService validateService, IMapper mapper, ICreditorAccountService service)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            Mapper = mapper;
            ApiVersion = "1.0.0";
        }

        private void VerifyUser()
        {
            IdentityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            IdentityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            IdentityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet("reports")]
        public IActionResult GetReport([FromQuery] string supplierName, [FromQuery]int month, [FromQuery]int year, int page = 1, int size = 25)
        {
            try
            {
                VerifyUser();
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                //int offSet = 7;
                var data = Service.GetReport(page, size, supplierName, month, year, offSet);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    finalBalance = data.Item2,
                    data = data.Item1.Data,
                    info = new
                    {
                        data.Item1.Count,
                        data.Item1.Order,
                        data.Item1.Selected
                    },
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

        [HttpGet("reports/downloads/xls")]
        public IActionResult GetXls([FromQuery]string supplierName, [FromQuery]int month, [FromQuery]int year)
        {
            try
            {
                VerifyUser();
                byte[] xlsInBytes;
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var xls = Service.GenerateExcel(supplierName, month, year, offSet);

                string fileName = string.Format("Kartu Hutang Periode {0} {1}.xlsx", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);

                xlsInBytes = xls.ToArray();

                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                return file;
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                  new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                  .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
       
        [HttpGet("reports/downloads/pdf")]
        public IActionResult GetPdf([FromQuery] string supplierName, [FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var data = Service.GeneratePdf(supplierName, month, year, offSet);
                var finalBalance = Service.GetFinalBalance(supplierName, month, year, offSet);

                MemoryStream stream = CreditorAccountPDFTemplate.GeneratePdfTemplate(data, supplierName, month, year, offSet, finalBalance);
                string fileName = string.Format("Kartu Hutang Periode {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = string.Format(fileName)
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

        [HttpPut("unit-payment-order")]
        public async Task<IActionResult> UnitPaymentOrderPut([FromBody] CreditorAccountUnitPaymentOrderPostedViewModel viewModel)
        {
            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel.CreditorAccounts);

                await Service.UpdateFromUnitPaymentOrderAsync(viewModel);

                return NoContent();
            }
            catch (NotFoundException)
            {
                Dictionary<string, object> Result =
                       new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                       .Fail();
                return BadRequest(Result);
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

        [HttpPost("unit-receipt-note")]
        public async Task<ActionResult> UnitReceiptNotePost([FromBody] List<CreditorAccountUnitReceiptNotePostedViewModel> viewModel)
        {
            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel);

                foreach (var model in viewModel)
                {
                    await Service.CreateFromUnitReceiptNoteAsync(model);
                }

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(string.Concat(Request.Path, "/", 0), Result);
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

        [HttpPut("unit-receipt-note")]
        public async Task<IActionResult> UnitReceiptNotePut([FromBody] CreditorAccountUnitReceiptNotePostedViewModel viewModel)
        {
            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel);

                await Service.UpdateFromUnitReceiptNoteAsync(viewModel);

                return NoContent();
            }
            catch (NotFoundException)
            {
                Dictionary<string, object> Result =
                       new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                       .Fail();
                return BadRequest(Result);
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

        [HttpPut("unit-receipt-note/delete")]
        public async Task<IActionResult> UnitReceiptNotePutDelete([FromBody] CreditorAccountUnitReceiptNotePostedViewModel viewModel)
        {
            try
            {
                VerifyUser();

                await Service.DeleteFromUnitReceiptNoteAsync(viewModel);

                return NoContent();
            }
            catch (NotFoundException)
            {
                Dictionary<string, object> Result =
                       new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                       .Fail();
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

        [HttpGet("unit-receipt-note")]
        public async Task<IActionResult> UnitReceiptNoteGet([FromQuery] string supplierCode, [FromQuery] string code, [FromQuery] string invoiceNo)
        {
            try
            {
                CreditorAccountUnitReceiptNotePostedViewModel vm = await Service.GetByUnitReceiptNote(supplierCode, code, invoiceNo);

                if (vm == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                        .Ok(Mapper, vm);
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

        [HttpDelete("unit-receipt-note/{id}")]
        public async Task<IActionResult> UnitReceiptNoteDelete([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                await Service.DeleteFromUnitReceiptNoteAsync(id);

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

        [HttpPost("bank-expenditure-note")]
        public async Task<ActionResult> BankExpenditureNotePost([FromBody] CreditorAccountBankExpenditureNotePostedViewModel viewModel)
        {
            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel);


                await Service.CreateFromBankExpenditureNoteAsync(viewModel);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(string.Concat(Request.Path, "/", 0), Result);
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

        [HttpPost("bank-expenditure-note/list")]
        public async Task<ActionResult> BankExpenditureNoteListPost([FromBody] List<CreditorAccountBankExpenditureNotePostedViewModel> viewModel)
        {
            try
            {
                VerifyUser();
                foreach(var item in viewModel)
                {
                    ValidateService.Validate(item);
                }

                await Service.CreateFromListBankExpenditureNoteAsync(viewModel);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(string.Concat(Request.Path, "/", 0), Result);
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

        [HttpPut("bank-expenditure-note")]
        public async Task<IActionResult> BankExpenditureNotePut([FromBody] CreditorAccountBankExpenditureNotePostedViewModel viewModel)
        {
            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel);

                await Service.UpdateFromBankExpenditureNoteAsync(viewModel);

                return NoContent();
            }
            catch (NotFoundException)
            {
                Dictionary<string, object> Result =
                       new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                       .Fail();
                return BadRequest(Result);
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

        [HttpPut("bank-expenditure-note/list")]
        public async Task<IActionResult> BankExpenditureNoteListPut([FromBody] List<CreditorAccountBankExpenditureNotePostedViewModel> viewModel)
        {
            try
            {
                VerifyUser();
                foreach(var item in viewModel)
                {
                    ValidateService.Validate(item);

                    await Service.UpdateFromBankExpenditureNoteAsync(item);
                }
                return NoContent();
            }
            catch (NotFoundException)
            {
                Dictionary<string, object> Result =
                       new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                       .Fail();
                return BadRequest(Result);
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

        [HttpGet("bank-expenditure-note")]
        public async Task<IActionResult> BankExpenditureNoteGet([FromQuery] string supplierCode, [FromQuery] string code, [FromQuery] string invoiceNo)
        {
            try
            {
                CreditorAccountBankExpenditureNotePostedViewModel vm = await Service.GetByBankExpenditureNote(supplierCode, code, invoiceNo);

                if (vm == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                        .Ok(Mapper, vm);
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

        [HttpDelete("bank-expenditure-note/{id}")]
        public async Task<IActionResult> BankExpenditureNoteDelete([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                await Service.DeleteFromBankExpenditureNoteAsync(id);

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

        [HttpDelete("bank-expenditure-note/list/{code}")]
        public async Task<IActionResult> BankExpenditureNoteDeleteList([FromRoute] string code)
        {
            try
            {
                VerifyUser();

                await Service.DeleteFromBankExpenditureNoteListAsync(code);

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

        [HttpPost("unit-payment-correction")]
        public async Task<IActionResult> UnitPaymentCorrectionPost([FromBody] CreditorAccountUnitPaymentCorrectionPostedViewModel viewModel)
        {
            try
            {
                VerifyUser();
                //ValidateService.Validate(viewModel);

                await Service.CreateFromUnitPaymentCorrection(viewModel);

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
    }
}
