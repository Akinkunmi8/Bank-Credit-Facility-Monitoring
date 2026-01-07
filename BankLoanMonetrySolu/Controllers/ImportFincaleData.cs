using Azure.Core;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankLoanMonetrySolu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportFinacleDataController : ControllerBase
    {
        private readonly ReadExcelFile _readExcelFile;

        public ImportFinacleDataController(ReadExcelFile readExcelFile)
        {
            _readExcelFile = readExcelFile;
        }

        [HttpPost("UploadExcel")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadExcel([FromForm] ExcelUploadDto request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("Excel file is required");

            try
            {
                var result = await _readExcelFile.ImportAsync(request.File);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }



}
