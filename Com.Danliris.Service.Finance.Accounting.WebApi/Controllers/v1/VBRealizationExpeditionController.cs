using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/vb-realization-expeditions")]
    [Authorize]
    public class VBRealizationExpeditionController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IVBRealizationDocumentExpeditionService _service;
        private readonly IValidateService _validateService;
        private const string ApiVersion = "1.0";

        public VBRealizationExpeditionController(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            _service = serviceProvider.GetService<IVBRealizationDocumentExpeditionService>();
            _validateService = serviceProvider.GetService<IValidateService>();
        }

        private void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet("vb-realization-to-verification")]
        public IActionResult GetVbRealizationToVerification([FromQuery] int vbId, [FromQuery] int vbRealizationId, [FromQuery] DateTimeOffset? realizationDate, [FromQuery] string vbRealizationRequestPerson, [FromQuery] int unitId)
        {
            try
            {
                VerifyUser();
                var data = _service.ReadRealizationToVerification(vbId, vbRealizationId, realizationDate, vbRealizationRequestPerson, unitId);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Data,
                    info = new
                    {
                        data.Count
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

        [HttpGet]
        public IActionResult Get([FromQuery] int vbId, [FromQuery] int vbRealizationId, [FromQuery] DateTimeOffset? realizationDate, [FromQuery] string vbRealizationRequestPerson, [FromQuery] int unitId, [FromQuery] VBRealizationPosition position = 0, [FromQuery] int page = 1, [FromQuery] int size = 25, [FromQuery] string order = "{}", [FromQuery] string keyword = "")
        {
            try
            {
                VerifyUser();
                var data = _service.Read(page, size, order, keyword, position, vbId, vbRealizationId, realizationDate, vbRealizationRequestPerson, unitId);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Data,
                    info = new
                    {
                        data.Count
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

        [HttpGet("verification")]
        public IActionResult GetVerification([FromQuery] int vbId, [FromQuery] int vbRealizationId, [FromQuery] DateTimeOffset? realizationDate, [FromQuery] string vbRealizationRequestPerson, [FromQuery] int unitId, [FromQuery] VBRealizationPosition position = 0, [FromQuery] int page = 1, [FromQuery] int size = 25, [FromQuery] string order = "{}", [FromQuery] string keyword = "")
        {
            try
            {
                VerifyUser();
                var data = _service.ReadVerification(page, size, order, keyword, position, vbId, vbRealizationId, realizationDate, vbRealizationRequestPerson, unitId);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Data,
                    info = new
                    {
                        data.Count
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

        [HttpPut("vb-to-verification")]
        public async Task<IActionResult> Post([FromBody] VBRealizationIdListDto viewModel)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(viewModel);

                await _service.SubmitToVerification(viewModel.VBRealizationIds);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
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

        [HttpPut("accept-for-verification")]
        public async Task<IActionResult> AcceptForVerification([FromBody] VBRealizationIdListDto viewModel)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(viewModel);

                await _service.VerificationDocumentReceipt(viewModel.VBRealizationIds);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
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

        [HttpPut("accept-for-cashier")]
        public async Task<IActionResult> AcceptForCashier([FromBody] VBRealizationIdListDto viewModel)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(viewModel);

                await _service.CashierReceipt(viewModel.VBRealizationIds);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
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

        [HttpPut("vb-verified/{id}")]
        public async Task<IActionResult> Verify([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                await _service.VerifiedToCashier(id);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return NoContent();
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPut("vb-cashier-delete/{id}")]
        public async Task<IActionResult> CashierDelete([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                await _service.CashierDelete(id);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return NoContent();
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPut("vb-reject/{id}")]
        public async Task<IActionResult> reject([FromRoute] int id, [FromBody] VBRealizationExpeditionRejectDto viewModel)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(viewModel);

                await _service.Reject(id, viewModel.Reason);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
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

        [HttpGet("reports")]
        public async Task<IActionResult> GetReport([FromQuery] int vbId, [FromQuery] int vbRealizationId, [FromQuery] string vbRequestName, [FromQuery] int unitId, [FromQuery] int divisionId, [FromQuery] DateTime? dateStart, [FromQuery] DateTime? dateEnd, [FromQuery] string status, [FromQuery] int page = 1, [FromQuery] int size = 25)
        {
            try
            {
                VerifyUser();

                if (dateEnd == null)
                    dateEnd = DateTime.MaxValue;
                else
                    dateEnd = dateEnd.GetValueOrDefault().AddHours(-1 * _identityService.TimezoneOffset);

                if (dateStart == null)
                    dateStart = DateTime.MinValue;
                else
                    dateStart = dateStart.GetValueOrDefault().AddHours(-1 * _identityService.TimezoneOffset);

                var reportResult = await _service.GetReports(vbId, vbRealizationId, vbRequestName, unitId, divisionId, dateStart.GetValueOrDefault().ToUniversalTime(), dateEnd.GetValueOrDefault().ToUniversalTime(), status, page, size);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = reportResult.Data,
                    info = new
                    {
                        total = reportResult.Total,
                        page,
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

        [HttpGet("reports/xls")]
        public async Task<IActionResult> GetReportXls([FromQuery] int vbId, [FromQuery] int vbRealizationId, [FromQuery] string vbRequestName, [FromQuery] int unitId, [FromQuery] int divisionId, [FromQuery] DateTime? dateStart, [FromQuery] DateTime? dateEnd, string status)
        {
            try
            {
                VerifyUser();
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);

                if (dateEnd == null)
                    dateEnd = DateTime.MaxValue;
                else
                    dateEnd = dateEnd.GetValueOrDefault().AddHours(-1 * _identityService.TimezoneOffset);

                if (dateStart == null)
                    dateStart = DateTime.MinValue;
                else
                    dateStart = dateStart.GetValueOrDefault().AddHours(-1 * _identityService.TimezoneOffset);

                //dateEndXls = (DateTime)dateEnd == DateTime.MaxValue ? "-" : dateEndXls;
                //dateStartXls = (DateTime)dateStart == DateTime.MinValue ? "-" : dateStartXls;

                var reportResult = await _service.GetReports(vbId, vbRealizationId, vbRequestName, unitId, divisionId, dateStart.GetValueOrDefault().ToUniversalTime(), dateEnd.GetValueOrDefault().ToUniversalTime(), status, 1, int.MaxValue);
                var stream = GenerateExcel(reportResult.Data, dateStart, dateEnd);

                var xls = stream.ToArray();

                var file = File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Ekspedisi");

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

        [HttpPut("post")]
        public async Task<IActionResult> ClearanceVBPost([FromBody] ClearanceFormDto form)
        {
            try
            {
                VerifyUser();
                int result = await _service.ClearanceVBPost(form);

                return Ok(result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        private MemoryStream GenerateExcel(IList<ReportDto> data, DateTime? dateStart, DateTime? dateEnd)
        {
            var timezoneoffset = _identityService.TimezoneOffset;
            DataTable dt = new DataTable();
            string title = "Laporan Ekspedisi Realisasi VB",
                dateFrom = dateStart != DateTime.MinValue ? dateStart.GetValueOrDefault().AddHours(timezoneoffset).ToString("dd MMMM yyyy", CultureInfo.InvariantCulture) : "-",
                dateTo = dateEnd != DateTime.MaxValue ? dateEnd.GetValueOrDefault().AddHours(timezoneoffset).ToString("dd MMMM yyyy", CultureInfo.InvariantCulture) : "-";

            dt.Columns.Add(new DataColumn() { ColumnName = "No. VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Realisasi VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tipe VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nama", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Bagian/Unit", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Divisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Unit Kirim", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Keperluan", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Keterangan Realisasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mata Uang VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nominal VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mata Uang Realisasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nominal Realisasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Realisasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Verif Terima", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nama Verifikator", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Verif Kirim Kasir/Retur", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Posisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Keterangan Retur", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Kasir Terima", DataType = typeof(string) });

            int index = 0;
            if (data.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "","", 0.ToString("#,##0.#0"), "", 0.ToString("#,##0.#0"), "", "", "", "", "", "", "");
                index++;
            }
            else
            {
                foreach (var datum in data)
                {
                    var verificationReceiptDate = datum.VerificationReceiptDate.HasValue ? datum.VerificationReceiptDate.GetValueOrDefault().AddHours(timezoneoffset).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";
                    var sendToVerificationDate = datum.SendToVerificationDate.HasValue ? datum.SendToVerificationDate.GetValueOrDefault().AddHours(timezoneoffset).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";
                    var verifiedDate = datum.VerifiedToCashierDate.HasValue ? datum.VerifiedToCashierDate.GetValueOrDefault().AddHours(timezoneoffset).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : datum.NotVerifiedDate.HasValue ? datum.NotVerifiedDate.GetValueOrDefault().AddHours(timezoneoffset).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";
                    var cashierReceiptDate = datum.CashierReceiptDate.HasValue ? datum.CashierReceiptDate.GetValueOrDefault().AddHours(timezoneoffset).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";
                    //var vbRealizationDate = datum.VBRealizationDate.AddHours(timezoneoffset).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var verifiedBy = !string.IsNullOrWhiteSpace(datum.VerifiedToCashierBy) ? datum.VerifiedToCashierBy : !string.IsNullOrWhiteSpace(datum.NotVerifiedBy) ? datum.NotVerifiedBy : "";

                    dt.Rows.Add(
                        datum.VBNo,
                        datum.VBRealizationNo,
                        datum.VBType.GetDisplayName(),
                        datum.VBRequestName,
                        datum.UnitName,
                        datum.DivisionName,
                        sendToVerificationDate,
                        datum.Purpose,
                        datum.RemarkRealization,
                        datum.CurrencyCode,
                        datum.VBAmount.ToString("#,##0.#0"),
                        datum.CurrencyCode,
                        datum.VBRealizationAmount.ToString("#,##0.#0"),
                        datum.VBRealizationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        verificationReceiptDate,
                        verifiedBy, 
                        verifiedDate, 
                        datum.Position.GetDisplayName(), 
                        datum.NotVerifiedReason, 
                        cashierReceiptDate);
                }
                index++;
            }

            return Lib.Helpers.Excel.CreateExcelWithTitle(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Reports") },
                new List<KeyValuePair<string, int>>() { new KeyValuePair<string, int>("Reports", index) }, title, dateFrom, dateTo, true);
        }
    

    }
}
