using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChecker
{
    /// <summary>
    /// 配置读取
    /// </summary>
    public interface IConfigReader
    {
        /// <summary>
        /// 获取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyname"></param>
        /// <returns></returns>
        T GetValue<T>(string keyname);
        /// <summary>
        /// 获取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyname"></param>
        /// <param name="defualtValue">默认值</param>
        /// <returns></returns>
        T GetValue<T>(string keyname, T defualtValue);

        /// <summary>
        /// 获取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyname"></param>
        /// <param name="tVal"></param>
        /// <returns></returns>
        bool TryGetValue<T>(string keyname, out T tVal);
    }
}
