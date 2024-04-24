using WebAPISample.Models;
using WebAPISample.Services;

namespace WebAPISample.Applications
{
    public interface IAccountApplication
    {
        ResponseModel Login(string id, string birth);
    }
    public class AccountApplication : IAccountApplication
    {
        private readonly IResponseService _response;
        private readonly ICheckService _checkService;
        public AccountApplication(IResponseService responseService, ICheckService checkService)
        {
            _response = responseService;
            _checkService = checkService;
        }
        public ResponseModel Login(string id, string birth)
        {
            if (!_checkService.ChkColumnValue(id,CheckMode.chkOnlyNumAndEng) ||
                !_checkService.ChkColumnValue(birth, CheckMode.chkOnlyNum))
            {
                return _response.Fail("9902","資料格式錯誤!");
            }

            if (id == "A123456789" && birth == "20240424")
            {
                return _response.Success("登入成功!");
            }
            else
            {
                return _response.Fail("9901", "ID或生日有誤!");
            }
        }
    }
}
