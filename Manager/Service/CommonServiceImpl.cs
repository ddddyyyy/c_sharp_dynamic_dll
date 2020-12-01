using Manager.Handler;
using Manager.Model;
using Manager.Util;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using static Manager.Service.CommonService;

namespace Manager.Service
{
    /// <summary>
    /// 服务模板实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple, // 并发模式
        IncludeExceptionDetailInFaults = true)]
    [ExceptionBehaviourAttribute(typeof(ExceptionHandler))]//配置异常处理器
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    class CommonServiceImpl : ICommonService
    {
        /// <summary>
        /// 任务加锁失败时的默认值，由于snowflake的机器id为1，所以不可能生成0
        /// </summary>
        static long unknown = 0;

        static ConcurrentDictionary<string, long> mutx = new ConcurrentDictionary<string, long>();

        public Stream GetHtml(string path)
        {
            string result = File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, @"WebFront\" + path + ".html"));
            byte[] resultBytes = Encoding.UTF8.GetBytes(result);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return new MemoryStream(resultBytes);
        }

        public WebResult SubmitTask(string[] fileNames, string dllName, string methodName)
        {
            return LockForDLL(dllName, methodName, fileNames);
        }

        public WebResult UploadFile(string filename, int length, Stream s)
        {
            FileStream targetStream;

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

        public WebResult Search(string[] args, string dllName, string methodName)
        {
            return LockForDLL(dllName, methodName, args);
        }


        /// <summary>
        /// 执行dll的时候对dll进行加锁，确保同一时间段内dll只能运行一个任务
        /// </summary>
        private WebResult LockForDLL(string dllName, string methodName, string[] args)
        {
            long oldVal = unknown;
            int retry = 10;
            while (retry > 0 && mutx.TryGetValue(dllName, out oldVal))
            {
                Console.WriteLine("DLL被占用");
                retry -= 1;
                Thread.Sleep(5000);
            }
            if (retry <= 0)
            {
                return WebResult.fail("有其它任务正在进行");
            }
            else
            {
                long id = Snowflake.Instance().GetId();
                if (mutx.TryAdd(dllName, id)) // 试图添加为自己的任务
                {
                    string[] res = null;
                    try
                    {
                        ProxyForm window = new ProxyForm(dllName, methodName, args); // 由于依赖于arcgis的dll需要在单线程（STA）且需要具有UI窗体，因此每次代理都将新建一个代理窗口
                        Thread thread = new Thread(() =>
                        {
                            window.Show();
                            window.Close();
                            res = window.res.Clone() as string[]; // 获取结果
                            //System.Windows.Threading.Dispatcher.Run();
                        });
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                        thread.Join();//等待UI线程结束
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("任务{0}失败", dllName);
                        Console.WriteLine("{0}", e);
                    }
                    retry = 3;
                    while (retry >= 0 && !mutx.TryRemove(dllName, out oldVal))
                    {
                        retry -= 1;
                        Thread.Sleep(100);
                    }
                    if (oldVal != id)
                    {
                        Console.WriteLine("{0}移除锁失败", dllName);
                    }
                    if (null != res)
                        return WebResult.success(res);
                    else
                        return WebResult.fail("任务执行失败");
                }
                else
                {
                    return WebResult.fail("有其它任务正在进行");
                }
            }
        }

    }
}
