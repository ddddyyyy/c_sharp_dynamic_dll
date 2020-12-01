using System;
using System.IO;
using System.Text;

namespace Manager.Util
{
    /// <summary>
    /// 将控制台的输出记录到debug日志里面
    /// </summary>
    class TextBoxWriter : TextWriter
    {
        TextWriter console;
        delegate void WriteFunc(string value);

        public TextBoxWriter(TextWriter console)
        {
            this.console = console;
        }

        /// <summary>
        /// 编码转换-UTF8
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
            //get { return Encoding.Unicode; }
        }

        /// <summary>
        /// 最低限度需要重写的方法
        /// </summary>
        public override void Write(string value)
        {
            Logger.Default.Debug(value);
            Console.SetOut(console);
            Console.Write(value);
            Console.SetOut(this);
        }

        /// <summary>
        /// 为提高效率直接处理一行的输出
        /// </summary>
        public override void WriteLine(string value)
        {
            Logger.Default.Debug(value);
            Console.SetOut(console);
            Console.WriteLine(value);
            Console.SetOut(this);
        }
    }
}

