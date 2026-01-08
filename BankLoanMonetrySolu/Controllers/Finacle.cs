using BankLoanMonetrySolu.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankLoanMonetrySolu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Finacle : ControllerBase
    {
        private readonly IFinacleData _finacleData;
        public Finacle(IFinacleData finacleData)
        {
            _finacleData = finacleData;
        }

        // 1️⃣ Get ALL Finacle data
        [HttpGet("All-FincaleData")]
        public async Task<IActionResult> GetAllFinacleData()
        {
            var response = await _finacleData.GetALlFincaleData();
            return Ok(response);
        }
        [HttpGet("All-loan")]
        public async Task<IActionResult> GetAllloan()
        {
            var response = await _finacleData.GetALLLoans();
            return Ok(response);
        }

        // 2️⃣ Get Finacle data by region
        [HttpGet("by-region/{region}")]
        public async Task<IActionResult> GetFinacleDataByRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region))
                return BadRequest("Region is required.");

            var response = await _finacleData.GetAllPendingConfirmationByRegion(region);
            return Ok(response);
        }

        // 3️⃣ Get ALL confirmed Finacle data
        [HttpGet("confirmed")]
        public async Task<IActionResult> GetAllConfirmedFinacleData( string region)
        {
            var response = await _finacleData.GetAllConfirmedFinacleData(region);
            return Ok(response);
        }

        // 4️⃣ Get ALL reconfirmed Finacle data by region
        [HttpGet("reconfirmed/by-region/{region}")]
        public async Task<IActionResult> GetAllReConfirmedFinacleDataByRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region))
                return BadRequest("Region is required.");

            var response = await _finacleData.GetAllReConfirmedFinacleDataByRegion(region);
            return Ok(response);
        }

        // 5️⃣ Get ALL approved Finacle data
        [HttpGet("approved")]
        public async Task<IActionResult> GetAllApprovedFinacleData(string region)
        {
            var response = await _finacleData.GetAllAprovedFinacleData(region);
            return Ok(response);
        }

        [HttpGet("CompareData")]
        public async Task<IActionResult> GetFinacleFinacle(string acctNum)
        {
            var response = await _finacleData.GetFinacleCompareData(acctNum);
            return Ok(response);
        }
    }
}

