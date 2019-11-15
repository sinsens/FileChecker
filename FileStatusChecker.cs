using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChecker
{
    public class FileStatusChecker : IDisposable, IFileChecker
    {
        public static Dictionary<string, bool> Files;
        private static readonly CheckerConfig checkerConfig;
        private static readonly object lockObj;
        private static FileStatusChecker _instance;
        private static FileStream fetchFileStream;
        private static FileStream missFileStream;
        private static FileStream checkedFileStream;

        //是否回收完毕
        private bool _disposed;

        private Stopwatch stopwatch = new Stopwatch();

        static FileStatusChecker()
        {
            lockObj = new object();
            Files = new Dictionary<string, bool>();
            checkerConfig = CheckerConfig.Instance;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        public static FileStatusChecker Instance
        {
            get
            {
                if (_instance == null) _instance = new FileStatusChecker();
                return _instance;
            }
        }

        /// <summary>
        /// 检查文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool CheckExists(string filename)
        {
            if (Files.ContainsKey(filename)) return Files[filename];
            bool exist = File.Exists(string.Format("{0}{1}", checkerConfig.BaseDirectory, filename));
            lock (lockObj)
            {
                Files.Add(filename, exist);
                checkedFileStream.WriteToFile(string.Format("{0},{1}\r\n", filename, exist ? "1" : "0", filename));
                (exist ? fetchFileStream : missFileStream).WriteToFile(string.Format("{0}\r\n", filename));
            }
            return exist;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            try
            {
                ("文件名列表文件名：" + checkerConfig.FileListFilename).WriteLog();
                ("文件类型为：" + checkerConfig.FileListFilename.ToLowerInvariant().Split('.').Last()).WriteLog();
                ("读取文件列表中。。。").WriteLog();
                stopwatch.Start();
                var filelist = FileReaderFactory.CreateFileReader(checkerConfig.FileListFilename).Load(checkerConfig.FileListFilename, checkerConfig.SkipRow, checkerConfig.ValueColumnIndex);
                string.Format("读取完毕，共 {0} 行记录，耗时 {1} ms", filelist.Count, stopwatch.ElapsedMilliseconds).WriteLog();
                stopwatch.Restart();
                "开始检查。。。".WriteLog();

                /// 打开日志文件写入流
                checkedFileStream = File.OpenWrite(string.Format("checked_files_{0}.txt", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
                fetchFileStream = File.OpenWrite(string.Format("fetch_files_{0}.txt", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
                missFileStream = File.OpenWrite(string.Format("miss_files_{0}.txt", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
                int current = 0;
                int total = filelist.Count;
                foreach (var item in filelist)
                {
                    current += 1;
                    CheckExists(item);
                    if (current % (total / 10) == 0)
                        string.Format("当前进度：{0}%", Math.Round((double)current / total * 100, 2)).WriteLog();
                }
                /*
                Parallel.ForEach(filelist, item =>
                 {
                     CheckExists(item);
                 });*/
            }
            catch (Exception ex)
            {
                string.Format("发生错误：{0}", ex.Message).WriteLog();
            }
            finally
            {
                if (checkedFileStream != null)
                    checkedFileStream.Close();
                if (fetchFileStream != null)
                    fetchFileStream.Close();
                if (missFileStream != null)
                    missFileStream.Close();
            }
            string.Format("检查结束，去重后总数{3}，有效文件 {0}， 无效文件 {1}，耗时 {2} ms", Files.Count(item => item.Value), Files.Count(item => !item.Value), stopwatch.ElapsedMilliseconds, Files.Count).WriteLog();
            Console.WriteLine("具体结果已写入：checked_files.txt , fetch_files.txt, miss_files.txt 这三个文件。");
            Console.WriteLine("按任意键退出");
            stopwatch.Stop();
            Console.Read();
        }


        public void WriteResultToFile()
        {
            using (var fileStream = File.OpenWrite("out.txt"))
            {
                Parallel.ForEach(Files, item =>
                {
                    File.WriteAllText("out.txt", string.Format("{0},{1}", item.Key, item.Value ? "1" : "0"));
                    if (item.Value)
                    {
                        File.WriteAllText("exist.txt", string.Format("{0}", item.Key));
                    }
                    else
                    {
                        File.WriteAllText("notfound.txt", string.Format("{0}", item.Key));
                    }
                });
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                if (checkedFileStream != null)
                    checkedFileStream.Close();
                if (fetchFileStream != null)
                    fetchFileStream.Close();
                if (missFileStream != null)
                    missFileStream.Close();
            }
            _disposed = true;
        }

        IFileChecker IFileChecker.Instance()
        {
            return Instance;
        }
    }
}