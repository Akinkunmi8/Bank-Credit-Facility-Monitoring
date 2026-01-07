using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CATController : ControllerBase
    {
        private readonly ICAT _catService;

        public CATController(ICAT catService)
        {
            _catService = catService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCAT([FromBody] CreditAvailmentTicketDto catDto)
        {
            if (catDto == null)
                return BadRequest(new { Success = false, Message = "CAT data is required" });

            var result = await _catService.CreateCatAsync(catDto);
            return result.IsSuccess ? Ok(result) : StatusCode(500, result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCAT()
        {
            var result = await _catService.GetAllCAT();
            return result.IsSuccess ? Ok(result) : StatusCode(500, result);
        }

        [HttpGet("GetByAccount/{accountNum}")]
        public async Task<IActionResult> GetCATByAccountNum(string accountNum)
        {
            if (string.IsNullOrWhiteSpace(accountNum))
                return BadRequest(new { Success = false, Message = "Account number is required" });

            var result = await _catService.GetCATByAccountNum(accountNum);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}

