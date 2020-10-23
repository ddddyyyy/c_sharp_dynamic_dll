using Manager.Model;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace Manager.Service
{
    /// <summary>
    /// 服务模板
    /// </summary>
    class CommonService
    {
        /// <summary>
        /// 简单定义两种方法，1、GetScore方法：通过GET请求传入name，返回对应的成绩；2、GetInfo方法：通过POST请求，传入Info对象，查找对应的User并返回给客户端
        /// </summary>
        [ServiceContract]
        public interface ICommonService
        {

            [OperationContract]
            [WebGet(UriTemplate = "test",
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Json,
                ResponseFormat = WebMessageFormat.Json)]
            WebResult GetTest();


            /// <summary>
            /// 提交任务（调用dll）
            /// </summary>
            /// <param name="fileNames">文件在服务器的路径集合</param>
            [OperationContract]
            [ServiceKnownType(typeof(string[]))]
            [WebInvoke(Method = "POST",
                UriTemplate = "task",
                BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Json,
                ResponseFormat = WebMessageFormat.Json)]
            WebResult SubmitTask(string[] fileNames);

        }


    }
}
