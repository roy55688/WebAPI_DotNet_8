namespace WebAPISample.Models
{
    public class ApiModel<T>
    {
        public T? DATA { get; set; }

        public string? MAC { get; set; }
    }

    public class DATA
    {

        /// <example>A123456789</example>
        public string cSID { get; set; } = string.Empty;

        /// <example>19951023</example>
        public string cbirth { get; set; } = string.Empty;
    }

}
