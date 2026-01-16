using Microsoft.AspNetCore.Mvc;

namespace BankLoanMonetrySolu.DTO
{


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

    public class FileViewDto
    {
        public Guid? FileId { get; set; }
        public string? FileName { get; set; }
        public string? DocType { get; set; }
        public string? ContentType  { get; set; }
    }

}
