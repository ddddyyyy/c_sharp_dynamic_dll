using Manager.Service;
using System;
using System.ServiceModel.Web;

namespace Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Uri baseAddress = new Uri("http://127.0.0.1:7788/common"); //服务模板的CommonService的访问地址前缀
                WebServiceHost commonService = new WebServiceHost(new CommonServiceImpl(), baseAddress);//绑定CommonService服务
                commonService.Open();
                Console.WriteLine("服务开启成功");
                Console.WriteLine("输入任意键关闭程序");
                Console.ReadKey();
                commonService.Close();
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
