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
                AppSettingsReader appSettingReader = new AppSettingsReader();
                Console.WriteLine("检查器：{0}", appSettingReader.GetValue("IFileChecker", typeof(string)) as string);
                Console.WriteLine("参照文件路径：{0}", appSettingReader.GetValue("FileListFilename", typeof(string)) as string);
                Console.WriteLine("按任意键开始");
                Console.ReadKey();
                Console.Clear();
                FileCheckerCreator.Create().Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
