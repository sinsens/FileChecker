using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileChecker
{
    public class TxtFileListReader : IFileListReader
    {
        public IList<string> Load(string filename, int skipRow, int valueColumnIndex)
        {
            List<string> list = new List<string>();
            var lines = File.ReadAllLines(filename, Encoding.UTF8);
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