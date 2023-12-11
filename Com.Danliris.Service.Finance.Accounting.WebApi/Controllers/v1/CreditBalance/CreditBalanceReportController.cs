using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.CreditBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.CreditBalance
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/credit-balance")]
    public class CreditBalanceReportController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly ICreditBalanceService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public CreditBalanceReportController(IIdentityService identityService, IValidateService validateService, IMapper mapper, ICreditBalanceService service)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            Mapper = mapper;
            ApiVersion = "1.0.0";
        }

        [HttpGet("reports")]
        public IActionResult GetReport([FromQuery] bool isImport, [FromQuery] int month, [FromQuery] int year, [FromQuery] string supplierName = null, int page = 1, int size = 25, bool isForeignCurrency = false, int divisionId = 0)
        {
            try
            {
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                //int offSet = 7;
                var data = Service.GetReport(isImport, page, size, supplierName, month, year, offSet, isForeignCurrency, divisionId);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Data,
                    info = new
                    {
                        data.Count,
                        data.Order,
                        data.Selected
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

        [HttpGet("reports/detail")]
        public IActionResult GetReportDetail([FromQuery] bool isImport, [FromQuery] int month, [FromQuery] int year, [FromQuery] string supplierCode = null, int page = 1, int size = 25, bool isForeignCurrency = false, int divisionId = 0)
        {
            try
            {
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                //int offSet = 7;
                var data = Service.GetReportDetail(isImport, supplierCode, month, year, offSet, isForeignCurrency, divisionId);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Data,
                    info = new
                    {
                        data.Count,
                        data.Order,
                        data.Selected
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

        [HttpGet("reports/detail/xls")]
        public IActionResult GetReportDetailXls([FromQuery] bool isImport, [FromQuery] int month, [FromQuery] int year, [FromQuery] string supplierCode = null, int page = 1, int size = 25, bool isForeignCurrency = false, int divisionId = 0)
        {
            try
            {
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                //int offSet = 7;
                var data = Service.GetReportDetail(isImport, supplierCode, month, year, offSet, isForeignCurrency, divisionId);
                var xls = Service.GenerateExcelDetail(data, divisionId, month, year);

                string fileName = "";

                if (isImport)
                {
                    fileName = string.Format("Rincian Saldo Hutang Impor Periode {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);
                }
                else
                {
                    if (isForeignCurrency)
                        fileName = string.Format("Rincian Saldo Hutang Lokal Valas Periode {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);
                    else
                        fileName = string.Format("Rincian Saldo Hutang Lokal Periode {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);
                }


                var xlsInBytes = xls.ToArray();

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

        [HttpGet("reports/downloads/xls")]
        public IActionResult GetXls([FromQuery] bool isImport, [FromQuery] int month, [FromQuery] int year, [FromQuery] string supplierName = null, bool isForeignCurrency = false, int divisionId = 0)
        {
            try
            {
                byte[] xlsInBytes;
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var xls = Service.GenerateExcel(isImport, supplierName, month, year, offSet, isForeignCurrency, divisionId);

                string fileName = "";

                if (isImport)
                {
                    fileName = string.Format("Saldo Hutang Impor Periode {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);
                }
                else
                {
                    if (isForeignCurrency)
                        fileName = string.Format("Saldo Hutang Lokal Valas Periode {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);
                    else
                        fileName = string.Format("Saldo Hutang Lokal Periode {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);
                }


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
        public IActionResult GetPdf([FromQuery] bool isImport, [FromQuery] int month, [FromQuery] int year, [FromQuery] string supplierName = null, bool isForeignCurrency = false, int divisionId = 0)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);

                var data = Service.GeneratePdf(isImport, supplierName, month, year, offSet, isForeignCurrency, divisionId);

                // DailyBankTransactionPDFTemplate PdfTemplate = new DailyBankTransactionPDFTemplate();
                // MemoryStream stream = PdfTemplate.GeneratePdfTemplate(data, clientTimeZoneOffset);
                MemoryStream stream;
                string fileName = "";
                if (isImport)
                {
                    stream = ImportCreditBalanceReportPDFTemplate.GeneratePdfTemplate(data, month, year, divisionId);
                    fileName = string.Format("Saldo Hutang Impor Periode {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);
                }
                else if (isForeignCurrency)
                {
                    stream = LocalValasCreditBalanceReportPDFTemplate.GeneratePdfTemplate(data, month, year, divisionId);
                    fileName = string.Format("Saldo Hutang Lokal Valas Periode {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);
                }
                else
                {
                    stream = LocalCreditBalanceReportPDFTemplate.GeneratePdfTemplate(data, month, year, divisionId);
                    fileName = string.Format("Saldo Hutang Lokal Periode {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);
                }

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = string.Format(fileName)
                };
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

    }
}
