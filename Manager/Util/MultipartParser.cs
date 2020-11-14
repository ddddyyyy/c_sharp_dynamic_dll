using System;
using System.IO;
using System.Text;

namespace Manager.Util
{
    /// <summary>
    /// 用于去除WebKitxxxx 的工具类
    /// </summary>
    public class MultipartParser
    {
        public MultipartParser(Stream stream)
        {
            this.Parse(stream, Encoding.UTF8);
        }

        /// <summary>
        /// 解析文件流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        private void Parse(Stream stream, Encoding encoding)
        {
            this.Success = false;

            byte[] contentTypeBytes = encoding.GetBytes("Content-Type:");
            byte[] wrapBytes = encoding.GetBytes("\r\n");
            byte[] wrapsBytes = encoding.GetBytes("\r\n\r\n");

            // Read the stream into a byte array
            byte[] data = ToByteArray(stream);


            // The first line should contain the delimiter \r\n
            int delimiterEndIndex = IndexOf(data, wrapBytes, 0);

            if (delimiterEndIndex > -1)
            {
                
                byte[] delimiterBytes = new byte[delimiterEndIndex + 2];
                delimiterBytes[0] = (byte)'\r';
                delimiterBytes[1] = (byte)'\n';
                // get the last line
                Buffer.BlockCopy(data, 0, delimiterBytes, 2, delimiterEndIndex);

                int firstContentTpyeIndex = IndexOf(data, contentTypeBytes, delimiterEndIndex + 2);

                if (firstContentTpyeIndex > -1)
                {
                    int firstDoubleWrapIndex = IndexOf(data, wrapsBytes, firstContentTpyeIndex + contentTypeBytes.Length);

                    /// last line must contain the \r\nxxxxxxxxxxxx
                    /// 这里可优化，从最后面找起，假设只有一段
                    int endIndex = IndexOf(data, delimiterBytes, firstDoubleWrapIndex + 4);

                    int contentLength = endIndex - firstDoubleWrapIndex - 4;

                    // Extract the file contents from the byte array
                    byte[] fileData = new byte[contentLength];

                    Buffer.BlockCopy(data, firstDoubleWrapIndex + 4, fileData, 0, contentLength);

                    this.FileContents = fileData;
                    this.Success = true;
                }



            }
        }

        private int IndexOf(byte[] searchWithin, byte[] serachFor, int startIndex)
        {
            int index = 0;
            int startPos = Array.IndexOf(searchWithin, serachFor[0], startIndex);

            if (startPos != -1)
            {
                while ((startPos + index) < searchWithin.Length)
                {
                    if (searchWithin[startPos + index] == serachFor[index])
                    {
                        index++;
                        if (index == serachFor.Length)
                        {
                            return startPos;
                        }
                    }
                    else
                    {
                        startPos = Array.IndexOf<byte>(searchWithin, serachFor[0], startPos + index);
                        if (startPos == -1)
                        {
                            return -1;
                        }
                        index = 0;
                    }
                }
            }

            return -1;
        }

        private byte[] ToByteArray(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public bool Success
        {
            get;
            private set;
        }

        public byte[] FileContents
        {
            get;
            private set;
        }
    }

}
