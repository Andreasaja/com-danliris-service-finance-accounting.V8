using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument
{
    public class VBRealizationDocumentExpenditureItemModel : StandardEntity
    {

        public VBRealizationDocumentExpenditureItemModel()
        {

        }

        public VBRealizationDocumentExpenditureItemModel(int vbRealizationDocumentId, VBRealizationDocumentNonPOExpenditureItemViewModel viewModel)
        {
            Date = viewModel.DateDetail.GetValueOrDefault();
            Remark = viewModel.Remark;
            Amount = viewModel.Amount;
            UseVat = viewModel.IsGetPPn;
            UseIncomeTax = viewModel.IsGetPPh;
            BLAWBNumber = viewModel.BLAWBNumber;
            PPnAmount = viewModel.PPnAmount;
            PPhAmount = viewModel.PPhAmount;
            if(viewModel.IncomeTax != null)
            {
                IncomeTaxId = viewModel.IncomeTax.Id;
                IncomeTaxName = viewModel.IncomeTax.Name;
                IncomeTaxRate = viewModel.IncomeTax.Rate.GetValueOrDefault();
            }
            if (viewModel.VatTax != null)
            {
                VatId = viewModel.VatTax.Id;
                VatRate = viewModel.VatTax.Rate;
            }
            IncomeTaxBy = viewModel.IncomeTaxBy;
            VBRealizationDocumentId = vbRealizationDocumentId;

        }

        public VBRealizationDocumentExpenditureItemModel(int headerId, FormItemDto element)
        {
            VBRealizationDocumentId = headerId;
            UnitPaymentOrderId = element.UnitPaymentOrder.Id.GetValueOrDefault();
            UnitPaymentOrderNo = element.UnitPaymentOrder.No;
            Amount = (decimal)element.UnitPaymentOrder.Amount.GetValueOrDefault();
            Date = element.UnitPaymentOrder.Date.GetValueOrDefault();
            UseVat = element.UnitPaymentOrder.UseVat.GetValueOrDefault();
            VatId = element.UnitPaymentOrder.VatTax.Id;
            VatRate = element.UnitPaymentOrder.VatTax.Rate;
            UseIncomeTax = element.UnitPaymentOrder.UseIncomeTax.GetValueOrDefault();
            IncomeTaxId = element.UnitPaymentOrder.IncomeTax.Id.GetValueOrDefault();
            IncomeTaxBy = element.UnitPaymentOrder.IncomeTaxBy;
            IncomeTaxName = element.UnitPaymentOrder.IncomeTax.Name;
            IncomeTaxRate = element.UnitPaymentOrder.IncomeTax.Rate.GetValueOrDefault();
            SupplierId = element.UnitPaymentOrder.Supplier.Id.GetValueOrDefault();
            SupplierCode = element.UnitPaymentOrder.Supplier.Code;
            SupplierName = element.UnitPaymentOrder.Supplier.Name;
            DivisionId = element.UnitPaymentOrder.Division.Id.GetValueOrDefault();
            DivisionCode = element.UnitPaymentOrder.Division.Code;
            DivisionName = element.UnitPaymentOrder.Division.Name;
        }

        public DateTimeOffset Date { get; private set; }
        public string? Remark { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }
        public bool UseVat { get; private set; }
        public string? VatId { get; private set; }
        public string? VatRate { get; private set; }
        public bool UseIncomeTax { get; private set; }
        public int IncomeTaxId { get; private set; }
        [MaxLength(64)]
        public string? IncomeTaxName { get; private set; }
        public double IncomeTaxRate { get; private set; }
        public int SupplierId { get; private set; }
        [MaxLength(64)]
        public string? SupplierCode { get; private set; }
        [MaxLength(512)]
        public string? SupplierName { get; private set; }
        public int DivisionId { get; private set; }
        [MaxLength(64)]
        public string? DivisionCode { get; private set; }
        [MaxLength(512)]
        public string? DivisionName { get; private set; }
        [MaxLength(64)]
        public string? IncomeTaxBy { get; private set; }

        public int VBRealizationDocumentId { get; private set; }
        public int UnitPaymentOrderId { get; private set; }
        [MaxLength(64)]
        public string? UnitPaymentOrderNo { get; private set; }
        [MaxLength(256)]
        public string? BLAWBNumber { get; private set; }
        public decimal PPnAmount { get; private set; }
        public decimal PPhAmount { get; private set; }

        public void SetDate(DateTimeOffset newDate, string user, string userAgent)
        {
            if(newDate != Date)
            {
                Date = newDate;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetRemark(string newRemark, string user, string userAgent)
        {
            if (newRemark != Remark)
            {
                Remark = newRemark;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetAmount(decimal newAmount, string user, string userAgent)
        {
            if (newAmount != Amount)
            {
                Amount = newAmount;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetUseVat(bool newUseVat, string user, string userAgent)
        {
            if (newUseVat != UseVat)
            {
                UseVat = newUseVat;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetVatTax(string newVatId, string newVatRate, string user, string userAgent)
        {
            if (newVatId != VatId)
            {
                VatId = newVatId;
                this.FlagForUpdate(user, userAgent);
            }

            if (newVatRate != VatRate)
            {
                VatRate = newVatRate;
                this.FlagForUpdate(user, userAgent);
            }

        }

        public void SetUseIncomeTax(bool newUseIncomeTax, string user, string userAgent)
        {
            if (newUseIncomeTax != UseIncomeTax)
            {
                UseIncomeTax = newUseIncomeTax;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetIncomeTax(int newIncomeTaxId, double newIncomeTaxRate, string newIncomeTaxName, string user, string userAgent)
        {
            if (newIncomeTaxId != IncomeTaxId)
            {
                IncomeTaxId = newIncomeTaxId;
                this.FlagForUpdate(user, userAgent);
            }

            if (newIncomeTaxRate != IncomeTaxRate)
            {
                IncomeTaxRate = newIncomeTaxRate;
                this.FlagForUpdate(user, userAgent);
            }

            if (newIncomeTaxName != IncomeTaxName)
            {
                IncomeTaxName = newIncomeTaxName;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetIncomeTaxBy(string newIncomeTaxBy, string user, string userAgent)
        {
            if (newIncomeTaxBy != IncomeTaxBy)
            {
                IncomeTaxBy = newIncomeTaxBy;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetBLAWBNumber(string newBLAWBNumber, string user, string userAgent)
        {
            if (newBLAWBNumber != BLAWBNumber)
            {
                BLAWBNumber = newBLAWBNumber;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetPPnAmount(decimal newPPnAmount, string user, string userAgent)
        {
            if (newPPnAmount != PPnAmount)
            {
                PPnAmount = newPPnAmount;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetPPhAmount(decimal newPPhAmount, string user, string userAgent)
        {
            if (newPPhAmount != PPhAmount)
            {
                PPhAmount = newPPhAmount;
                this.FlagForUpdate(user, userAgent);
            }
        }
    }
}
