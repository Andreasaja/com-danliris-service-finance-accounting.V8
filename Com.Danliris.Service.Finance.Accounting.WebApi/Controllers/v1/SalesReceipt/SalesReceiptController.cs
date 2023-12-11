using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.SalesReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.SalesReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.SalesReceipt;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Com.Danliris.Service.Production.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.SalesReceipt
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/sales-receipts")]
    [Authorize]

    public class SalesReceiptController : BaseController<SalesReceiptModel, SalesReceiptViewModel, ISalesReceiptService>
    {
        public SalesReceiptController(IIdentityService identityService, IValidateService validateService, ISalesReceiptService service, IMapper mapper) : base(identityService, validateService, service, mapper, "1.0.0")
        {
        }

        [HttpGet("pdf/{Id}")]
        public async Task<IActionResult> GetSalesReceiptPDF([FromRoute] int Id)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                int timeoffsset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                SalesReceiptModel model = await Service.ReadByIdAsync(Id);

                if (model == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    SalesReceiptViewModel viewModel = Mapper.Map<SalesReceiptViewModel>(model);
                    var detailViewModel = viewModel.SalesReceiptDetails.FirstOrDefault();

                    SalesReceiptPDFTemplate PdfTemplate = new SalesReceiptPDFTemplate();
                    MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel, detailViewModel, timeoffsset);
                    return new FileStreamResult(stream, "application/pdf")
                    {
                        FileDownloadName = "Kuitansi - " + viewModel.SalesReceiptNo + ".pdf"
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

        [HttpPost("sales-invoice-report")]
        public IActionResult GetSalesInvoiceData([FromBody] SalesInvoicePostForm dataForm)
        {
            try
            {
                var data = Service.GetSalesInvoice(dataForm);

                return Ok(data);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("reports")]
        public IActionResult GetReportAll(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, [FromHeader(Name = "x-timezone-offset")] string timezone)
        {
            //int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
            int offset = Convert.ToInt32(timezone);
            try
            {
                VerifyUser();
                var data = Service.GetReport(dateFrom, dateTo, offset);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data,
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

        [HttpGet("reports/xls")]
        public IActionResult GetXlsAll(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, [FromHeader(Name = "x-timezone-offset")] string timezone)
        {

            try
            {
                VerifyUser();
                byte[] xlsInBytes;
                //int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                int offset = Convert.ToInt32(timezone);
                var xls = Service.GenerateExcel(dateFrom, dateTo, offset);

                string filename = "Laporan Kwitansi.xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
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
    }
}
