using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.MemoDetailGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.MemoDetailGarmentPurchasing.ExcelGenerator;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.MemoDetailGarmentPurchasing
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/detail-garment-purchasing/memo")]
    [Authorize]

    public class MemoDetailGarmentPurchasingContoller: Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IValidateService _validateService;
        private readonly IMemoDetailGarmentPurchasingService _service;
        private readonly IMapper _mapper;
        private const string ApiVersion = "1.0";

        public MemoDetailGarmentPurchasingContoller(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            _validateService = serviceProvider.GetService<IValidateService>();
            _service = serviceProvider.GetService<IMemoDetailGarmentPurchasingService>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MemoDetailGarmentPurchasingViewModel viewModel)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(viewModel);

                
                await _service.CreateAsync(viewModel);
                
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
            catch (Exception ex)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, ex.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            try
            {
                var viewModel = await _service.ReadByIdAsync(Id);

                if (viewModel == null)
                {
                    var result = new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE).Fail();
                    return NotFound(result);
                }
                else
                {
                    var result = new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE).Ok<MemoDetailGarmentPurchasingViewModel>(_mapper, viewModel);
                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("report")]
        public IActionResult GetReport([FromQuery] DateTimeOffset date, int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}", int valas = -1)
        {
            try
            {
                var queryResult = _service.GetReport(date, page, size, order, select, keyword, filter, valas);

                var result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(_mapper, queryResult.Data, page, size, queryResult.Count, queryResult.Data.Count, queryResult.Order, select);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, ex.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] MemoDetailGarmentPurchasingViewModel viewModel)
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

        [HttpPut("posting")]
        public IActionResult Posting([FromBody] PostingFormDto form)
        {
            try
            {
                VerifyUser();
                _service.Posting(form.Ids);

                return NoContent();
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("reports/downloads/pdf")]
        public IActionResult GetPdf([FromQuery] DateTimeOffset date, int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}", int valas = -1)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);

                MemoryStream stream;

                var queryResult = _service.GetPDF(date, page, size, order, select, keyword, filter, valas);

                var month = date.Month;
                var year = date.Year;
                string monthString = "";

                if (month == 1)
                {
                    monthString = "Januari";
                }
                else if (month == 2)
                {
                    monthString = "Februari";
                }
                else if (month == 3)
                {
                    monthString = "Maret";
                }
                else if (month == 4)
                {
                    monthString = "April";
                }
                else if (month == 5)
                {
                    monthString = "Mei";
                }
                else if (month == 6)
                {
                    monthString = "Juni";
                }
                else if (month == 7)
                {
                    monthString = "Juli";
                }
                else if (month == 8)
                {
                    monthString = "Agustus";
                }
                else if (month == 9)
                {
                    monthString = "September";
                }
                else if (month == 10)
                {
                    monthString = "Oktober";
                }
                else if (month == 11)
                {
                    monthString = "November";
                }
                else if (month == 12)
                {
                    monthString = "Desember";
                }

                stream = MemorialJobGarmentDetailPDFTemplate.GeneratePdfTemplate(queryResult.Data, monthString, year);

                string fileName = "Laporan Rincian Memorial "+monthString+ " " +year;

                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = string.Format(fileName)
                };

            }
            catch (Exception ex)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, ex.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("reports/downloads/xls")]
        public IActionResult GetXls([FromQuery] DateTimeOffset date, int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}", int valas = -1)
        {
            try
            {
                var queryResult = _service.GetReport(date, page, size, order, select, keyword, filter, valas);

                int month = date.Date.Month;
                int year = date.Date.Year;
                var monthName = _months.FirstOrDefault(element => element.Key == month);

                MemoryStream result = new MemoryStream();
                var filename = $"Laporan Rincian Memorial";
                result = MemoDetailGarmentExcelGenerator.GenerateExcel(queryResult, "Laporan Rincian Data Memorial", month, year);
                filename += $" {monthName.Value} {year}.xlsx";

                var bytes = result.ToArray();

                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        private static readonly List<MonthName> _months = new List<MonthName>()
        {
            new MonthName(1, "Januari"),
            new MonthName(2, "Februari"),
            new MonthName(3, "Maret"),
            new MonthName(4, "April"),
            new MonthName(5, "Mei"),
            new MonthName(6, "Juni"),
            new MonthName(7, "Juli"),
            new MonthName(8, "Agustus"),
            new MonthName(9, "September"),
            new MonthName(10, "Oktober"),
            new MonthName(11, "November"),
            new MonthName(12, "Desember"),
        };
    }

    public class PostingFormDto
    {
        public PostingFormDto()
        {

        }

        public List<int> Ids { get; set; }
    }
}
