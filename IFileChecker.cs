using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChecker
{
    public interface IFileChecker
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        IFileChecker Instance();

        /// <summary>
        /// 开始执行
        /// </summary>
        void Start();
    }
}
