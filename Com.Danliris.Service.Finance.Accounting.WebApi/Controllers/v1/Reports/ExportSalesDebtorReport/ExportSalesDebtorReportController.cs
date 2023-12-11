using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Reports;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Rreports.ExportSalesDebtorReportController;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.Reports.ExportSalesDebtorReport
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/export-sales-debtor-report")]
    public class ExportSalesDebtorReportController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IExportSalesDebtorReportService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public ExportSalesDebtorReportController(IIdentityService identityService, IValidateService validateService, IExportSalesDebtorReportService service, IMapper mapper)
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
        public IActionResult Get([FromQuery] int month, [FromQuery] int year, [FromQuery] string type)
        {
            try
            {
                VerifyUser();
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                //int offSet = 7;
                var data = Service.GetMonitoring(month, year,type, offSet);

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
        public async Task<IActionResult> GetXls([FromQuery] int month, [FromQuery] int year, [FromQuery] string type)
        {
            try
            {
                VerifyUser();
                byte[] xlsInBytes;
                var xls = await Service.GenerateExcel(month,year,type);

                string filename = String.Format("Report Piutang {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

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
