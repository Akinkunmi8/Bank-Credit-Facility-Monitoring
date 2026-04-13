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
        CashCollateral,
        Other
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
        public Guid? AppId { get; set; }
        public string? FileName { get; set; }
        public string? DocType { get; set; }
        public string? ContentType  { get; set; }
        public bool IsVeriFied { get; set; }
        public bool IsFileExitOnServer { get; set; } = false;
        public string? Region { get; set; }
        public string? RequesterUsername { get; set; }
    }

    public class FileStatusUpdateDto
    {
        public Guid FileId { get; set; }
        public bool IsVerified { get; set; }
    }

    public class FileEditDto
    {
        public Guid? Id { get; set; }
        public Guid? AppId { get; set; }
        public string? FileName { get; set; }
        public string? DocType { get; set; }
        public string? ContentType { get; set; }
        public bool IsVeriFied { get; set; }
    }

}
