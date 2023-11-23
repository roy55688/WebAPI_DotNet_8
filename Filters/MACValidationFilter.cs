using WebAPISample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace WebAPISample.Filters
{
    /// <summary>
    /// 驗證MAC的Filter
    /// </summary>
    /// <param name="request"></param>

    public class MACValidationFilter : IActionFilter
    {
        private readonly ISignatureService _Signature;
        private readonly IResponseService _response;

        public MACValidationFilter(ISignatureService Signature, IResponseService responseService)
        {
            _Signature = Signature;
            _response = responseService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.ActionArguments["request"];
            var data = request?.GetType().GetProperty("DATA")?.GetValue(request);
            var mac = request?.GetType().GetProperty("MAC")?.GetValue(request);

            if (mac != null && data != null)
            {
                if (mac.ToString() != _Signature.MACDataEncode(JsonSerializer.Serialize(data)))
                {
                    context.Result = new ObjectResult(_response.Fail("9999", "驗證失敗"));
                    return;
                }
            }
            else
            {
                context.Result = new ObjectResult(_response.Fail("9999", "驗證失敗"));
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
