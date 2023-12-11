using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionPaymentReport;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionPaymentReport.ExcelGenerator;
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
    [Route("v{version:apiVersion}/garment-disposition-expeditions")]
    [Authorize]
    public class GarmentDispositionExpeditionController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IGarmentDispositionExpeditionService _service;
        private readonly IGarmentDispositionPaymentReportService _reportService;
        private readonly IValidateService _validateService;
        private const string ApiVersion = "1.0";
        public GarmentDispositionExpeditionController(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            _service = serviceProvider.GetService<IGarmentDispositionExpeditionService>();
            _reportService = serviceProvider.GetService<IGarmentDispositionPaymentReportService>();
            _validateService = serviceProvider.GetService<IValidateService>();
        }

        private void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string keyword, [FromQuery] int dispositionNoteId, [FromQuery] int supplierId, [FromQuery] GarmentPurchasingExpeditionPosition position, [FromQuery] string order = "{}", [FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string currencyCode = null, [FromQuery] string filter = "{}")
        {
            try
            {
                var result = _service.GetByPosition(keyword, page, size, order, position, dispositionNoteId, supplierId, currencyCode, filter);
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
          catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }

        [HttpPost("send-to-verification")]
        public async Task<IActionResult> SendToVerification([FromBody] SendToVerificationAccountingFormDto form)
        {
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
        public async Task<IActionResult> SendToAccounting([FromBody] SendToVerificationAccountingFormDto form)
        {
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
            try
            {
                VerifyUser();
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

        //[HttpGet]
        //public IActionResult Get([FromQuery] string keyword, [FromQuery] int internalNoteId, [FromQuery] int supplierId, [FromQuery] GarmentPurchasingExpeditionPosition position, [FromQuery] string order = "{}", [FromQuery] int page = 1, [FromQuery] int size = 10)
        //{
        //    try
        //    {
        //        var result = _service.GetByPosition(keyword, page, size, order, position, internalNoteId, supplierId);
        //        return Ok(new
        //        {
        //            apiVersion = ApiVersion,
        //            statusCode = General.OK_STATUS_CODE,
        //            message = General.OK_MESSAGE,
        //            data = result.Data,
        //            info = new
        //            {
        //                total = result.Count,
        //                page,
        //                size
        //            }
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
        //    }
        //}

        [HttpPut("verification-accepted")]
        public async Task<IActionResult> VerificationAccepted([FromBody] List<int> ids)
        {
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

        [HttpPut("purchasing-accepted")]
        public async Task<IActionResult> PurchasingAccepted([FromBody] List<int> ids)
        {
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

        [HttpGet("verified")]
        public IActionResult GetVerified([FromQuery] string keyword, [FromQuery] string order = "{}", [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
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

        [HttpPut("void-verification-accepted/{id}")]
        public async Task<IActionResult> VoidVerificationAccepted([FromRoute] int id)
        {
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

        [HttpPut("send-to-disposition-note/{id}")]
        public async Task<IActionResult> SendToDispositionNote([FromRoute] int id)
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

        //[HttpGet("verified")]
        //public IActionResult GetVerified([FromQuery] string keyword, [FromQuery] string order = "{}", [FromQuery] int page = 1, [FromQuery] int size = 10)
        //{
        //    try
        //    {
        //        var result = _service.GetVerified(keyword, page, size, order);
        //        return Ok(new
        //        {
        //            apiVersion = ApiVersion,
        //            statusCode = General.OK_STATUS_CODE,
        //            message = General.OK_MESSAGE,
        //            data = result.Data,
        //            info = new
        //            {
        //                total = result.Count,
        //                page,
        //                size,
        //                count = result.Data.Count
        //            }
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
        //    }
        //}

        //[HttpGet("verified/{id}")]
        //public IActionResult GetVerifiedById([FromRoute] int id)
        //{
        //    try
        //    {
        //        var result = _service.GetById(id);
        //        return Ok(new
        //        {
        //            apiVersion = ApiVersion,
        //            statusCode = General.OK_STATUS_CODE,
        //            message = General.OK_MESSAGE,
        //            data = result
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
        //    }
        //}

        [HttpPut("send-to-accounting/{id}")]
        public async Task<IActionResult> SendToAccounting([FromRoute] int id)
        {
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
        public async Task<IActionResult> GetReport([FromQuery] int dispositionId, [FromQuery] int epoId, [FromQuery] int supplierId, [FromQuery] GarmentPurchasingExpeditionPosition position, [FromQuery] string purchasingStaff, [FromQuery] DateTimeOffset? startDate, [FromQuery] DateTimeOffset? endDate)
        {
            try
            {
                VerifyUser();
                endDate = !endDate.HasValue ? DateTimeOffset.Now : endDate.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Date.AddHours(17);
                startDate = !startDate.HasValue ? DateTimeOffset.MinValue : startDate;

                var result = await _reportService.GetReport(dispositionId, epoId, supplierId, position, purchasingStaff, startDate.GetValueOrDefault(), endDate.GetValueOrDefault());
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

        [HttpGet("report/position-options")]
        public IActionResult GetPositions()
        {
            try
            {
                VerifyUser();

                var result = _reportService.GetPositionOptions();
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
        public async Task<IActionResult> GetReportXls([FromQuery] int dispositionId, [FromQuery] int epoId, [FromQuery] int supplierId, [FromQuery] GarmentPurchasingExpeditionPosition position, [FromQuery] string purchasingStaff, [FromQuery] DateTimeOffset? startDate, [FromQuery] DateTimeOffset? endDate)
        {
            try
            {
                VerifyUser();
                endDate = !endDate.HasValue ? DateTimeOffset.Now : endDate.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Date.AddHours(17);
                startDate = !startDate.HasValue ? DateTimeOffset.MinValue : startDate;

                var result = await _reportService.GetReport(dispositionId, epoId, supplierId, position, purchasingStaff, startDate.GetValueOrDefault(), endDate.GetValueOrDefault());

                var stream = DispositionPaymentReportExcelGenerator.GenerateExcel(result, startDate.GetValueOrDefault(), endDate.GetValueOrDefault(), _identityService.TimezoneOffset, position);

                var bytes = stream.ToArray();
                var filename = "Laporan Ekspedisi Disposisi Garment.xlsx";
                var file = File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace);
            }
        }
    }
}
