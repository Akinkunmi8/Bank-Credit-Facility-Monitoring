using Microsoft.AspNetCore.Mvc;

namespace BankLoanMonetrySolu.DTO
{
    //public class UploadFilesDto
    //{
    //    public string AppId { get; set; }

    //    // List of files
    //    public List<IFormFile> Files { get; set; }
    //    public LoanDocumentType FileTypes { get; set; }
    //}


    public enum LoanDocumentType
    {
        CAT,
        CACOM,
        OfferLetter,
        ApprovalMail,
        InsuranceCert,
        CashCollateral
    }


    public class UploadFilesDto
    {
        public string AppId { get; set; }
        [FromForm]
        public List<FileWithMetadata> FileItems { get; set; }
    }

    public class FileWithMetadata
    {
        [FromForm]
        public IFormFile File { get; set; }
        public LoanDocumentType DocType { get; set; }
    }

}
