using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebAPISample.Applications;
using WebAPISample.Filters;
using WebAPISample.Models;

//AccountController分成兩個Version：
//Version 1 可以直接呼叫
//Version 2 需要攜帶簽章

namespace WebAPISample.ControllersV1
{
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion(1)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountApplication _AccountApp;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountApplication accountApplication)
        {
            _AccountApp = accountApplication;
        }
        /// <summary>
        /// 登入
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion(1)]
        [HttpPost("Login")]
        public IActionResult Login(DATA request)
        {
            return Ok(_AccountApp.Login(request.cSID, request.cbirth));
        }
    }
}

namespace WebAPISample.ControllersV2
{
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion(2)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountApplication _AccountApp;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountApplication accountApplication)
        {
            _AccountApp = accountApplication;
        }
        /// <summary>
        /// 登入V2 - 須帶訊息識別碼(MAC)
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion(2)]
        [HttpPost("Login")]
        [TypeFilter(typeof(MACValidationFilter))]
        public IActionResult Login(ApiModel<DATA> request)
        {
            return Ok(_AccountApp.Login(request.DATA.cSID, request.DATA.cbirth));
        }
    }
}

