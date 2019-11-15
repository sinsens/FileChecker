using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace FileChecker
{
    public class TxtFileReader : IFileReader
    {
        public IList<string> Load(string filename, int skipRow, int valueColumnIndex)
        {
            List<string> list = new List<string>();
            var lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i < skipRow) continue;
                list.Add(lines[i].Split(',')[valueColumnIndex]);
                lines[i] = null;
            }
            return list;
        }

        public DataTable Load(string filename)
        {
            DataTable dt = new DataTable();
            var lines = File.ReadAllLines(filename);
            /// 表头
            foreach (var item in lines[0].Split(','))
            {
                dt.Columns.Add(item);
            }
            DataRow dr;
            for (int i = 1; i < lines.Length; i++)
            {
                dr = dt.NewRow();
                var items = lines[i].Split(',');
                for (int j = 0; j < items.Length; j++)
                {
                    dr[j] = items[j];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}