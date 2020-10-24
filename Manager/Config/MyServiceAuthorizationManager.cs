using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Manager.Handler
{
    /// <summary>
    /// 设置跨域，线上环境可能需要重新配置
    /// </summary>
    public class MyServiceAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {

            HttpResponseMessageProperty prop = new HttpResponseMessageProperty();
            prop.Headers.Add("Access-Control-Allow-Origin", "*");
            prop.Headers.Add("Referer", "http://127.0.0.1:7788");
            operationContext.OutgoingMessageProperties.Add(HttpResponseMessageProperty.Name, prop);
            return true;
        }
    }

}
