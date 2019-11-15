using System;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace FileChecker
{
    public class FileReaderFactory
    {
        private static IConfigReader configReader;
        static FileReaderFactory() {
            configReader = new AppSettingsConfigReader();
        }
        public static IFileReader CreateFileReader(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) {
                throw new FileNotFoundException("文件名为空！");
            }
            var keyValues = configReader.GetValue<string>("IFileReaders").Split('|');
            /// 文件扩展名
            string ext = filename.ToLowerInvariant().Split('.').LastOrDefault().ToLowerInvariant();
            /// 查询该扩展名是否已注册
            var regReader= keyValues.Where(o => o.Split(',').Length>1&& o.Split(',')[0].ToLower() == ext).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(regReader)) {
                var type = Type.GetType(regReader.Split(',')[1]);
                return Activator.CreateInstance(type) as IFileReader;
            }
            
            switch (ext)
            {
                case "txt":
                    return new TxtFileReader();
                default:
                    return new ExcelFileReader();
            }
        }
    }
}