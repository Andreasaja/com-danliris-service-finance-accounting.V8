using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingMemoTextile
{
    public class PurchasingMemoTextileModel : StandardEntity
    {
        public PurchasingMemoTextileModel()
        {

        }

        public PurchasingMemoTextileModel(int memoDetailId, string memoDetailDocumentNo, DateTimeOffset memoDetailDate, int memoDetailCurrencyId, string memoDetailCurrencyCode, double memoDetailCurrencyRate, int accountingBookId, string accountingBookType, string remark, string accountingBookCode)
        {
            MemoDetailId = memoDetailId;
            MemoDetailDocumentNo = memoDetailDocumentNo;
            MemoDetailDate = memoDetailDate;
            MemoDetailCurrencyId = memoDetailCurrencyId;
            MemoDetailCurrencyCode = memoDetailCurrencyCode;
            MemoDetailCurrencyRate = memoDetailCurrencyRate;
            AccountingBookId = accountingBookId;
            AccountingBookType = accountingBookType;
            Remark = remark;
            AccountingBookCode = accountingBookCode;
        }

        public int MemoDetailId { get; private set; }
        [MaxLength(32)]
        public string MemoDetailDocumentNo { get; private set; }
        public DateTimeOffset MemoDetailDate { get; private set; }
        public int MemoDetailCurrencyId { get; private set; }
        [MaxLength(32)]
        public string MemoDetailCurrencyCode { get; private set; }
        public double MemoDetailCurrencyRate { get; private set; }
        public int AccountingBookId { get; private set; }
        [MaxLength(256)]
        public string AccountingBookType { get; private set; }
        [MaxLength(128)]
        public string AccountingBookCode { get; private set; }
        public string Remark { get; private set; }
        public bool IsPosted { get; private set; }

        public void SetIsPosted(bool value)
        {
            IsPosted = value;
        }
    }
}
