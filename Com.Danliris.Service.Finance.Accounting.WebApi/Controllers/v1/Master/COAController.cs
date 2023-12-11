using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.Master;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Master;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Com.Danliris.Service.Production.WebApi.Utilities;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.Master.COAService;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.Master
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/master/chart-of-accounts")]
    [Authorize]
    public class COAController : BaseController<COAModel, COAViewModel, ICOAService>
    {
        private readonly string ContentType = "application/vnd.openxmlformats";
        private readonly string FileName = string.Concat("Error Log - ", typeof(COAModel).Name, " ", DateTime.Now.ToString("dd MMM yyyy"), ".csv");
        public COAController(IIdentityService identityService, IValidateService validateService, ICOAService service, IMapper mapper) : base(identityService, validateService, service, mapper, "1.0.0")
        {

        }

        private Action<COAModel> Transfrom => (coaModel) =>
        {
            var codeArray = coaModel.Code.Split('.');
            coaModel.Code1 = codeArray[0];
            coaModel.Code2 = codeArray[1];
            coaModel.Code3 = codeArray[2];
            coaModel.Code4 = codeArray[3];
            coaModel.Header = coaModel.Code.Substring(0, 1);
            coaModel.Subheader = coaModel.Code.Substring(0, 2);

        };

        public override async Task<ActionResult> Post([FromBody] COAViewModel viewModel)
        {
            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel);

                COAModel model = Mapper.Map<COAModel>(viewModel);
                Transfrom(model);
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

        public override async Task<IActionResult> Put([FromRoute] int id, [FromBody] COAViewModel viewModel)
        {
            try
            {
                VerifyUser();
                ValidateService.Validate(viewModel);

                if (id != viewModel.Id)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                        .Fail();
                    return BadRequest(Result);
                }

                COAModel model = Mapper.Map<COAModel>(viewModel);
                Transfrom(model);

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

        [HttpGet("all")]
        public IActionResult GetAll(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                var read = Service.GetAll();

                if (read.Count > 0 && !string.IsNullOrWhiteSpace(keyword))
                {
                    read = read.Where(element => element.Code.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) != -1 || (!string.IsNullOrWhiteSpace(element.Name) && element.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) != -1)).ToList();
                }

                List<COAViewModel> dataVM = Mapper.Map<List<COAViewModel>>(read);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Mapper, dataVM, 1, dataVM.Count, dataVM.Count, dataVM.Count, new Dictionary<string, string>(), new List<string>());
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

        [HttpPost("upload")]
        public async Task<IActionResult> PostCSVFileAsync()
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    VerifyUser();
                    var UploadedFile = Request.Form.Files[0];
                    StreamReader Reader = new StreamReader(UploadedFile.OpenReadStream());
                    List<string> FileHeader = new List<string>(Reader.ReadLine().Split(","));
                    var ValidHeader = Service.CsvHeader.SequenceEqual(FileHeader, StringComparer.OrdinalIgnoreCase);

                    if (ValidHeader)
                    {
                        Reader.DiscardBufferedData();
                        Reader.BaseStream.Seek(0, SeekOrigin.Begin);
                        Reader.BaseStream.Position = 0;

                        var csvReaderConfiguration = new CsvHelper.Configuration.CsvConfiguration(cultureInfo: CultureInfo.InvariantCulture)
                        {
                            MissingFieldFound = null,
                            Delimiter = ",",
                            HeaderValidated = null,


                        };
                        CsvReader Csv = new CsvReader(Reader, csvReaderConfiguration);
                        //Csv.Configuration.IgnoreQuotes = false;
                        //Csv.Configuration.Delimiter = ",";
                        //Csv.Configuration.RegisterClassMap<COAMap>();
                        //Csv.Configuration.HeaderValidated = null;

                        Csv.Read();
                        Csv.ReadHeader();
                        Csv.Context.RegisterClassMap<COAMap>();

                        List<COAViewModel> Data = Csv.GetRecords<COAViewModel>().ToList();

                        Tuple<bool, List<object>> Validated = Service.UploadValidate(ref Data, Request.Form.ToList());

                        Reader.Close();

                        if (Validated.Item1) /* If Data Valid */
                        {
                            List<COAModel> data = Mapper.Map<List<COAModel>>(Data);
                            foreach (var item in data)
                            {
                                Transfrom(item);
                            }
                            await Service.UploadData(data);


                            Dictionary<string, object> Result =
                                new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                                .Ok();
                            return Created(HttpContext.Request.Path, Result);

                        }
                        else
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                                using (CsvWriter csvWriter = new CsvWriter(streamWriter,CultureInfo.InvariantCulture))
                                {
                                    csvWriter.WriteRecords(Validated.Item2);
                                }

                                return File(memoryStream.ToArray(), ContentType, FileName);
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, object> Result =
                           new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, General.CSV_ERROR_MESSAGE)
                           .Fail();

                        return NotFound(Result);
                    }
                }
                else
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.NO_FILE_ERROR_MESSAGE)
                            .Fail();
                    return BadRequest(Result);
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

        [HttpGet("download")]
        public IActionResult DownloadTemplate()
        {
            try
            {
                byte[] csvInBytes;
                var csv = Service.DownloadTemplate();

                string fileName = "Chart of Account Template.csv";

                csvInBytes = csv.ToArray();

                var file = File(csvInBytes, "text/csv", fileName);
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

        [HttpGet("header")]
        public IActionResult GetCOAHeaderSubheader(string keyword = null)
        {
            var read = Service.GetAll();
            var result = new List<CodeName>();
            foreach (COACategoryEnum item in Enum.GetValues(typeof(COACategoryEnum)))
            {
                result.Add(new CodeName()
                {
                    Name = item.ToDescriptionString(),
                    Code = ((int)item).ToString()
                });
            }

            if (read.Count > 0 && !string.IsNullOrWhiteSpace(keyword))
            {
                read = read.Where(element => element.Code.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) != -1 || (!string.IsNullOrWhiteSpace(element.Name) && element.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) != -1)).ToList();

                var headers = read.Select(element => element.Header).Distinct().ToList();
                var subHeaders = read.Select(element => element.Subheader).Distinct().ToList();

                result = result.Where(element => headers.Contains(element.Code) || subHeaders.Contains(element.Code)).ToList();

            }

            return Ok(result);
        }

        [HttpGet("empty-names")]
        public async Task<IActionResult> GetWithEmptyNames()
        {
            try
            {
                var read = await Service.GetEmptyNames();

                List<COAViewModel> dataVM = Mapper.Map<List<COAViewModel>>(read);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(Mapper, dataVM);
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

        [HttpPut("empty-names")]
        public async Task<IActionResult> ReviseEmptyNameCoa([FromBody] List<COAViewModel> data)
        {
            try
            {
                VerifyUser();

                List<COAModel> models = Mapper.Map<List<COAModel>>(data);

                await Service.ReviseEmptyNamesCoa(models);

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
    }

    public class CodeName
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
