using System;
using System.Configuration;

namespace FileChecker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("检查器：{0}", CheckerConfig.appSettingsReader.GetValue("IFileChcker", typeof(string)) as string);
                Console.WriteLine("参照文件路径：{0}", CheckerConfig.Instance.FileListFilename);
                Console.WriteLine("按任意键开始");
                Console.ReadKey();
                Console.Clear();
                CheckerConfig.Instance.FileChecker.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}