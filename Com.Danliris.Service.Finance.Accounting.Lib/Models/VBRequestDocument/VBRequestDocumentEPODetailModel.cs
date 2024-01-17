using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument
{
    public class VBRequestDocumentEPODetailModel : StandardEntity
    {
        public VBRequestDocumentEPODetailModel()
        {

        }

        public VBRequestDocumentEPODetailModel(
            int vbRequestDocumentId,
            int epoId,
            string epoNo,
            string remark
            )
        {
            VBRequestDocumentId = vbRequestDocumentId;
            EPOId = epoId;
            EPONo = epoNo;
            Remark = remark;
        }

        public int VBRequestDocumentItemId { get; private set; }
        public int VBRequestDocumentId { get; private set; }
        public int EPOId { get; private set; }
        [MaxLength(64)]
        public string? EPONo { get; private set; }
        
        public string? Remark { get; }

        internal void UpdateFromForm(VBRequestDocumentWithPOItemFormDto item)
        {
            EPOId = item.PurchaseOrderExternal.Id.GetValueOrDefault();
            EPONo = item.PurchaseOrderExternal.No;
        }
    }
}
