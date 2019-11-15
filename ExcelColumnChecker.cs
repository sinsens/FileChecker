using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FileChecker
{
    /// <summary>
    /// 用于检查 Excel 表格中和文件列表相匹配的数据行
    /// </summary>
    public class ExcelColumnChecker:IDisposable, IFileChecker
    {
        /// <summary>
        /// 配置
        /// </summary>
        private static readonly CheckerConfig checkerConfig;

        private static ExcelColumnChecker _instance;
        private static FileStream fetchFileStream;
        private Stopwatch stopwatch = new Stopwatch();
        public static Dictionary<string, bool> Files;
        /// <summary>
        /// 回收状态
        /// </summary>
        private bool _disposed;

        static ExcelColumnChecker()
        {
            checkerConfig = CheckerConfig.Instance;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        public static ExcelColumnChecker Instance
        {
            get
            {
                if (_instance == null) _instance = new ExcelColumnChecker();
                return _instance;
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start() {
            try
            {
                ("文件名列表文件名：" + checkerConfig.FileListFilename).WriteLog();
                ("文件类型为：" + checkerConfig.FileListFilename.ToLowerInvariant().Split('.').Last()).WriteLog();
                ("读取文件列表中。。。").WriteLog();
                stopwatch.Start();

                var filelist = FileReaderFactory.CreateFileReader(checkerConfig.FileListFilename).Load(checkerConfig.FileListFilename, checkerConfig.SkipRow, checkerConfig.ValueColumnIndex);
                string.Format("读取完毕，共 {0} 行记录，耗时 {1} ms", filelist.Count, stopwatch.ElapsedMilliseconds).WriteLog();
                stopwatch.Restart();

                ("读取 Excel 文件中。。。").WriteLog();
                var dt = FileReaderFactory.CreateFileReader(checkerConfig.ExcelFileName).Load(checkerConfig.ExcelFileName);
                string.Format("读取完毕，共 {0} 行记录，耗时 {1} ms", dt.Rows.Count, stopwatch.ElapsedMilliseconds).WriteLog();

                "开始检查。。。".WriteLog();
                /// 打开日志文件写入流
                fetchFileStream = File.OpenWrite(string.Format("fetch_files_{0}.txt", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
                int current = 0;
                int total = filelist.Count;
                int colcount = dt.Columns.Count;
                string temp = string.Empty;
                StringBuilder tmpStr = new StringBuilder(2048);
                foreach (var item in filelist)
                {
                    current += 1;
                    var fetchDataRows = dt.Select(string.Format("{0}='{1}'", checkerConfig.ExcelFetchKeyname, item));
                    foreach (var row in fetchDataRows)
                    {
                        for (int i = 0; i < colcount; i++)
                        {
                            if (i < colcount-1)
                            {
                                tmpStr.AppendFormat("{0},", row[i]);
                            }
                            else {
                                tmpStr.AppendFormat("{0}\r\n ", row[i]);
                            }
                        }
                        
                    }
                    if (current % (total / 10) == 0) {
                        string.Format("当前进度：{0}%", Math.Round((double)current / total * 100, 2)).WriteLog();
                        fetchFileStream.WriteToFile(tmpStr.ToString());
                        tmpStr.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                string.Format("发生错误：{0}", ex.Message).WriteLog();
            }
            finally
            {
                if (fetchFileStream != null)
                    fetchFileStream.Close();
            }
            string.Format("检查结束，去重后总数{3}，有效文件 {0}， 无效文件 {1}，耗时 {2} ms", Files.Count(item => item.Value), Files.Count(item => !item.Value), stopwatch.ElapsedMilliseconds, Files.Count).WriteLog();
            Console.WriteLine("具体结果已写入：checked_files.txt , fetch_files.txt, miss_files.txt 这三个文件。");
            Console.WriteLine("按任意键退出");
            stopwatch.Stop();
            Console.Read();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                if (fetchFileStream != null)
                    fetchFileStream.Close();
            }
            _disposed = true;
        }

        IFileChecker IFileChecker.Instance()
        {
            return Instance;
        }
    }
}