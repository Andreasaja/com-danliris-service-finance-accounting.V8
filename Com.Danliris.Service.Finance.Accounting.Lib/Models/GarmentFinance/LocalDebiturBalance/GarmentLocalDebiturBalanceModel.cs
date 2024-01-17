using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.LocalDebiturBalance
{
    public class GarmentLocalDebiturBalanceModel : StandardEntity
    {
        public DateTimeOffset BalanceDate { get; set; }
        public int BuyerId { get; set; }
        [MaxLength(10)]
        public string? BuyerCode { get; set; }
        [StringLength(255)]
        public string? BuyerName { get; set; }
        public double BalanceAmount { get; set; }
    }
}
