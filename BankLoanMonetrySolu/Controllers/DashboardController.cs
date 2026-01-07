using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankLoanMonetrySolu.Controllers
{
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
        [HttpGet("finacle/all")]
        public async Task<IActionResult> GetAllFinacleData()
        {
            var result = await _finacleService.GetALlFincaleData();
            return Ok(result);
        }

        [HttpGet("finacle/approved")]
        public async Task<IActionResult> GetApprovedLoans(string region)
        {
            var result = await _finacleService.GetAllAprovedFinacleData(region);
            return Ok(result);
        }

        [HttpGet("finacle/confirmed")]
        public async Task<IActionResult> GetConfirmedLoans(string region)
        {
            var result = await _finacleService.GetAllConfirmedFinacleData(region);
            return Ok(result);
        }

        [HttpGet("finacle/reconfirmed/{region}")]
        public async Task<IActionResult> GetReConfirmedByRegion(string region)
        {
            var result = await _finacleService.GetAllReConfirmedFinacleDataByRegion(region);
            return Ok(result);
        }

        [HttpGet("tat/within")]
        public async Task<IActionResult> GetWithinTAT()
        {
            var result = await _tatService.GetWithinTAT();
            return Ok(result);
        }

        [HttpGet("tat/beyond")]
        public async Task<IActionResult> GetBeyondTAT()
        {
            var result = await _tatService.GetBeyondTAT();
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

        [HttpGet("View-LoanAndCustomrDetailt")]
        public async Task<IActionResult> ViewDetails(string appId)
        {
            var result = await _finacleService.GetCustormerAndLoanDetailsByAppId(appId);
            return Ok(result);
        }
    }

}

