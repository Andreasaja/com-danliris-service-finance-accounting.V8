using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow;
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
    [Route("v{version:apiVersion}/master/budget-cashflows-template")]
    [Authorize]
    public class BudgetCashflowMasterController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IBudgetCashflowService _service;
        private readonly IValidateService _validateService;
        private const string ApiVersion = "1.0";

        public BudgetCashflowMasterController(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            _service = serviceProvider.GetService<IBudgetCashflowService>();
            _validateService = serviceProvider.GetService<IValidateService>();
        }

        private void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpPost("cashflow-types")]
        public IActionResult PostCashflowType([FromBody] CashflowTypeFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = _service.CreateBudgetCashflowType(form);

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

        [HttpGet("cashflow-types")]
        public IActionResult GetCashflowType([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = _service.GetBudgetCashflowTypes(keyword, page, size);
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

        [HttpPut("cashflow-types/{id}")]
        public IActionResult PutCashflowType([FromRoute] int id, [FromBody] CashflowTypeFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                _service.UpdateBudgetCashflowType(id, form);

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

        [HttpDelete("cashflow-types/{id}")]
        public IActionResult DeleteCashflowType([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                _service.DeleteBudgetCashflowType(id);

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

        [HttpGet("cashflow-types/{id}")]
        public IActionResult GetCashflowTypeById(int id)
        {
            try
            {
                var result = _service.GetBudgetCashflowTypeById(id);
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

        [HttpPost("cashflow-categories")]
        public IActionResult PostCashflowCategory([FromBody] CashflowCategoryFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = _service.CreateBudgetCashflowCategory(form);

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

        [HttpGet("cashflow-categories")]
        public IActionResult GetCashflowCategories([FromQuery] int cashflowTypeId, [FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = _service.GetBudgetCashflowCategories(keyword, cashflowTypeId, page, size);
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

        [HttpPut("cashflow-categories/{id}")]
        public IActionResult PutCashflowCategories([FromRoute] int id, [FromBody] CashflowCategoryFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                _service.UpdateBudgetCashflowCategory(id, form);

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

        [HttpDelete("cashflow-categories/{id}")]
        public IActionResult DeleteCashflowCategory([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                _service.DeleteBudgetCashflowCategories(id);

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

        [HttpGet("cashflow-categories/{id}")]
        public IActionResult GetCashflowCategoryById(int id)
        {
            try
            {
                var result = _service.GetBudgetCashflowCategoryById(id);
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

        [HttpPost("cashflow-sub-categories")]
        public IActionResult PostCashflowSubCategory([FromBody] CashflowSubCategoryFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = _service.CreateBudgetCashflowSubCategory(form);

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

        [HttpGet("cashflow-sub-categories")]
        public IActionResult GetCashflowSubCategories([FromQuery] int cashflowCategoryId, [FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = _service.GetBudgetCashflowSubCategories(keyword, cashflowCategoryId, page, size);
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

        [HttpPut("cashflow-sub-categories/{id}")]
        public IActionResult PutCashflowSubCategories([FromRoute] int id, [FromBody] CashflowSubCategoryFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                _service.UpdateBudgetCashflowSubCategory(id, form);

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

        [HttpDelete("cashflow-sub-categories/{id}")]
        public IActionResult DeleteCashflowSubCategory([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                _service.DeleteBudgetCashflowSubCategories(id);

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

        [HttpGet("cashflow-sub-categories/{id}")]
        public IActionResult GetCashflowSubCategoryById(int id)
        {
            try
            {
                var result = _service.GetBudgetCashflowSubCategoryById(id);
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

        [HttpGet]
        public IActionResult Get([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = _service.GetBudgetCashflowMasterLayout(keyword, page, size);
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
    }
}
