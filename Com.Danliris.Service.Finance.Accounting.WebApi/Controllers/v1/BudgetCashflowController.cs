using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow.ExcelGenerator;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow.PdfGenerator;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.CacheService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/budget-cashflows")]
    [Authorize]
    public class BudgetCashflowController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IBudgetCashflowService _service;
        private readonly IValidateService _validateService;
        private readonly List<UnitDto> _units;
        private readonly List<DivisionDto> _divisions;
        private readonly List<UnitAccountingDto> _unitAccountings;
        private const string ApiVersion = "1.0";

        public BudgetCashflowController(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            _service = serviceProvider.GetService<IBudgetCashflowService>();
            _validateService = serviceProvider.GetService<IValidateService>();

            var cacheService = serviceProvider.GetService<ICacheService>();
            var jsonUnits = cacheService.GetString("Unit");
            //var jsonUnits = "[{\"Id\":0,\"Code\":\"IDR\",\"Name\":\"SPINNING 1\",\"DivisionId\":1}]";
            _units = JsonConvert.DeserializeObject<List<UnitDto>>(jsonUnits, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            var jsonDivisions = cacheService.GetString("Division");
            //var jsonDivisions = "[{\"Id\":0,\"Code\":\"SP\",\"Name\":\"SPINNING\"}]";
            _divisions = JsonConvert.DeserializeObject<List<DivisionDto>>(jsonDivisions, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            var jsonAccountingUnit = cacheService.GetString("AccountingUnit");
            _unitAccountings = JsonConvert.DeserializeObject<List<UnitAccountingDto>>(jsonAccountingUnit, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
        }

        private void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpPost]
        public IActionResult PostCashflowUnit([FromBody] CashflowUnitFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = _service.CreateBudgetCashflowUnit(form);

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

        [HttpPut]
        public IActionResult UpdateCashflowUnit([FromBody] CashflowUnitFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = _service.UpdateBudgetCashflowUnit(form);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return NoContent();
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

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int unitId, [FromQuery] DateTimeOffset date)
        {

            try
            {
                VerifyUser();
                var result = await _service.GetBudgetCashflowUnit(unitId, date);
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
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message + "\n" + e.StackTrace + "\n" + e.InnerException.Message + "\n" + e.InnerException.StackTrace)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("download/pdf")]
        public async Task<IActionResult> GeneratePdf([FromQuery] int unitId, [FromQuery] DateTimeOffset date)
        {

            try
            {
                VerifyUser();
                var data = await _service.GetBudgetCashflowUnit(unitId, date);
                var unit = _unitAccountings.FirstOrDefault(element => element.Id == unitId);
                var stream = CashflowUnitPdfGenerator.Generate(unit, date, _identityService.TimezoneOffset, data);
                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "Laporan Budget Cashflow.pdf"
                };
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("download/xls")]
        public async Task<IActionResult> GenerateXls([FromQuery] int unitId, [FromQuery] DateTimeOffset date)
        {

            try
            {
                VerifyUser();
                var data = await _service.GetBudgetCashflowUnit(unitId, date);
                //var unit = _units.FirstOrDefault(element => element.Id == unitId);
                var unit = await _service.GetUnitAccountingById(unitId);

                var stream = CashflowUnitExcelGenerator.Generate(unit, date, _identityService.TimezoneOffset, data);

                var bytes = stream.ToArray();

                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Budget Cashflow.xlsx");
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("divisions")]
        public async Task<IActionResult> GetDivision([FromQuery] int divisionId, [FromQuery] DateTimeOffset date)
        {

            try
            {
                VerifyUser();
                var result = await _service.GetBudgetCashflowDivision(divisionId, date);
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
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("divisions/download/pdf")]
        public async Task<IActionResult> GeneratePdfDivision([FromQuery] int divisionId, [FromQuery] DateTimeOffset date)
        {

            try
            {
                VerifyUser();
                var data = await _service.GetBudgetCashflowDivision(divisionId, date);
                var division = _divisions.FirstOrDefault(element => element.Id == divisionId);
                var stream = CashflowDivisionPdfGenerator.Generate(division, date, _identityService.TimezoneOffset, data);
                return new FileStreamResult(stream, "application/pdf")
                {
                    FileDownloadName = "Laporan Budget Cashflow.pdf"
                };
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("divisions/download/xls")]
        public async Task<IActionResult> GenerateXlsDivision([FromQuery] int divisionId, [FromQuery] DateTimeOffset date)
        {

            try
            {
                VerifyUser();
                var data = await _service.GetBudgetCashflowDivision(divisionId, date);
                var division = _divisions.FirstOrDefault(element => element.Id == divisionId);
                var stream = CashflowDivisionExcelGenerator.Generate(division, date, _identityService.TimezoneOffset, data);

                var bytes = stream.ToArray();

                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Laporan Budget Cashflow.xlsx");
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("items")]
        public IActionResult GetItems([FromQuery] int unitId, [FromQuery] int subCategoryId, [FromQuery] DateTimeOffset date)
        {

            try
            {
                VerifyUser();
                var result = _service.GetBudgetCashflowUnit(unitId, subCategoryId, date);
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
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPost("initial-cash-balance")]
        public IActionResult PostInitialCashBalance([FromBody] CashBalanceFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = _service.CreateInitialCashBalance(form);

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

        [HttpPut("initial-cash-balance")]
        public IActionResult UpdateInitialCashBalance([FromBody] CashBalanceFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = _service.UpdateInitialCashBalance(form);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return NoContent();
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

        [HttpGet("initial-cash-balance/items")]
        public IActionResult GetInitialCashBalanceItems([FromQuery] int unitId, [FromQuery] DateTimeOffset date)
        {

            try
            {
                VerifyUser();
                var result = _service.GetInitialCashBalance(unitId, date);
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
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPost("real-cash-balance")]
        public IActionResult PostRealCashBalance([FromBody] CashBalanceFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = _service.CreateRealCashBalance(form);

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

        [HttpPut("real-cash-balance")]
        public IActionResult UpdateRealCashBalance([FromBody] CashBalanceFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = _service.UpdateRealCashBalance(form);

                var result = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok();

                return NoContent();
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

        [HttpGet("real-cash-balance/items")]
        public IActionResult GetRealCashBalanceItems([FromQuery] int unitId, [FromQuery] DateTimeOffset date)
        {

            try
            {
                VerifyUser();
                var result = _service.GetRealCashBalance(unitId, date);
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
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }
    }
}
