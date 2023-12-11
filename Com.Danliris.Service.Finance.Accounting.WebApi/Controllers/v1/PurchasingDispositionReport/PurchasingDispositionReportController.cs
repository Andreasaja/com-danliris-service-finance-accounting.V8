using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionReport;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.PurchasingDispositionReport
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/purchasing-disposition-report")]
    [Authorize]
    public class PurchasingDispositionReportController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IPurchasingDispositionExpeditionService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public PurchasingDispositionReportController(IIdentityService identityService, IValidateService validateService, IMapper mapper, IPurchasingDispositionExpeditionService service)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            Mapper = mapper;
            ApiVersion = "1.0.0";
        }
        protected void VerifyUser()
        {
            IdentityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            IdentityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            IdentityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetReportAsync(string SPBStatus, string PaymentStatus, string bankExpenditureNoteNo,DateTimeOffset? dateFromPayment = null, DateTimeOffset? dateToPayment = null,DateTime ? dateFrom = null, DateTime? dateTo = null, int page = 1, int size = 25, string order = "{}", string filter = "{}")
        {
            try
            {
                VerifyUser();
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                //int offSet = 7;
                var data = await Service.GetReportAsync(page, size, order, filter, dateFrom, dateTo, dateFromPayment,dateToPayment, bankExpenditureNoteNo,SPBStatus,PaymentStatus, offSet);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Data,
                    info = new
                    {
                        data.Count,
                        data.Order,
                        data.Selected,
                        size
                    },
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

        [HttpGet("downloads/xls")]
        public async Task<IActionResult> GetXlsAsync(string SPBStatus,string PaymentStatus, string bankExpenditureNoteNo, DateTime? dateFromPayment = null, DateTime? dateToPayment = null, string filter = "{}", DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            try
            {
                VerifyUser();
                byte[] xlsInBytes;
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var xls = await Service.GenerateExcelAsync(1, int.MaxValue, "{}", filter, dateFrom, dateTo, dateFromPayment, dateToPayment, bankExpenditureNoteNo,SPBStatus, PaymentStatus, offSet);

                string fileName = "";
                if (dateFrom == null && dateTo == null)
                    fileName = string.Format("Ekspedisi Disposisi Pembelian");
                else if (dateFrom != null && dateTo == null)
                    fileName = string.Format("Ekspedisi Disposisi Pembelian {0}", dateFrom.Value.ToString("dd/MM/yyyy"));
                else if (dateFrom == null && dateTo != null)
                    fileName = string.Format("Ekspedisi Disposisi Pembelian {0}", dateTo.GetValueOrDefault().ToString("dd/MM/yyyy"));
                else
                    fileName = string.Format("Ekspedisi Disposisi Pembelian {0} - {1}", dateFrom.GetValueOrDefault().ToString("dd/MM/yyyy"), dateTo.Value.ToString("dd/MM/yyyy"));

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
    }
}
