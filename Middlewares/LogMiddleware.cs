using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using WebAPISample.Models;
using WebAPISample.Services;

namespace UBCP_WebAPISample.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, IIDCreateService iDCreateService)
        {
            LogModel logModel = new LogModel();

            try
            {
                logModel.TraceID = iDCreateService.ID;

                context.Request.EnableBuffering();

                string Income = string.Empty;

                logModel.ApiReqTime = DateTime.Now;

                var headers = context.Request.Headers;
                logModel.ApiReqHeader = SQL_Injection_Filter(string.Join(",", headers.Select(x => $"{x.Key}:{x.Value}")));

                //如果使用反向代理，應確認取回正確的request ip
                //string? ip = headers.ContainsKey(_options.RealHeaderIPKey) ?
                //    IPAddress.Parse(headers[_options.RealHeaderIPKey].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)[0]).ToString()
                //    : context.Connection.RemoteIpAddress?.ToString();

                logModel.IP = SQL_Injection_Filter(context.Connection.RemoteIpAddress?.ToString());
                logModel.Proxy = context.Connection.RemoteIpAddress?.ToString();

                logModel.ContentType = SQL_Injection_Filter(context.Request.ContentType);
                logModel.Host = SQL_Injection_Filter(context.Request.Host.Host);

                logModel.Controller = context.GetRouteValue("controller")?.ToString();
                logModel.Method = SQL_Injection_Filter(context.Request.Method);
                logModel.Action = context.GetRouteValue("action")?.ToString();

                using (StreamReader reader = new StreamReader(context.Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
                {
                    Income = await reader.ReadToEndAsync();
                    logModel.ApiReqBody = SQL_Injection_Filter(Income);
                }

                context.Request.Body.Position = 0;

                Stream originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    Stopwatch sw = Stopwatch.StartNew();
                    try
                    {
                        await _next(context);
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = 500;
                        logModel.ErrorMsg = ex.Message;
                    }
                    finally
                    {
                        sw.Stop();

                        logModel.Runtime = sw.ElapsedMilliseconds.ToString();

                        logModel.ApiResHeader = string.Join(",", context.Response.Headers.Select(x => $"{x.Key}:{x.Value}"));
                        responseBody.Seek(0, SeekOrigin.Begin);
                        string Outcome = await new StreamReader(responseBody).ReadToEndAsync();
                        logModel.ApiResBody = SQL_Injection_Filter(Outcome);
                        logModel.ApiHttpCode = context.Response.StatusCode.ToString();

                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);

                        logModel.ApiResTime = DateTime.Now;
                    }

                    
                }
            }
            catch (Exception ex)
            {
                logModel.ErrorMsg = ex.Message;
            }
            finally
            {
                //save Log
            }

        }
        public string? SQL_Injection_Filter(string? SQl_Str)
        {
            if (!string.IsNullOrEmpty(SQl_Str))
                return Regex.Replace(SQl_Str, @"\b(exec(ute)?|select|update|insert|delete|drop|create)\b|[;']|(-{2})|(/\*.*\*/)|[']|[;]", string.Empty, RegexOptions.IgnoreCase);
            else
                return SQl_Str;
        }
    }
}
