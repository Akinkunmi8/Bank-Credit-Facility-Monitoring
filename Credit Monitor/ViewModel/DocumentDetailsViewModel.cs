namespace Credit_Monitor.ViewModel
{
    public class DocumentDetailsViewModel
    {
        public Guid FileId { get; set; }
        public Guid BatchId { get; set; } // The actual filename on disk
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string DocType { get; set; } // e.g. "CAT", "CACOM"
        public string Extension { get; set; }

        // UI Helper for Bootstrap Icons based on your specific MimeTypes
        public string DisplayIcon => (ContentType ?? "").ToLower() switch
        {
            // PDF - Distinct Red
            "application/pdf" => "bi-file-pdf-fill text-danger",

            // Images - Distinct Blue
            "image/jpeg" or "image/jpg" or "image/png" or "image/gif" => "bi-file-earmark-image text-primary",
            "image/tiff" => "bi-file-image text-info",

            // Excel / Spreadsheets - Distinct Green
            "application/vnd.ms-excel" or
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" or
            "text/csv" => "bi-file-earmark-spreadsheet-fill text-success",

            // Word / Documents - Distinct Dark Blue
            "application/msword" or
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "bi-file-earmark-word-fill text-primary",

            // Text Files
            "text/plain" => "bi-file-earmark-text text-secondary",

            // Compressed / Zipped files
            "application/zip" or "application/x-zip-compressed" or "application/x-rar-compressed" => "bi-file-earmark-zip text-warning",

            // Default Fallback for everything else
            _ => "bi-file-earmark-fill text-muted"
        };
    }
}