using System;
using System.Collections.Generic;
using ExcelDataReader;
using System.Linq;
using System.Data;
using System.IO;

namespace FileChecker
{
    public class ExcelFileReader : IFileReader
    {
        private IExcelDataReader GetReader(string filename)
        {
            string filext = filename.ToLowerInvariant().Split('.').Last();
            if (!"xls,xlsx,csv".Contains(filext))
            {
                throw new NotSupportedException("只支持 *.xlsx, *.xls, *.csv 类型的文件");
            }
            IExcelDataReader reader;
            if (".xlsx".Contains(filext))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(File.OpenRead(filename));
            }
            else
            {
                reader = ExcelReaderFactory.CreateBinaryReader(File.OpenRead(filename));
            }
            return reader;
        }

        /// <summary>
        /// 读取单列
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="skipRow"></param>
        /// <param name="valueColumnIndex"></param>
        /// <returns></returns>
        public IList<string> Load(string filename, int skipRow, int valueColumnIndex)
        {
            List<string> list = new List<string>();
            using (var reader = GetReader(filename))
            {
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

        /// <summary>
        /// 读取并返回 DataTable
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public DataTable Load(string filename)
        {
            using (var reader = GetReader(filename))
            {
                DataTable dt = new DataTable();
                reader.Read();
                /// 表头
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dt.Columns.Add(reader.GetString(i));
                }
                /// 具体内容
                DataRow dr;
                while (reader.Read())
                {
                    dr = dt.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dr[i] = reader.GetString(i);
                    }
                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }
    }
}