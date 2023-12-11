using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBExpeditionRealizationReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.VBExpeditionRealizationReport
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/vb-expedition-realization-report")]
    [Authorize]

    public class VBExpeditionRealizationReportController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IVBExpeditionRealizationReportService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public VBExpeditionRealizationReportController(IIdentityService identityService, IValidateService validateService, IMapper mapper, IVBExpeditionRealizationReportService service)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            Mapper = mapper;
            ApiVersion = "1.0.0";
        }

        protected void ValidateUser()
        {
            IdentityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            IdentityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            IdentityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);

        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetReportAll(int vbRequestId, int vbRealizeId, string ApplicantName, int unitId, int divisionId, string isVerified, DateTimeOffset? realizeDateFrom, DateTimeOffset? realizeDateTo, [FromHeader(Name = "x-timezone-offset")] string timezone)
        {
            int offset = Convert.ToInt32(timezone);

            try
            {
                ValidateUser();
                var data = await Service.GetReport(vbRequestId, vbRealizeId, ApplicantName, unitId, divisionId, isVerified, realizeDateFrom, realizeDateTo, offset);

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
        public async Task<IActionResult> GetXlsAll(int vbRequestId, int vbRealizeId, string ApplicantName, int unitId, int divisionId, string isVerified, DateTimeOffset? realizeDateFrom, DateTimeOffset? realizeDateTo, [FromHeader(Name = "x-timezone-offset")] string timezone)
        {

            try
            {
                ValidateUser();
                byte[] xlsInBytes;
                int offset = Convert.ToInt32(timezone);

                var xls = await Service.GenerateExcel(vbRequestId, vbRealizeId, ApplicantName, unitId, divisionId, isVerified, realizeDateFrom, realizeDateTo, offset);

                string filename = "Laporan Status VB.xlsx";

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
