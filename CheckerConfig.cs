using System;

namespace FileChecker
{
    public class CheckerConfig
    {
        private static CheckerConfig checkerConfig;

        static CheckerConfig()
        {
            /// 读取配置
            appSettingsReader = new AppSettingsConfigReader();
        }

        public static IConfigReader appSettingsReader { get; }

        /// <summary>
        /// 获取实例
        /// </summary>
        public static CheckerConfig Instance
        {
            get
            {
                if (checkerConfig == null)
                    checkerConfig = new CheckerConfig();
                return checkerConfig;
            }
        }

        /// <summary>
        /// 基目录
        /// </summary>
        public string BaseDirectory { get => appSettingsReader.GetValue<string>("BaseDirectory"); }

        /// <summary>
        /// 要对比的 Excel 标题名称
        /// </summary>
        public string ExcelFetchKeyname
        {
            get => appSettingsReader.GetValue<string>("ExcelFetchKeyname");
        }

        /// <summary>
        /// Excel 文件路径
        /// </summary>
        public string ExcelFileName
        {
            get => appSettingsReader.GetValue<string>("ExcelFilename", "excelfile.xlsx");
        }

        /// <summary>
        /// 文件列表路径
        /// </summary>
        public string FileListFilename { get => appSettingsReader.GetValue<string>("FileListFilename", "flist.txt"); }

        /// <summary>
        /// 跳过的行数
        /// </summary>
        public int SkipRow { get => appSettingsReader.GetValue<int>("SkipRow", 0); }

        /// <summary>
        /// 值在第几列
        /// </summary>
        public int ValueColumnIndex { get => appSettingsReader.GetValue<int>("ValueColumnIndex", 0); }
    }
}