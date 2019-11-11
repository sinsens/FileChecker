using System;
using System.Collections.Generic;
using System.IO;

namespace FileChecker
{
    public class CsvFileListReader : IFileListReader
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
    }
}