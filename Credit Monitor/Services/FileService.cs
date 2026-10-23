using AutoMapper;
using Credit_Monitor.APICore;
using Credit_Monitor.APIDto;
using Credit_Monitor.Interfaces;
using Credit_Monitor.ViewModel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Credit_Monitor.Reports;

namespace Credit_Monitor.Services
{
    public class FileService : IFileService
    {
        private readonly IFileAPIService _fileAPIService;
        private readonly IMapper _mapper;
        private readonly ILoanService _loanService;
        private readonly ICovenantTrackerService _covenantService;
        private readonly ILogger<FileService> _logger; // 1. Add Logger

        public FileService(IFileAPIService fileAPIService, IMapper mapper, ILogger<FileService> logger,
            ILoanService loanService,ICovenantTrackerService covenantService)
        {
            _fileAPIService = fileAPIService;
            _mapper = mapper;
            _covenantService = covenantService;
            _loanService = loanService;
            _logger = logger;
        }

        public async Task<List<DocumentDetailsViewModel>> GetCustomerDocumentsAsync(Guid appId)
        {
            try
            {
                // 1. Call the API Service using the standardized AppId
                // Using the GetCustomerFileMetaData function we created earlier
                var fileDtos = await _fileAPIService.GetCustomerFileMetaData(appId);

                if (fileDtos == null || !fileDtos.Any())
                {
                    _logger.LogWarning("No files found for AppId: {AppId}", appId);
                    return new List<DocumentDetailsViewModel>();
                }

                // 2. Map to ViewModels
                // The MappingProfile now handles AppId -> BatchId and FileId automatically
                var viewModels = _mapper.Map<List<DocumentDetailsViewModel>>(fileDtos);

                _logger.LogInformation("Successfully retrieved and mapped {Count} documents for AppId: {AppId}",
                    viewModels.Count, appId);

                return viewModels;
            }
            catch (Exception ex)
            {
                // Log the failure specifically for the Credit Monitor dashboard
                _logger.LogError(ex, "Critical failure retrieving customer documents for AppId: {AppId}", appId);

                // Return an empty list or rethrow based on your UI preference
                // Returning empty allows the Dashboard to load without a 500 error
                return new List<DocumentDetailsViewModel>();
            }
        }

        public async Task<Guid?> UploadDocumentAsync(FileItemDto fileDto)
        {
            var newId = await _fileAPIService.AddFile(fileDto);
            if (newId != null)
            {
                _logger.LogInformation("Document record created successfully with ID: {Id}", newId);
            }
            return newId;
        }

        public async Task<List<DocumentDetailsViewModel>> UpdateDocumentAsync(Guid appId, FileItemDto fileDto)
        {
            var updatedDtos = await _fileAPIService.EditFile(appId, fileDto);

            if (updatedDtos == null || !updatedDtos.Any())
            {
                _logger.LogWarning("Update returned no data for AppId: {AppId}", appId);
                return new List<DocumentDetailsViewModel>();
            }

            return _mapper.Map<List<DocumentDetailsViewModel>>(updatedDtos);
        }

        public async Task<bool> ToggleVerificationAsync(Guid fileId, bool status)
        {
            var success = await _fileAPIService.UpdateFileVerification(fileId, status);
            if (success)
            {
                _logger.LogInformation("Verification for File {FileId} set to {Status}", fileId, status);
            }
            return success;
        }

        public async Task<bool> DeleteDocumentAsync(Guid appId, Guid fileId)
        {
            _logger.LogInformation("Attempting to delete document {FileId} for App {AppId}", fileId, appId);

            var success = await _fileAPIService.DeleteFile(appId, fileId);

            return success;
        }

        public async Task<byte[]> DownloadCreditReportAsync(
    Guid appId,
    string? generatedBy)
        {
            if (appId == Guid.Empty)
                throw new ArgumentException(
                    "Invalid application ID.", nameof(appId));

            try
            {
                // 1. Retrieve customer and loan information
                var profile = await _loanService
                    .GetPendingConfirmedLoanByIdAsync(appId.ToString());

                if (profile?.loan == null)
                    throw new KeyNotFoundException(
                        "Loan record not found.");

                // 2. Validate CMS approval
                if (!profile.loan.approved)
                    throw new InvalidOperationException(
                        "Credit Monitoring Report cannot be generated before CMS approval.");

                // 3. Retrieve documents and covenants
                var documents = await GetCustomerDocumentsAsync(appId);

                var covenants = await _covenantService
                    .GetCovenantDetailsAsync(appId);

                // 4. Build report model
                var reportModel = new CreditMonitoringReportViewModel
                {
                    CreditProfile = profile,

                    Documents = documents,

                    Covenants = covenants ?? new CovenantViewModel(),

                    GeneratedAt = DateTime.Now,

                    GeneratedBy = generatedBy
                };

                // 5. Configure QuestPDF license
                //QuestPDF.Settings.License = LicenseType.Community;

                // 6. Generate corporate PDF
                byte[] pdfBytes = new CorporateCreditReportDocument(reportModel)
                    .GeneratePdf();

                // 7. Log successful generation
                _logger.LogInformation(
                    "Credit report generated successfully for AppId {AppId} by {User}. Size: {Size} bytes",
                    appId,
                    generatedBy,
                    pdfBytes.Length);

                // 8. Return PDF
                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to generate credit report for AppId {AppId}",
                    appId);

                throw;
            }
        }

    }
}