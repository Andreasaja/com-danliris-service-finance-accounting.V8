using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingExpedition
{
    public interface IGarmentPurchasingExpeditionService
    {
        Task<int> SendToVerification(SendToVerificationAccountingForm form);
        Task<int> SendToAccounting(SendToVerificationAccountingForm form);
        Task<int> SendToAccounting(int id);
        Task<int> SendToCashier(int id);
        Task<int> SendToPurchasing(int id);
        Task<int> SendToPurchasingRejected(int id, string remark);
        Task<int> SendToPurchasingRejected(List<int> id, string remark);
        ReadResponse<IndexDto> GetSendToVerificationOrAccounting(string keyword, int page, int size, string order);
        ReadResponse<IndexDto> GetByPosition(string keyword, int page, int size, string order, GarmentPurchasingExpeditionPosition position, int internalNoteId, int supplierId, string currencyCode = null);
        ReadResponse<IndexDto> GetByPositionRetur(string keyword, int page, int size, string order, GarmentPurchasingExpeditionPosition position, int internalNoteId, int supplierId, string currencyCode = null);
        ReadResponse<IndexDto> GetVerified(string keyword, int page, int size, string order);
        Task<int> VerificationAccepted(List<int> ids);
        Task<int> CashierAccepted(List<int> ids);
        Task<int> AccountingAccepted(List<int> ids);
        Task<int> PurchasingAccepted(List<int> ids);
        Task<int> VoidVerificationAccepted(int id);
        Task<int> VoidCashierAccepted(int id);
        Task<int> VoidAccountingAccepted(int id);
        IndexDto GetById(int id);
    }
}
