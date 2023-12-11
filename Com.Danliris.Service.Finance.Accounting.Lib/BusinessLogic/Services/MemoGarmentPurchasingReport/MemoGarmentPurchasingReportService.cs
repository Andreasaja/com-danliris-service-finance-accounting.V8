using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.MemoGarmentPurchasingReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using Com.Moonlay.NetCore.Lib;
using System.Linq;
using System.IO;
using System.Data;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.MemoGarmentPurchasing;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.MemoGarmentPurchasingReport
{
    public class MemoGarmentPurchasingReportService : IMemoGarmentPurchasingReportService
    {
        private readonly FinanceDbContext _context;
        private readonly IIdentityService _identityService;
        private const string UserAgent = "finance-service";

        public MemoGarmentPurchasingReportService(FinanceDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _identityService = serviceProvider.GetService<IIdentityService>();
        }

        public ReadResponse<MemoGarmentPurchasingDetailModel> ReadReportDetailBased(int page, int size, string filter)
        {
            try
            {
                var query = _context.MemoGarmentPurchasingDetails.Include(x => x.MemoGarmentPurchasing).AsQueryable();

                var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(filter);
                //query = QueryHelper<MemoGarmentPurchasingDetailModel>.Filter(query, filterDictionary);
                foreach (var fil in filterDictionary)
                {
                    switch (fil.Key.ToLower())
                    {
                        case "accountingbookid":
                            query = query.Where(x => x.MemoGarmentPurchasing.AccountingBookId.Equals(int.Parse(fil.Value)));
                            break;
                        case "year":
                            DateTime dt;
                            if (DateTime.TryParseExact(fil.Value, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                                query = query.Where(x => x.MemoGarmentPurchasing.MemoDate.Year.Equals(dt.Year));
                            break;
                        case "month":
                            query = query.Where(x => x.MemoGarmentPurchasing.MemoDate.Month.Equals(int.Parse(fil.Value)));
                            break;
                        case "valas":
                            if (Boolean.Parse(fil.Value).Equals(true)) {
                                query = query.Where(x => x.MemoGarmentPurchasing.GarmentCurrenciesCode != "IDR");
                            }
                            else
                            {
                                query = query.Where(x => x.MemoGarmentPurchasing.GarmentCurrenciesCode.Equals("IDR"));
                            }
                            break;
                    }
                }

                var pageable = new Pageable<MemoGarmentPurchasingDetailModel>(query, page - 1, size);
                var data = pageable.Data.ToList();

                int totalData = pageable.TotalCount;

                return new ReadResponse<MemoGarmentPurchasingDetailModel>(data, totalData, new Dictionary<string, string>(), new List<string>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private ReadResponse<MemoGarmentPurchasingModel> ReadReport(int page, int size, string filter)
        {
            try
            {
                var query = _context.MemoGarmentPurchasings.Include(x => x.MemoGarmentPurchasingDetails).AsQueryable();

                var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(filter);
                //query = QueryHelper<MemoGarmentPurchasingDetailModel>.Filter(query, filterDictionary);
                foreach (var fil in filterDictionary)
                {
                    switch (fil.Key.ToLower())
                    {
                        case "accountingbookid":
                            query = query.Where(x => x.AccountingBookId.Equals(int.Parse(fil.Value)));
                            break;
                        case "year":
                            DateTime dt;
                            if (DateTime.TryParseExact(fil.Value, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                                query = query.Where(x => x.MemoDate.Year.Equals(dt.Year));
                            break;
                        case "month":
                            query = query.Where(x => x.MemoDate.Month.Equals(int.Parse(fil.Value)));
                            break;
                        case "valas":
                            if (Boolean.Parse(fil.Value).Equals(true))
                            {
                                query = query.Where(x => x.GarmentCurrenciesCode != "IDR");
                            }
                            else
                            {
                                query = query.Where(x => x.GarmentCurrenciesCode.Equals("IDR"));
                            }
                            break;
                    }
                }

                var pageable = new Pageable<MemoGarmentPurchasingModel>(query, page - 1, size);
                var data = pageable.Data.ToList();

                int totalData = pageable.TotalCount;

                return new ReadResponse<MemoGarmentPurchasingModel>(data, totalData, new Dictionary<string, string>(), new List<string>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public MemoryStream GenerateExcel(int year, int month, int accountingBookId, string accountingBookType, bool valas)
        {
            string title = "LAPORAN DATA MEMORIAL",
                date = new DateTime(year, month, 1).ToString("MMMM yyyy", new CultureInfo("id-ID"));
            var query = GetReportXlsData(year, month, accountingBookId, accountingBookType, valas);

            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn() { ColumnName = "Nomor Memo", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Memo", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No. Akun", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama Perkiraan", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Keterangan", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Debit", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Kredit", DataType = typeof(String) });

            int index = 0;
            if (query.Data.ToArray().Count() == 0)
            {
                result.Rows.Add("", "", "", "", 0.ToString("#,##0.#0"), 0.ToString("#,##0.#0")); // to allow column name to be generated properly for empty data as template
                index++;
            }
            else
            {
                foreach (var item in query.Data)
                {
                    index++;
                    result.Rows.Add(item.MemoGarmentPurchasing.MemoNo, item.MemoGarmentPurchasing.MemoDate.ToString("dd-MMM-yyyy", new CultureInfo("id-ID")), item.COANo, item.COAName, item.MemoGarmentPurchasing.Remarks ,item.DebitNominal.ToString("#,##0.#0"), item.CreditNominal.ToString("#,##0.#0"));
                }
            }
            if (string.IsNullOrEmpty(accountingBookType))
            {
                return Excel.CreateExcelWithTitleNonDateFilter(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Memorial") }, title, date, true, 10);
            }
            else{
                return Excel.CreateExcelWithTitleNonDateFilterMemoReport(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Memorial") }, title, date,accountingBookType, true, 10);
            }
        }

        public ReadResponse<MemoGarmentPurchasingModel> GetReportPdfData(int year, int month, int accountingBookId, string accountingBookType, bool valas, int page = 1, int size = 25)
        {
            object filter;

            if (accountingBookId > 0)
            {
                if (accountingBookType.ToLower().Equals("pembelian lokal"))
                {
                    filter = new
                    {
                        Year = year,
                        Month = month,
                        AccountingBookId = accountingBookId,
                        Valas = valas,
                        AccountingBookType = accountingBookType
                    };
                }
                else
                {
                    filter = new
                    {
                        Year = year,
                        Month = month,
                        AccountingBookId = accountingBookId,
                        AccountingBookType = accountingBookType
                    };
                }
            }
            else
                filter = new
                {
                    Year = year,
                    Month = month
                };

            string filterJson = JsonConvert.SerializeObject(filter);

            return ReadReport(page, size, filterJson);
        }

        private ReadResponse<MemoGarmentPurchasingDetailModel> GetReportXlsData(int year, int month, int accountingBookId, string accountingBookType, bool valas, int page = 1, int size = 25)
        {
            object filter;

            if (accountingBookId > 0) {
                if(accountingBookType.ToLower().Equals("pembelian lokal"))
                {
                    filter = new
                    {
                        Year = year,
                        Month = month,
                        AccountingBookId = accountingBookId,
                        Valas = valas,
                        AccountingBookType = accountingBookType
                    };
                }
                else
                {
                    filter = new
                    {
                        Year = year,
                        Month = month,
                        AccountingBookId = accountingBookId,
                        AccountingBooktype = accountingBookType
                    };
                }
            }                
            else
                filter = new
                {
                    Year = year,
                    Month = month
                };

            string filterJson = JsonConvert.SerializeObject(filter);

            return ReadReportDetailBased(page, size, filterJson);
        }
    }
}
