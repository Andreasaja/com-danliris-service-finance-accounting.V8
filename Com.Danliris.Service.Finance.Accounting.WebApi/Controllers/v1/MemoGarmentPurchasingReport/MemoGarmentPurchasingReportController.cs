using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.MemoGarmentPurchasingReport;
using System.Globalization;
using System.IO;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.MemoGarmentPurchasingReport
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/garment-purchasing/memo/report")]
    [Authorize]
    public class MemoGarmentPurchasingReportController : Controller
    {
        private readonly IIdentityService _identityService;
        //private readonly IValidateService _validateService;
        private readonly IMemoGarmentPurchasingReportService _service;
        private readonly IMapper _mapper;
        private const string ApiVersion = "1.0";

        public MemoGarmentPurchasingReportController(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            //_validateService = serviceProvider.GetService<IValidateService>();
            _service = serviceProvider.GetService<IMemoGarmentPurchasingReportService>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int size = 25, string filter = "{}")
        {
            try
            {
                var queryResult = _service.ReadReportDetailBased(page, size, filter);
                var dataViewModel = _mapper.Map<List<MemoGarmentPurchasingDetailModel>, List<MemoGarmentPurchasingReportViewModel>>(queryResult.Data);

                var result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(_mapper, dataViewModel, page, size, queryResult.Count, queryResult.Data.Count, queryResult.Order, queryResult.Selected);
                return Ok(result);
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("xls")]
        public async Task<IActionResult> GetXls(int year, int month, int accountingBookId, string accountingBookType, bool valas)
        {

            try
            {
                byte[] xlsInBytes;
                var xls = _service.GenerateExcel(year, month, accountingBookId, accountingBookType, valas);
                string fileName = $"Laporan Data Memorail - {new DateTime(year, month, 1).ToString("MMMM yyyy", new CultureInfo("id-ID"))}.xlsx";

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

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf(int year, int month, int accountingBookId, string accountingBookType, bool valas)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                int timeOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                string date = new DateTime(year, month, 1).ToString("MMMM yyyy", new CultureInfo("id-ID"));

                var report = _service.GetReportPdfData(year, month, accountingBookId, accountingBookType, valas);
                if (report.Count <= 0)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }

                MemoryStream stream = MemoGarmentPurchasingReportPdfTemplate.GeneratePdfTemplate(report.Data, date, accountingBookType);
                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = $"Laporan Data Memorail - {date}.pdf"
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
    }
}
