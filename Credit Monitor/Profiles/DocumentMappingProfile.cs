using AutoMapper;
using Credit_Monitor.APIDto;
using Credit_Monitor.ViewModel;

public class DocumentMappingProfile : Profile
{
    public DocumentMappingProfile()
    {
        CreateMap<FileDetailDto, DocumentDetailsViewModel>()
            .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => GetExtensionFromMime(src.ContentType)));
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