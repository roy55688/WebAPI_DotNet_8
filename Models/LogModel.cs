namespace WebAPISample.Models
{
    public class LogModel
    {
        public string? TraceID { get; set; }
        public string? IP { get; set; }
        public string? Proxy { get; set; } //若request經由其他代理傳入該欄位會記錄代理者的IP，若無則存入原始IP。
        public string? ContentType { get; set; }
        public string? Host { get; set; }
        public string? Controller { get; set; }
        public string? Method { get; set; }
        public string? Action { get; set; }
        public DateTime ApiReqTime { get; set; } = DateTime.Now;
        public string? ApiReqHeader { get; set; }
        public string? ApiReqBody { get; set; }
        public string? ApiHttpCode { get; set; }
        public DateTime ApiResTime { get; set; } = DateTime.Now; //若程式在給日期之前報錯，沒有初始值的日期欄位寫入DB會失敗
        public string? ApiResHeader { get; set; }
        public string? ApiResBody { get; set; }
        public string? Runtime { get; set; }
        public string? ErrorMsg { get; set; } = string.Empty;
    }
}
