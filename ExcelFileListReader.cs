using System;
using System.Collections.Generic;
using ExcelDataReader;

namespace FileChecker
{
    public class ExcelFileListReader : IFileListReader
    {
        public IList<string> Load(string filename, int skipRow, int valueColumnIndex)
        {
            List<string> list = new List<string>();
            using (var fileStream = System.IO.File.OpenRead(filename))
            {
                IExcelDataReader reader;
                if (filename.ToLowerInvariant().EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                }
                else if (filename.ToLowerInvariant().EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(fileStream);
                }
                else
                {
                    throw new NotSupportedException("只支持 *.xlsx, *.xls 类型的文件");
                }
                if (reader.FieldCount <= valueColumnIndex)
                {
                    throw new IndexOutOfRangeException("值下标超出了合理范围！");
                }
                int row = 0;
                while (reader.Read())
                {
                    if (row++ < skipRow) continue;
                    list.Add(reader.GetString(valueColumnIndex));
                }
            }
            return list;
        }
    }
}