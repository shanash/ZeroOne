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
        string encryptKey = string.Empty;
        T data = default;

        public SecureVar(T val = default)
        {
            CreateKey();
            if (!val.Equals(default))
            {
                Set(val);
            }
        }

        public SecureVar(SecureVar<T> val) : this(val != null ? val.Get() : default) { }

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

        public void CreateKey()
        {
            string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
            encryptKey = key;
        }

        public bool IsExistKey()
        {
            return !string.IsNullOrEmpty(encryptKey);
        }

        public static SecureVar<T>[] CreateSecureArray(T[] data)
        {
            var secureArray = new SecureVar<T>[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                secureArray[i] = new SecureVar<T>(data[i]);
            }
            return secureArray;
        }
    }
}
