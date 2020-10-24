using Manager.Handler;
using Manager.Model;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using static Manager.Service.CommonService;

namespace Manager.Service
{
    /// <summary>
    /// 服务模板实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Single,
        IncludeExceptionDetailInFaults = true)]
    [ExceptionBehaviourAttribute(typeof(ExceptionHandler))]//配置异常处理器
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    class CommonServiceImpl : ICommonService
    {

        public Stream GetHtml()
        {

            string result = File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, @"WebFront\Manager.html"));
            byte[] resultBytes = Encoding.UTF8.GetBytes(result);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return new MemoryStream(resultBytes);


        }

        public WebResult SubmitTask(string[] fileNames, string dllName)
        {
            if (Proxy.invokeTestMethod(dllName, fileNames))
            {
                return WebResult.success(null);
            }
            else
            {
                return WebResult.fail("执行任务失败");
            }
        }
    }
}
