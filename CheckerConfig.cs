using System;
using System.Configuration;
using System.Reflection;


namespace FileChecker
{
    /// <summary>
    /// 检查类型：
    /// FileStatus：文件是否存在
    /// ExcelColumn：Excel 的某字段是否匹配
    /// </summary>
    public enum CheckType
    {
        FileStatus = 0,
        ExcelColumn = 1
    }

    public class CheckerConfig
    {
        public static AppSettingsReader appSettingsReader { get; }
        private static IFileChecker _FileChecker;
        static CheckerConfig()
        {
            /// 读取配置
            appSettingsReader = new AppSettingsReader();
            try
            {
                Instance.BaseDirectory = appSettingsReader.GetValue("BaseDirectory", typeof(string)) as string;
                Instance.SkipRow = (int)appSettingsReader.GetValue("SkipRow", typeof(int));
                Instance.ValueColumnIndex = (int)appSettingsReader.GetValue("ValueColumnIndex", typeof(int));
                Instance.FileListFilename = appSettingsReader.GetValue("FileListFilename", typeof(string)) as string;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(string.Format("配置参数有误，参数名称：{0}，错误信息：{1}", ex.ParamName, ex.Message), ex.InnerException);
            }
            catch (TypeLoadException ex)
            {
                throw new TypeLoadException(string.Format("无法加载指定的类型，类型名称：{0}，错误信息：{1}", ex.TypeName, ex.Message), ex.InnerException);
            }
            catch
            {
                throw;
            }

        }
        /// <summary>
        /// 基目录
        /// </summary>
        public string BaseDirectory { get; set; }

        /// <summary>
        /// 检查器
        /// </summary>
        public IFileChecker FileChecker
        {
            get
            {
                if (_FileChecker == null)
                {
                    // 使用反射构建 IFileChecker 对象
                    string checkerClassName = appSettingsReader.GetValue("IFileChcker", typeof(string)) as string;
                    Type checker = Type.GetType(checkerClassName);
                    _FileChecker = Activator.CreateInstance(checker) as IFileChecker;
                }
                return _FileChecker;
            }
            private set { }
        }

        /// <summary>
        /// 文件列表路径
        /// </summary>
        public string FileListFilename { get; set; }

        /// <summary>
        /// 跳过的行数
        /// </summary>
        public int SkipRow { get; set; }

        /// <summary>
        /// 值在第几列
        /// </summary>
        public int ValueColumnIndex { get; set; }

        /// <summary>
        /// Excel 文件路径
        /// </summary>
        public string ExcelFileName { get {
                return appSettingsReader.GetValue("ExcelFilename", typeof(string)) as string;
            }set { } }

        /// <summary>
        /// 要对比的 Excel 标题名称
        /// </summary>
        public string ExcelFetchKeyname
        {
            get => appSettingsReader.GetValue("ExcelFetchKeyname", typeof(string)) as string;
        }

        private static CheckerConfig checkerConfig;
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
    }
}