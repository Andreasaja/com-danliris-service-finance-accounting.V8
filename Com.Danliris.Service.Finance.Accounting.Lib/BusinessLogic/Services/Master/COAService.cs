using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.Master;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.BasicUploadCsvService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MasterCOA;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.Master
{
    public class COAService : ICOAService, IBasicUploadCsvService<COAViewModel>
    {
        private const string UserAgent = "finance-service";
        protected DbSet<COAModel> DbSet;
        protected IIdentityService IdentityService;
        public FinanceDbContext DbContext;
        private readonly int CODE1_LENGTH = 4;
        private readonly int CODE_COUNT = 4;
        private readonly int CODE2_LENGTH = 2;
        private readonly int CODE3_LENGTH = 1;
        private readonly int CODE4_LENGTH = 2;

        private readonly List<string> Header = new List<string>()
        {
            "Kode", "Nama", "Report Type", "Nature", "Balance"
        };
        public List<string> CsvHeader => Header;


        public sealed class COAMap : ClassMap<COAViewModel>
        {
            public COAMap()
            {
                Map(x => x.Code).Index(0);
                Map(x => x.Name).Index(1);
                Map(x => x.ReportType).Index(2);
                Map(x => x.Nature).Index(3);
                Map(x => x.Balance).Index(4);
            }
        }

        public COAService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            DbContext = dbContext;
            this.DbSet = dbContext.Set<COAModel>();
            this.IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        public async Task<int> CreateAsync(COAModel model)
        {
            CreateModel(model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            await DeleteModel(id);
            return await DbContext.SaveChangesAsync();
        }

        public ReadResponse<COAModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<COAModel> query = DbSet;

            List<string> searchAttributes = new List<string>()
            {
                "Code", "Name"
            };

            query = QueryHelper<COAModel>.Search(query, searchAttributes, keyword);

            Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<COAModel>.Filter(query, filterDictionary);

            List<string> selectedFields = new List<string>()
                {
                    "Id", "Name", "Code","Path", "Nature", "CashAccount", "ReportType", "LastModifiedUtc", "Balance"
                };

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<COAModel>.Order(query, orderDictionary);

            query = query.Select(x => new COAModel()
            {
                Id = x.Id,
                Name = x.Name,
                Code1 = x.Code1,
                Code2 = x.Code2,
                Code3 = x.Code3,
                Code4 = x.Code4,
                Code = x.Code,
                Header = x.Header,
                Subheader = x.Subheader,
                Path = x.Path,
                CashAccount = x.CashAccount,
                Nature = x.Nature,
                ReportType = x.ReportType,
                Balance = x.Balance,
                LastModifiedUtc = x.LastModifiedUtc
            });

            Pageable<COAModel> pageable = new Pageable<COAModel>(query, page - 1, size);
            List<COAModel> data = pageable.Data.ToList();
            int totalData = pageable.TotalCount;

            return new ReadResponse<COAModel>(data, totalData, orderDictionary, selectedFields);
        }

        public async Task<COAModel> ReadByIdAsync(int id)
        {
            return await ReadModelById(id);
        }

        public async Task<int> UpdateAsync(int id, COAModel model)
        {
            UpdateModelAsync(id, model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task UploadData(List<COAModel> data)
        {
            await BulkInsert(data);
        }

        public void CreateModel(COAModel model)
        {
            EntityExtension.FlagForCreate(model, IdentityService.Username, UserAgent);
            DbSet.Add(model);
        }

        public Task<COAModel> ReadModelById(int id)
        {
            return DbSet.FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public void UpdateModelAsync(int id, COAModel model)
        {
            EntityExtension.FlagForUpdate(model, IdentityService.Username, UserAgent);
            DbSet.Update(model);
        }

        public async Task DeleteModel(int id)
        {
            COAModel model = await ReadModelById(id);
            EntityExtension.FlagForDelete(model, IdentityService.Username, UserAgent, true);
            DbSet.Update(model);
        }

        public Task<int> BulkInsert(IEnumerable<COAModel> entities)
        {
            return Task.Factory.StartNew(async () =>
            {
                const int pageSize = 1000;
                int offset = 0;
                int processed = 0;

                var batch = entities.Where((data, index) => offset <= index && index < offset + pageSize);
                using (var transaction = DbContext.Database.BeginTransaction())
                {
                    while (batch.Count() > 0)
                    {
                        foreach (var item in batch)
                        {
                            EntityExtension.FlagForCreate(item, IdentityService.Username, UserAgent);
                        }
                        DbSet.AddRange(batch);
                        var result = await DbContext.SaveChangesAsync();
                        processed += batch.Count();
                        offset += pageSize;
                    };
                    transaction.Commit();
                }

                return processed;
            }).Unwrap();
        }

        public Tuple<bool, List<object>> UploadValidate(ref List<COAViewModel> Data, List<KeyValuePair<string, StringValues>> Body)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;
            var dbData = DbSet.ToList();
            foreach (COAViewModel coaVM in Data)
            {
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(coaVM.Code))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh kosong, ");
                }
                else if (Data.Any(d => d != coaVM && d.Code.Equals(coaVM.Code)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                }
                else
                {
                    var codeArray = coaVM.Code.Split('.', StringSplitOptions.RemoveEmptyEntries);
                    if (codeArray.Count() != CODE_COUNT)
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Kode tidak valid, ");
                    }
                    else
                    {
                        if (!codeArray[0].All(char.IsDigit) || !codeArray[1].All(char.IsDigit) || !codeArray[2].All(char.IsDigit) || !codeArray[3].All(char.IsDigit))
                        {
                            ErrorMessage = string.Concat(ErrorMessage, "Kode tidak valid, ");
                        }
                        if (codeArray[0].Length > CODE1_LENGTH)
                        {
                            ErrorMessage = string.Concat(ErrorMessage, "Kode tidak valid, ");
                        }
                        else if (codeArray[0].Length < CODE1_LENGTH && codeArray[0].Length > 0)
                        {
                            string firstCode = codeArray[0];
                            codeArray[0] = firstCode.PadLeft(CODE1_LENGTH, '0');
                        }

                        if (codeArray[1].Length > CODE2_LENGTH)
                        {
                            ErrorMessage = string.Concat(ErrorMessage, "Kode tidak valid, ");
                        }
                        else if (codeArray[1].Length < CODE2_LENGTH && codeArray[1].Length > 0)
                        {
                            string secondCode = codeArray[1];
                            codeArray[1] = secondCode.PadLeft(CODE2_LENGTH, '0');
                        }

                        if (codeArray[2].Length > CODE3_LENGTH)
                        {
                            ErrorMessage = string.Concat(ErrorMessage, "Kode tidak valid, ");
                        }

                        if (codeArray[3].Length > CODE4_LENGTH)
                        {
                            ErrorMessage = string.Concat(ErrorMessage, "Kode tidak valid, ");
                        }
                        else if (codeArray[3].Length < CODE4_LENGTH && codeArray[3].Length > 0)
                        {
                            string fourthCode = codeArray[3];
                            codeArray[3] = fourthCode.PadLeft(CODE4_LENGTH, '0');
                        }
                        coaVM.Code = string.Join('.', codeArray);
                        if (dbData.Any(x => x.Code == coaVM.Code))
                        {
                            ErrorMessage = string.Concat(ErrorMessage, "Kode sudah ada di database, ");
                        }
                        if (dbData.Any(x => x.Name == coaVM.Name))
                        {
                            ErrorMessage = string.Concat(ErrorMessage, "Nama sudah ada di database, ");
                        }

                    }
                }

                if (string.IsNullOrWhiteSpace(coaVM.Name))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh kosong, ");
                }
                else if (Data.Any(d => d != coaVM && d.Name.Equals(coaVM.Name)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh duplikat, ");
                }




                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;

                    Error.Add("Kode", coaVM.Code);
                    Error.Add("Nama", coaVM.Name);
                    Error.Add("Report Type", coaVM.ReportType);
                    Error.Add("Nature", coaVM.Nature);
                    Error.Add("Error", ErrorMessage);

                    ErrorList.Add(Error);
                }
            }

            if (ErrorList.Count > 0)
            {
                Valid = false;
            }

            return Tuple.Create(Valid, ErrorList);
        }

        public MemoryStream DownloadTemplate()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream))
                {

                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        foreach (var item in CsvHeader)
                        {
                            csvWriter.WriteField(item);
                        }
                        csvWriter.NextRecord();
                    }
                }
                return stream;
            }
        }

        public List<COAModel> GetAll()
        {
            IQueryable<COAModel> query = DbSet.Select(x => new COAModel()
            {
                Id = x.Id,
                Name = x.Name,
                Code1 = x.Code1,
                Code2 = x.Code2,
                Code3 = x.Code3,
                Code4 = x.Code4,
                Code = x.Code,
                Header = x.Header,
                Subheader = x.Subheader,
                Path = x.Path,
                CashAccount = x.CashAccount,
                Nature = x.Nature,
                ReportType = x.ReportType,
                Balance = x.Balance,
                LastModifiedUtc = x.LastModifiedUtc
            });


            return query.OrderBy(x => x.Code).ToList();
        }

        public Task<List<COAModel>> GetEmptyNames()
        {
            return DbSet.Where(x => string.IsNullOrEmpty(x.Name) || string.IsNullOrWhiteSpace(x.Name)).ToListAsync();
        }

        public async Task<int> ReviseEmptyNamesCoa(List<COAModel> data)
        {
            var updatedData = data.Where(x => !string.IsNullOrEmpty(x.Name) && !string.IsNullOrWhiteSpace(x.Name));

            foreach(var item in updatedData)
            {
                var model = await ReadModelById(item.Id);
                model.Name = item.Name;
                UpdateModelAsync(model.Id, model);
            }
            return await DbContext.SaveChangesAsync();
        }
    }
}
