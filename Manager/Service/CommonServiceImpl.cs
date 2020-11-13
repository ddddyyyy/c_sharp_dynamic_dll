using Manager.Handler;
using Manager.Model;
using Manager.Util;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
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
                MultipartParser parser = new MultipartParser(s);
                if (parser != null && parser.Success)
                {
                    targetStream.Write(parser.FileContents, 0, parser.FileContents.Length);
                }

                targetStream.Close();
                sourceStream.Close();
            }

            if (targetStream == null)
            {
                return WebResult.fail("文件上传失败");
            }
            else
            {
                return WebResult.success(filePath);
            }
        }

        public  WebResult Search(string[] args, string dllName)
        {
            if (Proxy.invokeTestMethod(dllName, args))
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
