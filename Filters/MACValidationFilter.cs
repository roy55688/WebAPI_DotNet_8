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
            try
            {
                var request = context.ActionArguments["request"];
                var data = request?.GetType().GetProperty("DATA")?.GetValue(request);
                var mac = request?.GetType().GetProperty("MAC")?.GetValue(request);

                if (mac == null)
                {
                    
                    context.Result = new UnauthorizedObjectResult(_response.Fail("9995", "無訊息識別碼，請確認。"));
                    return;
                }

                if (data == null)
                {
                    context.Result = new UnauthorizedObjectResult(_response.Fail("9997", "資料內容為空。"));
                    return;
                }

                if (mac.ToString() != _Signature.MACDataEncode(JsonSerializer.Serialize(data)))
                {
                    context.Result = new UnauthorizedObjectResult(_response.Fail("9998", "簽章錯誤")) { StatusCode= 403 };
                    return;
                }


            }
            catch (Exception)
            {
                context.Result = new UnauthorizedObjectResult(_response.Fail("9999", "驗證失敗"));
            }

        }

        /// <inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
