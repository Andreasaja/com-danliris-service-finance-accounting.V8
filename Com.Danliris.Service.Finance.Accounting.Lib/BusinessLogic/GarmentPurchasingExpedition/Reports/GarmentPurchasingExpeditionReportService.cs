using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingExpedition;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingExpedition.Reports
{
    public class GarmentPurchasingExpeditionReportService : IGarmentPurchasingExpeditionReportService
    {
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;

        public GarmentPurchasingExpeditionReportService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        public MemoryStream GenerateExcel(int internalNoteId, int supplierId, GarmentPurchasingExpeditionPosition position, DateTimeOffset startDate, DateTimeOffset endDate, DateTimeOffset startDateAccounting, DateTimeOffset endDateAccounting)
        {
            var query = GetQuery(internalNoteId, supplierId, position, startDate, endDate, startDateAccounting, endDateAccounting);

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "No. NI", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. NI", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Supplier", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Jatuh Tempo", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nomor Invoice", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "DPP", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "PPN", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "PPh", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Total", DataType = typeof(double) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tipe Bayar", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Term Pembayaran", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tempo", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Posisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Pembelian Kirim Verifikasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Pembelian Kirim Accounting", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Keterangan", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Admin", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Verif Terima", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Verif Kirim", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Verifikator", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Terima Kasir", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Bukti Pengeluaran Bank", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Bukti Pengeluaran Bank", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Terima Pembelian", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Admin Pembelian", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Alasan", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Terima Accounting", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Admin Accounting", DataType = typeof(string) });

            if (query.Count() == 0)
            {
                dt.Rows.Add("", "", "", "", "", 0, 0, 0, 0, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            }
            else
            {
                var queryViewModel = query;
                var data = queryViewModel.OrderByDescending(s => s.LastModifiedUtc).ToList();
                foreach (var item in data)
                {
                    dt.Rows.Add(
                        item.InternalNoteNo,
                        item.InternalNoteDate.ToOffset(new TimeSpan(_identityService.TimezoneOffset, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.SupplierName,
                        item.InternalNoteDueDate.ToOffset(new TimeSpan(_identityService.TimezoneOffset, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.InvoicesNo,
                        item.DPP,
                        item.VAT,
                        item.IncomeTax,
                        item.TotalPaid,
                        item.PaymentType,
                        item.PaymentMethod,
                        item.PaymentDueDays.ToString(),
                        item.Position.ToDescriptionString(),
                        item.SendToVerificationDate?.ToOffset(new TimeSpan(_identityService.TimezoneOffset, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.SendToAccountingDate?.ToOffset(new TimeSpan(_identityService.TimezoneOffset, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.Remark,
                        item.SendToVerificationBy,
                        item.VerificationAcceptedDate?.ToOffset(new TimeSpan(_identityService.TimezoneOffset, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.VerificationSendDate?.ToOffset(new TimeSpan(_identityService.TimezoneOffset, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.VerificationAcceptedBy,
                        item.CashierAcceptedDate?.ToOffset(new TimeSpan(_identityService.TimezoneOffset, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.BankExpenditureNoteNo,
                        item.BankExpenditureNoteDate?.ToOffset(new TimeSpan(_identityService.TimezoneOffset, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.SendToPurchasingDate?.ToOffset(new TimeSpan(_identityService.TimezoneOffset, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.SendToPurchasingBy,
                        item.SendToPurchasingRemark,
                        item.AccountingAcceptedDate?.ToOffset(new TimeSpan(_identityService.TimezoneOffset, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.AccountingAcceptedBy
                        );
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Laporan Ekspedisi Garment") }, true);
        }

        public List<GarmentPurchasingExpeditionReportViewModel> GetReport(int internalNoteId, int supplierId, GarmentPurchasingExpeditionPosition position, DateTimeOffset startDate, DateTimeOffset endDate, DateTimeOffset startDateAccounting, DateTimeOffset endDateAccounting)
        {
            var query = GetQuery(internalNoteId, supplierId, position, startDate, endDate, startDateAccounting,  endDateAccounting);
            return query.ToList();
        }
        public List<GarmentPurchasingExpeditionReportViewModel> GetReportViewModel(int internalNoteId, int supplierId, GarmentPurchasingExpeditionPosition position, DateTimeOffset startDate, DateTimeOffset endDate, DateTimeOffset startDateAccounting, DateTimeOffset endDateAccounting)
        {
            var query = GetQuery(internalNoteId, supplierId, position, startDate, endDate,  startDateAccounting,  endDateAccounting);
            //var queryViewModel = query.Select(entity => new GarmentPurchasingExpeditionReportViewModel(entity));
            return query.ToList();
        }

        private IQueryable<GarmentPurchasingExpeditionReportViewModel> GetQuery(int internalNoteId, int supplierId, GarmentPurchasingExpeditionPosition position, DateTimeOffset startDate, DateTimeOffset endDate, DateTimeOffset startDateAccounting, DateTimeOffset endDateAccounting)
        {
            var result = _dbContext.GarmentPurchasingExpeditions.AsQueryable();

            if (position > 0)
                result = result.Where(entity => entity.Position == position);

            switch (position)
            {
                case GarmentPurchasingExpeditionPosition.Purchasing:
                    //result = result.Where(entity => entity.SendToPurchasingDate.GetValueOrDefault() >= startDate && entity.SendToPurchasingDate.GetValueOrDefault() <= endDate);
                    result = result.Where(entity => entity.SendToPurchasingDate.Value.AddHours(7) >= startDate && entity.SendToPurchasingDate.Value.AddHours(7) <= endDate);

                    break;
                case GarmentPurchasingExpeditionPosition.SendToVerification:
                    //result = result.Where(entity => entity.SendToVerificationDate.GetValueOrDefault() >= startDate && entity.SendToVerificationDate.GetValueOrDefault() <= endDate);
                    result = result.Where(entity => entity.SendToVerificationDate.Value.AddHours(7) >= startDate && entity.SendToVerificationDate.Value.AddHours(7) <= endDate);

                    break;
                case GarmentPurchasingExpeditionPosition.VerificationAccepted:
                    //result = result.Where(entity => entity.SendToCashierDate.GetValueOrDefault() >= startDate && entity.SendToCashierDate.GetValueOrDefault() <= endDate);
                    result = result.Where(entity => entity.SendToCashierDate.Value.AddHours(7) >= startDate && entity.SendToCashierDate.Value.AddHours(7) <= endDate);

                    break;
                case GarmentPurchasingExpeditionPosition.SendToCashier:
                    //result = result.Where(entity => entity.SendToCashierDate.GetValueOrDefault() >= startDate && entity.SendToCashierDate.GetValueOrDefault() <= endDate);
                    result = result.Where(entity => entity.SendToCashierDate.Value.AddHours(7) >= startDate && entity.SendToCashierDate.Value.AddHours(7) <= endDate);

                    break;
                case GarmentPurchasingExpeditionPosition.CashierAccepted:
                    //result = result.Where(entity => entity.CashierAcceptedDate.GetValueOrDefault() >= startDate && entity.CashierAcceptedDate.GetValueOrDefault() <= endDate);
                    result = result.Where(entity => entity.CashierAcceptedDate.Value.AddHours(7) >= startDate && entity.CashierAcceptedDate.Value.AddHours(7) <= endDate);

                    break;
                case GarmentPurchasingExpeditionPosition.AccountingAccepted:
                    //result = result.Where(entity => entity.AccountingAcceptedDate.GetValueOrDefault() >= startDate && entity.AccountingAcceptedDate.GetValueOrDefault() <= endDate);
                    result = result.Where(entity => entity.AccountingAcceptedDate.Value.AddHours(7) >= startDate && entity.AccountingAcceptedDate.Value.AddHours(7) <= endDate);

                    break;
                default:
                    //result = result.Where(entity => entity.SendToVerificationDate.GetValueOrDefault() >= startDate && entity.SendToVerificationDate.GetValueOrDefault() <= endDate);

                   // result = result.Where(entity => entity.SendToVerificationDate.GetValueOrDefault() >= startDate && entity.SendToVerificationDate.GetValueOrDefault() <= endDate);
                    result = result.Where(entity => (entity.SendToVerificationDate == null ? DateTimeOffset.MinValue : entity.SendToVerificationDate) >= startDate && (entity.SendToVerificationDate == null ? DateTimeOffset.MinValue : entity.SendToVerificationDate) <= endDate);

                    break;
            }

            if (internalNoteId > 0)
                result = result.Where(entity => entity.InternalNoteId == internalNoteId);

            if (supplierId > 0)
                result = result.Where(entity => entity.SupplierId == supplierId);

            if (position == GarmentPurchasingExpeditionPosition.Purchasing)
                result = result.Where(entity => !string.IsNullOrWhiteSpace(entity.SendToPurchasingRemark));

            if (internalNoteId > 0)
                result = result.Where(entity => entity.InternalNoteId == internalNoteId);

            //result = result.Where(entity => entity.SendToAccountingDate.Value.AddHours(7) >= startDateAccounting && entity.SendToAccountingDate.Value.AddHours(7) <= endDateAccounting);
            result = result.Where(entity => (entity.SendToAccountingDate == null? DateTimeOffset.MinValue : entity.SendToAccountingDate) >= startDateAccounting && (entity.SendToAccountingDate == null ? DateTimeOffset.MinValue : entity.SendToAccountingDate) <= endDateAccounting);


            var query = from a in result
                        join b in _dbContext.DPPVATBankExpenditureNoteItems on a.InternalNoteId equals b.InternalNoteId into l
                        from b in l.DefaultIfEmpty()
                        join c in _dbContext.DPPVATBankExpenditureNotes on b.DPPVATBankExpenditureNoteId equals c.Id into m
                        from c in m.DefaultIfEmpty()
                        select new GarmentPurchasingExpeditionReportViewModel
                        {
                            InternalNoteId = a.InternalNoteId,
                            InternalNoteNo = a.InternalNoteNo,
                            InternalNoteDate = a.InternalNoteDate,
                            InternalNoteDueDate = a.InternalNoteDueDate,
                            SupplierId = a.SupplierId,
                            SupplierName = a.SupplierName,
                            DPP = a.DPP,
                            VAT = a.VAT,
                            CorrectionAmount = a.CorrectionAmount,
                            IncomeTax = a.IncomeTax,
                            TotalPaid = a.TotalPaid,
                            CurrencyId = a.CurrencyId,
                            CurrencyCode = a.CurrencyCode,
                            PaymentType = a.PaymentType,
                            InvoicesNo = a.InvoicesNo,
                            PaymentMethod = a.PaymentMethod,
                            PaymentDueDays = a.PaymentDueDays,
                            Remark = a.Remark,
                            Position = a.Position,
                            SendToVerificationDate = a.SendToVerificationDate,
                            SendToVerificationBy = a.SendToVerificationBy,
                            VerificationAcceptedDate = a.VerificationAcceptedDate,
                            VerificationAcceptedBy = a.VerificationAcceptedBy,
                            SendToCashierDate = a.SendToCashierDate,
                            SendToCashierBy = a.SendToCashierBy,
                            CashierAcceptedDate = a.CashierAcceptedDate,
                            CashierAcceptedBy = a.CashierAcceptedBy,
                            SendToPurchasingDate = a.SendToPurchasingDate,
                            SendToPurchasingBy = a.SendToPurchasingBy,
                            SendToPurchasingRemark = a.SendToPurchasingRemark,
                            SendToAccountingDate = a.SendToAccountingDate,
                            SendToAccountingBy = a.SendToAccountingBy,
                            AccountingAcceptedDate = a.AccountingAcceptedDate,
                            AccountingAcceptedBy = a.AccountingAcceptedBy,
                            VerificationSendDate = 
                            (a.Position == GarmentPurchasingExpeditionPosition.SendToCashier || a.Position == GarmentPurchasingExpeditionPosition.CashierAccepted ||  a.Position == GarmentPurchasingExpeditionPosition.DispositionPayment? a.SendToCashierDate :
                            a.Position == GarmentPurchasingExpeditionPosition.SendToAccounting || a.Position == GarmentPurchasingExpeditionPosition.AccountingAccepted? a.SendToAccountingDate : a.SendToPurchasingDate
                            ),
                            BankExpenditureNoteNo = b == null? "-":( a.CashierAcceptedDate == null? "-": c.DocumentNo),
                            BankExpenditureNoteDate = a.CashierAcceptedDate != null ? c.Date : (DateTimeOffset?)null

                        };
        

            return query;
        }
    }
}
