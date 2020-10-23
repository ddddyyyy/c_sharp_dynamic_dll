using System.Runtime.Serialization;

namespace Manager.Model
{
    /// <summary>
    /// Web请求返回的通用实体
    /// </summary>
    [DataContract]
    public class WebResult
    {
        /// <summary>
        /// 状态码，可用于区别不同的出错情况
        /// </summary>
        [DataMember]
        public int code;

        /// <summary>
        /// 请求返回信息
        /// </summary>
        [DataMember]
        public string msg;

        /// <summary>
        /// 请求返回数据
        /// </summary>
        [DataMember]
        public object data;


        /// <summary>
        /// 服务调用成功
        /// </summary>
        /// <param name="data">返回前端的数据（如果有）</param>
        /// <returns></returns>
        public static WebResult success(object data)
        {
            WebResult result = new WebResult
            {
                code = 0,
                data = data,
                msg = "success"

            };
            return result;
        }


        /// <summary>
        /// 服务调用失败
        /// </summary>
        /// <param name="msg">出错信息</param>
        /// <returns></returns>
        public static WebResult fail(string msg)
        {
            WebResult result = new WebResult
            {
                code = -1,
                msg = msg

            };
            return result;
        }

    }
}
