using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.LocalDPPVATBankExpenditureNoteMonthlyRecap;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.GarmentFinance.Report.LocalDPPVATBankExpenditureNoteMonthlyRecap
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/dpp-vat-expenditure-note/local-monthly-recap")]
    public class LocalDPPVATBankExpenditureNoteMonthlyRecapMonthlyRecapController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly ILocalDPPVATBankExpenditureNoteMonthlyRecapService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public LocalDPPVATBankExpenditureNoteMonthlyRecapMonthlyRecapController(IIdentityService identityService, IValidateService validateService, ILocalDPPVATBankExpenditureNoteMonthlyRecapService service, IMapper mapper)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            ApiVersion = "1.0.0";
            Mapper = mapper;
        }

        //protected void VerifyUser()
        //{
        //    IdentityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
        //    IdentityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
        //    IdentityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        //}

        [HttpGet("monitoring")]
        public IActionResult GetReport([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            try
            {
                //VerifyUser();
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var data = Service.GetMonitoring(dateFrom, dateTo, offSet);

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
        public IActionResult GetXls([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            try
            {
                //VerifyUser();

                byte[] xlsInBytes;
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);

                var xls = Service.GenerateExcel(dateFrom, dateTo, offset);

                string filename = String.Format("Jurnal Pengeluaran Kas Bank Lokal - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

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

        [HttpGet("detail/download")]
        public IActionResult GetDetailXls([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            try
            {
                //VerifyUser();

                byte[] xlsInBytes;
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);

                var xls = Service.GenerateDetailExcel(dateFrom, dateTo, offset);

                string filename = String.Format("Detail Jurnal Pengeluaran Kas Bank Lokal - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

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