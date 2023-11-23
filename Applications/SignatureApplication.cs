using System.Text.Json;
using WebAPISample.Models;
using WebAPISample.Services;

namespace WebAPISample.Applications
{
    public interface ISignatureApplication
    {
        public string Signature(DATA data);
        public string Verify(ApiModel<DATA> data);
        public string Test();
    }
    public class SignatureApplication : ISignatureApplication
    {
        private readonly ISignatureService _Signature;
        private readonly IResponseService _response;
        public SignatureApplication(ISignatureService Signature,IResponseService responseService)
        {
            _Signature = Signature;
            _response = responseService;
        }

        public string Test()
        {
            return _response.Success("TEST");
        }
        public string Signature(DATA data)
        {
            ApiModel<DATA> model = new ApiModel<DATA>()
            {
                DATA = data,
                MAC = _Signature.MACDataEncode(JsonSerializer.Serialize(data))
            };
            return _response.Success(model);
        }
        public string Verify(ApiModel<DATA> data)
        {
            return _Signature.MACDataEncode(JsonSerializer.Serialize(data.DATA)) == data.MAC ? 
                _response.Success("驗證成功") : _response.Fail("9999","驗證失敗") ;
        }
    }
}
