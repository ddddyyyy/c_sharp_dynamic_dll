using System;

namespace ClassLibraryTest
{
    /// <summary>
    /// DLL暴露的接口
    /// </summary>
    public class Interface
    {
        public bool test(string[] files)
        {
            Console.WriteLine("I am ClassLibraryTest");
            Console.WriteLine("接收到了数组：{0}", string.Join(" ", files));
            return false;
        }
    }
}
