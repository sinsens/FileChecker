using System;
using System.Collections.Generic;

namespace FileChecker
{
    public interface IFileReader:IFileReadAsDataTable
    {
        IList<string> Load(string filename, int skipRow, int valueColumnIndex);
    }
}