using WebAPISample.Models;

namespace WebAPISample.Services
{
    /// <summary>
    /// 轉換回傳內容
    /// </summary>
    public interface IResponseService
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        ResponseModel Success(object? info = null);

        /// <summary>
        /// 失敗 or 錯誤
        /// </summary>
        /// <param name="rtnCode"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        ResponseModel Fail(string rtnCode,object? info = null);

    }

    /// <summary>
    /// 轉換回傳內容
    /// </summary>
    public class ResponseService : IResponseService
    {
        private readonly string _id;
        public ResponseService(IIDCreateService iDCreateService)
        {
            _id = iDCreateService.ID;
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ResponseModel Success(object? info = null)
        {
            ResponseModel responseModel = new ResponseModel();

            responseModel.traceid = _id;

            responseModel.rtncode = "0000";

            responseModel.info = info ?? new object();

            return responseModel;
        }

        /// <summary>
        /// 失敗 or 錯誤
        /// </summary>
        /// <param name="rtnCode"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public ResponseModel Fail(string rtnCode,object? info = null)
        {
            ResponseModel responseModel = new ResponseModel();

            responseModel.traceid = _id;

            responseModel.rtncode = rtnCode;

            responseModel.info = info ?? new object();

            return responseModel;
        }

    }
}
