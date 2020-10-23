using Manager.Model;
using System.ServiceModel;
using System.ServiceModel.Activation;
using static Manager.Service.CommonService;

namespace Manager.Service
{
    /// <summary>
    /// 服务模板实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    class CommonServiceImpl : ICommonService
    {

        public WebResult GetTest()
        {
            return WebResult.success("!!!!");
        }

        public WebResult SubmitTask(string[] fileNames)
        {
            WebResult result = new WebResult();
            result.data = fileNames;
            return result;
        }
    }
}
