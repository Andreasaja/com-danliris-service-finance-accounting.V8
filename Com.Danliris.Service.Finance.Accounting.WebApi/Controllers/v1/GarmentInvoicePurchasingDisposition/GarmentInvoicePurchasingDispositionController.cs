using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition.Report;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.GarmentInvoicePurchasingDisposition
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/garment-invoice-purchasing-disposition")]
    [Authorize]
    public class GarmentInvoicePurchasingDispositionController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IGarmentInvoicePurchasingDispositionService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public GarmentInvoicePurchasingDispositionController(IIdentityService identityService, IValidateService validateService, IMapper mapper, IGarmentInvoicePurchasingDispositionService service)
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
        public IActionResult Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                ReadResponse<GarmentInvoicePurchasingDispositionModel> read = Service.Read(page, size, order, select, keyword, filter);

                List<GarmentInvoicePurchasingDispositionViewModel> dataVM = Mapper.Map<List<GarmentInvoicePurchasingDispositionViewModel>>(read.Data);

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
        public async Task<ActionResult> Post([FromBody] GarmentInvoicePurchasingDispositionViewModel viewModel)
        {
            try
            {
                VerifyUser();

                ValidateService.Validate(viewModel);
                GarmentInvoicePurchasingDispositionModel model = Mapper.Map<GarmentInvoicePurchasingDispositionModel>(viewModel);
                //model.FixFailAutoMapper(viewModel.AccountBank.BankCode);
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
                GarmentInvoicePurchasingDispositionViewModel viewModel = Mapper.Map<GarmentInvoicePurchasingDispositionViewModel>(model);
                viewModel.Items.ForEach(s => s.DiffTotalPaidPayment = s.TotalPaid - (s.TotalPaidPayment + s.TotalPaidPaymentBefore));
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

                    GarmentInvoicePurchasingDispositionPdfTemplate PdfTemplate = new GarmentInvoicePurchasingDispositionPdfTemplate();
                    MemoryStream stream = PdfTemplate.GeneratePdfTemplate(viewModel, clientTimeZoneOffset);

                    return new FileStreamResult(stream, "application/pdf")
                    {
                        FileDownloadName = $"Bukti Pembayaran Disposisi {viewModel.PaymentDispositionNo}.pdf"
                    };
                    //return Ok("link pdf");
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
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] GarmentInvoicePurchasingDispositionViewModel viewModel)
        {
            try
            {
                VerifyUser();

                ValidateService.Validate(viewModel);
                GarmentInvoicePurchasingDispositionModel model = Mapper.Map<GarmentInvoicePurchasingDispositionModel>(viewModel);
                await Service.UpdateAsync(id, model);

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
                ReadResponse<GarmentInvoicePurchasingDispositionItemModel> read = Service.ReadDetailsByEPOId(epoId);

                List<GarmentInvoicePurchasingDispositionItemViewModel> dataVM = Mapper.Map<List<GarmentInvoicePurchasingDispositionItemViewModel>>(read.Data);

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

        [HttpPut("post")]
        public async Task<IActionResult> PaymentDispositionNotePost([FromBody] GarmentInvoicePurchasingDispositionPostingViewModel form)
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
		[HttpGet("loader")]
		public IActionResult GetLoader(string keyword = null, string filter = "{}")
		{
			try
			{
				int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
				ReadResponse<GarmentInvoicePurchasingDispositionNoVM> Result = Service.GetLoader(keyword, filter);

				return Ok(new
				{
					apiVersion = "1.0.0",
					data = Result.Data,
					message = Utilities.General.OK_MESSAGE,
					statusCode = Utilities.General.OK_STATUS_CODE
				});
			}
			catch (Exception e)
			{
				var result = new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message + "\n" + e.StackTrace);
				return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, result);
			}
		}
		[HttpGet("monitoring")]
		public IActionResult GetMonitoring([FromQuery] string invoiceNo, [FromQuery] string dispositionNo, [FromQuery] DateTimeOffset startDate, [FromQuery] DateTimeOffset endDate)
		{
			try
			{
				VerifyUser();
				int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
				//int offSet = 7;
				var data = Service.GetMonitoring(invoiceNo, dispositionNo,startDate,endDate, offSet);

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
		public async Task<IActionResult> GetReportAllXls([FromQuery] string invoiceNo, [FromQuery] string dispositionNo, [FromQuery] DateTimeOffset startDate, [FromQuery] DateTimeOffset endDate)
		{
			try
			{
				try
				{
					VerifyUser();
					byte[] xlsInBytes;
					//int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
					var xls = await Service.DownloadReportXls(invoiceNo, dispositionNo, startDate, endDate);
 
					string filename = String.Format("Laporan Bukti Pembayaran Disposisi Garment {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

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
			catch (Exception e)
			{
				var result = new ResultFormatter(ApiVersion, Utilities.General.INTERNAL_ERROR_STATUS_CODE, e.Message + "\n" + e.StackTrace);
				return StatusCode(Utilities.General.INTERNAL_ERROR_STATUS_CODE, result);
			}
		}

	}
}
