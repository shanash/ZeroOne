using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FluffyDuck.Util
{
    public static class FileUtils
    {
        public static bool IsExistFile(string path)
        {
            return File.Exists(path);
        }

        public static string GetBinaryFileData(string path)
        {
            if (File.Exists(path))
            {
                var d = File.ReadAllText(path, Encoding.UTF8);
                return d;
            }
            return string.Empty;
        }

        /// <summary>
        /// 암호화된 json 데이터를 T 클래스 타입으로 적용
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="sec_binary"></param>
        public static void LoadJsonDataSecurityByData<T>(ref T data, string sec_binary)
        {
            var dec_data = Security.AESDecrypt256(sec_binary);
            data = JsonUtility.FromJson<T>(dec_data);
        }

        public static void LoadJsonDataByData<T>(ref T data, string binary)
        {
            data = JsonUtility.FromJson<T>(binary);
        }

        public static async void LoadJsonDataAsync(string path, System.Action<string> callback)
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(path);
            TextAsset txt_asset = await handle.Task;
            if (txt_asset != null)
            {
                callback?.Invoke(txt_asset.text);
            }
            else
            {
                callback?.Invoke(string.Empty);
            }
            
        }


        /// <summary>
        /// Json 타입으로 데이터 저장.
        /// 기본적으로 암호화 하여 저장한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="file_path"></param>
        public static void SaveJsonDataSecurity<T>(T data, string file_path)
        {
            var serialized = JsonUtility.ToJson(data);
            var enc_data = Security.AESEncrypt256(serialized);
            File.WriteAllText(file_path, enc_data, Encoding.UTF8);
        }
        /// <summary>
        /// Json 타입의 데이터를 읽어준다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file_path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool LoadJsonDataSecurity<T>(string file_path, ref T data)
        {
            try
            {
                if (File.Exists(file_path))
                {
                    var d = File.ReadAllText(file_path, Encoding.UTF8);
                    var dec_data = Security.AESDecrypt256(d);
                    data = JsonUtility.FromJson<T>(dec_data);
                    return true;
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception(string.Format("Json Parsing Error [{0}]", e.StackTrace));
            }

            return false;
        }
        /// <summary>
        /// Json 타입으로 데이터 저장
        /// 암호화 하지 않음
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="file_path"></param>
        public static void SaveJsonData<T>(T data, string file_path)
        {
            var serialized = JsonUtility.ToJson(data);
            File.WriteAllText(file_path, serialized, Encoding.UTF8);
        }
        /// <summary>
        /// Json 타입으로 데이터를 읽어준다.
        /// 암호화 하지 않음
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file_path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool LoadJsonData<T>(string file_path, ref T data)
        {
            if (File.Exists(file_path))
            {
                try
                {
                    var d = File.ReadAllText(file_path, Encoding.UTF8);
                    data = JsonUtility.FromJson<T>(d);
                    return true;
                }
                catch (System.Exception e)
                {
                    throw new System.Exception(string.Format("Json Parsing Error [{0}]", e.StackTrace));
                }

            }
            return false;
        }

        /// <summary>
        /// 파일 저장. 암호화 없이 그냥 저장
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void SaveFileData(string data, string path)
        {
            File.WriteAllText(path, data, Encoding.UTF8);
        }
        /// <summary>
        /// 파일 로드. 암호화 없이 파일 로드
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string LoadFileData(string path)
        {
            if (File.Exists(path))
            {
                var d = File.ReadAllText(path, Encoding.UTF8);
                if (!string.IsNullOrEmpty(d))
                {
                    return d;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 파일 저장. string 데이터를 암호화 하여 저장
        /// </summary>
        /// <param name="data">string data</param>
        /// <param name="path">파일 저장 경로</param>
        public static void SaveFileDataSecurity(string data, string path)
        {
            var enc_data = Security.AESEncrypt256(data);
            File.WriteAllText(path, enc_data, Encoding.UTF8);
        }
        /// <summary>
        /// 파일 로드. 암호화된 데이터를 읽고, 디코딩 후 반환
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string LoadFileDataSecurity(string path)
        {
            if (File.Exists(path))
            {
                var d = File.ReadAllText(path, Encoding.UTF8);
                if (!string.IsNullOrEmpty(d))
                {
                    return Security.AESDecrypt256(d);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Serializable 데이터 저장
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="file_path"></param>
        public static void SaveFileData<T>(T data, string file_path)
        {
            FileStream fs = null;
            if (File.Exists(file_path))
            {
                fs = File.OpenWrite(file_path);
            }
            else
            {
                fs = File.Create(file_path);
            }
            //  데이터 저장
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, data);
            }
            catch (System.Exception e)
            {
                Debug.LogErrorFormat("CommonUtils.SaveFileData() Error : {0}", e.StackTrace);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 리소스 읽기
        /// Resources 내의 경로
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file_path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool LoadLocalFileData<T>(string file_path, ref T data)
        {
            TextAsset asset = Resources.Load(file_path) as TextAsset;
            if (asset != null)
            {
                bool is_success = true;
                Stream s = new MemoryStream(asset.bytes);
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    data = (T)bf.Deserialize(s);
                }
                catch (System.Exception e)
                {
                    is_success = false;
                    Debug.LogErrorFormat("CommonUtils.LoadLocalFileData() Error : {0}", e.StackTrace);
                }
                finally
                {
                    s.Close();
                }
                return is_success;
            }

            return false;
        }

        /// <summary>
        /// Serializable 데이터 로드
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file_path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool LoadFileData<T>(string file_path, ref T data)
        {
            if (File.Exists(file_path))
            {
                bool is_success = true;
                FileStream fs = File.Open(file_path, FileMode.Open);
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    data = (T)bf.Deserialize(fs);
                }
                catch (System.Exception e)
                {
                    is_success = false;
                    Debug.LogErrorFormat("CommonUtils.LoadFileData() Error : {0}", e.StackTrace);
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
                return is_success;
            }

            return false;
        }

        /// <summary>
        /// 파일 삭제하기
        /// </summary>
        /// <param name="file_path"></param>
        public static void DeleteFile(string file_path)
        {
            if (File.Exists(file_path))
            {
                File.Delete(file_path);
            }
        }

        /// <summary>
        /// 파일 확장명 변경하기
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string ChangeFileExt(string filename, string ext)
        {
            return string.Format("{0}.{1}", Path.GetFileNameWithoutExtension(filename), ext);
        }
        public static string GetFilenameWithoutExt(string filenam)
        {
            return Path.GetFileNameWithoutExtension(filenam);
        }
    }

}

