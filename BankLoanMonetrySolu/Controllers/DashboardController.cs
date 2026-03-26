using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankLoanMonetrySolu.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IFinacleData _finacleService;
        private readonly ITAT _tatService;
        private readonly IReportService _reportService;
        public DashboardController(
            IFinacleData finacleService,
            ITAT tatService,IReportService reportService)
        {
            _finacleService = finacleService;
            _tatService = tatService;
            _reportService = reportService;
        }
        [HttpGet("finacle/pending")]
        public async Task<IActionResult> GetAllFinacleData(string region)
        {
            var result = await _finacleService.GetTotalPendingCount(region);
            return Ok(result);
        }

        [HttpGet("finacle/approved")]
        public async Task<IActionResult> GetApprovedLoans(string region)
        {
            var result = await _finacleService.GetTotalApproveCount(region);
            return Ok(result);
        }

        [HttpGet("finacle/confirmed")]
        public async Task<IActionResult> GetConfirmedLoans(string region)
        {
            var result = await _finacleService.GetTotalConfirmedCount(region);
            return Ok(result);
        }

        [HttpGet("finacle/reconfirmed/{region}")]
        public async Task<IActionResult> GetReConfirmedByRegion(string region)
        {
            var result = await _finacleService.GetTotalReConfirmedCount(region);
            return Ok(result);
        }
        [HttpGet("finacle/resolved")]

        public async Task<IActionResult> GetResolvedCountByRegion(string region)
        {
            var result = await _finacleService.GetTotalResolvedCount(region);
            return Ok(result);
        }

        [HttpGet("tatcount/within/Unreview/{region}")]
        public async Task<IActionResult> GetWithinTATForUnreview(string region)
        {
            var result = await _tatService.GetWithinTATForUnReview(region);
            return Ok(result);
        }
        [HttpGet("tatcount/within/Unresolve/{region}")]
        public async Task<IActionResult> GetWithinTATForUnresolve(string region)
        {
            var result = await _tatService.GetWithinTATForUnResolved(region);
            return Ok(result);
        }

        [HttpGet("tatcount/beyond/Unreview/{region}")]
        public async Task<IActionResult> GetBeyondTATForUnreview(string region)
        {
            var result = await _tatService.GetBeyondTATForUnreview(region);
            return Ok(result);
        }
        [HttpGet("tatcount/beyond/UnApprove/{region}")]
        public async Task<IActionResult> GetBeyondTATForUnAprove(string region)
        {
            var result = await _tatService.GetBeyondTATForReviewedforApproval(region);
            return Ok(result);
        }
        [HttpGet("tatcount/beyond/Unresolve/{region}")]
        public async Task<IActionResult> GetBeyondTATForUnresolve(string region)
        {
            var result = await _tatService.GetBeyondTATForUnResolved(region);
            return Ok(result);
        }
        [HttpGet("data/beyond/unresolved/{region}")]
        public async Task<IActionResult> GetDataBeyondTATForUnresolved(string region)
        {
            // This calls the method returning List<UnresolvedTATDetailDto>
            var result = await _tatService.GetDataBeyondTATForUnResolved(region);

            if (result == null || !result.Any())
                return NotFound(new { message = $"No records found beyond TAT for region: {region}" });

            return Ok(result);
        }

        [HttpGet("data/within/unresolved/{region}")]
        public async Task<IActionResult> GetDataWithinTATForUnresolved(string region)
        {
            var result = await _tatService.GetDataWithinTATForUnResolved(region);
            return Ok(result);
        }

        [HttpGet("data/beyond/unreview/{region}")]
        public async Task<IActionResult> GetDataBeyondTATForUnReview(string region)
        {
            var result = await _tatService.GetDataBeyondTATForUnReview(region);
            return Ok(result);
        }

        [HttpGet("data/within/unreview/{region}")]
        public async Task<IActionResult> GetDataWithinTATForUnReview(string region)
        {
            var result = await _tatService.GetDataWithinTATForUnReview(region);
            return Ok(result);
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] FinacleStatusUpdateDto dto)
        {
            var result = await _finacleService.UpdateFinacleStatus(dto);

            if (!result)
                return NotFound("Finacle record not found");

            return Ok(new
            {
                success = true,
                message = $"Status '{dto.Action}' applied successfully"
            });
        }

        [HttpGet("View-LoanAndCustomerDetails")]
        public async Task<IActionResult> ViewDetails(string appId)
        {
            var result = await _finacleService.GetCustormerAndLoanDetailsByAppId(appId);
            return Ok(result);
        }

        [HttpGet("remarks/{appId}")]
        public async Task<IActionResult> GetRemarkByAppId(Guid appId)
        {
            // Validate the GUID is not empty
            if (appId == Guid.Empty)
            {
                return BadRequest(new ServiceResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = "Invalid Application ID provided."
                });
            }

            var result = await _finacleService.GetRemarkByAppId(appId);

            if (result == null)
            {
                return NotFound(new ServiceResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = "No discrepancy remarks found for this application."
                });
            }

            // Returns the record if found
            return Ok(result);
        }
        [HttpPost("remarks/save")]
        public async Task<IActionResult> SaveRemark([FromBody] DiscrepancyRemarkDto dto)
        {
            if (dto == null || dto.AppId == Guid.Empty)
            {
                return BadRequest(new ServiceResponseDto { IsSuccess = false, ResponseMessage = "Invalid data provided." });
            }

            var result = await _finacleService.UpsertDiscrepancyRemark(dto);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("add-file")]
        public async Task<IActionResult> AddFile([FromBody] FileViewDto dto)
        {
            var fileId = await _finacleService.AddFile(dto);

            if (string.IsNullOrEmpty(fileId) || fileId.Contains("Invalid"))
                return BadRequest(new { success = false, message = fileId });

            return Ok(new
            {
                success = true,
                message = "File properties saved successfully",
                data = new { id = fileId }
            });
        }

        [HttpGet("get-by-app/{appId}")]
        public async Task<IActionResult> GetFilesByApp(Guid appId)
        {
            var files = await _finacleService.GetFileByAppIdAsync(appId);

            if (files == null || !files.Any())
                return NotFound(new { success = false, message = "No files found for this application" });

            return Ok(new
            {
                success = true,
                data = files
            });
        }

        [HttpPut("edit-file/{appId}")]
        public async Task<IActionResult> EditFile(Guid appId, [FromBody] FileEditDto dto)
        {
            try
            {
                var updatedFiles = await _finacleService.EditFileByAppIdAsync(appId, dto);

                return Ok(new
                {
                    success = true,
                    message = "File updated successfully",
                    data = updatedFiles
                });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("update-file-verify")]
        public async Task<IActionResult> UpdateFileVerify([FromBody] FileStatusUpdateDto dto)
        {
            var result = await _finacleService.UpdateFileStatus(dto.FileId, dto.IsVerified);

            if (!result)
                return NotFound(new { success = false, message = "File record not found" });

            return Ok(new
            {
                success = true,
                message = $"Verification status updated to {dto.IsVerified}"
            });
        }

        [HttpDelete("delete-file/{appId}/{fileId}")]
        public async Task<IActionResult> DeleteFile(Guid appId, Guid fileId)
        {
            var result = await _finacleService.DeleteFileByFileId(appId, fileId);

            if (result == "File record not found.")
                return NotFound(new { success = false, message = result });

            return Ok(new
            {
                success = true,
                message = result
            });
        }

        [HttpGet("download-full-report")]
        public async Task<IActionResult> DownloadFullReport([FromQuery] ReportRequestDto request)
        {
            try
            {
                // 1. Generate the file bytes from the service
                byte[] fileBytes = await _reportService.GenerateFullReport(request);

                if (fileBytes == null || fileBytes.Length == 0)
                {
                    return NotFound("No data found for the selected criteria.");
                }

                // 2. Determine file metadata based on the format
                string fileName = $"{request.Role}_FullReport_{DateTime.Now:yyyyMMdd_HHmm}";
                string contentType;
                string extension;

                if (request.Format.ToUpper() == "EXCEL")
                {
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    extension = "xlsx";
                }
                else
                {
                    contentType = "application/pdf";
                    extension = "pdf";
                }

                // 3. Return the file result
                return File(fileBytes, contentType, $"{fileName}.{extension}");
            }
            catch (Exception ex)
            {
                // Log the exception in a real banking environment
                return StatusCode(500, $"Internal server error during report generation: {ex.Message}");
            }
        }
    }

}

