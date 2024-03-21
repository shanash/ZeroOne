using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FluffyDuck.Addressable
{
    /// <summary>
    /// 어드레서블로 빌드한 에셋번들 파일들을 다운로드 받는 역할
    /// </summary>
    public class AddressableContentDownloader
    {
        static readonly int CALL_HANDLER_GAP_MILLISECONDS = 1000;
        //TODO: 상수 string 정리 필요
        static readonly string ADDRESSABLE_GROUP = "Assets/AssetResources/Addressables/AddressableGroup";
        static readonly string VERSION_KEY = $"{ADDRESSABLE_GROUP}Version";

        public delegate void DownloadProgressHandler(string key, float value, double size);
        event DownloadProgressHandler _On_Download_Progress;


        /// <summary>
        /// 다운로더 초기화
        /// </summary>
        /// <returns>실패</returns>
        public async Task<bool> Init()
        {
            var handle = Addressables.InitializeAsync();
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Addressables.InitializeAsync() 실패 : NetworError?");
                return false;
            }

            TextAsset ta = await Addressables.LoadAssetAsync<TextAsset>(VERSION_KEY).Task;
            if (!int.TryParse(ta.text, out int data_version))
            {
                Debug.LogError($"{VERSION_KEY}이 숫자가 아닙니다");
                return false;
            }

            AddressableWrapper.Data_Version = data_version;

            return true;
        }

        public bool HasNewDataVersion()
        {
            int last_data_version = PlayerPrefs.GetInt("Data_Version", 0);
            Debug.Log($"Last Data Version : {last_data_version}");
            Debug.Log($"AddressableWrapper.Data_Version : {AddressableWrapper.Data_Version}");
            return AddressableWrapper.Data_Version > last_data_version;
        }

        public void AddDownloadProgressHandler(DownloadProgressHandler handler)
        {
            _On_Download_Progress += handler;
        }

        public void RemoveDownloadProgressHandler(DownloadProgressHandler handler)
        {
            _On_Download_Progress -= handler;
        }

        /// <summary>
        /// 필요한 파일을 다운로드 받습니다
        /// 다운로드를 실패하면 진행하지 못하게 해야 하기때문에 성공실패 여부를 리턴합니다
        /// </summary>
        /// <returns>다운로드 성공 실패 여부</returns>
        public async Task<bool> Download()
        {
            try
            {
                var list_resource_locators = Addressables.ResourceLocators.ToList();
                int cnt = list_resource_locators.Count;

                // 전체 키들중에서 어드레서블 빌드때 자동으로 만들어주는 그룹 확인용 키들만 확인합니다
                for (int i = 0; i < cnt; i++)
                {
                    foreach (var key in list_resource_locators[i].Keys)
                    {
                        // 요게 그룹확인용 키
                        if (!key.ToString().Contains($"{ADDRESSABLE_GROUP}_"))
                        {
                            continue;
                        }

                        string group_key = key.ToString();

                        // 다운로드 전체 사이즈를 계산하고(메가바이트로 변환)
                        var size = BytesToMegabytes(await Addressables.GetDownloadSizeAsync(group_key).Task);
                        Debug.Log($"Group : {group_key} : {size}");

                        if (size > 0)
                        {
                            var hDownload = Addressables.DownloadDependenciesAsync(group_key, false);

                            while (hDownload.Status == AsyncOperationStatus.None)
                            {
                                _On_Download_Progress(group_key.Replace($"{ADDRESSABLE_GROUP}_", ""), hDownload.GetDownloadStatus().Percent, size);

                                await Task.Delay(CALL_HANDLER_GAP_MILLISECONDS);
                            }

                            _On_Download_Progress(group_key.Replace($"{ADDRESSABLE_GROUP}_", ""), 1.0f, size);
                            await Task.Yield();

                            Addressables.Release(hDownload);
                        }
                    }
                }

                PlayerPrefs.SetInt("Data_Version", AddressableWrapper.Data_Version);
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }

            return true;
        }

        double BytesToMegabytes(long bytes)
        {
            return (double)bytes / 1024 / 1024;
        }
    }
}
