using System.Text.Json;
using WebAPISample.Models;
using WebAPISample.Services;

namespace WebAPISample.Applications
{
    public interface ISignatureApplication
    {
        public ResponseModel Signature(DATA data);
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
        public ResponseModel Signature(DATA data)
        {
            ApiModel<DATA> model = new ApiModel<DATA>()
            {
                DATA = data,
                MAC = _Signature.MACDataEncode(JsonSerializer.Serialize(data))
            };
            return _response.Success(model);
        }
    }
}
