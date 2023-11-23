using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;
using WebAPISample.Applications;
using WebAPISample.Filters;
using WebAPISample.Models;
using WebAPISample.Services;

namespace WebAPISample.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
        /// Test(GET)
        /// </summary>
        /// <returns></returns>
        [HttpGet("Test")]
        public IActionResult Test()
        {
            _logger.LogInformation("中文字測試");
            return Ok(_SignatureApp.Test());
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "cSID": "A123456789",
        ///        "cbirth": "19951023"
        ///     }
        /// </remarks>
        [HttpPost("Signature")]
        public IActionResult Signature(DATA request)
        {
            return Ok(_SignatureApp.Signature(request));
        }

        /// <summary>
        /// 驗證
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Verify")]
        [TypeFilter(typeof(MACValidationFilter))]
        public IActionResult Verify(ApiModel<DATA> request)
        {
            return Ok(_SignatureApp.Verify(request));
        }
    }
}
