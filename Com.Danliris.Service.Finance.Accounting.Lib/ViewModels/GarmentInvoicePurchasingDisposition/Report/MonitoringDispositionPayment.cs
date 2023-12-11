using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition.Report
{
	public class MonitoringDispositionPayment
	{
		public string InvoiceNo { get; set; }
		public DateTimeOffset InvoiceDate { get; set; }

		public string DispositionNo { get; set; }
		public DateTimeOffset DispositionDate { get; set; }
		public DateTimeOffset DispositionDueDate { get; set; }
		public string BankName { get; set; }
		public string CurrencySymbol { get; set; }
		public string SupplierName { get; set; }
		public string ProformaNo { get; set; }
		public string Category { get; set; }
		public double VatAmount { get; set; }
		public double TotalAmount { get; set; }
		public double TotalPaidToSupplier { get; set; }
		public double TotalDifference { get; set; }
		public double Paid { get; set; }
		public double TotalPaid { get; set; }
		public string PaymentType { get; set; }

	}
}
