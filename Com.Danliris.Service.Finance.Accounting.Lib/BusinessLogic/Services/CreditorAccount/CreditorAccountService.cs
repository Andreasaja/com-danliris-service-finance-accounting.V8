using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditorAccount;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.CreditorAccount
{
    public class CreditorAccountService : ICreditorAccountService
    {
        private const string UserAgent = "finance-service";
        protected DbSet<CreditorAccountModel> DbSet;
        protected IIdentityService IdentityService;
        public FinanceDbContext DbContext;

        public CreditorAccountService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<CreditorAccountModel>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        public void CreateModel(CreditorAccountModel model)
        {
            EntityExtension.FlagForCreate(model, IdentityService.Username, UserAgent);
            DbSet.Add(model);
        }

        public async Task<int> CreateAsync(CreditorAccountModel model)
        {
            CreateModel(model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task DeleteModel(int id)
        {
            CreditorAccountModel model = await ReadModelById(id);
            EntityExtension.FlagForDelete(model, IdentityService.Username, UserAgent, true);
            DbSet.Update(model);
        }

        public async Task<int> DeleteAsync(int id)
        {
            await DeleteModel(id);
            return await DbContext.SaveChangesAsync();
        }

        public Task<CreditorAccountModel> ReadModelById(int id)
        {
            return DbSet.FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public async Task<int> UpdateAsync(int id, CreditorAccountModel model)
        {
            UpdateModel(id, model);
            return await DbContext.SaveChangesAsync();
        }

        public void UpdateModel(int id, CreditorAccountModel model)
        {
            EntityExtension.FlagForUpdate(model, IdentityService.Username, UserAgent);
            DbSet.Update(model);
        }

        public MemoryStream GenerateExcel(string suplierName, int month, int year, int offSet)
        {
            var data = GetReport(suplierName, month, year);
            string title = "Kartu Hutang",
                date = new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("dd MMMM yyyy");

            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Bon Terima Unit", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Bukti Pengeluaran Bank", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor NI/SPB", DataType = typeof(string) });
            //dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Memo", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Invoice", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Koreksi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tempo Pembayaran", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice DPP", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice DPP Valas", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice PPN", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nilai Invoice Total", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mutasi Pembelian", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mutasi Pembayaran", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Saldo Akhir", DataType = typeof(string) });

            int index = 0;
            if (data.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "TOTAL", "", "", "", "IDR", 0.ToString("#,##0.#0"));
                index++;
            }
            else
            {
                decimal totalBalance = 0;
                foreach (var item in data)
                {
                    if (!string.IsNullOrWhiteSpace(item.Remark))
                    {
                        dt.Rows.Add("", "", "", "", "", "", "", "", "", item.Remark, "", "", "IDR", item.FinalBalance.ToString("#,##0.#0"));
                    }
                    else
                    {
                        dt.Rows.Add(
                        item.Date.AddHours(offSet).ToString("dd-MMM-yyyy"),
                        item.UnitReceiptNoteNo,
                        item.BankExpenditureNoteNo,
                        item.UnitPaymentOrderNo,
                        //item.MemoNo,
                        item.InvoiceNo,
                        item.UnitPaymentCorrectionNoteNo,
                        item.PaymentDuration,
                        item.DPPAmount.ToString("#,##0.#0"),
                        item.DPPAmountCurrency.ToString("#,##0.#0"),
                        item.VATAmount.ToString("#,##0.#0"),
                        item.Mutation.ToString("#,##0.#0"),
                        item.PurchaseAmount.ToString("#,##0.#0"),
                        item.PaymentAmount.ToString("#,##0.#0"),
                        item.FinalBalance.ToString("#,##0.#0"));
                        totalBalance = item.FinalBalance;
                    }

                    index++;
                }

                dt.Rows.Add("", "", "", "", "", "", "", "", "", "TOTAL", "", "", "IDR", totalBalance.ToString("#,##0.#0"));
                index++;
            }
            return Excel.CreateExcelWithTitleNonDateFilterWithSupplierName(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Kartu Hutang") }, title, suplierName, date, true, index);
        }

        public List<CreditorAccountViewModel> GeneratePdf(string suplierName, int month, int year, int offSet)
        {
            var data = GetReport(suplierName, month, year, offSet).Item1.ToList();

            return data;
        }

        public decimal? GetFinalBalance(string suplierName, int month, int year, int offSet)
        {
            var data = GetReport(suplierName, month, year, offSet).Item2;

            return data;
        }

        public (ReadResponse<DebtCardDto>, decimal) GetReport(int page, int size, string suplierName, int month, int year, int offSet)
        {
            var queries = GetReport(suplierName, month, year);
            var finalBalance = queries.LastOrDefault() == null ? 0 : queries.LastOrDefault().FinalBalance;

            var pageable = new Pageable<DebtCardDto>(queries, page - 1, size);
            var data = pageable.Data.ToList();

            return (new ReadResponse<DebtCardDto>(queries, pageable.TotalCount, new Dictionary<string, string>(), new List<string>()), finalBalance);
        }

        private List<CreditorAccountViewModel> GetPreviousMonthReport(IQueryable<CreditorAccountModel> supplierQuery, int month, int year, int offSet)
        {
            List<CreditorAccountViewModel> result = new List<CreditorAccountViewModel>();
            List<CreditorAccountModel> previousMonthCreditorAccount = new List<CreditorAccountModel>();
            var timeOffset = new TimeSpan(IdentityService.TimezoneOffset, 0, 0);
            var dateSearch = new DateTimeOffset(year, month, 1, 0, 0, 0, timeOffset);
            var debtQuery = supplierQuery.Where(x => x.FinalBalance > 0 && x.UnitReceiptNoteDate.HasValue && x.UnitReceiptNoteDate.Value < dateSearch).ToList();
            var paidQuery = supplierQuery.Where(x => x.FinalBalance == 0 && x.BankExpenditureNoteDate.HasValue && x.BankExpenditureNoteDate.Value.Month == month && x.BankExpenditureNoteDate.Value.Year == year && x.UnitReceiptNoteDate.HasValue && x.UnitReceiptNoteDate.Value < dateSearch).ToList();

            previousMonthCreditorAccount.AddRange(debtQuery);
            previousMonthCreditorAccount.AddRange(paidQuery);

            foreach (var item in previousMonthCreditorAccount.OrderBy(x => x.UnitReceiptNoteDate.GetValueOrDefault()))
            {
                decimal unitReceiptMutaion = 0;
                decimal bankExpenditureMutation = 0;
                decimal memoMutation = 0;
                //if (!string.IsNullOrEmpty(item.UnitReceiptNoteNo))
                //{
                CreditorAccountViewModel vm = new CreditorAccountViewModel
                {
                    Id = item.Id,
                    UnitReceiptNoteNo = item.UnitReceiptNoteNo,
                    Date = item.UnitReceiptNoteDate.Value,
                    InvoiceNo = item.InvoiceNo,
                    DPP = item.UnitReceiptNoteDPP,
                    //PPN = item.UnitReceiptNotePPN,
                    Total = item.UnitReceiptNoteDPP,
                    Mutation = item.UnitReceiptMutation,
                    Products = item.Products,
                    //MemoNo = item.MemoNo

                };
                unitReceiptMutaion = vm.Mutation;
                //}

                if (!string.IsNullOrEmpty(item.MemoNo) && item.MemoDate.HasValue && item.MemoDate.Value.AddHours(offSet).Month == month && item.MemoDate.Value.AddHours(offSet).Year == year)
                {
                    vm.UnitReceiptNoteNo = item.UnitReceiptNoteNo;
                    vm.MemoNo = item.MemoNo;
                    vm.Date = item.UnitReceiptNoteDate.Value;
                    vm.PPN = item.UnitReceiptNotePPN;
                    vm.Products = item.Products;
                    vm.Total = item.UnitReceiptNoteDPP + item.UnitReceiptNotePPN;
                }

                if (!string.IsNullOrEmpty(item.BankExpenditureNoteNo) && item.BankExpenditureNoteDate.HasValue && item.BankExpenditureNoteDate.Value.AddHours(offSet).Month == month && item.BankExpenditureNoteDate.Value.AddHours(offSet).Year == year)
                {
                    //CreditorAccountViewModel vm = new CreditorAccountViewModel
                    //{
                    vm.BankExpenditureNoteNo = item.BankExpenditureNoteNo;
                    vm.Date = item.UnitReceiptNoteDate.Value;
                    vm.InvoiceNo = item.InvoiceNo;
                    vm.DPP = item.BankExpenditureNoteDPP;
                    vm.PPN = item.BankExpenditureNotePPN;
                    vm.Total = item.BankExpenditureNoteMutation;
                    vm.MutationPayment = item.BankExpenditureNoteMutation * -1;
                    vm.MemoNo = item.MemoNo;

                    //};
                    bankExpenditureMutation = vm.MutationPayment;
                    //result.Add(vm);
                }

                if (!string.IsNullOrEmpty(item.PurchasingMemoNo))
                {
                    vm.MemoNo = item.PurchasingMemoNo;
                }

                //if (!string.IsNullOrEmpty(item.MemoNo))
                //{
                //    //CreditorAccountViewModel vm = new CreditorAccountViewModel
                //    //{
                //    vm.MemoNo = item.MemoNo;
                //    vm.Date = item.MemoDate.Value;
                //    vm.InvoiceNo = item.InvoiceNo;
                //    vm.DPP = item.MemoDPP;
                //    vm.PPN = item.MemoPPN;
                //    vm.Total = item.MemoMutation;
                //    vm.Mutation = item.MemoMutation;

                //    //};
                //    memoMutation = vm.Mutation;
                //    //result.Add(vm);
                //}

                result.Add(vm);
            }

            if (result.Count > 0)
            {
                CreditorAccountViewModel totalPrevious = new CreditorAccountViewModel()
                {
                    FinalBalance = result.Sum(x => x.Mutation + x.MutationPayment)
                };
                result.Add(totalPrevious);
            }

            return result;
        }

        public List<DebtCardDto> GetReport(string supplierName, int month, int year)
        {
            DateTimeOffset firstDayOfMonth = new DateTime(year, month, 1);
            DateTimeOffset lastDayOfMonth = firstDayOfMonth.AddMonths(1);
            var unitReceiptNoteStartBalance = DbContext.CreditorAccounts.Where(entity => entity.UnitReceiptNoteDate.HasValue && entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime < firstDayOfMonth.DateTime && string.IsNullOrWhiteSpace(entity.UnitPaymentCorrectionNo) && (entity.IsStartBalance || entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).Year >= 2021)).Sum(entity => entity.UnitReceiptNoteDPP - entity.IncomeTaxAmount);

            var unitPaymentOrderStartBalance = DbContext.CreditorAccounts.Where(entity => entity.MemoDate.HasValue && entity.MemoDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime < firstDayOfMonth.DateTime && string.IsNullOrWhiteSpace(entity.UnitPaymentCorrectionNo) && (entity.IsStartBalance || entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).Year >= 2021)).Sum(entity => entity.UnitReceiptNotePPN);

            var bankExpenitureNoteStartBalance = DbContext.CreditorAccounts.Where(entity => entity.BankExpenditureNoteDate.HasValue && entity.BankExpenditureNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime < firstDayOfMonth.DateTime && string.IsNullOrWhiteSpace(entity.UnitPaymentCorrectionNo) && (entity.IsStartBalance || entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).Year >= 2021)).Sum(entity => entity.BankExpenditureNoteMutation);

            var correctionStartBalance = DbContext.CreditorAccounts.Where(entity => entity.UnitPaymentCorrectionDate.HasValue && entity.UnitPaymentCorrectionDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime < firstDayOfMonth.DateTime && !string.IsNullOrWhiteSpace(entity.UnitPaymentCorrectionNo) && (entity.IsStartBalance || entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).Year >= 2021)).Sum(entity => entity.UnitPaymentCorrectionMutation);

            if (!string.IsNullOrWhiteSpace(supplierName))
            {
                unitReceiptNoteStartBalance = DbContext.CreditorAccounts.Where(entity => entity.UnitReceiptNoteDate.HasValue && entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime < firstDayOfMonth.DateTime && string.IsNullOrWhiteSpace(entity.UnitPaymentCorrectionNo) && entity.SupplierName == supplierName && (entity.IsStartBalance || entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).Year >= 2021)).Sum(entity => entity.UnitReceiptNoteDPP - entity.IncomeTaxAmount);
                unitPaymentOrderStartBalance = DbContext.CreditorAccounts.Where(entity => entity.MemoDate.HasValue && entity.MemoDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime < firstDayOfMonth.DateTime && string.IsNullOrWhiteSpace(entity.UnitPaymentCorrectionNo) && entity.SupplierName == supplierName && (entity.IsStartBalance || entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).Year >= 2021)).Sum(entity => entity.UnitReceiptNotePPN);
                bankExpenitureNoteStartBalance = DbContext.CreditorAccounts.Where(entity => entity.BankExpenditureNoteDate.HasValue && entity.BankExpenditureNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime < firstDayOfMonth.DateTime && string.IsNullOrWhiteSpace(entity.UnitPaymentCorrectionNo) && entity.SupplierName == supplierName && (entity.IsStartBalance || entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).Year >= 2021)).Sum(entity => entity.BankExpenditureNoteMutation);
                correctionStartBalance = DbContext.CreditorAccounts.Where(entity => entity.UnitPaymentCorrectionDate.HasValue && entity.UnitPaymentCorrectionDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime < firstDayOfMonth.DateTime && !string.IsNullOrWhiteSpace(entity.UnitPaymentCorrectionNo) && entity.SupplierName == supplierName && (entity.IsStartBalance || entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).Year >= 2021)).Sum(entity => entity.UnitPaymentCorrectionMutation);
            }

            var query = DbContext.CreditorAccounts.Where(entity => (entity.UnitReceiptNoteDate.HasValue && entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Month == month && entity.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Year == year) || (entity.MemoDate.HasValue && entity.MemoDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Month == month && entity.MemoDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Year == year) || (entity.UnitPaymentCorrectionDate.HasValue && entity.UnitPaymentCorrectionDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Month == month && entity.UnitPaymentCorrectionDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Year == year) || (entity.BankExpenditureNoteDate.HasValue && entity.BankExpenditureNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Month == month && entity.BankExpenditureNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Year == year) || (entity.PurchasingMemoDate.HasValue && entity.PurchasingMemoDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Month == month && entity.PurchasingMemoDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Year == year));

            //if (divisionId > 0)
            //    query = query.Where(entity => entity.DivisionId == divisionId);

            if (!string.IsNullOrWhiteSpace(supplierName))
                query = query.Where(entity => entity.SupplierName == supplierName);

            var queryResult = query.ToList();

            var tempResult = new List<DebtCardDto>();
            foreach (var item in queryResult)
            {
                var currencyCode = item.CurrencyCode;
                var currencyRate = item.CurrencyRate;
                var date = item.UnitReceiptNoteDate;
                var unitReceiptNoteNo = item.UnitReceiptNoteNo;
                var bankExpenditureNoteNo = "";
                var unitPaymentOrderNo = "";
                var invoiceNo = "";
                var unitPaymentCorrectionNoteNo = "";
                int.TryParse(item.PaymentDuration, out var paymentDuration);
                var dppAmount = (decimal)0;
                var dppAmountCurency = (decimal)0;
                var vatAmount = (decimal)0;
                var products = item.Products;

                var paymentAmount = (decimal)0;

                if (string.IsNullOrWhiteSpace(item.UnitPaymentCorrectionNo))
                {
                    if (!string.IsNullOrWhiteSpace(item.UnitReceiptNoteNo) && item.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Month == month && item.UnitReceiptNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Year == year)
                    {
                        dppAmount = item.UnitReceiptNoteDPP - item.IncomeTaxAmount;
                        if (item.CurrencyCode != "IDR")
                            dppAmountCurency = item.DPPCurrency - (item.IncomeTaxAmount / item.CurrencyRate);
                    }

                    if (!string.IsNullOrWhiteSpace(item.MemoNo) && item.MemoDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Month == month && item.MemoDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Year == year)
                    {
                        date = item.MemoDate;
                        invoiceNo = item.InvoiceNo;
                        unitPaymentOrderNo = item.MemoNo;
                        vatAmount = item.UnitReceiptNotePPN;
                    }

                    if (!string.IsNullOrWhiteSpace(item.BankExpenditureNoteNo) && item.BankExpenditureNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Month == month && item.BankExpenditureNoteDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Year == year)
                    {
                        date = item.BankExpenditureNoteDate;
                        invoiceNo = item.InvoiceNo;
                        unitPaymentOrderNo = item.MemoNo;
                        bankExpenditureNoteNo = item.BankExpenditureNoteNo;
                        paymentAmount = item.BankExpenditureNoteMutation;
                    }

                    if (!string.IsNullOrWhiteSpace(item.PurchasingMemoNo) && item.PurchasingMemoDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Month == month && item.PurchasingMemoDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Year == year)
                    {
                        bankExpenditureNoteNo = item.PurchasingMemoNo;
                        paymentAmount = (decimal)item.PurchasingMemoAmount;
                    }

                    var mutation = dppAmount + vatAmount;
                    var purchaseAmount = mutation;
                    tempResult.Add(new DebtCardDto(date.GetValueOrDefault(), unitReceiptNoteNo, bankExpenditureNoteNo, unitPaymentOrderNo, invoiceNo, unitPaymentCorrectionNoteNo, paymentDuration, dppAmount, dppAmountCurency, vatAmount, mutation, purchaseAmount, paymentAmount, products));
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(item.UnitPaymentCorrectionNo) && item.UnitPaymentCorrectionDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Month == month && item.UnitPaymentCorrectionDate.GetValueOrDefault().AddHours(IdentityService.TimezoneOffset).DateTime.Year == year)
                    {
                        date = item.UnitPaymentCorrectionDate;
                        unitPaymentCorrectionNoteNo = item.UnitPaymentCorrectionNo;
                        invoiceNo = item.InvoiceNo;
                        unitPaymentOrderNo = item.MemoNo;
                        dppAmount = item.UnitPaymentCorrectionDPP;
                        if (item.CurrencyCode != "IDR")
                            dppAmountCurency = item.UnitPaymentCorrectionDPP / item.CurrencyRate;
                        vatAmount = item.UnitPaymentCorrectionPPN;

                        if (item.IncomeTaxAmount != 0)
                        {
                            //var incomeTaxRate = item.IncomeTaxAmount / item.UnitReceiptNoteDPP;
                            //var incomeTaxCorrection = item.UnitPaymentCorrectionDPP * incomeTaxRate;
                            dppAmount -= item.IncomeTaxAmount;
                            if (item.CurrencyCode != "IDR")
                                dppAmountCurency -= (item.IncomeTaxAmount / item.CurrencyRate);
                        }

                        var mutation = dppAmount + vatAmount;
                        var purchaseAmount = mutation;
                        tempResult.Add(new DebtCardDto(date.GetValueOrDefault(), unitReceiptNoteNo, bankExpenditureNoteNo, unitPaymentOrderNo, invoiceNo, unitPaymentCorrectionNoteNo, paymentDuration, dppAmount, dppAmountCurency, vatAmount, mutation, purchaseAmount, paymentAmount, products));
                    }
                }
            }

            tempResult = tempResult.OrderBy(element => element.Date).ThenBy(element => element.UnitReceiptNoteNo).ThenBy(element => element.UnitPaymentOrderNo).ThenBy(element => element.UnitPaymentCorrectionNoteNo).ThenBy(element => element.BankExpenditureNoteNo).ToList();

            var startBalance = unitReceiptNoteStartBalance + unitPaymentOrderStartBalance + correctionStartBalance - bankExpenitureNoteStartBalance;
            var result = new List<DebtCardDto>();
            var previousMonthSummary = new DebtCardDto("SALDO AWAL", startBalance);
            result.Add(previousMonthSummary);
            foreach (var item in tempResult)
            {
                startBalance = startBalance + item.PurchaseAmount - item.PaymentAmount;
                item.SetFinalBalance(startBalance);
                result.Add(item);
            }

            return result;
        }

        public (List<CreditorAccountViewModel>, decimal) GetReport(string suplierName, int month, int year, int offSet)
        {
            var supplierQuery = DbContext.CreditorAccounts.AsQueryable();
            var result = GetPreviousMonthReport(supplierQuery, month, year, offSet);

            //var currentQuery = supplierQuery.Where(x => x.UnitReceiptNoteDate.HasValue && x.UnitReceiptNoteDate.Value.Month == month && x.UnitReceiptNoteDate.Value.Year == year || (x.UnitPaymentCorrectionDate.HasValue && x.UnitPaymentCorrectionDate.Value.Month == month && x.UnitPaymentCorrectionDate.Value.Year == year));
            var currentQuery = supplierQuery.Where(x => (x.UnitReceiptNoteDate.HasValue && x.UnitReceiptNoteDate.Value.AddHours(offSet).Month == month && x.UnitReceiptNoteDate.Value.AddHours(offSet).Year == year) || (x.MemoDate.HasValue && x.MemoDate.Value.AddHours(offSet).Month == month && x.MemoDate.Value.AddHours(offSet).Year == year) || (x.BankExpenditureNoteDate.HasValue && x.BankExpenditureNoteDate.Value.AddHours(offSet).Month == month && x.BankExpenditureNoteDate.Value.AddHours(offSet).Year == year) || (x.UnitPaymentCorrectionDate.HasValue && x.UnitPaymentCorrectionDate.Value.AddHours(offSet).Month == month && x.UnitPaymentCorrectionDate.Value.AddHours(offSet).Year == year));

            //if (currentQuery.Count() == 0)
            //{
            //    return (new List<CreditorAccountViewModel>(), 0);
            //}

            currentQuery = currentQuery.Where(x => !result.Select(element => element.Id).Contains(x.Id));

            var items = currentQuery.OrderBy(x => x.UnitReceiptNoteDate.GetValueOrDefault()).ToList();

            foreach (var item in items)
            {
                decimal unitReceiptMutation = 0;
                decimal bankExpenditureMutation = 0;
                //decimal memoMutation = 0;
                //if (!string.IsNullOrEmpty(item.UnitReceiptNoteNo))
                //{
                CreditorAccountViewModel vm = new CreditorAccountViewModel();

                if (!string.IsNullOrEmpty(item.UnitReceiptNoteNo) && item.UnitReceiptNoteDate.HasValue && item.UnitReceiptNoteDate.Value.AddHours(offSet).Month == month && item.UnitReceiptNoteDate.Value.AddHours(offSet).Year == year)
                {
                    vm.UnitReceiptNoteNo = item.UnitReceiptNoteNo;
                    vm.Products = item.Products;
                    vm.Date = item.UnitReceiptNoteDate.GetValueOrDefault();
                    vm.InvoiceNo = item.InvoiceNo;
                    vm.DPP = item.UnitReceiptNoteDPP;
                    vm.DPPCurrency = item.DPPCurrency;
                    //PPN = item.UnitReceiptNotePPN,
                    vm.Total = item.UnitReceiptNoteDPP;
                    vm.Mutation = item.CurrencyRate != 1 ? item.UnitReceiptMutation * item.CurrencyRate : item.UnitReceiptMutation;
                    vm.PaymentDuration = item.PaymentDuration;
                    //MemoNo = item.MemoNo,
                    //CorrectionNo = item.UnitPaymentCorrectionNo
                };
                unitReceiptMutation = vm.Mutation;
                //result.Add(vm);
                //}

                if (!string.IsNullOrEmpty(item.MemoNo) && item.MemoDate.HasValue && item.MemoDate.Value.AddHours(offSet).Month == month && item.MemoDate.Value.AddHours(offSet).Year == year)
                {
                    vm.UnitReceiptNoteNo = item.UnitReceiptNoteNo;
                    vm.MemoNo = item.MemoNo;
                    vm.Date = item.MemoDate.Value;
                    vm.PPN = item.UnitReceiptNotePPN;
                    vm.Products = item.Products;
                    vm.Total = item.UnitReceiptNoteDPP + item.UnitReceiptNotePPN;
                }

                if (!string.IsNullOrEmpty(item.BankExpenditureNoteNo) && item.BankExpenditureNoteDate.HasValue && item.BankExpenditureNoteDate.Value.AddHours(offSet).Month == month && item.BankExpenditureNoteDate.Value.AddHours(offSet).Year == year)
                {
                    vm.BankExpenditureNoteNo = item.BankExpenditureNoteNo;
                    vm.Date = item.BankExpenditureNoteDate.GetValueOrDefault();
                    //vm.InvoiceNo = item.InvoiceNo;
                    //vm.DPP = item.BankExpenditureNoteDPP;
                    //vm.PPN = item.BankExpenditureNotePPN;
                    //vm.Total = item.BankExpenditureNoteMutation;
                    vm.MutationPayment = item.BankExpenditureNoteMutation * -1;
                    vm.MemoNo = item.MemoNo;
                    vm.UnitReceiptNoteNo = item.UnitReceiptNoteNo;
                    vm.PaymentDuration = item.PaymentDuration;
                    vm.Products = item.Products;
                    //};
                    bankExpenditureMutation = vm.MutationPayment;
                    //}
                }

                if (!string.IsNullOrEmpty(item.PurchasingMemoNo))
                {
                    vm.MemoNo = item.PurchasingMemoNo;
                }

                if (!string.IsNullOrEmpty(item.UnitPaymentCorrectionNo) && item.UnitPaymentCorrectionDate.HasValue && item.UnitPaymentCorrectionDate.Value.AddHours(offSet).Month == month && item.UnitPaymentCorrectionDate.Value.AddHours(offSet).Year == year)
                {
                    vm.BankExpenditureNoteNo = item.BankExpenditureNoteNo;
                    //vm.Date = item.BankExpenditureNoteDate.GetValueOrDefault();
                    vm.InvoiceNo = item.InvoiceNo;
                    vm.DPP = item.UnitPaymentCorrectionDPP;
                    vm.PPN = item.UnitPaymentCorrectionPPN;
                    vm.Total = item.UnitPaymentCorrectionDPP + item.UnitPaymentCorrectionPPN;
                    vm.Mutation = item.UnitPaymentCorrectionMutation;
                    vm.MemoNo = item.MemoNo;
                    vm.PaymentDuration = item.PaymentDuration;
                    vm.Products = item.Products;
                    //};
                    //bankExpenditureMutation = vm.Mutation.GetValueOrDefault();
                    //}
                }

                if (item.PurchasingMemoId > 0)
                {
                    vm.BankExpenditureNoteNo = item.PurchasingMemoNo;
                }

                result.Add(vm);

                //if (!string.IsNullOrEmpty(item.MemoNo))
                //{
                //    if (item.MemoDate.HasValue && item.MemoDate.Value.Month == month && item.MemoDate.Value.Year == year)
                //    {
                //        CreditorAccountViewModel vm = new CreditorAccountViewModel
                //        {
                //            MemoNo = item.MemoNo,
                //            Date = item.MemoDate.Value,
                //            InvoiceNo = item.InvoiceNo,
                //            DPP = item.MemoDPP,
                //            PPN = item.MemoPPN,
                //            Total = item.MemoMutation,
                //            Mutation = item.MemoMutation,

                //        };
                //        memoMutation = vm.Mutation.GetValueOrDefault();
                //        result.Add(vm);
                //    }

                //}

                CreditorAccountViewModel resultVM = new CreditorAccountViewModel()
                {
                    InvoiceNo = item.InvoiceNo,
                    Mutation = unitReceiptMutation - bankExpenditureMutation,
                    FinalBalance = unitReceiptMutation - bankExpenditureMutation,
                    Currency = item.CurrencyCode,
                    CurrencyRate = item.CurrencyRate,
                    DPPCurrency = item.DPPCurrency
                };

                result.Add(resultVM);
            }

            return (result, result.Sum(x => x.FinalBalance));
        }

        public async Task<int> UpdateFromUnitReceiptNoteAsync(CreditorAccountUnitReceiptNotePostedViewModel viewModel)
        {
            CreditorAccountModel data = await DbSet.FirstOrDefaultAsync(x => x.UnitReceiptNoteNo == viewModel.Code);
            if (data == null)
                throw new NotFoundException();

            data.UnitReceiptNotePPN = (viewModel.UseIncomeTax ? (decimal)0.1 * viewModel.DPP : 0);
            data.UnitReceiptNoteNo = viewModel.Code;
            data.UnitReceiptNoteDPP = viewModel.DPP;
            data.UnitReceiptNoteDate = viewModel.Date;
            data.UnitReceiptMutation = viewModel.DPP + (viewModel.UseIncomeTax ? (decimal)0.1 * viewModel.DPP : 0);
            data.FinalBalance = data.UnitReceiptMutation + data.BankExpenditureNoteMutation;
            data.DivisionId = viewModel.DivisionId;
            data.DivisionCode = viewModel.DivisionCode;
            data.DivisionName = viewModel.DivisionName;
            data.UnitId = viewModel.UnitId;
            data.UnitCode = viewModel.UnitCode;
            data.UnitName = viewModel.UnitName;


            UpdateModel(data.Id, data);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<CreditorAccountUnitReceiptNotePostedViewModel> GetByUnitReceiptNote(string supplierCode, string unitReceiptNote, string invoiceNo)
        {
            CreditorAccountModel data;

            if (string.IsNullOrEmpty(invoiceNo))
                data = await DbSet.FirstOrDefaultAsync(x => x.SupplierCode == supplierCode && x.UnitReceiptNoteNo == unitReceiptNote);
            else
                data = await DbSet.FirstOrDefaultAsync(x => x.SupplierCode == supplierCode && x.UnitReceiptNoteNo == unitReceiptNote && x.InvoiceNo == invoiceNo);

            if (data == null)
                return null;

            return new CreditorAccountUnitReceiptNotePostedViewModel()
            {
                CreditorAccountId = data.Id,
                Code = data.UnitReceiptNoteNo,
                Date = data.UnitReceiptNoteDate.Value,
                DPP = data.UnitReceiptNoteDPP,
                PPN = data.UnitReceiptNotePPN,
                InvoiceNo = data.InvoiceNo,
                SupplierCode = data.SupplierCode,
                SupplierName = data.SupplierName,
                Currency = data.CurrencyCode
            };
        }

        public async Task<CreditorAccountBankExpenditureNotePostedViewModel> GetByBankExpenditureNote(string supplierCode, string bankExpenditureNote, string invoiceNo)
        {
            CreditorAccountModel data = await DbSet.FirstOrDefaultAsync(x => x.SupplierCode == supplierCode && x.BankExpenditureNoteNo == bankExpenditureNote && x.InvoiceNo == invoiceNo);

            if (data == null)
                return null;

            return new CreditorAccountBankExpenditureNotePostedViewModel()
            {
                CreditorAccountId = data.Id,
                Mutation = data.BankExpenditureNoteMutation,
                Id = data.BankExpenditureNoteId,
                Code = data.BankExpenditureNoteNo,
                Date = data.BankExpenditureNoteDate.Value,
                InvoiceNo = data.InvoiceNo,
                SupplierCode = data.SupplierCode,
                SupplierName = data.SupplierName
            };
        }

        //public async Task<CreditorAccountMemoPostedViewModel> GetByMemo(string supplierCode, string memoNo, string invoiceNo)
        //{
        //    CreditorAccountModel data= await DbSet.FirstOrDefaultAsync(x => x.SupplierCode == supplierCode && x.MemoNo == memoNo && x.InvoiceNo == invoiceNo);

        //    if (data == null)
        //        return null;

        //    return new CreditorAccountMemoPostedViewModel()
        //    {
        //        CreditorAccountId = data.Id,
        //        Code = data.MemoNo,
        //        Date = data.MemoDate.Value,
        //        DPP = data.MemoDPP,
        //        PPN = data.MemoPPN,
        //        InvoiceNo = data.InvoiceNo,
        //        SupplierCode = data.SupplierCode,
        //        SupplierName = data.SupplierName
        //    };
        //}

        public async Task<int> CreateFromBankExpenditureNoteAsync(CreditorAccountBankExpenditureNotePostedViewModel viewModel)
        {
            var creditorAccount = DbSet.Where(x => x.BankExpenditureNoteNo == null && x.SupplierCode == viewModel.SupplierCode && x.UnitPaymentCorrectionNo == null && x.MemoNo == viewModel.MemoNo && x.BankExpenditureNoteNo == null).ToList();

            CreditorAccountModel model = await DbSet.FirstOrDefaultAsync(x => x.BankExpenditureNoteNo == null && x.SupplierCode == viewModel.SupplierCode && x.UnitPaymentCorrectionNo == null && x.MemoNo == viewModel.MemoNo && x.BankExpenditureNoteNo == null);

            decimal remaining = viewModel.Mutation;

            if (creditorAccount.Count > 1)
            {
                foreach (var item in creditorAccount)
                {
                    if (remaining >= item.FinalBalance)
                    {
                        var newModel = await DbContext.CreditorAccounts.FirstOrDefaultAsync(entity => entity.UnitReceiptNoteNo == item.UnitReceiptNoteNo && entity.BankExpenditureNoteNo != null);

                        if (newModel != null)
                        {
                            item.UnitReceiptNoteDate = newModel.UnitReceiptNoteDate;
                            item.MemoDate = newModel.MemoDate;
                            item.BankExpenditureNoteMutation = (item.UnitReceiptNoteDPP + item.UnitReceiptNotePPN - item.IncomeTaxAmount) - newModel.BankExpenditureNoteMutation;
                            item.UnitReceiptNoteDPP = 0;
                            item.UnitReceiptNotePPN = 0;
                            item.BankExpenditureNoteDate = viewModel.Date;
                            item.BankExpenditureNoteId = viewModel.Id;
                            item.BankExpenditureNoteNo = viewModel.Code;
                            item.FinalBalance = item.UnitReceiptMutation + (item.BankExpenditureNoteMutation * -1) + item.MemoMutation + (newModel.BankExpenditureNoteMutation * -1);

                            UpdateModel(item.Id, item);
                        }
                        else
                        {
                            item.BankExpenditureNoteDate = viewModel.Date;
                            item.BankExpenditureNoteId = viewModel.Id;
                            item.BankExpenditureNoteMutation = item.UnitReceiptNoteDPP + item.UnitReceiptNotePPN - item.IncomeTaxAmount;
                            item.BankExpenditureNoteNo = viewModel.Code;
                            item.FinalBalance = item.UnitReceiptMutation + (item.BankExpenditureNoteMutation * -1) + item.MemoMutation;

                            UpdateModel(item.Id, item);
                        }

                        remaining -= item.BankExpenditureNoteMutation;
                    }
                    else
                    {
                        var newModel = await DbContext.CreditorAccounts.FirstOrDefaultAsync(entity => entity.UnitReceiptNoteNo == item.UnitReceiptNoteNo && entity.BankExpenditureNoteNo != null);

                        if (newModel == null)
                        {
                            var newCreditorAccount = new CreditorAccountModel(
                                item.SupplierName,
                                item.SupplierCode,
                                item.SupplierIsImport,
                                item.DivisionId,
                                item.DivisionCode,
                                item.DivisionName,
                                item.UnitId,
                                item.UnitCode,
                                item.UnitName,
                                item.UnitPaymentCorrectionId,
                                item.UnitPaymentCorrectionNo,
                                item.UnitPaymentCorrectionDPP,
                                item.UnitPaymentCorrectionPPN,
                                item.UnitPaymentCorrectionMutation,
                                item.UnitPaymentCorrectionDate.GetValueOrDefault(),
                                item.UnitReceiptNoteNo,
                                item.Products,
                                item.UnitReceiptNoteDate,
                                item.UnitReceiptNoteDPP,
                                item.UnitReceiptNotePPN,
                                item.UnitReceiptMutation,
                                viewModel.Id,
                                viewModel.Code,
                                viewModel.Date,
                                item.BankExpenditureNoteDPP,
                                item.BankExpenditureNotePPN,
                                remaining,
                                item.MemoNo,
                                item.MemoDate,
                                item.MemoDPP,
                                item.MemoPPN,
                                item.MemoMutation,
                                item.PaymentDuration,
                                item.InvoiceNo,
                                0,
                                item.CurrencyCode,
                                item.DPPCurrency,
                                item.CurrencyRate,
                                0,
                                0,
                                item.ExternalPurchaseOrderNo
                                );

                            EntityExtension.FlagForCreate(newCreditorAccount, IdentityService.Username, UserAgent);
                            DbSet.Add(newCreditorAccount);

                            item.UnitReceiptNoteDate = null;
                            item.MemoDate = null;
                            item.FinalBalance = (item.UnitReceiptMutation + (remaining * -1) + item.MemoMutation);

                            UpdateModel(item.Id, item);
                        }
                        else
                        {
                            model.UnitReceiptNoteDate = newModel.UnitReceiptNoteDate;
                            model.MemoDate = newModel.MemoDate;
                            model.UnitReceiptNoteDPP = 0;
                            model.UnitReceiptNotePPN = 0;
                            model.BankExpenditureNoteDate = viewModel.Date;
                            model.BankExpenditureNoteId = viewModel.Id;
                            model.BankExpenditureNoteMutation = viewModel.Mutation;
                            model.BankExpenditureNoteNo = viewModel.Code;
                            model.FinalBalance = model.UnitReceiptMutation + (model.BankExpenditureNoteMutation * -1) + model.MemoMutation;

                            UpdateModel(model.Id, model);
                        }
                    }
                }

                return await DbContext.SaveChangesAsync();
            }
            else
            {

                if (model == null)
                {
                    var newModel = await DbContext.CreditorAccounts.FirstOrDefaultAsync(entity => entity.MemoNo == viewModel.MemoNo && entity.BankExpenditureNoteNo != null);

                    var newCreditorAccount = new CreditorAccountModel(
                                newModel.SupplierName,
                                newModel.SupplierCode,
                                newModel.SupplierIsImport,
                                newModel.DivisionId,
                                newModel.DivisionCode,
                                newModel.DivisionName,
                                newModel.UnitId,
                                newModel.UnitCode,
                                newModel.UnitName,
                                newModel.UnitPaymentCorrectionId,
                                newModel.UnitPaymentCorrectionNo,
                                newModel.UnitPaymentCorrectionDPP,
                                newModel.UnitPaymentCorrectionPPN,
                                newModel.UnitPaymentCorrectionMutation,
                                newModel.UnitPaymentCorrectionDate.GetValueOrDefault(),
                                newModel.UnitReceiptNoteNo,
                                newModel.Products,
                                newModel.UnitReceiptNoteDate,
                                0,
                                0,
                                newModel.UnitReceiptMutation,
                                viewModel.Id,
                                viewModel.Code,
                                viewModel.Date,
                                newModel.BankExpenditureNoteDPP,
                                newModel.BankExpenditureNotePPN,
                                viewModel.Mutation,
                                newModel.MemoNo,
                                newModel.MemoDate,
                                newModel.MemoDPP,
                                newModel.MemoPPN,
                                newModel.MemoMutation,
                                newModel.PaymentDuration,
                                newModel.InvoiceNo,
                                newModel.UnitReceiptMutation + (viewModel.Mutation * -1) + newModel.MemoMutation,
                                newModel.CurrencyCode,
                                newModel.DPPCurrency,
                                newModel.CurrencyRate,
                                newModel.VATAmount,
                                newModel.IncomeTaxAmount,
                                newModel.ExternalPurchaseOrderNo
                                );

                    EntityExtension.FlagForCreate(newCreditorAccount, IdentityService.Username, UserAgent);
                    DbSet.Add(newCreditorAccount);
                }
                else
                {
                    decimal previousPayment = 0;

                    var newModel = await DbContext.CreditorAccounts.FirstOrDefaultAsync(entity => entity.UnitReceiptNoteNo == model.UnitReceiptNoteNo && entity.BankExpenditureNoteNo != null);

                    if (newModel != null)
                    {
                        model.UnitReceiptNoteDate = newModel.UnitReceiptNoteDate;
                        model.MemoDate = newModel.MemoDate;
                        model.UnitReceiptNoteDPP = 0;
                        model.UnitReceiptNotePPN = 0;
                        previousPayment = newModel.BankExpenditureNoteMutation;
                    }

                    model.BankExpenditureNoteDate = viewModel.Date;
                    model.BankExpenditureNoteId = viewModel.Id;
                    model.BankExpenditureNoteMutation = viewModel.Mutation;
                    model.BankExpenditureNoteNo = viewModel.Code;
                    model.FinalBalance = model.UnitReceiptMutation + (model.BankExpenditureNoteMutation * -1) + model.MemoMutation + (previousPayment * -1);

                    UpdateModel(model.Id, model);
                }

                return await DbContext.SaveChangesAsync();
            }
        }

        public async Task<int> CreateFromListBankExpenditureNoteAsync(List<CreditorAccountBankExpenditureNotePostedViewModel> viewModels)
        {
            var creditUnitReceiptNotes = viewModels
                .GroupBy(x => new { x.UnitReceiptNoteNo, x.SupplierCode, x.MemoNo })
                .Select(x => new
                {
                    UnitReceiptNoteNo = x.Key.UnitReceiptNoteNo,
                    Mutation = x.Sum(item => item.Mutation),
                    SupplierCode = x.Key.SupplierCode,
                    MemoNo = x.Key.MemoNo,
                    BankExpenditureNoteNo = x.First().Code,
                    Id = x.First().Id,
                    Date = x.First().Date
                })
                .ToList();

            foreach (var item in creditUnitReceiptNotes)
            {
                CreditorAccountModel model = await DbSet.FirstOrDefaultAsync(x => x.SupplierCode == item.SupplierCode && x.UnitPaymentCorrectionNo == null && x.MemoNo == item.MemoNo && x.UnitReceiptNoteNo == item.UnitReceiptNoteNo && x.BankExpenditureNoteNo == null);

                if (model == null)
                {
                    CreditorAccountModel existingModel = await DbSet.FirstOrDefaultAsync(x => x.SupplierCode == item.SupplierCode && x.UnitPaymentCorrectionNo == null && x.MemoNo == item.MemoNo && x.UnitReceiptNoteNo == item.UnitReceiptNoteNo && x.BankExpenditureNoteNo != null);

                    if (item.BankExpenditureNoteNo != existingModel.BankExpenditureNoteNo)
                    {
                        var newCreditorAccount = new CreditorAccountModel(
                                    existingModel.SupplierName,
                                    existingModel.SupplierCode,
                                    existingModel.SupplierIsImport,
                                    existingModel.DivisionId,
                                    existingModel.DivisionCode,
                                    existingModel.DivisionName,
                                    existingModel.UnitId,
                                    existingModel.UnitCode,
                                    existingModel.UnitName,
                                    existingModel.UnitPaymentCorrectionId,
                                    existingModel.UnitPaymentCorrectionNo,
                                    existingModel.UnitPaymentCorrectionDPP,
                                    existingModel.UnitPaymentCorrectionPPN,
                                    existingModel.UnitPaymentCorrectionMutation,
                                    existingModel.UnitPaymentCorrectionDate.GetValueOrDefault(),
                                    existingModel.UnitReceiptNoteNo,
                                    existingModel.Products,
                                    existingModel.UnitReceiptNoteDate,
                                    0,
                                    0,
                                    existingModel.UnitReceiptMutation,
                                    item.Id,
                                    item.BankExpenditureNoteNo,
                                    item.Date,
                                    existingModel.BankExpenditureNoteDPP,
                                    existingModel.BankExpenditureNotePPN,
                                    item.Mutation,
                                    existingModel.MemoNo,
                                    existingModel.MemoDate,
                                    existingModel.MemoDPP,
                                    existingModel.MemoPPN,
                                    existingModel.MemoMutation,
                                    existingModel.PaymentDuration,
                                    existingModel.InvoiceNo,
                                    existingModel.UnitReceiptMutation + (item.Mutation * -1) + existingModel.MemoMutation,
                                    existingModel.CurrencyCode,
                                    existingModel.DPPCurrency,
                                    existingModel.CurrencyRate,
                                    0,
                                    0,
                                    existingModel.ExternalPurchaseOrderNo
                                    );

                        EntityExtension.FlagForCreate(newCreditorAccount, IdentityService.Username, UserAgent);
                        DbSet.Add(newCreditorAccount);
                    }
                    else
                    {
                        existingModel.BankExpenditureNoteMutation += item.Mutation;
                        existingModel.FinalBalance = existingModel.UnitReceiptMutation + (existingModel.BankExpenditureNoteMutation * -1) + existingModel.MemoMutation;

                        UpdateModel(existingModel.Id, existingModel);
                    }
                }
                else
                {
                    model.BankExpenditureNoteDate = item.Date;
                    model.BankExpenditureNoteId = item.Id;
                    model.BankExpenditureNoteMutation = item.Mutation;
                    model.BankExpenditureNoteNo = item.BankExpenditureNoteNo;
                    model.FinalBalance = model.UnitReceiptMutation + (model.BankExpenditureNoteMutation * -1) + model.MemoMutation;

                    UpdateModel(model.Id, model);
                }
            }

            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateFromBankExpenditureNoteAsync(CreditorAccountBankExpenditureNotePostedViewModel viewModel)
        {
            CreditorAccountModel data = await DbSet.FirstOrDefaultAsync(x => x.SupplierCode == viewModel.SupplierCode && x.BankExpenditureNoteNo == viewModel.Code && x.InvoiceNo == viewModel.InvoiceNo);

            if (data == null)
                throw new NotFoundException();

            data.BankExpenditureNoteNo = viewModel.Code;
            data.BankExpenditureNoteDate = viewModel.Date;
            data.BankExpenditureNoteMutation = viewModel.Mutation;
            data.FinalBalance = data.UnitReceiptMutation + (data.BankExpenditureNoteMutation * -1) + data.MemoMutation;


            UpdateModel(data.Id, data);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> CreateFromUnitReceiptNoteAsync(CreditorAccountUnitReceiptNotePostedViewModel viewModel)
        {
            CreditorAccountModel model = new CreditorAccountModel()
            {
                UnitReceiptNotePPN = viewModel.PPN,
                FinalBalance = viewModel.DPP + viewModel.PPN,
                InvoiceNo = viewModel.InvoiceNo,
                SupplierCode = viewModel.SupplierCode,
                SupplierName = viewModel.SupplierName,
                SupplierIsImport = viewModel.SupplierIsImport,
                UnitReceiptMutation = viewModel.DPP + viewModel.PPN,
                UnitReceiptNoteDate = viewModel.Date,
                UnitReceiptNoteDPP = viewModel.DPP,
                UnitReceiptNoteNo = viewModel.Code,
                CurrencyCode = viewModel.Currency,
                DPPCurrency = viewModel.DPPCurrency,
                CurrencyRate = viewModel.CurrencyRate,
                PaymentDuration = viewModel.PaymentDuration,
                Products = viewModel.Products,
                DivisionId = viewModel.DivisionId,
                DivisionCode = viewModel.DivisionCode,
                DivisionName = viewModel.DivisionName,
                UnitId = viewModel.UnitId,
                UnitCode = viewModel.UnitCode,
                UnitName = viewModel.UnitName,
                ExternalPurchaseOrderNo = viewModel.ExternalPurchaseOrderNo,
                VATAmount = viewModel.VATAmount,
                IncomeTaxAmount = viewModel.IncomeTaxAmount,
                IncomeTaxNo = viewModel.IncomeTaxNo
            };

            return await CreateAsync(model);
        }

        public async Task<int> DeleteFromUnitReceiptNoteAsync(int id)
        {
            CreditorAccountModel model = await DbSet.FirstOrDefaultAsync(x => x.Id == id);

            return await DeleteAsync(model.Id);
        }

        public async Task<int> DeleteFromBankExpenditureNoteAsync(int id)
        {
            CreditorAccountModel model = await DbSet.FirstOrDefaultAsync(x => x.Id == id);

            model.BankExpenditureNoteDate = null;
            model.BankExpenditureNoteDPP = 0;
            model.BankExpenditureNoteId = 0;
            model.BankExpenditureNoteMutation = 0;
            model.BankExpenditureNoteNo = null;
            model.BankExpenditureNotePPN = 0;
            model.FinalBalance = model.UnitReceiptMutation + model.BankExpenditureNoteMutation + model.MemoMutation;

            return await UpdateAsync(model.Id, model);
        }

        public async Task<int> DeleteFromBankExpenditureNoteListAsync(string code)
        {
            var models = await DbSet.Where(x => x.BankExpenditureNoteNo == code).ToListAsync();
            foreach (var model in models)
            {
                model.BankExpenditureNoteDate = null;
                model.BankExpenditureNoteDPP = 0;
                model.BankExpenditureNoteId = 0;
                model.BankExpenditureNoteMutation = 0;
                model.BankExpenditureNoteNo = null;
                model.BankExpenditureNotePPN = 0;
                model.FinalBalance = model.UnitReceiptMutation + model.BankExpenditureNoteMutation + model.MemoMutation;
                await UpdateAsync(model.Id, model);
            }
            return 1;
        }

        public async Task<int> UpdateFromUnitPaymentOrderAsync(CreditorAccountUnitPaymentOrderPostedViewModel viewModel)
        {
            if (viewModel.CreditorAccounts != null)
            {
                foreach (var item in viewModel.CreditorAccounts)
                {
                    var creditorAccount = await DbContext.CreditorAccounts.FirstOrDefaultAsync(x => x.SupplierCode == item.SupplierCode && x.UnitReceiptNoteNo == item.Code && x.ExternalPurchaseOrderNo == item.ExternalPurchaseOrderNo);

                    if (creditorAccount != null)
                    {
                        creditorAccount.InvoiceNo = viewModel.InvoiceNo;
                        creditorAccount.MemoNo = viewModel.MemoNo;
                        creditorAccount.MemoDate = viewModel.MemoDate;
                        creditorAccount.MemoDPP = item.MemoDPP;
                        //creditorAccount.MemoDPPCurrency = item.MemoDPPCurrency;
                        creditorAccount.MemoMutation = item.MemoMutation;
                        creditorAccount.MemoPPN = item.MemoPPN;
                        //creditorAccount.PaymentDuration = viewModel.PaymentDuration;
                    }

                    //creditorAccount.InvoiceNo = viewModel.InvoiceNo;
                    //creditorAccount.MemoNo = viewModel.MemoNo;
                    //creditorAccount.MemoDate = viewModel.MemoDate;
                    //creditorAccount.MemoDPP = item.MemoDPP;
                    ////creditorAccount.MemoDPPCurrency = item.MemoDPPCurrency;
                    //creditorAccount.MemoMutation = item.MemoMutation;
                    //creditorAccount.MemoPPN = item.MemoPPN;
                    ////creditorAccount.PaymentDuration = viewModel.PaymentDuration;
                }
                return await DbContext.SaveChangesAsync();
            }
            else
            {
                return 0;
            }

        }

        //private async Task<CreditorAccountUnitReceiptNotePostedViewModel> GetCreditorAccountFromUnitReceiptNote(string urnNo)
        //{
        //    var jsonSerializerSettings = new JsonSerializerSettings
        //    {
        //        MissingMemberHandling = MissingMemberHandling.Ignore
        //    };

        //    //var token = GetTokenAsync().Result;

        //    var categoryUri = APIEndpoint.Purchasing + $"unit-receipt-notes/by-user/correction-note/";
        //    var categoryResponse = _http.GetAsync(categoryUri, token).Result;

        //    var categoryResult = new BaseResponse<List<CategoryCOAResult>>()
        //    {
        //        data = new List<CategoryCOAResult>()
        //    };
        //    if (categoryResponse.IsSuccessStatusCode)
        //    {
        //        categoryResult = JsonConvert.DeserializeObject<BaseResponse<List<CategoryCOAResult>>>(categoryResponse.Content.ReadAsStringAsync().Result, jsonSerializerSettings);
        //    }

        //    _cacheManager.Set(MemoryCacheConstant.Categories, categoryResult.data);
        //}

        public async Task<int> CreateFromMemoAsync(CreditorAccountMemoPostedViewModel viewModel)
        {
            CreditorAccountModel model = await DbSet.FirstOrDefaultAsync(x => x.SupplierCode == viewModel.SupplierCode && x.InvoiceNo == viewModel.InvoiceNo);

            if (model == null)
                throw new NotFoundException();

            model.MemoDate = viewModel.Date;
            model.MemoDPP = viewModel.DPP;
            model.MemoPPN = viewModel.PPN;
            model.MemoNo = viewModel.Code;
            model.MemoMutation = viewModel.DPP + viewModel.PPN;
            model.FinalBalance = model.UnitReceiptMutation + model.BankExpenditureNoteMutation + model.MemoMutation;

            UpdateModel(model.Id, model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteFromUnitReceiptNoteAsync(CreditorAccountUnitReceiptNotePostedViewModel viewModel)
        {
            CreditorAccountModel model = await DbSet.FirstOrDefaultAsync(x => x.UnitReceiptNoteNo == viewModel.Code);

            if (model != null)
                return await DeleteAsync(model.Id);
            else
                return 0;
        }

        public async Task<int> CreateFromUnitPaymentCorrection(CreditorAccountUnitPaymentCorrectionPostedViewModel viewModel)
        {
            var model = await DbContext.CreditorAccounts.FirstOrDefaultAsync(entity => entity.UnitReceiptNoteNo == viewModel.UnitReceiptNoteNo);

            var result = 0;

            if (model != null)
            {
                var correction = new CreditorAccountModel(
                    model.SupplierName,
                    model.SupplierCode,
                    model.SupplierIsImport,
                    model.DivisionId,
                    model.DivisionCode,
                    model.DivisionName,
                    model.UnitId,
                    model.UnitCode,
                    model.UnitName,
                    viewModel.UnitPaymentCorrectionId,
                    viewModel.UnitPaymentCorrectionNo,
                    viewModel.UnitPaymentCorrectionDPP,
                    viewModel.UnitPaymentCorrectionPPN,
                    viewModel.UnitPaymentCorrectionMutation,
                    viewModel.UnitPaymentCorrectionDate,
                    model.UnitReceiptNoteNo,
                    model.Products,
                    model.UnitReceiptNoteDate,
                    model.UnitReceiptNoteDPP,
                    model.UnitReceiptNotePPN,
                    model.UnitReceiptMutation,
                    model.BankExpenditureNoteId,
                    model.BankExpenditureNoteNo,
                    model.BankExpenditureNoteDate,
                    model.BankExpenditureNoteDPP,
                    model.BankExpenditureNotePPN,
                    model.BankExpenditureNoteMutation,
                    model.MemoNo,
                    model.MemoDate,
                    model.MemoDPP,
                    model.MemoPPN,
                    model.MemoMutation,
                    model.PaymentDuration,
                    model.InvoiceNo,
                    model.FinalBalance,
                    model.CurrencyCode,
                    model.DPPCurrency,
                    model.CurrencyRate,
                    (viewModel.UnitPaymentCorrectionPPN / (model.CurrencyRate == 0 ? 1 : model.CurrencyRate)),
                    ((viewModel.UnitPaymentCorrectionDPP + viewModel.UnitPaymentCorrectionPPN) - viewModel.UnitPaymentCorrectionMutation),
                    model.ExternalPurchaseOrderNo
                    );

                EntityExtension.FlagForCreate(correction, IdentityService.Username, UserAgent);
                DbSet.Add(correction);
                result = await DbContext.SaveChangesAsync();
            }

            return result;
        }

        public int CreateFromPurchasingMemoTextile(CreditorAccountPurchasingMemoTextileFormDto form)
        {
            var models = DbContext.CreditorAccounts.Where(element => element.MemoNo == form.UnitPaymentOrderNo);
            foreach (var model in models)
            {
                model.SetPurchasingMemo(form.PurchasingMemoId, form.PurchasingMemoNo, form.PurchasingMemoAmount, form.PurchasingMemoDate);
                EntityExtension.FlagForUpdate(model, IdentityService.Username, UserAgent);
            }

            DbContext.CreditorAccounts.UpdateRange(models);
            return DbContext.SaveChanges();
        }

        public int DeleteFromPurchasingMemoTextile(CreditorAccountPurchasingMemoTextileFormDto form)
        {
            var models = DbContext.CreditorAccounts.Where(element => element.MemoNo == form.UnitPaymentOrderNo);
            foreach (var model in models)
            {
                model.RemovePurchasingMemo();
                EntityExtension.FlagForUpdate(model, IdentityService.Username, UserAgent);
            }

            DbContext.CreditorAccounts.UpdateRange(models);
            return DbContext.SaveChanges();
        }
    }
}
