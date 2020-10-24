using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Handler
{
    /// <summary> 
    /// WCF服务端异常处理器 
    /// </summary> 
    public class ExceptionHandler : IErrorHandler
    {
        #region IErrorHandler Members

        bool IErrorHandler.HandleError(Exception error)
        {
            return true;
        }

        void IErrorHandler.ProvideFault(Exception ex, MessageVersion version, ref Message msg)
        {
            string err = string.Format("调用WCF接口 '{0}' 出错，详情：{1}\r\n", ex.TargetSite.Name, ex.StackTrace);
            Console.WriteLine(err);
            //这里仅仅输出到控制台，具体可以结合日志管理框架扩展
        }

        #endregion
    }
}
