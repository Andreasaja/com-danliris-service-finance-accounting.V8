using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingExpedition.Reports;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
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
    [Route("v{version:apiVersion}/garment-purchasing-expeditions")]
    [Authorize]

    public class GarmentPurchasingExpeditionController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IGarmentPurchasingExpeditionService _service;
        private readonly IGarmentPurchasingExpeditionReportService _reportService;
        private readonly IValidateService _validateService;
        private const string ApiVersion = "1.0";
        public GarmentPurchasingExpeditionController(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            _service = serviceProvider.GetService<IGarmentPurchasingExpeditionService>();
            _reportService = serviceProvider.GetService<IGarmentPurchasingExpeditionReportService>();
            _validateService = serviceProvider.GetService<IValidateService>();
        }

        private void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpPost("send-to-verification")]
        public async Task<IActionResult> SendToVerification([FromBody] SendToVerificationAccountingForm form)
        {
            //SUDAH BISA
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = await _service.SendToVerification(form);


                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return Created(string.Concat(Request.Path, "/", id), result);
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

        [HttpPost("send-to-accounting")]
        public async Task<IActionResult> SendToAccounting([FromBody] SendToVerificationAccountingForm form)
        {
            //SUDAH BISA
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = await _service.SendToAccounting(form);


                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return Created(string.Concat(Request.Path, "/", id), result);
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

        [HttpGet("send-to-verification-or-accounting")]
        public IActionResult GetSendToVerificationOrAccounting([FromQuery] string keyword, [FromQuery] string order = "{}", [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            //SUDAH BISA
            try
            {
                var result = _service.GetSendToVerificationOrAccounting(keyword, page, size, order);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    statusCode = General.OK_STATUS_CODE,
                    message = General.OK_MESSAGE,
                    data = result.Data,
                    info = new
                    {
                        total = result.Count,
                        page,
                        size,
                        count = result.Data.Count
                    }
                });
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string keyword, [FromQuery] int internalNoteId, [FromQuery] int supplierId, [FromQuery] GarmentPurchasingExpeditionPosition position, [FromQuery] string order = "{}", [FromQuery] int page = 1, [FromQuery] int size = 10,[FromQuery]string currencyCode = null)
        {
            //sudah bisa
            try
            {
                VerifyUser();
                if (position == GarmentPurchasingExpeditionPosition.Purchasing)
                {
                    var result_retur = _service.GetByPositionRetur(keyword, page, size, order, position, internalNoteId, supplierId, currencyCode);
                    return Ok(new
                    {
                        apiVersion = ApiVersion,
                        statusCode = General.OK_STATUS_CODE,
                        message = General.OK_MESSAGE,
                        data = result_retur.Data,
                        info = new
                        {
                            total = result_retur.Count,
                            page,
                            size
                        }
                    });
                }
                else
                {
                    var result = _service.GetByPosition(keyword, page, size, order, position, internalNoteId, supplierId, currencyCode);
                    return Ok(new
                    {
                        apiVersion = ApiVersion,
                        statusCode = General.OK_STATUS_CODE,
                        message = General.OK_MESSAGE,
                        data = result.Data,
                        info = new
                        {
                            total = result.Count,
                            page,
                            size
                        }
                    });
                }
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }

        [HttpPut("verification-accepted")]
        public async Task<IActionResult> VerificationAccepted([FromBody] List<int> ids)
        {
            //SUDAH BISA
            try
            {
                VerifyUser();

                await _service.VerificationAccepted(ids);

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("cashier-accepted")]
        public async Task<IActionResult> CashierAccepted([FromBody] List<int> ids)
        {
            //sudah bisa
            try
            {
                VerifyUser();

                await _service.CashierAccepted(ids);

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("accounting-accepted")]
        public async Task<IActionResult> AccountingAccepted([FromBody] List<int> ids)
        {
            //SUDAH
            try
            {
                VerifyUser();

                await _service.AccountingAccepted(ids);

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }
        [HttpPut("accounting-disposition-not-ok")]
        public async Task<IActionResult> AccountingAccepted([FromBody] RejectionForm form)
        {
            try
            {
                VerifyUser();

                await _service.SendToPurchasingRejected(form.ids,form.Remark);

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("purchasing-accepted")]
        public async Task<IActionResult> PurchasingAccepted([FromBody] List<int> ids)
        {
            //sudah bisa
            try
            {
                VerifyUser();

                await _service.PurchasingAccepted(ids);

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("void-verification-accepted/{id}")]
        public async Task<IActionResult> VoidVerificationAccepted([FromRoute] int id)
        {
            //SUDAH BISA
            try
            {
                VerifyUser();

                await _service.VoidVerificationAccepted(id);

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("void-cashier-accepted/{id}")]
        public async Task<IActionResult> VoidCashierAccepted([FromRoute] int id)
        {
            //sudah
            try
            {
                VerifyUser();

                await _service.VoidCashierAccepted(id);

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("void-accounting-accepted/{id}")]
        public async Task<IActionResult> VoidAccountingAccepted([FromRoute] int id)
        {
            //sudah
            try
            {
                VerifyUser();

                await _service.VoidAccountingAccepted(id);

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("send-to-internal-note/{id}")]
        public async Task<IActionResult> SendToInternalNote([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                await _service.SendToPurchasing(id);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("verified")]
        public IActionResult GetVerified([FromQuery] string keyword, [FromQuery] string order = "{}", [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            //selesai
            try
            {
                var result = _service.GetVerified(keyword, page, size, order);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    statusCode = General.OK_STATUS_CODE,
                    message = General.OK_MESSAGE,
                    data = result.Data,
                    info = new
                    {
                        total = result.Count,
                        page,
                        size,
                        count = result.Data.Count
                    }
                });
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }

        [HttpGet("verified/{id}")]
        public IActionResult GetVerifiedById([FromRoute] int id)
        {
            //sudah
            try
            {
                var result = _service.GetById(id);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    statusCode = General.OK_STATUS_CODE,
                    message = General.OK_MESSAGE,
                    data = result
                });
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }

        [HttpPut("send-to-accounting/{id}")]
        public async Task<IActionResult> SendToAccounting([FromRoute] int id)
        {
            //sudah
            try
            {
                VerifyUser();

                await _service.SendToAccounting(id);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("send-to-cashier/{id}")]
        public async Task<IActionResult> SendToCashier([FromRoute] int id)
        {
            //sudah
            try
            {
                VerifyUser();

                await _service.SendToCashier(id);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPut("send-to-purchasing-rejected/{id}")]
        public async Task<IActionResult> SendToPurchasingRejected([FromRoute] int id, [FromBody] RejectionForm form)
        {
            //sudah
            try
            {
                VerifyUser();

                await _service.SendToPurchasingRejected(id, form.Remark);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return NoContent();
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("report")]
        public IActionResult GetReport(int internalNoteId, int supplierId, GarmentPurchasingExpeditionPosition position, DateTimeOffset? startDate, DateTimeOffset? endDate, DateTimeOffset? startDateAccounting, DateTimeOffset? endDateAccounting)
        {
            //sudah
            try
            {
                VerifyUser();
                endDate = !endDate.HasValue ? DateTimeOffset.Now : endDate.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Date.AddHours(17);
                startDate = !startDate.HasValue ? DateTimeOffset.MinValue : startDate;

                endDateAccounting = !endDateAccounting.HasValue ? DateTimeOffset.Now : endDateAccounting.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Date.AddHours(17);
                startDateAccounting = !startDateAccounting.HasValue ? DateTimeOffset.MinValue : startDateAccounting;

                var result = _reportService.GetReportViewModel(internalNoteId, supplierId, position, startDate.GetValueOrDefault(), endDate.GetValueOrDefault(), startDateAccounting.GetValueOrDefault(), endDateAccounting.GetValueOrDefault());
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    statusCode = General.OK_STATUS_CODE,
                    message = General.OK_MESSAGE,
                    data = result
                });
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }

        [HttpGet("report/xls")]
        public IActionResult GetReportXls(int internalNoteId, int supplierId, GarmentPurchasingExpeditionPosition position, DateTimeOffset? startDate, DateTimeOffset? endDate, DateTimeOffset? startDateAccounting, DateTimeOffset? endDateAccounting)
        {
            //proses
            try
            {
                VerifyUser();
                endDate = !endDate.HasValue ? DateTimeOffset.Now : endDate.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Date.AddHours(17);
                startDate = !startDate.HasValue ? DateTimeOffset.MinValue : startDate;


                endDateAccounting = !endDateAccounting.HasValue ? DateTimeOffset.Now : endDateAccounting.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Date.AddHours(17);
                startDateAccounting = !startDateAccounting.HasValue ? DateTimeOffset.MinValue : startDateAccounting;

                var stream = _reportService.GenerateExcel(internalNoteId, supplierId, position, startDate.GetValueOrDefault(), endDate.GetValueOrDefault(), startDateAccounting.GetValueOrDefault(), endDateAccounting.GetValueOrDefault());
                //var stream = _reportService.GenerateExcel(internalNoteId, supplierId, position, !startDate.HasValue ? DateTimeOffset.MinValue : startDate, endDate.Value.AddDays(7), startDateAccounting.Value.AddDays(7), endDateAccounting.Value.AddDays(7));

                //var stream = _reportService.GenerateExcel(internalNoteId, supplierId, position, (startDate == null ? DateTimeOffset.MinValue : startDate), endDate.GetValueOrDefault(), startDateAccounting.GetValueOrDefault(), endDateAccounting.GetValueOrDefault());
                //(entity.SendToAccountingDate == null ? DateTimeOffset.MinValue : entity.SendToAccountingDate)

                var bytes = stream.ToArray();
                var filename = "Laporan Ekspedisi Garment.xlsx";
                var file = File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }
    }

    public class RejectionForm
    {
        public string Remark { get; set; }
        public List<int> ids { get; set; }
    }
}
