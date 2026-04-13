using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankLoanMonetrySolu.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConventTrackerController : ControllerBase
    {
        private readonly IConventTracker _conventService;

        public ConventTrackerController(IConventTracker conventService)
        {
            _conventService = conventService;
        }

        /// <summary>
        /// Retrieves both Financial and Non-Financial Covenants for a specific AppId.
        /// </summary>
        /// <param name="appId">The unique identifier for the loan application.</param>
        [HttpGet("GetTracker/{appId}")]
        public async Task<IActionResult> GetTracker(string appId)
        {
            try
            {
                var result = await _conventService.GetLoanCovenantTrackerResponsesAsync(appId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Partially updates a Non-Financial Covenant (Description, Type, or Frequency).
        /// </summary>
        /// <param name="appId">The unique identifier for the loan application.</param>
        /// <param name="updateDto">
        [HttpPut("EditNonFinancial/{appId}")]
        public async Task<IActionResult> EditNonFinancial(string appId, [FromBody] EditNonFinancialCovenantDto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest("Update data is required.");
            }

            var result = await _conventService.EditNonFinaciaConeventTracker(appId, updateDto);

            if (result == "Success")
            {
                return Ok(new { message = "Covenant updated successfully." });
            }

            if (result == "Invalid Guid Format" || result == "Covenant record not found.")
            {
                return BadRequest(result);
            }

            return StatusCode(500, result);
        }

        [HttpPost("add-covenant/{appId}")]
        public async Task<IActionResult> AddCovenant(string appId, [FromBody] AddNonFincialConvDto addDto)
        {
            if (addDto == null)
            {
                return BadRequest("Request body cannot be null.");
            }
            var result = await _conventService.AddNonFinaciaConeventTracker(appId, addDto);
            if (result == "Success")
            {
                return Ok(new { message = "Non-Financial Covenant added successfully." });
            }
            return BadRequest(new { error = result });
        }

        [HttpPost("add-finacia-covenant/{appId}")]
        public async Task<IActionResult> AddFinaciaCovenant(string appId, [FromBody] ConventFinacialDto addDto)
        {
            if (addDto == null)
                return BadRequest("Request body cannot be null.");

            var result = await _conventService.AddFinacialConvenatTracker(appId, addDto);

            // FIX: Changed '=' to '==' to properly check the boolean
            if (result.isSuccess)
            {
                return Ok(new
                {
                    message = "Financial Covenant added successfully.",
                    data = result.Financials // Returning the added record for your table
                });
            }

            return BadRequest(new { error = result.Message });
        }

        [HttpPost("bulk-upload-covenants")]
        public async Task<IActionResult> BulkUploadCovenants(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid Excel file.");

            var result = await _conventService.UploadFinancialCovenantExcel(file);

            if (result.isSuccess)
            {
                return Ok(new
                {
                    message = result.Message,
                    data = result.uploadedDatas, // The list for your frontend table
                    errors = result.Errors       // Any row-specific warnings
                });
            }

            return BadRequest(new
            {
                message = result.Message,
                errors = result.Errors
            });
        }
    }
}