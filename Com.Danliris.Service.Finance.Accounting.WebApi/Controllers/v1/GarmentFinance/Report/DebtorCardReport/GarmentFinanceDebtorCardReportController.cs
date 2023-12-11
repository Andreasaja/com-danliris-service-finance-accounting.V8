using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.DebtorCard;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.GarmentFinance.Report.DebtorCardReport
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/debtor-card-report")]
    public class GarmentFinanceDebtorCardReportController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IGarmentFinanceDebtorCardReportService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public GarmentFinanceDebtorCardReportController(IIdentityService identityService, IValidateService validateService, IGarmentFinanceDebtorCardReportService service, IMapper mapper)
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
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var xls = Service.GenerateExcel(month, year,buyer, offSet);

                string filename = String.Format("Report Kartu Debitur - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));
                
                return File(xls.Item1.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", xls.Item2);
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
