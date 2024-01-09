using System;

namespace FluffyDuck.Util
{
    /// <summary>
    /// double / float / int 값만 사용
    /// Serializable 가능할까?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SecureVar<T>
    {
        string encryptData = string.Empty;
        T data;
        string encryptKey = string.Empty;

        public SecureVar()
        {
            string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
            SetKey(key);
        }

        public void Set(T val)
        {
            if (string.IsNullOrEmpty(encryptKey))
            {
                encryptData = Security.AESEncrypt256(val.ToString());
            }
            else
            {
                encryptData = Security.AESEncrypt256(val.ToString(), encryptKey);
            }

            data = val;
        }
        public T Get()
        {
            if (string.IsNullOrEmpty(encryptKey))
            {
                return (T)Convert.ChangeType(Security.AESDecrypt256(encryptData), typeof(T));
            }
            else
            {
                return (T)Convert.ChangeType(Security.AESDecrypt256(encryptData, encryptKey), typeof(T));
            }
        }

        public T GetUnSafeData()
        {
            return (T)Convert.ChangeType(data, typeof(T));
        }


        public void SetKey(string key)
        {
            encryptKey = key;
        }
        public bool IsSetKey()
        {
            return !string.IsNullOrEmpty(encryptKey);
        }
    }

}
