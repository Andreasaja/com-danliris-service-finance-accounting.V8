using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument
{
    public class VBRequestDocumentModel : StandardEntity
    {
        public VBRequestDocumentModel()
        {

        }


        // New Ctor With Index
        public VBRequestDocumentModel(string documentNo, DateTimeOffset date, DateTimeOffset realizationEstimationDate, int suppliantUnitId, string suppliantUnitCode, string suppliantUnitName,
                int suppliantDivisionId, string suppliantDivisionCode, string suppliantDivisionName, int currencyId, string currencyCode, string currencySymbol, string currencyDescription,
                double currencyRate, string purpose, decimal amount, bool isPosted, bool isCompleted, VBType type, int index, bool isInklaring, string noBl, string noPo, string typePurchasing)
            : this(documentNo, date, realizationEstimationDate, suppliantUnitId, suppliantUnitCode, suppliantUnitName, suppliantDivisionId, suppliantDivisionCode, suppliantDivisionName,
                currencyId, currencyCode, currencySymbol, currencyDescription, currencyRate, purpose, amount, isPosted, isCompleted, type, isInklaring, noBl, noPo, typePurchasing)
        {
            Index = index;
        }

        public VBRequestDocumentModel(
            string documentNo,
            DateTimeOffset date,
            DateTimeOffset realizationEstimationDate,
            int suppliantUnitId,
            string suppliantUnitCode,
            string suppliantUnitName,
            int suppliantDivisionId,
            string suppliantDivisionCode,
            string suppliantDivisionName,
            int currencyId,
            string currencyCode,
            string currencySymbol,
            string currencyDescription,
            double currencyRate,
            string purpose,
            decimal amount,
            bool isPosted,
            bool isCompleted,
            VBType type,
            bool isInklaring,
            string noBl,
            string noPo,
            string typePurchasing
            )
        {
            DocumentNo = documentNo;
            Date = date;
            RealizationEstimationDate = realizationEstimationDate;
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
            CurrencySymbol = currencySymbol;
            CurrencyRate = currencyRate;
            CurrencyDescription = currencyDescription;
            Purpose = purpose;
            Amount = amount;
            IsPosted = isPosted;
            ApprovalStatus = ApprovalStatus.Draft;
            IsCompleted = isCompleted;
            Type = type;

            SuppliantUnitId = suppliantUnitId;
            SuppliantUnitCode = suppliantUnitCode;
            SuppliantUnitName = suppliantUnitName;
            SuppliantDivisionId = suppliantDivisionId;
            SuppliantDivisionCode = suppliantDivisionCode;
            SuppliantDivisionName = suppliantDivisionName;
            IsInklaring = isInklaring;
            NoBL = noBl;
            NoPO = noPo;
            TypePurchasing = typePurchasing;
        }

        [MaxLength(64)]
        public string? DocumentNo { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public DateTimeOffset RealizationEstimationDate { get; private set; }
        public int CurrencyId { get; private set; }
        [MaxLength(64)]
        public string? CurrencyCode { get; private set; }
        [MaxLength(64)]
        public string? CurrencySymbol { get; private set; }
        public double CurrencyRate { get; private set; }
        [MaxLength(256)]
        public string? CurrencyDescription { get; private set; }
        public string? Purpose { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }
        public bool IsPosted { get; private set; }
        public VBType Type { get; private set; }
        public int SuppliantUnitId { get; private set; }
        [MaxLength(64)]
        public string? SuppliantUnitCode { get; private set; }
        [MaxLength(256)]
        public string? SuppliantUnitName { get; private set; }
        public int SuppliantDivisionId { get; private set; }
        [MaxLength(64)]
        public string? SuppliantDivisionCode { get; private set; }
        [MaxLength(256)]
        public string? SuppliantDivisionName { get; private set; }

        public int Index { get; private set; }

        public bool IsRealized { get; private set; }


        public ApprovalStatus ApprovalStatus { get; private set; }
        public string? CancellationReason { get; private set; }
        public DateTimeOffset? ApprovalDate { get; private set; }
        [MaxLength(256)]
        public string? ApprovedBy { get; private set; }

        public DateTimeOffset? CancellationDate { get; private set; }
        [MaxLength(256)]
        public string? CanceledBy { get; private set; }

        public bool IsCompleted { get; private set; }
        public DateTimeOffset? CompletedDate { get; private set; }
        [MaxLength(256)]
        public string? CompletedBy { get; private set; }
        [DefaultValue(false)]
        public bool IsInklaring { get; private set; }
        public string? NoBL { get; private set; }
        public string? NoPO { get; private set; }
        public string? TypePurchasing { get; private set; }
        public int BankId { get; private set; }
        public string? BankCode { get; private set; }
        public string? BankBankCode { get; private set; }
        public string? BankAccountName { get; private set; }
        public string? BankAccountNumber { get; private set; }
        public string? BankBankName { get; private set; }
        public string? BankAccountCOA { get; private set; }
        public long BankCurrencyId { get; private set; }
        public string? BankCurrencyCode { get; private set; }
        public string? BankCurrencySymbol { get; private set; }
        public double BankCurrencyRate { get; private set; }
        public string? BankCurrencyDescription { get; private set; }
        public string? BankDocumentNo { get; private set; }


        public void SetDate(DateTimeOffset newDate, string user, string userAgent)
        {
            if (newDate != Date)
            {
                Date = newDate;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetRealizationEstimationDate(DateTimeOffset newRealizationEstimationDate, string user, string userAgent)
        {
            if (newRealizationEstimationDate != RealizationEstimationDate)
            {
                RealizationEstimationDate = newRealizationEstimationDate;
                this.FlagForUpdate(user, userAgent);
            }
        }

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

        public void SetAmount(decimal newAmount, string user, string userAgent)
        {
            if (newAmount != Amount)
            {
                Amount = newAmount;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetPurpose(string newPurpose, string user, string userAgent)
        {
            if (newPurpose != Purpose)
            {
                Purpose = newPurpose;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetIsApproved(string user, string userAgent)
        {
            ApprovalStatus = ApprovalStatus.Approved;
            ApprovalDate = DateTimeOffset.UtcNow;
            ApprovedBy = user;
            this.FlagForUpdate(user, userAgent);
        }

        public void SetApprovedDate(DateTimeOffset newApprovedDate, string user, string userAgent)
        {
            if (newApprovedDate != ApprovalDate)
            {
                ApprovalDate = newApprovedDate;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetApprovedBy(string newApprovedBy, string user, string userAgent)
        {
            if (newApprovedBy != ApprovedBy)
            {
                ApprovedBy = newApprovedBy;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetIsCompleted(bool newFlagIsCompleted, string user, string userAgent)
        {
            if (newFlagIsCompleted != IsCompleted)
            {
                IsCompleted = newFlagIsCompleted;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetCompletedDate(DateTimeOffset newCompletedDate, string user, string userAgent)
        {
            if (newCompletedDate != CompletedDate)
            {
                CompletedDate = newCompletedDate;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetCompletedBy(string newCompletedBy, string user, string userAgent)
        {
            if (newCompletedBy != CompletedBy)
            {
                CompletedBy = newCompletedBy;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void SetIsRealized(bool newFlagIsRealized, string user, string userAgent)
        {

            if (newFlagIsRealized != IsRealized)
            {
                IsRealized = newFlagIsRealized;
                this.FlagForUpdate(user, userAgent);
            }
        }

        public void UpdateFromForm(VBRequestDocumentWithPOFormDto form)
        {
            Date = form.Date.GetValueOrDefault();
            RealizationEstimationDate = form.RealizationEstimationDate.GetValueOrDefault();
            CurrencyId = form.Currency.Id.GetValueOrDefault();
            CurrencyCode = form.Currency.Code;
            CurrencySymbol = form.Currency.Symbol;
            CurrencyRate = form.Currency.Rate.GetValueOrDefault();
            CurrencyDescription = form.Currency.Description;
            Purpose = form.Purpose;
            Amount = form.Amount.GetValueOrDefault();
            SuppliantUnitId = form.SuppliantUnit.Id.GetValueOrDefault();
            SuppliantUnitCode = form.SuppliantUnit.Code;
            SuppliantUnitName = form.SuppliantUnit.Name;
            SuppliantDivisionId = form.SuppliantUnit.Division.Id.GetValueOrDefault();
            SuppliantDivisionCode = form.SuppliantUnit.Division.Code;
            SuppliantDivisionName = form.SuppliantUnit.Division.Name;
            TypePurchasing = form.TypePurchasing;
        }

        public void SetCancellation(string reason, string username, string userAgent)
        {
            ApprovalStatus = ApprovalStatus.Cancelled;
            CancellationReason = reason;
            CanceledBy = username;
            CancellationDate = DateTimeOffset.UtcNow;
            this.FlagForUpdate(username, userAgent);
        }

        public void SetInklaring(string noBl, string noPo)
        {
            NoBL = noBl;
            NoPO = noPo;
        }

        public void SetBank(AccountBankViewModel bankViewModel, string username, string userAgent)
        {
            if (bankViewModel != null)
            {
                BankId = bankViewModel.Id;
                BankCode = bankViewModel.Code;
                BankBankCode = bankViewModel.BankCode;
                BankAccountName = bankViewModel.AccountName;
                BankAccountNumber = bankViewModel.AccountNumber;
                BankBankName = bankViewModel.BankName;
                BankAccountCOA = bankViewModel.AccountCOA;
                if (bankViewModel.Currency != null)
                {
                    BankCurrencyId = bankViewModel.Currency.Id;
                    BankCurrencyCode = bankViewModel.Currency.Code;
                    BankCurrencySymbol = bankViewModel.Currency.Symbol;
                    BankCurrencyRate = bankViewModel.Currency.Rate;
                    BankCurrencyDescription = bankViewModel.Currency.Description;
                }
            }
        }
        public void SetBankDocumentNo(string bankDocumentNo, string username, string userAgent)
        {
            BankDocumentNo = bankDocumentNo;
            this.FlagForUpdate(username, userAgent);
        }
    }
}
