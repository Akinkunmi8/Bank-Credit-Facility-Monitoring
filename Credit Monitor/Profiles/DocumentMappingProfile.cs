using AutoMapper;
using Credit_Monitor.APIDto;
using Credit_Monitor.ViewModel;

public class DocumentMappingProfile : Profile
{
    public DocumentMappingProfile()
    {
        CreateMap<FileItemDto, DocumentDetailsViewModel>()
             // Map Id to FileId
             .ForMember(dest => dest.FileId, opt => opt.MapFrom(src => src.Id))

             // Map AppId to BatchId (since BatchId is what your VM uses)
             .ForMember(dest => dest.BatchId, opt => opt.MapFrom(src => src.AppId))

             // Extract the Extension from FileName
             .ForMember(dest => dest.Extension, opt => opt.MapFrom(src =>
                 !string.IsNullOrEmpty(src.FileName) && src.FileName.Contains(".")
                     ? Path.GetExtension(src.FileName).ToLower()
                     : "unknown"))

             // Ensure strings are handled if null (optional but safer)
             .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName ?? "Untitled"))
             .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType))
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.isVeriFied))
             .ForMember(dest => dest.DocType, opt => opt.MapFrom(src => src.DocType));
    }
    private string GetExtensionFromMime(string mime)
    {
        if (string.IsNullOrWhiteSpace(mime)) return "dat";

        mime = mime.ToLower();

        if (mime.Contains("pdf")) return "pdf";
        if (mime.Contains("spreadsheetml") || mime.Contains("ms-excel")) return "xlsx";
        if (mime.Contains("wordprocessingml") || mime.Contains("msword")) return "docx";
        if (mime.Contains("text/plain")) return "txt";
        if (mime.Contains("text/csv")) return "csv";
        if (mime.Contains("image/jpeg")) return "jpg";
        if (mime.Contains("image/png")) return "png";
        if (mime.Contains("image/tiff")) return "tiff";
        if (mime.Contains("zip")) return "zip";

        return "dat"; // Default for unknown binary streams
    }
}