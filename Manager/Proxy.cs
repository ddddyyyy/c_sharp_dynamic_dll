using System;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Windows.Forms;

namespace Manager
{

    /// <summary>
    /// 代理类
    /// </summary>
    class ProxyObject : MarshalByRefObject
    {
        /// <summary>
        /// DLL的存放路径，这里使用exe路径下的dynamic_dll文件夹
        /// </summary>
        private static string DLL_ROOT_PATH = Path.Combine(Environment.CurrentDirectory, "dynamic_dll");


        Assembly assembly = null;

        public void resetConsoleOutput(Util.TextBoxWriter writer)
        {
            Console.SetOut(writer);
        }

        /// <summary>
        /// 通过路径加载dll
        /// </summary>
        /// <param name="name">dll的名称</param>
        public void LoadAssembly(string name)
        {
            if (null == name) { throw new FaultException("DLL 名不能为"); }
            assembly = Assembly.LoadFile(Path.Combine(DLL_ROOT_PATH, name + ".dll"));
        }
        /// <summary>
        /// 代理执行方法
        /// </summary>
        /// <param name="fullClassName">要加载的类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数列表</param>
        /// <returns>代理的方法的返回值</returns>
        public object Invoke(string fullClassName, string methodName, params Object[] args)
        {
            if (assembly == null)
            {
                Console.WriteLine("assembly is null");
                return false;
            }
            Type tp = assembly.GetType(fullClassName);
            if (tp == null)
            {
                Console.WriteLine("class is null");
                return false;
            }
            MethodInfo method = tp.GetMethod(methodName);
            if (method == null)
            {
                Console.WriteLine("method is null");
                return false;
            }
            Object obj = Activator.CreateInstance(tp);
            object res = method.Invoke(obj, args);
            if (obj.GetType().IsSubclassOf(typeof(Form)))
            {
                (obj as Form).Close(); // 释放新建的窗口资源
            }
            if (typeof(bool) == res.GetType()) // bool值的返回值封装成字符串数组
            {
                return new string[] { Convert.ToString(res) };
            }
            return res;

        }
    }
    class Proxy
    {

        /// <summary>
        /// 代理dll的某个方法
        /// </summary>
        /// <param name="args">参数：字符串列表</param>
        /// <param name="dllName">dll名字</param>
        /// <param name="method">要执行的方法名</param>
        /// <returns>string数组</returns>
        public static string[] invoke(string[] args, string dllName, string method)
        {
            string callingDomainName = AppDomain.CurrentDomain.FriendlyName;
            AppDomain ad = AppDomain.CreateDomain("DLL dynamic Load - " + DateTime.Now);
            ProxyObject obj = (ProxyObject)ad.CreateInstanceFromAndUnwrap(callingDomainName, "Manager.ProxyObject");
            obj.resetConsoleOutput(Program.writer); // 重定向控制台输出到文本框
            obj.LoadAssembly(dllName);
            var res = obj.Invoke(dllName + ".Interface", method, new object[] { args });
            AppDomain.Unload(ad);
            if (typeof(string[]) == res.GetType())
            {
                return res as string[];
            }
            else if (typeof(bool) == res.GetType())
            {
                return new string[] { "reflection occur error" };
            }
            else
            {
                return new string[] { "unknow return type" };
            }

        }
    }
}