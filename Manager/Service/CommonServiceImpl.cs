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

        public WebResult UploadFile(string filename, int length, Stream s)
        {
            FileStream targetStream = null;

            Stream sourceStream = s;
            string filePath = Path.Combine(System.Environment.CurrentDirectory, "WebFront", filename); //文件存放的路径写死为当前运行目录下的WebFront文件夹
            
            using (targetStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))//写文件
            {
                //read from the input stream in 6K chunks
                //and save to output stream
                const int bufferLen = 65000;
                byte[] buffer = new byte[bufferLen];
                int count = 0;

                while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                {
                    targetStream.Write(buffer, 0, count);
                }

                targetStream.Close();
                sourceStream.Close();
            }

            return WebResult.success(filePath);
        }

    }
}
