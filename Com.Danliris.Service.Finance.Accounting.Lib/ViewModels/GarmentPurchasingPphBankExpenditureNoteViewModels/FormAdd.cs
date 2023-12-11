using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels
{
    public class FormAdd
    {
        public string INNo { get; set; }
        public int INId { get; set; }
        public string Remark { get; set; }
        public DateTimeOffset? INDate { get; set; }
        public DateTimeOffset? INDueDate { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyRate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public List<Item> Items { get; set; }
        public GarmentPurchasingInvoiceInfoDto GarmentInvoice { get; set; }
        //public List<Detail> DataView { get; set; }
        public bool IsCreatedVB { get; set; }
        public int Position { get; set; }
        public string UId { get; set; }
        public double TotalIncomeTaxNI { get; set; }
        //public bool Active { get; set; }
        //public DateTimeOffset CreatedUtc { get; set; }
        //public string CreatedBy { get; set; }
        //public string CreatedAgent { get; set; }
        //public DateTimeOffset LastModifiedUtc { get; set; }
        //public string LastModifiedBy { get; set; }
        //public string LastModifiedAgent { get; set; }
        //public bool IsDeleted { get; set; }
        //public DateTimeOffset DeletedUtc { get; set; }
        //public string DeletedBy { get; set; }
        //public string DeletedAgent { get; set; }
        public int Id { get; set; }
    }

    public class Item
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public double? TotalAmount { get; set; }
        public List<Detail> Details { get; set; }
        public GarmentPurchasingInvoiceInfoDto GarmentInvoice { get; set; }

        //public int GarmentINId { get; set; }
        //public bool Active { get; set; }
        //public DateTimeOffset CreatedUtc { get; set; }
        //public string CreatedBy { get; set; }
        //public string CreatedAgent { get; set; }
        //public DateTimeOffset LastModifiedUtc { get; set; }
        //public string LastModifiedBy { get; set; }
        //public string LastModifiedAgent { get; set; }
        //public bool IsDeleted { get; set; }
        //public DateTimeOffset DeletedUtc { get; set; }
        //public string DeletedBy { get; set; }
        //public string DeletedAgent { get; set; }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }
        public double TotalIncomeTax { get; set; }
    }

    public class Detail
    {
        public int DOId { get; set; }
        public string DONo { get; set; }
        public int EPOId { get; set; }
        public string EPONo { get; set; }
        public string POSerialNumber { get; set; }
        public string RONo { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentType { get; set; }
        public int PaymentDueDays { get; set; }
        public DateTimeOffset PaymentDueDate { get; set; }
        public DateTimeOffset DODate { get; set; }
        public int InvoiceDetailId { get; set; }
        public string ProductCode { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public int Quantity { get; set; }
        public string UnitId { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public int UOMId { get; set; }
        public string UOMUnit { get; set; }
        public decimal PricePerDealUnit { get; set; }
        public decimal PriceTotal { get; set; }
        public int InvoiceId { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public double InvoiceTotalAmount { get; set; }
        public DeliveryOrderInfo GarmentDeliveryOrder { get; set; }

        //public int? GarmentItemINId { get; set; }
        //public bool Active { get; set; }
        //public DateTimeOffset? CreatedUtc { get; set; }
        //public string CreatedBy { get; set; }
        //public string CreatedAgent { get; set; }
        //public DateTimeOffset? LastModifiedUtc { get; set; }
        //public string LastModifiedBy { get; set; }
        //public string LastModifiedAgent { get; set; }
        //public bool IsDeleted { get; set; }
        //public DateTimeOffset? DeletedUtc { get; set; }
        //public string DeletedBy { get; set; }
        //public string DeletedAgent { get; set; }
        public int? Id { get; set; }
    }
    public class DeliveryOrderInfo
    {
        public int Id { get; set; }
        public long CustomsId { get; set; }
        public string DONo { get; set; }
        public DateTimeOffset DODate { get; set; }
        public DateTimeOffset ArrivalDate { get; set; }

        /* Supplier */
        public long SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }

        public string ShipmentType { get; set; }
        public string ShipmentNo { get; set; }

        public string Remark { get; set; }
        public bool IsClosed { get; set; }
        public bool IsCustoms { get; set; }
        public bool IsInvoice { get; set; }
        public string InternNo { get; set; }
        public string BillNo { get; set; }
        public string PaymentBill { get; set; }
        public double TotalAmount { get; set; }

        public bool? IsCorrection { get; set; }

        public bool? UseVat { get; set; }
        public bool? UseIncomeTax { get; set; }

        public int? IncomeTaxId { get; set; }
        public string IncomeTaxName { get; set; }
        public double? IncomeTaxRate { get; set; }

        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public long? DOCurrencyId { get; set; }
        public string DOCurrencyCode { get; set; }
        public double? DOCurrencyRate { get; set; }
    }
}
