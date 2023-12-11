namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument
{
    public class PostingJournalDto
    {

        public PostingJournalDto(string vBRequestDocumentNo, string documentNo)
        {
            VBRequestDocumentNo = vBRequestDocumentNo;
            DocumentNo = documentNo;
        }

        public string VBRequestDocumentNo { get; private set; }
        public string DocumentNo { get; private set; }
    }
}