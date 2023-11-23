namespace WebAPISample.Services
{
    public interface IIDCreateService
    {
        string ID { get; }
    }

    public class IDCreateService : IIDCreateService
    {

        private string _id;
        public IDCreateService()
        {
            object thisLock = new object();
            lock (thisLock)
            {
                var strNo = DateTime.Now.ToString("yyMMddHHmmss");
                var iSeed = CreateRandSeed();
                var strExt = iSeed.ToString().Substring(2, 3) + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString().Substring(8, 5);
                strNo += strExt;
                _id = strNo;
            }
        }
        public static int CreateRandSeed()
        {
            var guid = Guid.NewGuid().ToString();
            int iSeed = guid.GetHashCode();
            return iSeed;
        }
        public string ID => _id;
    }
}
