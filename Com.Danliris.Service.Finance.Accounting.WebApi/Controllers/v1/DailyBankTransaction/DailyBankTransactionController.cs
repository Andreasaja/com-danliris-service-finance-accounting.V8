using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction.Excel;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Com.Danliris.Service.Production.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.DailyBankTransaction
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/daily-bank-transactions")]
    [Authorize]
    public class DailyBankTransactionController : BaseController<DailyBankTransactionModel, DailyBankTransactionViewModel, IDailyBankTransactionService>
    {
        public DailyBankTransactionController(IIdentityService identityService, IValidateService validateService, IDailyBankTransactionService service, IMapper mapper) : base(identityService, validateService, service, mapper, "1.0.0")
        {
        }

        public override async Task<ActionResult> Post([FromBody] DailyBankTransactionViewModel viewModel)
        {
            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel);

                DailyBankTransactionModel model = Mapper.Map<DailyBankTransactionModel>(viewModel);
                if (viewModel.Status.Equals("OUT", StringComparison.OrdinalIgnoreCase) && viewModel.SourceType.Equals("pendanaan", StringComparison.OrdinalIgnoreCase))
                {

                    await Service.CreateInOutTransactionAsync(model);
                }
                else
                {

                    await Service.CreateAsync(model);
                }

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, Utilities.General.CREATED_STATUS_CODE, Utilities.General.OK_MESSAGE)
                    .Ok();
                return Created(String.Concat(Request.Path, "/", 0), Result);
            }
            catch (ServiceValidationException e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, Utilities.General.BAD_REQUEST_STATUS_CODE, Utilities.General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("mutation/report")]
        public IActionResult GetReport(int bankId, int month, int year)
        {
            try
            {
                int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
                ReadResponse<DailyBankTransactionModel> Result = Service.GetReport(bankId, month, year, clientTimeZoneOffset);

                return Ok(new
                {
                    apiVersion = "1.0.0",
                    data = Result.Data,
                    message = Utilities.General.OK_MESSAGE,
                    statusCode = Utilities.General.OK_STATUS_CODE
                });
            }
            catch(Exception e)
            {
                var result = new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message + "\n" + e.StackTrace);
                return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("mutation/report/download")]
        public IActionResult GetReportXls(int bankId, int month, int year,string currencyCode)
        {
            try
            {
                byte[] xlsInBytes;
                int clientTimeZoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);

                var xls = Service.GetExcel(bankId, month, year, clientTimeZoneOffset);

                string filename = String.Format("Mutasi Bank Harian - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;

            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("mutation/report/pdf")]
        public IActionResult GetReportPdf(int bankId, int month, int year)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                int clientTimeZoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);

                var data = Service.GeneratePdf(bankId, month, year, clientTimeZoneOffset);
                var beforeBalance = Service.GetBeforeBalance(bankId, month, year, clientTimeZoneOffset);
                var dataAccountBank = Service.GetDataAccountBank(bankId);

                // DailyBankTransactionPDFTemplate PdfTemplate = new DailyBankTransactionPDFTemplate();
                // MemoryStream stream = PdfTemplate.GeneratePdfTemplate(data, clientTimeZoneOffset);
                MemoryStream stream = DailyBankTransactionPDFTemplate.GeneratePdfTemplate(data, month, year, beforeBalance, dataAccountBank, clientTimeZoneOffset);
                string filename = string.Format("Mutasi Bank Harian - {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year);
                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = string.Format(filename)
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

        [HttpDelete("by-reference-no/{referenceNo}")]
        public async Task<IActionResult> DeleteByReferenceNo([FromRoute] string referenceNo)
        {
            try
            {
                VerifyUser();

                await Service.DeleteByReferenceNoAsync(referenceNo);

                return NoContent();
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }


        [HttpPut("posting")]
        public async Task<IActionResult> Posting([FromBody] List<int> ids)
        {
            VerifyUser();

            await Service.Posting(ids);

            return NoContent();
        }

        [HttpGet("daily-balance/report/accountbank")]
        public IActionResult GetDailyBalanceAccountBankReport(int bankId, DateTime startDate, DateTime endDate, string divisionName)
        {
            var Result = Service.GetDailyBalanceReport(bankId, startDate, endDate, divisionName);

            return Ok(new
            {
                apiVersion = "1.0.0",
                data = Result,
                message = Utilities.General.OK_MESSAGE,
                statusCode = Utilities.General.OK_STATUS_CODE
            });
        }

        [HttpGet("daily-balance/report/currency")]
        public IActionResult GetDailyBalanceCurrencyReport(int bankId, DateTime startDate, DateTime endDate, string divisionName)
        {
            var Result = Service.GetDailyBalanceCurrencyReport(bankId, startDate, endDate, divisionName);

            return Ok(new
            {
                apiVersion = "1.0.0",
                data = Result,
                message = Utilities.General.OK_MESSAGE,
                statusCode = Utilities.General.OK_STATUS_CODE
            });
        }

        [HttpGet("daily-balance/report/download")]
        public IActionResult GetDailyBalanceReportXls(int bankId, DateTime startDate, DateTime endDate, string divisionName)
        {
            try
            {
                byte[] xlsInBytes;
                int clientTimeZoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);

                var xls = Service.GenerateExcelDailyBalance(bankId, startDate, endDate, divisionName, clientTimeZoneOffset);

                string filename = String.Format("Saldo Bank Harian - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;

            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("report")]
        public IActionResult GetReportAll(string referenceNo, int accountBankId, string division, DateTimeOffset? startDate, DateTimeOffset? endDate, int page = 0, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
                VerifyUser();
                ReadResponse<DailyBankTransactionModel> Result = Service.GetReportAll(referenceNo,accountBankId,division,startDate,endDate,page,size,order,select,keyword,filter);

                return Ok(new
                {
                    apiVersion = "1.0.0",
                    data = Result.Data,
                    message = Utilities.General.OK_MESSAGE,
                    statusCode = Utilities.General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message + "\n" + e.StackTrace);
                return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("loader")]
        public IActionResult GetLoader( string keyword = null, string filter = "{}")
        {
            try
            {
                int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
                ReadResponse<DailyBankTransactionModel> Result = Service.GetLoader(keyword, filter);

                return Ok(new
                {
                    apiVersion = "1.0.0",
                    data = Result.Data,
                    message = Utilities.General.OK_MESSAGE,
                    statusCode = Utilities.General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message + "\n" + e.StackTrace);
                return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("report/xls-in")]
        public IActionResult GetReportAllXlsIn(string referenceNo, int accountBankId,string accountBankName, string division, DateTimeOffset? startDate, DateTimeOffset? endDate, int page = 0, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
                VerifyUser();
                ReadResponse<DailyBankTransactionModel> Result = Service.GetReportAll(referenceNo, accountBankId, division, startDate, endDate, page, size, order, select, keyword, filter);
               
                var filename = "Laporan Bank Harian Masuk";

                var xls = AutoDailyBankTransactionExcelGenerator.CreateIn(filename, Result.Data,referenceNo, accountBankId, accountBankName,division, startDate, endDate, clientTimeZoneOffset);

                var xlsInBytes = xls.ToArray();

                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
                
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message + "\n" + e.StackTrace);
                return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("report/xls-out")]
        public IActionResult GetReportAllXlsOut(string referenceNo, int accountBankId, string accountBankName, string division, DateTimeOffset? startDate, DateTimeOffset? endDate, int page = 0, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
                VerifyUser();
                ReadResponse<DailyBankTransactionModel> Result = Service.GetReportAll(referenceNo, accountBankId, division, startDate, endDate, page, size, order, select, keyword, filter);

                var filename = "Laporan Bank Harian Keluar";

                var xls = AutoDailyBankTransactionExcelGenerator.CreateOut(filename, Result.Data, referenceNo, accountBankId, accountBankName, division, startDate, endDate, clientTimeZoneOffset);

                var xlsInBytes = xls.ToArray();

                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;

            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message + "\n" + e.StackTrace);
                return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }
    }
}
