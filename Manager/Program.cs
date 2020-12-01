using Manager.Handler;
using Manager.Service;
using Manager.Util;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace Manager
{

    class Program
    {
        public static TextBoxWriter writer;

        [MTAThread]
        static void Main(string[] args)
        {
            writer =  new TextBoxWriter(Console.Out);
            Console.SetOut(writer);

            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.EngineOrDesktop); // 注册arcgis控件，注意如果在这里就注册的话，调用的dll将共享同一个arcgis环境（猜测，建议在dll自行注册，然后注释掉这行代码）
            try
            {
                Uri baseAddress = new Uri("http://127.0.0.1:7788/common"); //服务模板的CommonService的访问地址前缀
                WebServiceHost commonService = new WebServiceHost(new CommonServiceImpl(), baseAddress);//绑定CommonService服务
                commonService.Authorization.ServiceAuthorizationManager = new MyServiceAuthorizationManager(); // 配置跨域

                //为了防止413，提高request header 大小
                WebHttpBinding restBinding = new WebHttpBinding();
                restBinding.MaxReceivedMessageSize = 1048576000;
                restBinding.TransferMode = TransferMode.Streamed;
                ServiceEndpoint restService = commonService.AddServiceEndpoint(typeof(CommonService.ICommonService), restBinding, "");
                restService.Behaviors.Add(new WebHttpBehavior());

                commonService.Open();
                Console.WriteLine("服务开启成功，监听了7788端口，输入任意键结束该控制台进程");
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Web服务开启失败：{0}\r\n{1}", ex.Message, ex.StackTrace);
                Console.WriteLine("如果绑定地址失败，使用管理员模式运行该程序或者检查端口是否被占用");
                Console.ReadKey();
            }
        }
    }
}
