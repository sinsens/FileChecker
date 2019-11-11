using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChecker
{
    /// <summary>
    /// 从数据源读取并返回DataTable
    /// </summary>
    public interface IDataSourceReadAsDataTable
    {
        /// <summary>
        /// 从数据源读取并返回DataTable
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        DataTable Load(string filename);
    }
}
