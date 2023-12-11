using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.PaymentDispositionNote
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/payment-disposition-note")]
    [Authorize]
    public class PaymentDispositionNoteController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IPaymentDispositionNoteService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public PaymentDispositionNoteController(IIdentityService identityService, IValidateService validateService, IMapper mapper, IPaymentDispositionNoteService service)
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

        [HttpGet]
        public IActionResult Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                ReadResponse<PaymentDispositionNoteModel> read = Service.Read(page, size, order, select, keyword, filter);

                List<PaymentDispositionNoteViewModel> dataVM = Mapper.Map<List<PaymentDispositionNoteViewModel>>(read.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Mapper, dataVM, page, size, read.Count, dataVM.Count, read.Order, read.Selected);
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

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PaymentDispositionNoteViewModel viewModel)
        {
            try
            {
                VerifyUser();
                
                ValidateService.Validate(viewModel);
                viewModel.BankAccountCOA = viewModel.AccountBank.AccountCOA;
                PaymentDispositionNoteModel model = Mapper.Map<PaymentDispositionNoteModel>(viewModel);
                model.FixFailAutoMapper(viewModel.AccountBank.BankCode);
                await Service.CreateAsync(model);

                Dictionary<string, object> Result =
                       new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                       .Ok();
                return Created(String.Concat(Request.Path, "/", 0), Result);
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

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var indexAcceptPdf = Request.Headers["Accept"].ToList().IndexOf("application/pdf");
                var model = await Service.ReadByIdAsync(id);
                PaymentDispositionNoteViewModel viewModel = Mapper.Map<PaymentDispositionNoteViewModel>(model);

                if (model == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
                }

                if (indexAcceptPdf < 0)
                {
                    return Ok(new
                    {
                        apiVersion = ApiVersion,
                        data = viewModel,
                        message = General.OK_MESSAGE,
                        statusCode = General.OK_STATUS_CODE
                    });
                }
                else
                {
                    int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());

                    //PaymentDispositionNotePDFTemplate PdfTemplate = new PaymentDispositionNotePDFTemplate();
                    //MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel, clientTimeZoneOffset);
                    MemoryStream stream = Service.GeneratePdfTemplate(viewModel, clientTimeZoneOffset);

                    return new FileStreamResult(stream, "application/pdf")
                    {
                        FileDownloadName = $"Bukti Pembayaran Disposisi {viewModel.PaymentDispositionNo}.pdf"
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

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                await Service.DeleteAsync(id);

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]PaymentDispositionNoteViewModel viewModel)
        {
            try
            {
                VerifyUser();

                ValidateService.Validate(viewModel);
                PaymentDispositionNoteModel model = Mapper.Map<PaymentDispositionNoteModel>(viewModel);
                await Service.UpdateAsync(id,model);

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

        [HttpGet("byEpoId/{epoId}")]
        public IActionResult GetDetailsByEpoId([FromRoute] string epoId)
        {
            try
            {
                ReadResponse<PaymentDispositionNoteItemModel> read = Service.ReadDetailsByEPOId(epoId);

                List<PaymentDispositionNoteItemViewModel> dataVM = Mapper.Map<List<PaymentDispositionNoteItemViewModel>>(read.Data);

                //Dictionary<string, object> Result =
                //    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                //    .Ok(Mapper, dataVM, 1, size, read.Count, dataVM.Count, read.Order, read.Selected);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = dataVM,
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

        [HttpGet("report")]
        public IActionResult GetReport([FromQuery] int bankExpenditureId, [FromQuery] int dispositionId, [FromQuery] int supplierId, [FromQuery] int divisionId, [FromQuery] DateTimeOffset? startDate, [FromQuery] DateTimeOffset? endDate)
        {
            try
            {
                startDate = startDate == null ? new DateTime(1970, 1, 1).ToUniversalTime() : startDate;
                endDate = endDate == null ? DateTime.Now.ToUniversalTime() : endDate;
                var result = Service.GetReport(bankExpenditureId, dispositionId, supplierId, divisionId, startDate.GetValueOrDefault(), endDate.GetValueOrDefault());

                //Dictionary<string, object> Result =
                //    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                //    .Ok(Mapper, dataVM, 1, size, read.Count, dataVM.Count, read.Order, read.Selected);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = result,
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

        [HttpGet("report/xls")]
        public IActionResult GetReportXls([FromQuery] int bankExpenditureId, [FromQuery] int dispositionId, [FromQuery] int supplierId, [FromQuery] int divisionId, [FromQuery] DateTimeOffset? startDate, [FromQuery] DateTimeOffset? endDate)
        {
            try
            {
                VerifyUser();
                startDate = startDate == null ? new DateTime(1970, 1, 1).ToUniversalTime() : startDate;
                endDate = endDate == null ? DateTime.Now.ToUniversalTime() : endDate;
                var result = Service.GetReport(bankExpenditureId, dispositionId, supplierId, divisionId, startDate.GetValueOrDefault(), endDate.GetValueOrDefault());
                var xls = Service.GetXls(result);


                var filename = string.Format("Laporan Disposisi Pembayaran - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

                var xlsInBytes = xls.ToArray();
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

        [HttpPut("post")]
        public async Task<IActionResult> PaymentDispositionNotePost([FromBody] PaymentDispositionNotePostDto form)
        {
            try
            {
                VerifyUser();
                var result = await Service.Post(form);

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

        [HttpGet("get-expedition/by-position")]
        public IActionResult GetAllCashierPosition(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                ReadResponse<PurchasingDispositionExpeditionModel> read = Service.GetAllByPosition(page, size, order, select, keyword, filter);

                List<PurchasingDispositionExpeditionViewModel> dataVM = Mapper.Map<List<PurchasingDispositionExpeditionViewModel>>(read.Data);

                foreach (var item in dataVM)
                {
                    var service = Service.GetAmountPaidAndIsPosted(item.dispositionNo);

                    item.AmountPaid = service.AmountPaid;
                    item.IsPosted = service.IsPosted;
                }

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Mapper, dataVM, page, size, read.Count, dataVM.Count, read.Order, read.Selected);
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
