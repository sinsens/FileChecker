using System;
using System.Configuration;

namespace FileChecker
{
    internal class AppSettingsConfigReader : IConfigReader
    {
        private static AppSettingsReader appSettingsReader;

        static AppSettingsConfigReader()
        {
            appSettingsReader = new AppSettingsReader();
        }

        /// <summary>
        /// 尝试获取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyname"></param>
        /// <returns></returns>
        public T GetValue<T>(string keyname)
        {
            return (T)appSettingsReader.GetValue(keyname, typeof(T));
        }

        /// <summary>
        /// 获取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyname"></param>
        /// <param name="defualtValue">默认值</param>
        /// <returns></returns>
        public T GetValue<T>(string keyname, T defualtValue)
        {
            if (TryGetValue<T>(keyname, out T val))
            {
                return val;
            }
            return defualtValue;
        }

        /// <summary>
        /// 尝试获取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyname"></param>
        /// <param name="tVal"></param>
        /// <returns></returns>
        public bool TryGetValue<T>(string keyname, out T tVal)
        {
            tVal = default(T);
            try
            {
                tVal = (T)appSettingsReader.GetValue(keyname, typeof(T));
                return true;
            }
            catch
            {
            }
            return false;
        }
    }
}