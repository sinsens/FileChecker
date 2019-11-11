using System;
using System.Linq;

namespace FileChecker
{
    public class FileListFactory
    {
        public static IFileListReader CreateFileListReader(string fillanme)
        {
            switch (fillanme.ToLowerInvariant().Split('.').Last())
            {
                default:
                case "txt":
                    return new TxtFileListReader();

                case "csv":
                    return new CsvFileListReader();

                case "xls":
                case "xlsx":
                    return new ExcelFileListReader();
            }
        }
    }
}