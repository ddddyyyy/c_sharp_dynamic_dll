using Manager.Model;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;


namespace Manager.Service
{
    /// <summary>
    /// 服务模板
    /// </summary>
    class CommonService
    {

        [ServiceContract]
        public interface ICommonService
        {

            /// <summary>
            ///  返回网页
            /// </summary>
            /// <returns></returns>
            [OperationContract]
            [WebGet(UriTemplate = "/index/{path}",
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Json)]
            System.IO.Stream GetHtml(string path);


            /// <summary>
            /// 数据存储
            /// </summary>
            /// <param name="fileNames">文件在服务器的路径的集合</param>
            /// <param name="dllName">要调用的dll的名字</param>
            /// <param name="methodName">要调用的方法名</param>
            [OperationContract]
            [ServiceKnownType(typeof(string[]))]
            [WebInvoke(Method = "POST",
                UriTemplate = "task/{dllName}/{methodName}",
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Json,
                ResponseFormat = WebMessageFormat.Json)]
            WebResult SubmitTask(string[] fileNames, string dllName, string methodName);


            /// <summary>
            /// 上传文件
            /// </summary>
            /// <param name="filename">要上传的文件名</param>
            /// <param name="length">文件长度</param>
            /// <param name="s">文件流</param>
            /// <returns>文件上传成功之后文件在系统中的路径</returns>
            [OperationContract]
            [WebInvoke(Method = "POST",
                BodyStyle = WebMessageBodyStyle.Bare,
                ResponseFormat = WebMessageFormat.Json,
                UriTemplate = "Uploadfile?fileName={filename}&length={length}")]
            WebResult UploadFile(string filename, int length, System.IO.Stream s);

            /// <summary>
            /// 查询
            /// </summary>
            /// <param name="args">参数列表</param>
            /// <param name="dllName">要调用的dll的名字</param>
            /// <param name="methodName">要调用的方法名</param>
            [OperationContract]
            [ServiceKnownType(typeof(string[]))]
            [WebInvoke(Method = "POST",
                UriTemplate = "search/{dllName}/{methodName}",
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Json,
                ResponseFormat = WebMessageFormat.Json)]
            WebResult Search(string[] args, string dllName, string methodName);

        }


    }
}
