using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/vb-request-documents")]
    [Authorize]
    public class VBRequestDocumentController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IVBRequestDocumentService _service;
        private readonly IValidateService _validateService;
        private const string ApiVersion = "1.0";

        public VBRequestDocumentController(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            _service = serviceProvider.GetService<IVBRequestDocumentService>();
            _validateService = serviceProvider.GetService<IValidateService>();
        }

        private void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpPost("non-po")]
        public async Task<IActionResult> PostNonPO([FromBody] VBRequestDocumentNonPOFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = await _service.CreateNonPO(form);


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

        [HttpGet]
        public virtual IActionResult Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                VerifyUser();
                ReadResponse<VBRequestDocumentModel> read = _service.Get(page, size, order, select, keyword, filter);

                //List<TViewModel> dataVM = Mapper.Map<List<TViewModel>>(read.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(null, read.Data, page, size, read.Count, read.Data.Count, read.Order, read.Selected);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("by-user")]
        public virtual IActionResult GetByUser(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                VerifyUser();
                ReadResponse<VBRequestDocumentModel> read = _service.GetByUser(page, size, order, select, keyword, filter);

                //List<TViewModel> dataVM = Mapper.Map<List<TViewModel>>(read.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(null, read.Data, page, size, read.Count, read.Data.Count, read.Order, read.Selected);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("non-po/{id}")]
        public async Task<IActionResult> GetNonPOById([FromRoute] int id)
        {
            try
            {
                var model = await _service.GetNonPOById(id);

                if (model == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                        .Ok(null, model);
                    return Ok(Result);
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPut("non-po/{id}")]
        public virtual async Task<IActionResult> PutNonPO([FromRoute] int id, [FromBody] VBRequestDocumentNonPOFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                if (id != form.Id)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                        .Fail();
                    return BadRequest(Result);
                }

                await _service.UpdateNonPO(id, form);

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

        [HttpDelete("non-po/{id}")]
        public virtual async Task<IActionResult> DeleteNonPO([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                await _service.DeleteNonPO(id);

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

        [HttpGet("non-po/pdf/{id}")]
        public async Task<IActionResult> GetPDFNonPO([FromRoute] int id)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                int timeoffsset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var data = await _service.GetNonPOById(id);

                if (data == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    VBRequestDocumentNonPOPDFTemplate PdfTemplate = new VBRequestDocumentNonPOPDFTemplate();
                    MemoryStream stream = PdfTemplate.GeneratePdfTemplate(data, timeoffsset);
                    return new FileStreamResult(stream, "application/pdf")
                    {
                        FileDownloadName = "Permohonan VB Tanpa PO - " + data.DocumentNo + ".pdf"
                    };
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPost("with-po")]
        public IActionResult PostWithPO([FromBody] VBRequestDocumentWithPOFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                var id = _service.CreateWithPO(form);


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
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message + " " + e.StackTrace)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("with-po/{id}")]
        public IActionResult GetWithPOById([FromRoute] int id)
        {
            try
            {
                var model = _service.GetWithPOById(id);

                if (model == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                        .Ok(null, model);
                    return Ok(Result);
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPut("with-po/{id}")]
        public IActionResult PutWithPO([FromRoute] int id, [FromBody] VBRequestDocumentWithPOFormDto form)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(form);

                if (id != form.Id)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                        .Fail();
                    return BadRequest(Result);
                }

                _service.UpdateWithPO(id, form);

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

        [HttpDelete("with-po/{id}")]
        public IActionResult DeleteWithPO([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                _service.DeleteWithPO(id);

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

        [HttpGet("with-po/pdf/{id}")]
        public IActionResult GetPDFWithPO([FromRoute] int id)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                int timeoffsset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                var data = _service.GetWithPOById(id);

                if (data == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }
                else
                {
                    var PdfTemplate = new VBRequestDocumentWithPOPDFTemplate();
                    MemoryStream stream = PdfTemplate.GeneratePdfTemplate(data, timeoffsset);
                    return new FileStreamResult(stream, "application/pdf")
                    {
                        FileDownloadName = "Permohonan VB Dengan PO - " + data.DocumentNo + ".pdf"
                    };
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("not-approved")]
        public virtual IActionResult GetNotApprovedData(int type, int vbId = 0, int suppliantUnitId = 0, DateTime? date = null, string order = "{}")
        {
            try
            {
                VerifyUser();
                var read = _service.GetNotApprovedData(type, vbId, suppliantUnitId, date, order);

                //List<TViewModel> dataVM = Mapper.Map<List<TViewModel>>(read.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(null, read);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPost("approval")]
        public async Task<IActionResult> ApprovalData([FromBody] ApprovalVBFormDto data)
        {
            try
            {
                VerifyUser();

                var result = await _service.ApprovalData(data);


                var response = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok(null, result);

                return Ok(response);
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpPost("cancellation")]
        public async Task<IActionResult> CanccellationDocuments([FromBody] CancellationFormDto form)
        {
            try
            {
                VerifyUser();

                var result = await _service.CancellationDocuments(form);


                var response = new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE).Ok(null, result);

                return Ok(response);
            }
            catch (Exception e)
            {
                var result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("purchasing/{id}")]
        public IActionResult GetVBForPurchasing([FromRoute] int id)
        {
            try
            {
                var model = _service.GetVBForPurchasing(id);

                Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                        .Ok(null, model);
                return Ok(Result);
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
