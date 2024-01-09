using System;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyDuck.Util
{
    /// <summary>
    /// 본 클래스는 로컬 데이터 저장을 위한 클래스
    /// 서버에 저장할 필요는 없고, 클라이언트 로컬에 저장되어야 할 일부 데이터를 저장하기 위한 기능을 함.
    /// PlayerPref 사용.
    /// </summary>
    public class GameConfig : Singleton<GameConfig>
    {

        Dictionary<string, object> Key_Values;

        GameConfig() { }

        protected override void Initialize()
        {
            Key_Values = new Dictionary<string, object>();
        }

        public void SetGameConfig<T>(GAME_CONFIG_KEY key, object val)
        {
            var key_str = key.ToString();
            if (Key_Values.ContainsKey(key_str))
            {
                Key_Values[key_str] = val;
            }
            else
            {
                Key_Values.Add(key_str, val);
            }

            Type t = typeof(T);

            if (t == typeof(int))
            {
                SaveIntValue(key_str, (int)val);
            }
            else if (t == typeof(float))
            {
                SaveFloatValue(key_str, (float)val);
            }
            else if (t == typeof(string))
            {
                SaveStringValue(key_str, (string)val);
            }
        }
        void SaveIntValue(string key, int val)
        {
            PlayerPrefs.SetInt(key, (int)val);
            PlayerPrefs.Save();
        }
        void SaveFloatValue(string key, float val)
        {
            PlayerPrefs.SetFloat(key, val);
            PlayerPrefs.Save();
        }

        void SaveStringValue(string key, string val)
        {
            PlayerPrefs.SetString(key, val);
            PlayerPrefs.Save();
        }

        object GetIntValue(string key, int default_value)
        {
            return PlayerPrefs.GetInt(key, default_value);
        }
        object GetFloatValue(string key, float default_value)
        {
            return PlayerPrefs.GetFloat(key, default_value);
        }
        object GetStringValue(string key, string default_value)
        {
            return PlayerPrefs.GetString(key, default_value);
        }

        public T GetGameConfigValue<T>(GAME_CONFIG_KEY key, object default_value)
        {
            if (Key_Values.ContainsKey(key.ToString()))
            {
                return (T)Key_Values[key.ToString()];
            }
            else
            {
                object result = null;
                Type t = typeof(T);
                if (t == typeof(int))
                {
                    result = GetIntValue(key.ToString(), (int)default_value);
                }
                else if (t == typeof(float))
                {
                    result = GetFloatValue(key.ToString(), (float)default_value);
                }
                else if (t == typeof(string))
                {
                    result = GetStringValue(key.ToString(), (string)default_value);
                }
                if (result != null)
                {
                    Key_Values.Add(key.ToString(), result);
                }
                return (T)result;
            }
        }

        public bool HasKey(GAME_CONFIG_KEY key)
        {
            if (Key_Values.ContainsKey(key.ToString()))
            {
                return true;
            }

            return PlayerPrefs.HasKey(key.ToString());
        }

        public void RemoveGameconfig(GAME_CONFIG_KEY key)
        {
            if (Key_Values.ContainsKey(key.ToString()))
            {
                Key_Values.Remove(key.ToString());
            }
            PlayerPrefs.DeleteKey(key.ToString());
            PlayerPrefs.Save();
        }

        public void ClearAllConfig()
        {
            Key_Values.Clear();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}
