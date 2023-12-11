namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public class UnitDto
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DivisionDto Division { get; set; }
        public int? VBDocumentLayoutOrder { get; set; }
    }
}