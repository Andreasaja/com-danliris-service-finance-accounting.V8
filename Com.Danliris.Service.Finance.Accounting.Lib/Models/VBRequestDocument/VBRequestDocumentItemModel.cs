using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument
{
    public class VBRequestDocumentItemModel : StandardEntity
    {
        public VBRequestDocumentItemModel()
        {

        }

        public VBRequestDocumentItemModel(
            int vbRequestDocumentId,
            int unitId,
            string unitName,
            string unitCode,
            int divisionId,
            string divisionName,
            string divisionCode,
            bool isSelected,
            int vbDocumentLayoutOrder
            )
        {
            VBRequestDocumentId = vbRequestDocumentId;
            UnitId = unitId;
            UnitName = unitName;
            UnitCode = unitCode;
            DivisionId = divisionId;
            DivisionName = divisionName;
            DivisionCode = divisionCode;
            IsSelected = isSelected;

            VBDocumentLayoutOrder = vbDocumentLayoutOrder;
        }

        public VBRequestDocumentItemModel(
            int vbRequestDocumentId,
            int vbRequestDocumentEPODetailId,
            int unitId,
            string unitName,
            string unitCode,
            int divisionId,
            string divisionName,
            string divisionCode,
            int epoId,
            bool useIncomeTax,
            int incomeTaxId,
            string incomeTaxName,
            double incomeTaxRate,
            string incomeTaxBy,
            int productId,
            string productCode,
            string productName,
            double defaultQuantity,
            int defaultUOMId,
            string defaultUOMUnit,
            double dealQuantity,
            int dealUOMId,
            string dealUOMUnit,
            double conversion,
            double price,
            bool useVat,
            string vatId,
            string vatRate
            )
        {
            VBRequestDocumentId = vbRequestDocumentId;
            VBRequestDocumentEPODetailId = vbRequestDocumentEPODetailId;
            UnitId = unitId;
            UnitName = unitName;
            UnitCode = unitCode;
            DivisionId = divisionId;
            DivisionName = divisionName;
            DivisionCode = divisionCode;
            EPOId = epoId;
            UseIncomeTax = useIncomeTax;
            IncomeTaxId = incomeTaxId;
            IncomeTaxName = incomeTaxName;
            IncomeTaxRate = incomeTaxRate;
            IncomeTaxBy = incomeTaxBy;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DefaultQuantity = defaultQuantity;
            DefaultUOMId = defaultUOMId;
            DefaultUOMUnit = defaultUOMUnit;
            DealQuantity = dealQuantity;
            DealUOMId = dealUOMId;
            DealUOMUnit = dealUOMUnit;
            Conversion = conversion;
            Price = price;
            UseVat = useVat;
            VatId = vatId;
            VatRate = vatRate;

        }

        public int VBRequestDocumentId { get; private set; }
        public int VBRequestDocumentEPODetailId { get; private set; }
        public int UnitId { get; private set; }
        [MaxLength(256)]
        public string? UnitName { get; private set; }
        [MaxLength(64)]
        public string? UnitCode { get; private set; }
        public int DivisionId { get; private set; }
        [MaxLength(256)]
        public string? DivisionName { get; private set; }
        [MaxLength(64)]
        public string? DivisionCode { get; private set; }
        public int EPOId { get; private set; }
        public bool UseIncomeTax { get; private set; }
        public int IncomeTaxId { get; private set; }
        [MaxLength(64)]
        public string? IncomeTaxName { get; private set; }
        public double IncomeTaxRate { get; private set; }
        [MaxLength(64)]
        public string? IncomeTaxBy { get; private set; }
        public double AmountByUnit { get; private set; }
        public bool IsSelected { get; private set; }

        public int ProductId { get; private set; }
        [MaxLength(64)]
        public string? ProductCode { get; private set; }
        [MaxLength(512)]
        public string? ProductName { get; private set; }
        public double DefaultQuantity { get; private set; }
        public int DefaultUOMId { get; private set; }
        [MaxLength(64)]
        public string? DefaultUOMUnit { get; private set; }
        public double DealQuantity { get; private set; }
        public int DealUOMId { get; private set; }
        [MaxLength(64)]
        public string? DealUOMUnit { get; private set; }
        public double Conversion { get; private set; }
        public double Price { get; private set; }
        public bool UseVat { get; private set; }

        public string? VatId { get; private set; }
        public string? VatRate { get; private set; }

        public int VBDocumentLayoutOrder { get; private set; }

        public void SetUnit(int newUnitId, string newUnitName, string newUnitCode, string user, string userAgent)
        {
            if (newUnitId != UnitId)
            {
                UnitId = newUnitId;
                this.FlagForUpdate(user, userAgent);
            }

            if (newUnitName != UnitName)
            {
                UnitName = newUnitName;
                this.FlagForUpdate(user, userAgent);
            }

            if (newUnitCode != UnitCode)
            {
                UnitCode = newUnitCode;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetDivision(int newDivisionId, string newDivisionName, string newDivisionCode, string user, string userAgent)
        {
            if (newDivisionId != DivisionId)
            {
                DivisionId = newDivisionId;
                this.FlagForUpdate(user, userAgent);
            }

            if (newDivisionName != DivisionName)
            {
                DivisionName = newDivisionName;
                this.FlagForUpdate(user, userAgent);
            }

            if (newDivisionCode != DivisionCode)
            {
                DivisionCode = newDivisionCode;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetIsSelected(bool newFlagIsSelected, string user, string userAgent)
        {
            if (newFlagIsSelected != IsSelected)
            {
                IsSelected = newFlagIsSelected;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetVBDocumentLayoutOrder(int newVBDocumentLayoutOrder, string user, string userAgent)
        {
            if (newVBDocumentLayoutOrder != VBDocumentLayoutOrder)
            {
                VBDocumentLayoutOrder = newVBDocumentLayoutOrder;
                this.FlagForUpdate(user, userAgent);
            }
        }
    }
}
