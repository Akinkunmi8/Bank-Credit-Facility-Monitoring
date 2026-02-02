using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BankLoanMonetrySolu.Controllers
{
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
    }
}