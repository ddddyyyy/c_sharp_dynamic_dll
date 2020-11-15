using Manager.Handler;
using Manager.Service;
using Manager.Util;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Threading;
using System.Windows.Forms;

namespace Manager
{
    public partial class Main : Form
    {

        WebServiceHost commonService;


        public Main()
        {
            InitializeComponent();
            Console.SetOut(new TextBoxWriter(textBox1)); //控制台输出重定向到textBox，DLL中的输出不行

            try
            {
                Uri baseAddress = new Uri("http://127.0.0.1:7788/common"); //服务模板的CommonService的访问地址前缀
                commonService = new WebServiceHost(new CommonServiceImpl(), baseAddress);//绑定CommonService服务
                commonService.Authorization.ServiceAuthorizationManager = new MyServiceAuthorizationManager(); // 配置跨域

                //为了防止413，提高request header 大小
                WebHttpBinding restBinding = new WebHttpBinding();
                restBinding.MaxReceivedMessageSize = 1048576000;
                restBinding.TransferMode = TransferMode.Streamed;
                ServiceEndpoint restService = commonService.AddServiceEndpoint(typeof(CommonService.ICommonService), restBinding, "");
                restService.Behaviors.Add(new WebHttpBehavior());

                commonService.Open();
                Console.WriteLine("服务开启成功，监听了7788端口");

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
