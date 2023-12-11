using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction
{
    public class MasterCOAService : IMasterCOAService
    {
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;

        public MasterCOAService(IServiceProvider serviceProvider)
        {
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        public async Task<List<IdCOAResult>> GetCOADivisions()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.Core + $"master/divisions?size={int.MaxValue}";
            var response = await http.GetAsync(uri);

            var result = new BaseResponse<List<IdCOAResult>>()
            {
                data = new List<IdCOAResult>()
            };

            if (response.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<BaseResponse<List<IdCOAResult>>>(response.Content.ReadAsStringAsync().Result, jsonSerializerSettings);
            }
            return result.data;
        }

        public async Task<List<IdCOAResult>> GetCOAUnits()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.Core + $"master/units?size={int.MaxValue}";
            var response = await http.GetAsync(uri);

            var result = new BaseResponse<List<IdCOAResult>>()
            {
                data = new List<IdCOAResult>()
            };

            if (response.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<BaseResponse<List<IdCOAResult>>>(response.Content.ReadAsStringAsync().Result, jsonSerializerSettings);
            }
            return result.data;
        }
    }

    public interface IMasterCOAService
    {
        Task<List<IdCOAResult>> GetCOAUnits();
        Task<List<IdCOAResult>> GetCOADivisions();
    }

    public class IdCOAResult
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string COACode { get; set; }
    }

    public class BaseResponse<T>
    {
        public string apiVersion { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public T data { get; set; }

        public static implicit operator BaseResponse<T>(BaseResponse<string> v)
        {
            throw new NotImplementedException();
        }
    }
}
