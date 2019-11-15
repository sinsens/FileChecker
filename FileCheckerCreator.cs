using System;
using System.Configuration;

namespace FileChecker
{
    internal class FileCheckerCreator
    {
        /// 读取配置
        private static AppSettingsReader appSettingsReader;

        private static IFileChecker fileChecker;

        static FileCheckerCreator()
        {
            appSettingsReader = new AppSettingsReader();
        }

        /// <summary>
        /// 检查器
        /// </summary>
        public static IFileChecker Create()
        {
            if (fileChecker == null)
            {
                // 使用反射构建 IFileChecker 对象
                string checkerClassName = appSettingsReader.GetValue("IFileChecker", typeof(string)) as string;
                Type checker = Type.GetType(checkerClassName);
                fileChecker = Activator.CreateInstance(checker) as IFileChecker;
            }
            return fileChecker;
        }
    }
}