using System;
using System.Collections.Generic;

namespace FileChecker
{
    public interface IFileListReader
    {
        IList<string> Load(string filename, int skipRow, int valueColumnIndex);
    }
}