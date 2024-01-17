using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument
{
    public class VBRealizationDocumentModel : StandardEntity
    {
        public VBRealizationDocumentModel()
        {

        }

        public VBRealizationDocumentModel(VBRealizationDocumentNonPOViewModel viewModel)
        {
            Type = VBType.NonPO;
            Index = viewModel.Index;
            DocumentNo = viewModel.DocumentNo;
            Date = viewModel.Date.GetValueOrDefault();
            VBNonPoType = viewModel.VBNonPOType;
            Amount = viewModel.Amount;
            BLAWBNumber = viewModel.BLAWBNumber;
            ContractPONumber = viewModel.ContractPONumber;
            IsInklaring = viewModel.IsInklaring;
            if (viewModel.VBDocument != null && viewModel.VBDocument.Id > 0)
            {
                VBRequestDocumentId = viewModel.VBDocument.Id;
                VBRequestDocumentNo = viewModel.VBDocument.DocumentNo;
                VBRequestDocumentDate = viewModel.VBDocument.Date;
                VBRequestDocumentRealizationEstimationDate = viewModel.VBDocument.RealizationEstimationDate;
                VBRequestDocumentCreatedBy = viewModel.VBDocument.CreatedBy;
                VBRequestDocumentAmount = viewModel.VBDocument.Amount.GetValueOrDefault();
                VBRequestDocumentPurpose = viewModel.VBDocument.Purpose;
                IsInklaring = viewModel.VBDocument.IsInklaring;
            }

            if (viewModel.Unit != null)
            {
                SuppliantUnitCode = viewModel.Unit.Code;
                SuppliantUnitId = viewModel.Unit.Id;
                SuppliantUnitName = viewModel.Unit.Name;

                if (viewModel.Unit.Division != null)
                {
                    SuppliantDivisionCode = viewModel.Unit.Division.Code;
                    SuppliantDivisionId = viewModel.Unit.Division.Id;
                    SuppliantDivisionName = viewModel.Unit.Division.Name;
                }
            }

            if (viewModel.Currency != null)
            {
                CurrencyCode = viewModel.Currency.Code;
                CurrencyDescription = viewModel.Currency.Description;
                CurrencyId = viewModel.Currency.Id;
                CurrencyRate = viewModel.Currency.Rate;
                CurrencySymbol = viewModel.Currency.Symbol;
            }
            DocumentType = viewModel.DocumentType;
            Position = VBRealizationPosition.Purchasing;
            Remark = viewModel.Remark;
        }

        public void UpdateVerified(VBRealizationPosition position, string reason, string username, string userAgent)
        {
            Position = position;
            VerificationDate = DateTimeOffset.Now;
            VerifiedBy = username;
            IsVerified = true;
            NotVerifiedReason = reason;
            this.FlagForUpdate(username, userAgent);
        }

        public VBRealizationDocumentModel(CurrencyDto currency, DateTimeOffset? date, UnitDto suppliantUnit, Tuple<string, int> documentNo, decimal amount, string remark)
        {
            CurrencyCode = currency.Code;
            CurrencyDescription = currency.Description;
            CurrencyId = currency.Id.GetValueOrDefault();
            CurrencyRate = currency.Rate.GetValueOrDefault();
            CurrencySymbol = currency.Symbol;
            Date = date.GetValueOrDefault();
            SuppliantDivisionCode = suppliantUnit.Division.Code;
            SuppliantDivisionId = suppliantUnit.Division.Id.GetValueOrDefault();
            SuppliantDivisionName = suppliantUnit.Division.Name;
            SuppliantUnitCode = suppliantUnit.Code;
            SuppliantUnitId = suppliantUnit.Id.GetValueOrDefault();
            SuppliantUnitName = suppliantUnit.Name;
            Type = VBType.WithPO;
            DocumentType = RealizationDocumentType.NonVB;
            DocumentNo = documentNo.Item1;
            Index = documentNo.Item2;
            Position = VBRealizationPosition.Purchasing;
            Amount = amount;
            Remark = remark;
        }

        public VBRealizationDocumentModel(DateTimeOffset? date, VBRequestDocumentModel vbRequest, Tuple<string, int> documentNo, decimal amount, string remark)
        {
            Date = date.GetValueOrDefault();
            Type = VBType.WithPO;
            DocumentType = RealizationDocumentType.WithVB;
            Index = documentNo.Item2;
            DocumentNo = documentNo.Item1;

            Amount = amount;
            CurrencyCode = vbRequest.CurrencyCode;
            CurrencyDescription = vbRequest.CurrencyDescription;
            CurrencyId = vbRequest.CurrencyId;
            CurrencyRate = vbRequest.CurrencyRate;
            CurrencySymbol = vbRequest.CurrencySymbol;
            SuppliantDivisionCode = vbRequest.SuppliantDivisionCode;
            SuppliantDivisionId = vbRequest.SuppliantDivisionId;
            SuppliantDivisionName = vbRequest.SuppliantDivisionName;
            SuppliantUnitCode = vbRequest.SuppliantUnitCode;
            SuppliantUnitId = vbRequest.SuppliantUnitId;
            SuppliantUnitName = vbRequest.SuppliantUnitName;

            VBRequestDocumentAmount = vbRequest.Amount;
            VBRequestDocumentCreatedBy = vbRequest.CreatedBy;
            VBRequestDocumentDate = vbRequest.Date;
            VBRequestDocumentId = vbRequest.Id;
            VBRequestDocumentNo = vbRequest.DocumentNo;
            VBRequestDocumentRealizationEstimationDate = vbRequest.RealizationEstimationDate;
            VBRequestDocumentPurpose = vbRequest.Purpose;

            Position = VBRealizationPosition.Purchasing;
            Remark = remark;
        }

        

        public VBRealizationPosition Position { get; private set; }
        public string? Remark { get; private set; }
        public VBType Type { get; private set; }
        public RealizationDocumentType DocumentType { get; private set; }
        public int Index { get; private set; }
        [MaxLength(64)]
        public string? DocumentNo { get; private set; }
        public DateTimeOffset Date { get; private set; }
        [MaxLength(32)]
        public string? VBNonPoType { get; private set; }

        public void SetIsCompleted(DateTimeOffset? completedDate, string username, string userAgent, string referenceNo)
        {
            IsCompleted = true;
            CompletedDate = completedDate;
            CompletedBy = username;
            ReferenceNo = referenceNo;
            this.FlagForUpdate(username, userAgent);
        }

        public int VBRequestDocumentId { get; private set; }
        [MaxLength(64)]
        public string? VBRequestDocumentNo { get; private set; }
        public DateTimeOffset? VBRequestDocumentDate { get; private set; }
        public DateTimeOffset? VBRequestDocumentRealizationEstimationDate { get; private set; }
        [MaxLength(256)]
        public string? VBRequestDocumentCreatedBy { get; private set; }
        public string? VBRequestDocumentPurpose { get; private set; }
        public int SuppliantUnitId { get; private set; }
        [MaxLength(64)]
        public string? SuppliantUnitCode { get; private set; }
        [MaxLength(64)]
        public string? SuppliantUnitName { get; private set; }
        public int SuppliantDivisionId { get; private set; }
        [MaxLength(64)]
        public string? SuppliantDivisionCode { get; private set; }
        [MaxLength(64)]
        public string? SuppliantDivisionName { get; private set; }
        public int CurrencyId { get; private set; }
        [MaxLength(64)]
        public string? CurrencyCode { get; private set; }
        [MaxLength(64)]
        public string? CurrencySymbol { get; private set; }
        public double CurrencyRate { get; private set; }
        [MaxLength(256)]
        public string? CurrencyDescription { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal VBRequestDocumentAmount { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }

        public bool IsVerified { get; private set; }
        public string? NotVerifiedReason { get; private set; }
        public DateTimeOffset? VerificationDate { get; private set; }
        [MaxLength(256)]
        public string? VerifiedBy { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTimeOffset? CompletedDate { get; private set; }
        [MaxLength(512)]
        public string? CompletedBy { get; private set; }
        [MaxLength(128)]
        public string? ReferenceNo { get; private set; }
        [MaxLength(256)]
        public string? BLAWBNumber { get; private set; }
        [MaxLength(256)]
        public string? ContractPONumber { get; private set; }
        public bool IsInklaring { get; private set; }
        public void SetCurrency(int newCurrencyId, string newCurrencyCode, string newCurrencySymbol, double newCurrencyRate, string newCurrencyDescription, string user, string userAgent)
        {
            if (newCurrencyId != CurrencyId)
            {
                CurrencyId = newCurrencyId;
                this.FlagForUpdate(user, userAgent);
            }

            if (newCurrencyCode != CurrencyCode)
            {
                CurrencyCode = newCurrencyCode;
                this.FlagForUpdate(user, userAgent);
            }

            if (newCurrencySymbol != CurrencySymbol)
            {
                CurrencySymbol = newCurrencySymbol;
                this.FlagForUpdate(user, userAgent);
            }

            if (newCurrencyRate != CurrencyRate)
            {
                CurrencyRate = newCurrencyRate;
                this.FlagForUpdate(user, userAgent);
            }

            if (newCurrencyDescription != CurrencyDescription)
            {
                CurrencyDescription = newCurrencyDescription;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetUnit(int newUnitId, string newUnitCode, string newUnitName, string user, string userAgent)
        {
            if (newUnitId != SuppliantUnitId)
            {
                SuppliantUnitId = newUnitId;
                this.FlagForUpdate(user, userAgent);
            }

            if (newUnitCode != SuppliantUnitCode)
            {
                SuppliantUnitCode = newUnitCode;
                this.FlagForUpdate(user, userAgent);
            }

            if (newUnitName != SuppliantUnitName)
            {
                SuppliantUnitName = newUnitName;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetDivision(int newDivisionId, string newDivisionCode, string newDivisionName, string user, string userAgent)
        {
            if (newDivisionId != SuppliantDivisionId)
            {
                SuppliantDivisionId = newDivisionId;
                this.FlagForUpdate(user, userAgent);
            }

            if (newDivisionCode != SuppliantDivisionCode)
            {
                SuppliantDivisionCode = newDivisionCode;
                this.FlagForUpdate(user, userAgent);
            }

            if (newDivisionName != SuppliantDivisionName)
            {
                SuppliantDivisionName = newDivisionName;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetVBRequest(int newVBRequestId, string newVBRequestNo, DateTimeOffset? newVBRequestDate, DateTimeOffset? newVBRequestVBRequestDocumentRealizationEstimationDate,
            string newVBRequestCreatedBy, decimal newVBRequestAmount, string newPurpose, string user, string userAgent)
        {
            if (newVBRequestId != VBRequestDocumentId)
            {
                VBRequestDocumentId = newVBRequestId;
                this.FlagForUpdate(user, userAgent);
            }

            if (newVBRequestNo != VBRequestDocumentNo)
            {
                VBRequestDocumentNo = newVBRequestNo;
                this.FlagForUpdate(user, userAgent);
            }

            if (newVBRequestDate != VBRequestDocumentDate)
            {
                VBRequestDocumentDate = newVBRequestDate;
                this.FlagForUpdate(user, userAgent);
            }

            if (newVBRequestVBRequestDocumentRealizationEstimationDate != VBRequestDocumentRealizationEstimationDate)
            {
                VBRequestDocumentRealizationEstimationDate = newVBRequestVBRequestDocumentRealizationEstimationDate;
                this.FlagForUpdate(user, userAgent);
            }

            if (newVBRequestCreatedBy != VBRequestDocumentCreatedBy)
            {
                VBRequestDocumentCreatedBy = newVBRequestCreatedBy;
                this.FlagForUpdate(user, userAgent);
            }

            if (newVBRequestAmount != VBRequestDocumentAmount)
            {
                VBRequestDocumentAmount = newVBRequestAmount;
                this.FlagForUpdate(user, userAgent);
            }

            if(newPurpose != VBRequestDocumentPurpose)
            {
                VBRequestDocumentPurpose = newPurpose;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void Update(VBRequestDocumentModel vbRequest)
        {
            VBRequestDocumentAmount = vbRequest.Amount;
            VBRequestDocumentCreatedBy = vbRequest.CreatedBy;
            VBRequestDocumentDate = vbRequest.Date;
            VBRequestDocumentId = vbRequest.Id;
            VBRequestDocumentNo = vbRequest.DocumentNo;
            VBRequestDocumentRealizationEstimationDate = vbRequest.RealizationEstimationDate;
            VBRequestDocumentPurpose = vbRequest.Purpose;
        }

        public void Update(FormDto form)
        {
            Date = form.Date.GetValueOrDefault();
            //SuppliantDivisionCode = form.SuppliantUnit.Division.Code;
            //SuppliantDivisionId = form.SuppliantUnit.Division.Id.GetValueOrDefault();
            //SuppliantDivisionName = form.

            //if (form.VBRequestDocument!= null && form.VBRequestDocument.Id.GetValueOrDefault() > 0)
            //{

            //}
        }

        public void SetAmount(decimal newAmount, string user, string userAgent)
        {
            if(newAmount != Amount)
            {
                Amount = newAmount;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void UpdatePosition(VBRealizationPosition position, string user, string userAgent)
        {
            Position = position;
            this.FlagForUpdate(user, userAgent);
        }

        public void SetRemark(string remark)
        {
            Remark = remark;
        }
    }
}
