using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.LocalSalesOutstanding;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.GarmentFinance.Report.LocalOutstandingSalesReport
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/local-outstanding-sales-report")]
    public class GarmentFinanceLocalOutstandingSalesReportController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IGarmentFinanceLocalSalesOutstandingReportService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public GarmentFinanceLocalOutstandingSalesReportController(IIdentityService identityService, IValidateService validateService, IGarmentFinanceLocalSalesOutstandingReportService service, IMapper mapper)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            ApiVersion = "1.0.0";
            Mapper = mapper;
        }


        protected void VerifyUser()
        {
            IdentityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            IdentityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            IdentityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet("monitoring")]
        public IActionResult Get([FromQuery] int month, [FromQuery] int year, [FromQuery] string buyer)
        {
            try
            {
                VerifyUser();
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                //int offSet = 7;
                var data = Service.GetMonitoring(month, year, buyer, offSet);

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

        [HttpGet("download")]
        public async Task<IActionResult> GetXls([FromQuery] int month, [FromQuery] int year, [FromQuery] string buyer)
        {
            try
            {
                VerifyUser();
                byte[] xlsInBytes;
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);

                var xls = await Service.GenerateExcel(month, year, buyer, offSet);

                string filename = String.Format("Report Outstanding Penjualan Local {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

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
