using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance.Excel;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.GarmentDebtBalance
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/garment-debt-balance")]
    public class GarmentDebtBalanceController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IGarmentDebtBalanceService Service;
        private readonly string ApiVersion;
        private readonly IMapper Mapper;

        public GarmentDebtBalanceController(IIdentityService identityService, IValidateService validateService, IGarmentDebtBalanceService service, IMapper mapper)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            ApiVersion = "1.0.0";
            Mapper = mapper;
        }

        protected void VerifyUser()
        {
            IdentityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            IdentityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            IdentityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }


        [HttpGet]
        public IActionResult Get([FromQuery] GarmentDebtBalanceFilterViewModel filter)
        {
            try
            {
                VerifyUser();
                int offSet = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                //int offSet = 7;
                var data = Service.GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex(filter.supplierId, filter.month, filter.year);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Data,
                    info = new
                    {
                        data.Count,
                        data.Order,
                        data.Selected
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
        public async Task<IActionResult> GetXls([FromQuery] GarmentDebtBalanceFilterViewModel filter)
        {
            try
            {
                VerifyUser();
                var data = Service.GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex(filter.supplierId, filter.month, filter.year);

                MemoryStream result = new MemoryStream();
                var filename = "Kartu Hutang.xlsx";
                result = GarmentBalanceCardExcelGenerator.GenerateExcel(data, filter.month, filter.year, filter.supplierName, filter.import, IdentityService.TimezoneOffset);
                //filename += ".xlsx";

                var bytes = result.ToArray();

                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("downloads/pdf")]
        public IActionResult GetPDF([FromQuery] GarmentDebtBalanceFilterViewModel filter)
        {
            try
            {
                VerifyUser();
                var data = Service.GetDebtBalanceCardWithBalanceBeforeIndex(filter.supplierId, filter.month, filter.year);

                MemoryStream result = new MemoryStream();
                var filename = "Kartu Hutang.pdf";
                result = Lib.BusinessLogic.GarmentDebtBalance.Pdf.GarmentBalanceCardPDFGenerator.Generate(data, filter.month, filter.year, false, filter.import, IdentityService.TimezoneOffset,filter.supplierName);
                //filename += ".xlsx";

                var bytes = result.ToArray();

                return File(bytes, "application/pdf", filename);
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
        public IActionResult GetLoader(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                GarmentDebtBalanceIndexDto data = Service.GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex("GarmentDeliveryOrderNo", page, size, order, select, keyword, filter);

                //List<GarmentInvoicePurchasingDispositionViewModel> dataVM = Mapper.Map<List<GarmentInvoicePurchasingDispositionViewModel>>(read.Data);

                //Dictionary<string, object> Result =
                //    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                //    .Ok(Mapper, dataVM, page, size, read.Count, dataVM.Count, read.Order, read.Selected);
                //return Ok(Result);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Data,
                    info = new
                    {
                        data.Count,
                        data.Order,
                        data.Selected
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

        [HttpGet("loader-bills-no")]
        public IActionResult GetLoaderByBillsNo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                GarmentDebtBalanceIndexDto data = Service.GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex("BillsNo", page, size, order, select, keyword, filter);

                //List<GarmentInvoicePurchasingDispositionViewModel> dataVM = Mapper.Map<List<GarmentInvoicePurchasingDispositionViewModel>>(read.Data);

                //Dictionary<string, object> Result =
                //    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                //    .Ok(Mapper, dataVM, page, size, read.Count, dataVM.Count, read.Order, read.Selected);
                //return Ok(Result);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Data,
                    info = new
                    {
                        data.Count,
                        data.Order,
                        data.Selected
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

        [HttpGet("loader-payment-bills")]
        public IActionResult GetLoaderByPaymentBills(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                GarmentDebtBalanceIndexDto data = Service.GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex("PaymentBills", page, size, order, select, keyword, filter);

                //List<GarmentInvoicePurchasingDispositionViewModel> dataVM = Mapper.Map<List<GarmentInvoicePurchasingDispositionViewModel>>(read.Data);

                //Dictionary<string, object> Result =
                //    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                //    .Ok(Mapper, dataVM, page, size, read.Count, dataVM.Count, read.Order, read.Selected);
                //return Ok(Result);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Data,
                    info = new
                    {
                        data.Count,
                        data.Order,
                        data.Selected
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
    }
}
