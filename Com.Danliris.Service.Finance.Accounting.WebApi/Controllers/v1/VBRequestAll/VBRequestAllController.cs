using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBRequestAll;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.VBRequestAll
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/vb-request-all")]
    [Authorize]

    public class VBRequestAllController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IValidateService _validateService;
        private readonly IVBRequestAllService _service;
        private readonly IMapper _mapper;
        private const string ApiVersion = "1.0";

        public VBRequestAllController(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            _validateService = serviceProvider.GetService<IValidateService>();
            _service = serviceProvider.GetService<IVBRequestAllService>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                var queryResult = _service.Read(page, size, order, select, keyword, filter);

                var result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(_mapper, queryResult.Data, page, size, queryResult.Count, queryResult.Data.Count, queryResult.Order, select);
                return Ok(result);
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message).Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }
    }
}
