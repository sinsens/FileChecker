using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChecker
{
    /// <summary>
    /// 通用扩展方法库
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 写入控制台
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog(this string msg)
        {
            Console.WriteLine("{0}\t{1}", DateTime.Now, msg);
        }

        /// <summary>
        /// 写入文件记录
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="content"></param>
        public static void WriteToFile(this FileStream fs, string content)
        {
            fs.Seek(0, SeekOrigin.End);
            byte[] temp = Encoding.UTF8.GetBytes(content);
            fs.Write(temp, 0, temp.Length);
        }
    }
}
