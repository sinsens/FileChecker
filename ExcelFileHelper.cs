using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ExcelDataReader;

namespace FileChecker
{
    public class ExcelReadHelper : IDataSourceReadAsDataTable
    {
        private static char splitChar = ',';
        public DataTable Load(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
            {
                throw new FileNotFoundException("文件不存在：" + filename);
            }
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
                DataTable  dt = new DataTable();
                reader.Read();
                
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dt.Columns.Add(reader.GetString(i));
                }
                DataRow dr;
                while (reader.Read())
                {
                    dr = dt.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dr[i]=reader.GetString(i);
                    }
                    dt.Rows.Add(dr);
                }

                return dt;
            }

        }
    }
}
