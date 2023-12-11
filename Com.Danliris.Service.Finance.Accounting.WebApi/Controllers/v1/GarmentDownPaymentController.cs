using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDownPaymentReport;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDownPaymentReport.Excel;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
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
    [Route("v{version:apiVersion}/garment-down-payments")]
    [Authorize]
    public class GarmentDownPaymentController : Controller
    {
        private const string ApiVersion = "1.0.0";
        private readonly IGarmentDownPaymentReportService _service;
        private readonly IIdentityService _identityService;

        public GarmentDownPaymentController(IServiceProvider serviceProvider)
        {
            _service = serviceProvider.GetService<IGarmentDownPaymentReportService>();
            _identityService = serviceProvider.GetService<IIdentityService>();
        }

        private void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet]
        public IActionResult GetSummary([FromQuery] SupplierType supplierType, [FromQuery] DateTimeOffset date)
        {
            try
            {
                VerifyUser();

                var data = _service.GetReport(supplierType, date);

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

        [HttpGet("xls")]
        public IActionResult GetReportXls([FromQuery] SupplierType supplierType, [FromQuery] DateTimeOffset date)
        {
            try
            {
                VerifyUser();

                var data = _service.GetReport(supplierType, date);
                var stream = GarmentDownPaymentExcelGenerator.Generate(data, date, _identityService.TimezoneOffset);

                var bytes = stream.ToArray();

                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Uang Muka.xlsx");
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }

    }
}
