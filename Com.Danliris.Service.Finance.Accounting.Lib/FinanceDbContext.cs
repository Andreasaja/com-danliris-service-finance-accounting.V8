using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePayment;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.Memo;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.SalesReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VbNonPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingPphBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePurchasingDisposition;
using Com.Moonlay.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.AccountingBook;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingMemoDetailTextile;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingMemoTextile;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.LocalDebiturBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Adjustment;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
        {
        }

        public DbSet<COAModel> ChartsOfAccounts { get; set; }

        public DbSet<CreditorAccountModel> CreditorAccounts { get; set; }

        public DbSet<DailyBankTransactionModel> DailyBankTransactions { get; set; }
        public DbSet<BankTransactionMonthlyBalanceModel> BankTransactionMonthlyBalances { get; set; }

        public DbSet<JournalTransactionModel> JournalTransactions { get; set; }
        public DbSet<JournalTransactionItemModel> JournalTransactionItems { get; set; }
        public DbSet<JournalTransactionNumber> JournalTransactionNumbers { get; set; }

        public DbSet<LockTransactionModel> LockTransactions { get; set; }

        public DbSet<PurchasingDispositionExpeditionModel> PurchasingDispositionExpeditions { get; set; }
        public DbSet<PurchasingDispositionExpeditionItemModel> PurchasingDispositionExpeditionItems { get; set; }

        public DbSet<PaymentDispositionNoteModel> PaymentDispositionNotes { get; set; }
        public DbSet<PaymentDispositionNoteItemModel> PaymentDispositionNoteItems { get; set; }
        public DbSet<PaymentDispositionNoteDetailModel> PaymentDispositionNoteDetails { get; set; }

        public DbSet<OthersExpenditureProofDocumentModel> OthersExpenditureProofDocuments { get; set; }
        public DbSet<OthersExpenditureProofDocumentItemModel> OthersExpenditureProofDocumentItems { get; set; }

        public DbSet<SalesReceiptModel> SalesReceipts { get; set; }
        public DbSet<SalesReceiptDetailModel> SalesReceiptDetails { get; set; }

        public DbSet<MemoModel> Memos { get; set; }
        public DbSet<MemoItemModel> MemoItems { get; set; }

        public DbSet<DownPaymentModel> DownPayments { get; set; }

        public DbSet<VbRequestModel> VbRequests { get; set; }
        public DbSet<VbRequestDetailModel> VbRequestsDetails { get; set; }

        public DbSet<RealizationVbModel> RealizationVbs { get; set; }
        public DbSet<RealizationVbDetailModel> RealizationVbDetails { get; set; }

        public DbSet<VBRealizationDocumentExpeditionModel> VBRealizationDocumentExpeditions { get; set; }

        public DbSet<VBRequestDocumentModel> VBRequestDocuments { get; set; }
        public DbSet<VBRequestDocumentItemModel> VBRequestDocumentItems { get; set; }
        public DbSet<VBRequestDocumentEPODetailModel> VBRequestDocumentEPODetails { get; set; }

        public DbSet<GarmentInvoicePaymentModel> GarmentInvoicePayments { get; set; }
        public DbSet<GarmentInvoicePaymentItemModel> GarmentInvoicePaymentItems { get; set; }
        
        public DbSet<VBRealizationDocumentModel> VBRealizationDocuments { get; set; }
        public DbSet<VBRealizationDocumentExpenditureItemModel> VBRealizationDocumentExpenditureItems { get; set; }
        public DbSet<VBRealizationDocumentUnitCostsItemModel> VBRealizationDocumentUnitCostsItems { get; set; }

        public DbSet<GarmentPurchasingExpeditionModel> GarmentPurchasingExpeditions { get; set; }
        public DbSet<GarmentDispositionExpeditionModel> GarmentDispositionExpeditions { get; set; }

        public DbSet<BudgetCashflowTypeModel> BudgetCashflowTypes { get; set; }
        public DbSet<BudgetCashflowCategoryModel> BudgetCashflowCategories { get; set; }
        public DbSet<BudgetCashflowSubCategoryModel> BudgetCashflowSubCategories { get; set; }
        public DbSet<BudgetCashflowUnitModel> BudgetCashflowUnits { get; set; }
        public DbSet<InitialCashBalanceModel> InitialCashBalances { get; set; }
        public DbSet<RealCashBalanceModel> RealCashBalances { get; set; }
        
        public DbSet<DPPVATBankExpenditureNoteModel> DPPVATBankExpenditureNotes { get; set; }
        public DbSet<DPPVATBankExpenditureNoteItemModel> DPPVATBankExpenditureNoteItems { get; set; }
        public DbSet<DPPVATBankExpenditureNoteDetailModel> DPPVATBankExpenditureNoteDetails { get; set; }
        public DbSet<DPPVATBankExpenditureNoteDetailDoModel> DPPVATBankExpenditureNoteDetailDos { get; set; }


        public DbSet<GarmentPurchasingPphBankExpenditureNoteModel> GarmentPurchasingPphBankExpenditureNotes { get; set; }
        public DbSet<GarmentPurchasingPphBankExpenditureNoteItemModel> GarmentPurchasingPphBankExpenditureNoteItems { get; set; }
        public DbSet<GarmentPurchasingPphBankExpenditureNoteInvoiceModel> GarmentPurchasingPphBankExpenditureNoteInvoices { get; set; }

        public DbSet<GarmentInvoicePurchasingDispositionModel> GarmentInvoicePurchasingDispositions { get; set; }
        public DbSet<GarmentInvoicePurchasingDispositionItemModel> GarmentInvoicePurchasingDispositionItems { get; set; }
        public DbSet<GarmentDebtBalanceModel> GarmentDebtBalances { get; set; }
        
        public DbSet<MemoGarmentPurchasingModel> MemoGarmentPurchasings { get; set; }
        public DbSet<MemoGarmentPurchasingDetailModel> MemoGarmentPurchasingDetails { get; set; }
        public DbSet<MemoDetailGarmentPurchasingModel> MemoDetailGarmentPurchasings { get; set; }
        public DbSet<MemoDetailGarmentPurchasingDispositionModel> MemoDetailGarmentPurchasingDispositions { get; set; }
        public DbSet<MemoDetailGarmentPurchasingDetailModel> MemoDetailGarmentPurchasingDetails { get; set; }

        public DbSet<AccountingBookModel> AccountingBooks { get; set; }

        public DbSet<GarmentFinanceMemorialModel> GarmentFinanceMemorials { get; set; }
        public DbSet<GarmentFinanceMemorialItemModel> GarmentFinanceMemorialItems { get; set; }

        public DbSet<PurchasingMemoDetailTextileModel> PurchasingMemoDetailTextiles { get; set; }
        public DbSet<PurchasingMemoDetailTextileItemModel> PurchasingMemoDetailTextileItems { get; set; }
        public DbSet<PurchasingMemoDetailTextileDetailModel> PurchasingMemoDetailTextileDetails { get; set; }
        public DbSet<PurchasingMemoDetailTextileUnitReceiptNoteModel> PurchasingMemoDetailTextileUnitReceiptNotes { get; set; }

        public DbSet<BankCashReceiptModel> GarmentFinanceBankCashReceipts { get; set; }
        public DbSet<BankCashReceiptItemModel> GarmentFinanceBankCashReceiptItems { get; set; }

        public DbSet<BankCashReceiptDetailModel> GarmentFinanceBankCashReceiptDetails { get; set; }
        public DbSet<BankCashReceiptDetailItemModel> GarmentFinanceBankCashReceiptDetailItems { get; set; }
        public DbSet<BankCashReceiptDetailOtherItemModel> GarmentFinanceBankCashReceiptDetailOtherItems { get; set; }

        public DbSet<PurchasingMemoTextileModel> PurchasingMemoTextiles { get; set; }
        public DbSet<PurchasingMemoTextileItemModel> PurchasingMemoTextileItems { get; set; }

        public DbSet<GarmentFinanceMemorialDetailModel> GarmentFinanceMemorialDetails { get; set; }
        public DbSet<GarmentFinanceMemorialDetailItemModel> GarmentFinanceMemorialDetailItems { get; set; }
        public DbSet<GarmentFinanceMemorialDetailOtherItemModel> GarmentFinanceMemorialDetailOtherItems { get; set; }
        public DbSet<GarmentFinanceMemorialDetailRupiahItemModel> GarmentFinanceMemorialDetailRupiahItems { get; set; }

        public DbSet<GarmentFinanceMemorialDetailLocalModel> GarmentFinanceMemorialDetailLocals { get; set; }
        public DbSet<GarmentFinanceMemorialDetailLocalItemModel> GarmentFinanceMemorialDetailLocalItems { get; set; }
        public DbSet<GarmentFinanceMemorialDetailLocalOtherItemModel> GarmentFinanceMemorialDetailLocalOtherItems { get; set; }

        public DbSet<GarmentFinanceBankCashReceiptDetailLocalModel> GarmentFinanceBankCashReceiptDetailLocals { get; set; }
        public DbSet<GarmentFinanceBankCashReceiptDetailLocalItemModel> GarmentFinanceBankCashReceiptDetailLocalItems { get; set; }
        public DbSet<GarmentFinanceBankCashReceiptDetailLocalOtherItemModel> GarmentFinanceBankCashReceiptDetailLocalOtherItems { get; set; }

        public DbSet<GarmentLocalDebiturBalanceModel> GarmentLocalDebiturBalances { get; set; }

        public DbSet<GarmentFinanceAdjustmentModel> GarmentFinanceAdjustments { get; set; }
        public DbSet<GarmentFinanceAdjustmentItemModel> GarmentFinanceAdjustmentItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<JournalTransactionItemModel>().Property(x => x.Debit).HasColumnType("decimal(18,2)");
            //modelBuilder.Entity<JournalTransactionItemModel>().Property(x => x.Credit).HasColumnType("decimal(18,2)");
            //modelBuilder.Entity<COAModel>().Property(x => x.Balance).HasColumnType("decimal(18,2)");
            //modelBuilder.Entity<VBRealizationDocumentModel>().Property(x => x.VBRequestDocumentAmount).HasColumnType("decimal(18,2)");
            //modelBuilder.Entity<VBRealizationDocumentExpenditureItemModel>().Property(x => x.Amount).HasColumnType("decimal(18,2)");
            //modelBuilder.Entity<VBRealizationDocumentUnitCostsItemModel>().Property(x => x.Amount).HasColumnType("decimal(18,2)");
            //modelBuilder.Entity<VBRequestDocumentModel>().Property(x => x.Amount).HasColumnType("decimal(18,2)");
            //modelBuilder.Entity<VBRealizationDocumentModel>().Property(x => x.Amount).HasColumnType("decimal(18,2)");
            ////AccountingBook
            //modelBuilder.Entity<AccountingBookModel>().HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
            ConfigureEntities(modelBuilder);
        }

        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
            Type baseType = typeof(IStandardEntity);
            foreach (IStandardEntity item in from typeInfo in GetType().Assembly.DefinedTypes
                                             where baseType.IsAssignableFrom(typeInfo) && !typeInfo.IsAbstract && typeInfo.IsClass
                                             select typeInfo into info
                                             select Activator.CreateInstance(info) as IStandardEntity)
            {
                EntityTypeBuilder builder = modelBuilder.Entity(item.GetType());
                ConfigureProperties(builder);
                ConfigureQueryFilter(builder);
            }
        }

        

        private void ConfigureProperties(EntityTypeBuilder builder)
        {
            builder.Property("LastModifiedBy").IsRequired().HasMaxLength(255);
            builder.Property("LastModifiedAgent").IsRequired().HasMaxLength(255);
            builder.Property("CreatedBy").IsRequired().HasMaxLength(255);
            builder.Property("CreatedAgent").IsRequired().HasMaxLength(255);
            builder.Property("DeletedBy").IsRequired().HasMaxLength(255);
            builder.Property("DeletedAgent").IsRequired().HasMaxLength(255);
        }

        private static void ConfigureQueryFilter(EntityTypeBuilder builder)
        {
            ParameterExpression parameterExpression = Expression.Parameter(((ITypeBase)builder.Metadata).ClrType, "IsDeleted");
            MemberExpression left = Expression.Property(parameterExpression, "IsDeleted");
            ConstantExpression right = Expression.Constant(false);
            BinaryExpression body = Expression.Equal(left, right);
            builder.HasQueryFilter(Expression.Lambda(body, parameterExpression));
        }
    }
}
