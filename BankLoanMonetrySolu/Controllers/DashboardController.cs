using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
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

        public DashboardController(
            IFinacleData finacleService,
            ITAT tatService)
        {
            _finacleService = finacleService;
            _tatService = tatService;
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

            var result = await _finacleService.AddDiscrepancyRemark(dto);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }

}

