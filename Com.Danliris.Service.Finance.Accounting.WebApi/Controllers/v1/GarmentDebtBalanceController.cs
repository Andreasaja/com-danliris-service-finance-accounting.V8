using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance.Excel;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance.Pdf;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/garment-debt-balances")]
    [Authorize]
    public class GarmentDebtBalanceController : Controller
    {
        private const string ApiVersion = "1.0.0";
        private readonly IGarmentDebtBalanceService _service;
        private readonly IIdentityService _identityService;

        public GarmentDebtBalanceController(IServiceProvider serviceProvider)
        {
            _service = serviceProvider.GetService<IGarmentDebtBalanceService>();
            _identityService = serviceProvider.GetService<IIdentityService>();
        }

        private void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpPost("customs")]
        public IActionResult PostCustoms([FromBody] CustomsFormDto form)
        {
            try
            {
                VerifyUser();

                var id = _service.CreateFromCustoms(form);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return Created(string.Concat(Request.Path, "/", id), result);
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("invoice/{deliveryOrderId}")]
        public IActionResult PutInvoice([FromRoute] int deliveryOrderId, [FromBody] InvoiceFormDto form)
        {
            try
            {
                VerifyUser();

                var id = _service.UpdateFromInvoice(deliveryOrderId, form);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

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

        [HttpPut("internal-note/{deliveryOrderId}")]
        public IActionResult PutInternalNote([FromRoute] int deliveryOrderId, [FromBody] InternalNoteFormDto form)
        {
            try
            {
                VerifyUser();

                var id = _service.UpdateFromInternalNote(deliveryOrderId, form);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

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

        [HttpPut("bank-expenditure-note/{deliveryOrderId}")]
        public IActionResult PutBankExpenditureNote([FromRoute] int deliveryOrderId, [FromBody] BankExpenditureNoteFormDto form)
        {
            try
            {
                VerifyUser();

                var id = _service.UpdateFromBankExpenditureNote(deliveryOrderId, form);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

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

        [HttpPut("remove-customs/{deliveryOrderId}")]
        public IActionResult RemoveCustom([FromRoute] int deliveryOrderId)
        {
            try
            {
                VerifyUser();

                var id = _service.RemoveBalance(deliveryOrderId);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

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

        [HttpPut("remove-invoice/{deliveryOrderId}")]
        public IActionResult RemoveInvoice([FromRoute] int deliveryOrderId)
        {
            try
            {
                VerifyUser();

                var id = _service.EmptyInvoiceValue(deliveryOrderId);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

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

        [HttpPut("remove-internal-note/{deliveryOrderId}")]
        public IActionResult RemoveInternalNote([FromRoute] int deliveryOrderId)
        {
            try
            {
                VerifyUser();

                var id = _service.EmptyInternalNoteValue(deliveryOrderId);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

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

        [HttpPut("remove-bank-expenditure-note/{deliveryOrderId}")]
        public IActionResult RemoveBankExpenditureNote([FromRoute] int deliveryOrderId)
        {
            try
            {
                VerifyUser();

                var id = _service.EmptyBankExpenditureNoteValue(deliveryOrderId);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

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

        [HttpGet("summary")]
        public IActionResult GetSummary([FromQuery] int supplierId, [FromQuery] int month, [FromQuery] int year, [FromQuery] bool isForeignCurrency, [FromQuery] bool supplierIsImport)
        {
            try
            {
                VerifyUser();

                var data = _service.GetDebtBalanceSummary(supplierId, month, year, isForeignCurrency, supplierIsImport);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data,
                    info = new
                    {
                        Count = data.Count,
                        Order = new List<string>(),
                        Selected = new List<string>()
                    },
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("summary/downloads/pdf")]
        public IActionResult GetPdf([FromQuery] int supplierId, [FromQuery] int month, [FromQuery] int year, [FromQuery] bool isForeignCurrency, [FromQuery] bool supplierIsImport)
        {
            try
            {
                VerifyUser();

                var data = _service.GetDebtBalanceSummary(supplierId, month, year, isForeignCurrency, supplierIsImport);
                var stream = GarmentDebtBalancePdfGenerator.Generate(data, month, year, isForeignCurrency, supplierIsImport, _identityService.TimezoneOffset);

                var filename = "SALDO HUTANG LOKAL";
                if (isForeignCurrency)
                    filename = "SALDO HUTANG LOKAL VALAS";
                if (supplierIsImport)
                    filename = "SALDO HUTANG IMPOR";
                filename += ".pdf";

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = filename
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

        [HttpGet("summary/downloads/xls")]
        public IActionResult GetXls([FromQuery] int supplierId, [FromQuery] int month, [FromQuery] int year, [FromQuery] bool isForeignCurrency, [FromQuery] bool supplierIsImport)
        {
            try
            {
                VerifyUser();

                var data = _service.GetDebtBalanceSummary(supplierId, month, year, isForeignCurrency, supplierIsImport);
                var stream = GarmentDebtBalanceExcelGenerator.Generate(data, month, year, isForeignCurrency, supplierIsImport, _identityService.TimezoneOffset);

                var filename = "SALDO HUTANG LOKAL";
                if (isForeignCurrency)
                    filename = "SALDO HUTANG LOKAL VALAS";
                if (supplierIsImport)
                    filename = "SALDO HUTANG IMPOR";
                filename += ".xlsx";

                var bytes = stream.ToArray();

                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("summary-with-group-currency")]
        public IActionResult GetSummaryAndTotalByCurrency([FromQuery] int supplierId, [FromQuery] int month, [FromQuery] int year, [FromQuery] bool isForeignCurrency, [FromQuery] bool supplierIsImport)
        {
            try
            {
                VerifyUser();

                var data = _service.GetDebtBalanceSummaryAndTotalCurrency(supplierId, month, year, isForeignCurrency, supplierIsImport);

                //var result = new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE).Ok(id);

                //return Created(string.Concat(Request.Path, "/", id), result);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data,
                    info = new
                    {
                        Count = data.Data.Count,
                        Order = new List<string>(),
                        Selected = new List<string>()
                    },
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("downloads/xls")]
        public async Task<IActionResult> GetXls([FromQuery] int supplierId,[FromQuery]string supplierName, [FromQuery] int month, [FromQuery] int year, [FromQuery] bool isForeignCurrency, [FromQuery] bool supplierIsImport)
        {
            try
            {
                //VerifyUser();
                var data = _service.GetDebtBalanceSummaryAndTotalCurrency(supplierId, month, year, isForeignCurrency, supplierIsImport);

                MemoryStream result = new MemoryStream();
                var filename = "Saldo Hutang Lokal Valas.xlsx";
                result = Lib.BusinessLogic.GarmentDebtBalance.Excel.GarmentDebtBalanceValasLokalExcel.GenerateExcel(data,month,year,supplierName,isForeignCurrency,_identityService.TimezoneOffset);
                //filename += ".xlsx";

                var bytes = result.ToArray();

                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("downloads/pdf")]
        public IActionResult GetPDF([FromQuery] int supplierId, [FromQuery] string supplierName, [FromQuery] int month, [FromQuery] int year, [FromQuery] bool isForeignCurrency, [FromQuery] bool supplierIsImport)
        {
            try
            {
                //VerifyUser();
                var data = _service.GetDebtBalanceSummaryAndTotalCurrency(supplierId, month, year, isForeignCurrency, supplierIsImport);

                MemoryStream result = new MemoryStream();
                var filename = "Saldo Hutang Lokal Valas.pdf";
                result = Lib.BusinessLogic.GarmentDebtBalance.Pdf.GarmentDebtBalanceValasLokalPdf.Generate(data, month, year, isForeignCurrency, _identityService.TimezoneOffset);
                //filename += ".xlsx";

                var bytes = result.ToArray();

                return File(bytes, "application/pdf", filename);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("detail")]
        public IActionResult GetDetail([FromQuery] DateTimeOffset arrivalDate, [FromQuery] GarmentDebtBalanceDetailFilterEnum supplierTypeFilter, [FromQuery] int supplierId, [FromQuery] int currencyId, [FromQuery] string paymentType)
        {
            try
            {
                VerifyUser();

                var data = _service.GetDebtBalanceDetail(arrivalDate, supplierTypeFilter, supplierId, currencyId, paymentType);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data,
                    info = new
                    {
                        Count = data.Count,
                        Order = new List<string>(),
                        Selected = new List<string>()
                    },
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("detail/downloads/pdf")]
        public IActionResult GetPdf([FromQuery] DateTimeOffset arrivalDate, [FromQuery] GarmentDebtBalanceDetailFilterEnum supplierTypeFilter, [FromQuery] int supplierId, [FromQuery] int currencyId, [FromQuery] string paymentType)
        {
            try
            {
                VerifyUser();

                var data = _service.GetDebtBalanceDetail(arrivalDate, supplierTypeFilter, supplierId, currencyId, paymentType);
                var stream = GarmentDebtBalanceDetailPDFGenerator.Generate(data, arrivalDate, _identityService.TimezoneOffset);

                var filename = "LAPORAN RINCIAN HUTANG";
                filename += ".pdf";

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = filename
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

        [HttpGet("detail/downloads/xls")]
        public IActionResult GetXls([FromQuery] DateTimeOffset arrivalDate, [FromQuery] GarmentDebtBalanceDetailFilterEnum supplierTypeFilter, [FromQuery] int supplierId, [FromQuery] int currencyId, [FromQuery] string paymentType)
        {
            try
            {
                VerifyUser();

                var data = _service.GetDebtBalanceDetail(arrivalDate, supplierTypeFilter, supplierId, currencyId, paymentType);
                var stream = GarmentDebtBalanceDetailExcelGenerator.Generate(data, arrivalDate, _identityService.TimezoneOffset);

                var filename = "LAPORAN RINCIAN HUTANG";
                filename += ".xlsx";

                var bytes = stream.ToArray();

                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }
    }
}
