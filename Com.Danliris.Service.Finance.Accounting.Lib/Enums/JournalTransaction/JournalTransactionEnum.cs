using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Enums.JournalTransaction
{
    public static class JournalTransactionStatus
    {
        public const string Draft = "DRAFT";
        public const string Posted = "POSTED";
    }

    public static class JournalNumberGenerator
    {
        public static string GetDivisionByNumber(int number)
        {
            var result = "";
            switch(number)
            {
                case 1:
                    result = "S";
                    break;
                case 2:
                    result = "W";
                    break;
                case 3:
                    result = "F";
                    break;
                case 4:
                    result = "G";
                    break;
                default:
                    result = "U";
                    break;
            }

            return result;
        }
    }
}
