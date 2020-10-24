using System;

namespace DllTest
{
    /// <summary>
    /// DLL暴露的接口
    /// </summary>
    public class Interface
    {
        public bool test(string[] names)
        {
            Console.WriteLine("I am DllTest");
            Console.WriteLine("接收到了数组：{0}", string.Join(" ", names));
            return true;
        }

        public bool SubmitTask(string[] fileNmaes)
        {
            return true;
        }


    }
}
