using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO
{
    public class RealizationVbWithPOViewModel : BaseViewModel, IValidatableObject
    {
        public string VBRealizationNo { get; set; }
        public string TypeVBNonPO { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? DateEstimate { get; set; }
        public ICollection<DetailSPB> Items { get; set; }
        public DetailVB numberVB { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date == null)
                yield return new ValidationResult("Tanggal harus diisi!", new List<string> { "Date" });

            if (string.IsNullOrWhiteSpace(TypeVBNonPO))
                yield return new ValidationResult("Jenis VB harus diisi!", new List<string> { "TypeVBNonPO" });
            else
            {
                if (TypeVBNonPO == "Dengan Nomor VB")
                {
                    if (numberVB == null)
                        yield return new ValidationResult("Nomor VB harus diisi!", new List<string> { "VBCode" });
                }
                else
                {
                    if (DateEstimate == null)
                        yield return new ValidationResult("Tanggal Estimasi harus diisi!", new List<string> { "DateEstimate" });
                }
            }



            int cnt = 0;

            if (Items == null || Items.Count <= 0)
            {
                yield return new ValidationResult("Data harus diisi!", new List<string> { "Item" });
            }
            //else
            //{
            //    //foreach (var itm in Items)
            //    //{
            //    //    if (itm.IsSave == false)
            //    //    {
            //    //        cnt += 1;
            //    //    }
            //    //}

            //    //if (Items.Count == cnt)
            //    //{
            //    //    yield return new ValidationResult("Data harus dipilih!", new List<string> { "Item" });
            //    //}
            //}

        }
    }
}