using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebAPISample.Applications;
using WebAPISample.Models;

namespace WebAPISample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion(1)]
    [ApiVersion(2)]
    public class SignatureController : ControllerBase
    {

        private readonly ISignatureApplication _SignatureApp;
        private readonly ILogger<SignatureController> _logger;

        public SignatureController(ISignatureApplication SignatureApp, ILogger<SignatureController> logger)
        {
            _SignatureApp = SignatureApp;
            _logger = logger;
        }
        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("連線測試OK");
        }

        /// <summary>
        /// 加簽
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Signature")]
        public IActionResult Signature(DATA request)
        {
            return Ok(_SignatureApp.Signature(request));
        }
    }
}
