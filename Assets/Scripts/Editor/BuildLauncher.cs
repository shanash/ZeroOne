#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FluffyDuck.EditorUtil
{
    public class BuildLauncher : EditorWindow
    {
        const string VERSION_TEXT_GUID = "aa5ec36119d9e49a795ca758b8137a7d";
        const string ADDRESSABLE_GROUP_TEXT_PATH = "AssetResources/Addressables";
        const string ADDRESSABLE_GROUP = "AddressableGroup";
        static readonly string ADDRESSABLE_GROUP_ROOT_TEXT_PATH = $"Assets/{ADDRESSABLE_GROUP_TEXT_PATH}";
        static readonly string ADDRESSABLE_VERSION_FILE_PATH = $"{ADDRESSABLE_GROUP_ROOT_TEXT_PATH}/{ADDRESSABLE_GROUP}Version.txt";

        static readonly string IS_REMOTE_PATH_KEY = "IsRemotePath";
        static readonly string IS_ADDRESSABLES_BUILD_KEY = "IsAddressablesBuild";
        static readonly string IS_PLAYER_BUILD_KEY = "IsPlayerBuild";
        static readonly string IS_CLEAN_BUILD_KEY = "IsCleanBuild";

        static readonly string BUILT_IN_DATA_GROUP_NAME = "Built In Data";
        static readonly string DEFAULT_GROUP_NAME = "Default";

        static readonly string[] Dropdown_Options = new string[] { "Local", "Remote" };

        //static readonly string ANDROID_SDK_EDITOR_PATH = "PlaybackEngines/AndroidPlayer/SDK";
        //static readonly string ANDROID_SDK_EDITOR_PATH_AT_WINDOWS = $"Data/{ANDROID_SDK_EDITOR_PATH}";
        //static readonly string ADB_PATH_AT_ANDROID_SDK = "platform-tools/adb";

        //static List<DeviceInfo> Device_Infos = new List<DeviceInfo>();
        //static string _Adb_Path = string.Empty;
        static bool IsBuildAndRun { get; set; }

        static string BUILD_PATH
        {
            get
            {
                return $"{EditorUserBuildSettings.activeBuildTarget}/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}_{PlayerSettings.Android.bundleVersionCode}.{(EditorUserBuildSettings.buildAppBundle ? "aab" : "apk")}";
            }
        }

        static AddressableAssetSettings s_Settings
        {
            get
            {
                return AddressableAssetSettingsDefaultObject.Settings;
            }
        }

        static bool IsRemotePath
        {
            get => EditorPrefs.GetBool(IS_REMOTE_PATH_KEY, false);
            set => EditorPrefs.SetBool(IS_REMOTE_PATH_KEY, value);
        }

        static bool IsAddressablesBuild
        {
            get => EditorPrefs.GetBool(IS_ADDRESSABLES_BUILD_KEY, true);
            set => EditorPrefs.SetBool(IS_ADDRESSABLES_BUILD_KEY, value);
        }

        static bool IsPlayerBuild
        {
            get => EditorPrefs.GetBool(IS_PLAYER_BUILD_KEY, false);
            set => EditorPrefs.SetBool(IS_PLAYER_BUILD_KEY, value);
        }

        static bool IsCleanBuild
        {
            get => EditorPrefs.GetBool(IS_CLEAN_BUILD_KEY, false);
            set => EditorPrefs.SetBool(IS_CLEAN_BUILD_KEY, value);
        }

        static int Select_Asset_Location_Index
        {
            set
            {
                IsRemotePath = (value == 1);
            }

            get
            {
                return IsRemotePath ? 1 : 0;
            }
        }

        static string Keystore_Password
        {
            get => PlayerSettings.Android.keystorePass;
            set => PlayerSettings.Android.keystorePass = value;
        }

        static string Key_Alias_Name
        {
            get => PlayerSettings.Android.keyaliasName;
            set => PlayerSettings.Android.keyaliasName = value;
        }

        static string Key_Alias_Password
        {
            get => PlayerSettings.Android.keyaliasPass;
            set => PlayerSettings.Android.keyaliasPass = value;
        }

        /*
        static string Adb_Path
        {
            get
            {
                if (!string.IsNullOrEmpty(_Adb_Path))
                {
                    return _Adb_Path;
                }
                string sdkPath = EditorPrefs.GetString("AndroidSdkRoot");

                // 사용자가 설정한 SDK 경로가 없는 경우 Unity의 내장된 SDK 경로를 사용
                if (string.IsNullOrEmpty(sdkPath))
                {
                    string unityEditorFolder = Path.GetDirectoryName(EditorApplication.applicationPath);
                    string android_sdk_path = ANDROID_SDK_EDITOR_PATH;
                    if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        android_sdk_path = ANDROID_SDK_EDITOR_PATH_AT_WINDOWS;
                    }

                    sdkPath = Path.Combine(unityEditorFolder, android_sdk_path);
                }

                Debug.Log("Android SDK 경로: " + sdkPath);

                // ADB 경로 구성
                string adbPath = Path.Combine(sdkPath, ADB_PATH_AT_ANDROID_SDK);

                // Windows 시스템인 경우 .exe 확장자 추가
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    adbPath += ".exe";
                }

                // ADB 경로 유효성 확인
                if (!File.Exists(adbPath))
                {
                    ErrorDialog($"ADB 경로를 찾을 수 없습니다. Android SDK 경로를 확인해주세요. : {adbPath}");
                    return string.Empty;
                }

                _Adb_Path = adbPath;
                return _Adb_Path;
            }
        }
        */

        public static void BuildAssetForLocalEditor()
        {
            IsRemotePath = false;
            IsAddressablesBuild = true;
            IsPlayerBuild = false;
            _ = Build(false);
        }
        
        public static void ShowWindow()
        {
            GetWindow<BuildLauncher>("Build Launcher", true, typeof(BuildPlayerWindow));
        }

        void OnGUI()
        {
            GUILayout.Label("Addressables Build Options", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            Select_Asset_Location_Index = EditorGUILayout.Popup("Select Asset Location", Select_Asset_Location_Index, Dropdown_Options);
            IsCleanBuild = EditorGUILayout.Toggle("Clean Asset Build", IsCleanBuild);

            GUILayout.Space(10);

            GUILayout.Label("Asset & Player Build Options", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box"); // 그룹 박스 시작
            IsAddressablesBuild = EditorGUILayout.Toggle("Asset", IsAddressablesBuild);
            IsPlayerBuild = EditorGUILayout.Toggle("Player", IsPlayerBuild);
            EditorGUILayout.EndVertical();

            Keystore_Password = EditorGUILayout.PasswordField("Keystore Password", Keystore_Password);
            Key_Alias_Name = EditorGUILayout.TextField("Key Alias Name", Key_Alias_Name);
            Key_Alias_Password = EditorGUILayout.PasswordField("Key Alias Password", Key_Alias_Password);

            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace(); // 버튼들을 아래로 밀어내기

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace(); // 버튼들을 오른쪽으로 밀어내기

            // 고정된 크기로 버튼 추가
            if (GUILayout.Button("Build", GUILayout.Width(100), GUILayout.Height(30)))
            {
                _ = Build(false);
            }

            if (GUILayout.Button("Build And Run", GUILayout.Width(100), GUILayout.Height(30)))
            {
                _ = Build(true);
            }
            GUILayout.EndHorizontal();
        }

        static async Task<string> ExecuteCommand(string fullpath_filename, string arguments)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = fullpath_filename,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            await WaitForExitAsync(process);

            return output.Trim();
        }

        public static Task WaitForExitAsync(System.Diagnostics.Process process, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default)
                cancellationToken.Register(() => tcs.SetCanceled());

            return tcs.Task;
        }

        static void ErrorDialog(string message)
        {
            EditorUtility.DisplayDialog("Error", $"{message}", "OK");
        }

        static async Task Build(bool is_build_and_run)
        {
            if (!IsAddressablesBuild && !IsPlayerBuild)
            {
                // 사용자가 최소한 하나의 체크박스를 선택하지 않았을 때의 경고
                ErrorDialog("You must select at least one item at Asset & Player Build Options");
                return;
            }

            IsBuildAndRun = is_build_and_run;

            await Task.Yield();

            BuildAddressablesAndPlayer(default, default, default);
        }

        public static void BuildAddressablesAndPlayer(string keystore_pw, string key_alias_name, string key_alias_pw)
        {
            if (!IsAddressablesBuild && !IsPlayerBuild)
            {
                ErrorDialog("어드레서블이든 플레이어든 하나는 빌드해야 합니다\nSet Addressables Build 혹은 Set Player Build를 켜주세요");
                return;
            }

            bool addressable_build_succeeded = true;

            if (IsAddressablesBuild)
            {
                addressable_build_succeeded = BuildAddressables();
            }

            if (IsPlayerBuild)
            {
                if (string.IsNullOrEmpty(Keystore_Password) || string.IsNullOrEmpty(Key_Alias_Name) || string.IsNullOrEmpty(Key_Alias_Password))
                {
                    // 사용자가 최소한 하나의 체크박스를 선택하지 않았을 때의 경고
                    ErrorDialog($"다음 값이 비어 있습니다.\n{(string.IsNullOrEmpty(Keystore_Password) ? "\nKeystore Password" : "")}{(string.IsNullOrEmpty(Key_Alias_Name) ? "\nKey Alias Name" : "")}{(string.IsNullOrEmpty(Key_Alias_Password) ? "\nKey Alias Password" : "")}");
                    return;
                }

                if (!addressable_build_succeeded)
                {
                    ErrorDialog("어드레서블 빌드가 실패해서 플레이어를 빌드할 수 없습니다");
                    return;
                }

                if (!Directory.Exists(EditorUserBuildSettings.activeBuildTarget.ToString()))
                {
                    Directory.CreateDirectory(EditorUserBuildSettings.activeBuildTarget.ToString());
                }

                try
                {
                    EditorUserBuildSettings.SetBuildLocation(BuildTarget.Android, BUILD_PATH);
                    BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
                    buildPlayerOptions.locationPathName = BUILD_PATH;
                    buildPlayerOptions.options |= IsBuildAndRun ? BuildOptions.AutoRunPlayer : BuildOptions.None;

                    // BuildPlayerOptions 구성
                    buildPlayerOptions = GetBuildPlayerOptions(false, buildPlayerOptions);

                    BuildPipeline.BuildPlayer(buildPlayerOptions);

                    Debug.Log("Build completed: " + BUILD_PATH);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        static BuildPlayerOptions GetBuildPlayerOptions(bool askForLocation = false, BuildPlayerOptions defaultOptions = new BuildPlayerOptions())
        {
            try
            {
                // Get static internal "GetBuildPlayerOptionsInternal" method
                MethodInfo method = typeof(BuildPlayerWindow.DefaultBuildMethods).GetMethod(
                    "GetBuildPlayerOptionsInternal",
                    BindingFlags.NonPublic | BindingFlags.Static);

                // invoke internal method
                return (BuildPlayerOptions)method.Invoke(
                    null,
                    new object[] { askForLocation, defaultOptions });
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw e;
            }
        }

        public static void PrintStaticProperties(Type type)
        {
            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                object value = prop.GetValue(null);
                Debug.Log($"{prop.Name} = {value}");
            }
        }

        static void RemoveMissingFiles()
        {
            List<AddressableAssetEntry> entriesToRemove = new List<AddressableAssetEntry>();

            foreach (AddressableAssetGroup group in s_Settings.groups)
            {
                foreach (AddressableAssetEntry entry in group.entries)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(entry.guid);
                    if (string.IsNullOrEmpty(assetPath) || !File.Exists(assetPath))
                    {
                        entriesToRemove.Add(entry);
                    }
                }
            }

            foreach (AddressableAssetEntry entry in entriesToRemove)
            {
                s_Settings.RemoveAssetEntry(entry.guid, false);
                Debug.Log("Removed missing file from Addressables: " + entry.guid);
            }

            EditorUtility.SetDirty(s_Settings);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 어드레서블로 에셋번들 파일들 빌드
        /// </summary>
        /// <returns>성공여부</returns>
        static bool BuildAddressables()
        {
            if (IsCleanBuild)
            {
                DeleteAddressableBuildFolder();
                DeleteAddressableBuildInfo();
                DeleteAddressableCache();
            }

            RefreshRemoteBuildAndLoadPath();

            if (!ValidateSemVerBundleVersion())
            {
                return false;
            }
            try
            {
                RemoveMissingFiles();

                foreach (var group in s_Settings.groups)
                {
                    CreateGroupDummyText(group);
                }

                HashStorage.Reload();
                
                var groups = CheckForChangedAssets();

                int current_data_version = 0;
                string old_override_player_version = s_Settings.OverridePlayerVersion;
                Debug.Assert(int.TryParse(old_override_player_version, out int data_version), "기존에 저장된 OverridePlayerVersion의 형식이 맞지 않습니다 : 데이터 버전란이 숫자가 아닙니다");
                current_data_version = data_version;
                if (groups.Count > 0)
                {
                    current_data_version++;
                }

                string override_player_version = $"{current_data_version.ToString("D3")}";
                AddAdressableVersionText(override_player_version);

                if (groups.Count > 0)
                {
                    foreach (var group in groups)
                    {
                        CreateGroupDummyText(group);
                    }

                    ProcessEntries((entry) =>
                    {
                        string oldAddress = entry.address;
                        if (entry.parentGroup != null)
                        {
                            entry.labels.Clear();
                            var labels = s_Settings.GetLabels();
                            if (!labels.Contains(entry.parentGroup.Name))
                            {
                                s_Settings.AddLabel(entry.parentGroup.Name);
                            }
                            entry.SetLabel(entry.parentGroup.Name, true);
                        }
                        try
                        {
                            string fileName = Path.GetFileName(oldAddress);
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

                            // 파일 이름에 확장자가 있으면 제거
                            if (fileNameWithoutExtension != fileName)
                            {
                                string newAddress = oldAddress.Replace(fileName, fileNameWithoutExtension);
                                entry.SetAddress(newAddress);
                                return true;
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Log(entry);
                            Debug.LogError(e);
                        }

                        return false;
                    },
                    (allAssetEntries) =>
                    {
                        s_Settings.SetDirty(AddressableAssetSettings.ModificationEvent.BatchModification, allAssetEntries, true);
                        AssetDatabase.SaveAssets();
                    });
                }

                s_Settings.OverridePlayerVersion = override_player_version;

                EditorUtility.SetDirty(s_Settings);
                AssetDatabase.SaveAssets();

                AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);

                bool success = string.IsNullOrEmpty(result.Error);

                if (!success)
                {
                    Debug.LogError("Addressables build error encountered: " + result.Error);
                }

                return success;
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception : {e}");
            }

            return false;
        }

        /// <summary>
        /// 빌드 결과물 폴더 자체를 삭제합니다
        /// </summary>
        public static void DeleteAddressableBuildFolder()
        {
            RefreshRemoteBuildAndLoadPath();

            var included_symbol_path = s_Settings.profileSettings.GetValueByName(s_Settings.activeProfileId, IsRemotePath ? AddressableAssetSettings.kRemoteBuildPath : AddressableAssetSettings.kLocalBuildPath);
            var real_path = s_Settings.RemoteCatalogBuildPath.GetValue(s_Settings);

            string build_folder_path = ExtractStringBeforeFirstPattern(included_symbol_path, real_path);

            if (!Directory.Exists(build_folder_path))
            {
                Debug.LogError($"지정된 경로가 존재하지 않습니다: {build_folder_path}");
                return;
            }

            DeleteDirectory(build_folder_path, false);
            Debug.Log($"빌드 데이터가 전부 삭제되었습니다: {Application.dataPath}/{build_folder_path}");

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 빌드를 위해 가지고 있던 정보를 날립니다.
        /// </summary>
        public static void DeleteAddressableBuildInfo()
        {
            Debug.Log("Clear PlayerPrefs Hashes!");

            foreach (var group in s_Settings.groups)
            {
                if (group.Name.Equals(BUILT_IN_DATA_GROUP_NAME))
                {
                    continue;
                }

                HashStorage.Delete(group.Name);
            }

            HashStorage.Save();

            Debug.Log("Reset Override Player Version");
            s_Settings.OverridePlayerVersion = "000";

            if (!ValidateSemVerBundleVersion())
            {
                PlayerSettings.bundleVersion = "0.0.0";
            }

            AssetDatabase.SaveAssets();

            Debug.Log("delete cache at " + Application.persistentDataPath);
            var list = Directory.GetDirectories(Application.persistentDataPath);

            foreach (var item in list)
            {
                Debug.Log("Delete" + " " + item);
                Directory.Delete(item, true);
            }

            if (File.Exists(ADDRESSABLE_VERSION_FILE_PATH))
            {
                File.Delete(ADDRESSABLE_VERSION_FILE_PATH);
                File.Delete($"{ADDRESSABLE_VERSION_FILE_PATH}.meta");
                Debug.Log("delete file at " + ADDRESSABLE_VERSION_FILE_PATH);
            }

            Caching.ClearCache();

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 어드레서블로 다운로드 받아서 사용중이었던 캐시에 쌓여있는 데이터를 날립니다
        /// PlayerPrefs에 저장된 버전도 날립니다
        /// </summary>
        public static void DeleteAddressableCache()
        {
            foreach (var group in s_Settings.groups)
            {
                if (group.Name.Equals(BUILT_IN_DATA_GROUP_NAME))
                {
                    continue;
                }
                Debug.Log($"Clear Cache Group : {group.Name}");
                Addressables.ClearDependencyCacheAsync(group.Name);
            }
            Addressables.ClearResourceLocators();

            PlayerPrefs.DeleteKey("Data_Version");
            PlayerPrefs.Save();

            Debug.Log("Clear Complete");

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        static void DeleteDirectory(string path, bool delete_root)
        {
            // 디렉토리 존재 여부 확인
            if (!Directory.Exists(path))
            {
                Debug.Log($"지정된 경로가 존재하지 않습니다: {path}");
                return;
            }

            // 하위 디렉토리 및 파일을 모두 삭제
            foreach (string directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory, true);
            }

            foreach (string file in Directory.GetFiles(path))
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            if (delete_root)
            {
                // 루트 디렉토리 삭제
                Directory.Delete(path, false);
            }
        }

        static void RefreshRemoteBuildAndLoadPath()
        {
            bool is_remote_path = IsRemotePath;
            foreach (var group in s_Settings.groups)
            {
                if (group.Name.Equals(BUILT_IN_DATA_GROUP_NAME))
                {
                    continue;
                }

                var schema = group.GetSchema<BundledAssetGroupSchema>();

                schema.BuildPath.SetVariableByName(s_Settings, is_remote_path ? AddressableAssetSettings.kRemoteBuildPath : AddressableAssetSettings.kLocalBuildPath);
                schema.LoadPath.SetVariableByName(s_Settings, is_remote_path ? AddressableAssetSettings.kRemoteLoadPath : AddressableAssetSettings.kLocalLoadPath);
                
            }
            s_Settings.RemoteCatalogBuildPath.SetVariableByName(s_Settings, is_remote_path ? AddressableAssetSettings.kRemoteBuildPath : AddressableAssetSettings.kLocalBuildPath);
            s_Settings.RemoteCatalogLoadPath.SetVariableByName(s_Settings, is_remote_path ? AddressableAssetSettings.kRemoteLoadPath : AddressableAssetSettings.kLocalLoadPath);

            s_Settings.ActivePlayModeDataBuilderIndex = s_Settings.DataBuilders.FindIndex(db => db.GetType().Name == (is_remote_path ? "BuildScriptPackedPlayMode" : "BuildScriptFastMode"));

            Debug.Log($"Set {(is_remote_path ? "Remote" : "Local")} Path");
        }

        public static async Task ModifyVersionTextMeta()
        {
            string find_word = "guid: ";
            string guid = string.Empty;

            while (!File.Exists($"{ADDRESSABLE_VERSION_FILE_PATH}.meta"))
            {
                await Task.Delay(1000);
            }

            using (FileStream fs = new FileStream($"{ADDRESSABLE_VERSION_FILE_PATH}.meta", FileMode.Open, FileAccess.ReadWrite))
            {
                List<string> lines = new List<string>();
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line = sr.ReadLine();
                    lines.Add(line);
                    while (!line.Contains(find_word))
                    {
                        line = sr.ReadLine();
                        if (string.IsNullOrEmpty(line))
                        {
                            Debug.Assert(false, $"{ADDRESSABLE_VERSION_FILE_PATH} 메타 파일의 guid가 없습니다?");
                            return;
                        }
                        lines.Add(line);
                    }

                    guid = line.Replace(find_word, "");

                    if (!guid.Equals(VERSION_TEXT_GUID))
                    {
                        lines[lines.Count - 1] = $"{find_word}{VERSION_TEXT_GUID}";
                        while(!string.IsNullOrEmpty(line = sr.ReadLine()))
                        {
                            lines.Add(line);
                        }

                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            foreach (var l in lines)
                            {
                                sw.WriteLine(l);
                            }
                        }
                    }
                }

            }

        }

        public static bool ExistVersionText()
        {
            return File.Exists(ADDRESSABLE_VERSION_FILE_PATH);
        }

        public static void CreateVersionText()
        {
            Directory.CreateDirectory(ADDRESSABLE_GROUP_ROOT_TEXT_PATH);

            using (FileStream fs = new FileStream(ADDRESSABLE_VERSION_FILE_PATH, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("000");
                }
            }
        }

        static void AddAdressableVersionText(string override_player_version)
        {
            if (File.Exists(ADDRESSABLE_VERSION_FILE_PATH))
            {
                CreateVersionText();
                return;
            }

            bool asset_exist = AssetDatabase.GetMainAssetTypeAtPath(ADDRESSABLE_VERSION_FILE_PATH) != null;

            string version_text = string.Empty;

            AddressableAssetGroup default_group = null;

            foreach (var group in s_Settings.groups)
            {
                if (group.Name.Equals(DEFAULT_GROUP_NAME))
                {
                    default_group = group;
                    break;
                }
            }

            Debug.Assert(default_group != null, "Default 그룹을 찾을 수 없습니다.");

            if (asset_exist)
            {
                using (StreamReader sr = new StreamReader(ADDRESSABLE_VERSION_FILE_PATH))
                {
                    version_text = sr.ReadToEnd();
                }
            }

            if (version_text.Equals(override_player_version))
            {
                return;
            }

            using (FileStream fs = new FileStream(ADDRESSABLE_VERSION_FILE_PATH, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(override_player_version);
                }
            }

            AssetDatabase.Refresh();

            string guid = AssetDatabase.AssetPathToGUID(ADDRESSABLE_VERSION_FILE_PATH);

            Debug.Assert(!string.IsNullOrEmpty(guid), $"{ADDRESSABLE_VERSION_FILE_PATH} AssetDatabase.Refresh()를 했는데 왜 없을까?");

            if (s_Settings.FindAssetEntry(guid) != null)
            {
                return;
            }

            AddressableAssetEntry entry = s_Settings.CreateOrMoveEntry(guid, default_group);
            entry.address = ADDRESSABLE_VERSION_FILE_PATH;
            List<AddressableAssetEntry> entries = new List<AddressableAssetEntry>
            {
                entry
            };

            s_Settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entries, true);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 그룹에 하나밖에 없는 더미텍스트를 만든다
        /// 에셋들이 여러개의 그룹에 Dependencie되는 경우가 많아서
        /// 하나의 에셋번들 파일만 정확히 다운로드 받는 걸 체크하기가 힘들다
        /// </summary>
        static void CreateGroupDummyText(AddressableAssetGroup group)
        {
            Directory.CreateDirectory(ADDRESSABLE_GROUP_ROOT_TEXT_PATH);

            List<AddressableAssetEntry> textEntries = new List<AddressableAssetEntry>();
            if (group.Name.Equals(BUILT_IN_DATA_GROUP_NAME))
            {
                return;
            }

            string addressable_info_file_path = $"{ADDRESSABLE_GROUP_ROOT_TEXT_PATH}/{ADDRESSABLE_GROUP}_{group.Name}.txt";
            
            bool asset_exist = AssetDatabase.GetMainAssetTypeAtPath(addressable_info_file_path) != null;
            if (!asset_exist)
            {
                string file_path = $"{Application.dataPath}/{ADDRESSABLE_GROUP_TEXT_PATH}/{ADDRESSABLE_GROUP}_{group.Name}.txt";
                using (FileStream fs = new FileStream(file_path, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(group.Name);
                    }
                }
            }

            AssetDatabase.Refresh();

            string guid = AssetDatabase.AssetPathToGUID(addressable_info_file_path);

            Debug.Assert(!string.IsNullOrEmpty(guid), $"{addressable_info_file_path} AssetDatabase.Refresh()를 했는데 왜 없을까?");

            if (s_Settings.FindAssetEntry(guid) != null)
            {
                return;
            }

            AddressableAssetEntry entry = s_Settings.CreateOrMoveEntry(guid, group);
            entry.address = addressable_info_file_path;
            textEntries.Add(entry);

            s_Settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, textEntries, true);
            AssetDatabase.SaveAssets();
        }

        static void ProcessEntries(Func<AddressableAssetEntry, bool> processEntry, Action<List<AddressableAssetEntry>> onChangesMade)
        {
            var allAssetEntries = new List<AddressableAssetEntry>();
            s_Settings.GetAllAssets(allAssetEntries, true);

            bool anyChanges = false;

            foreach (var entry in allAssetEntries)
            {
                if (string.IsNullOrEmpty(entry.guid) || entry.parentGroup.Name.Equals(BUILT_IN_DATA_GROUP_NAME))
                {
                    continue;
                }

                anyChanges |= processEntry(entry); // 사용자 정의 로직 호출
            }

            if (anyChanges)
            {
                onChangesMade(allAssetEntries); // 변경 사항이 있을 때 실행할 로직 호출
            }
        }

        /// <summary>
        /// 메이저.마이너.패치의 번들 버전형식이 올바른지 확인
        /// </summary>
        /// <returns>번들버전 형식 확인</returns>
        static bool ValidateSemVerBundleVersion()
        {
            string bundle_version = PlayerSettings.bundleVersion;

            string[] versionNums = bundle_version.Split('.');
            if (versionNums.Length != 3)
            {
                Debug.LogError("번들버전 형식이 맞지 않습니다 : 점 두개가 번들버전에 없습니다.");
                return false;
            }

            foreach (var versionNum in versionNums)
            {
                if (!int.TryParse(versionNum, out _))
                {
                    Debug.LogError("번들버전 형식이 맞지 않습니다 : 숫자가 아닌 값이 있습니다.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// HashStorage를 이용하여 전체 파일 중 어떤 에셋그룹(=파일)의 데이터가 변경되었는지 확인
        /// </summary>
        /// <returns>변경여부</returns>
        static List<AddressableAssetGroup> CheckForChangedAssets()
        {
            // Addressable Asset 설정 가져오기
            List<AddressableAssetGroup> groups = new List<AddressableAssetGroup>();

            Dictionary<string, AddressableAssetGroup> dic_groups = new();
            // 각 에셋의 현재 해시 값을 계산하고 저장된 해시와 비교
            foreach (var group in s_Settings.groups)
            {
                if (group.Name.Equals(BUILT_IN_DATA_GROUP_NAME))
                {
                    continue;
                }

                if (CheckForChangedAssets(group))
                {
                    groups.Add(group);
                }

                dic_groups.Add(group.Name, group);
            }

            List<string> last_group_names = HashStorage.GetGroupList();

            var deleted_group_names = last_group_names.Except(dic_groups.Keys.ToList()).ToList();

            foreach (var group_name in deleted_group_names)
            {
                HashStorage.Delete(group_name);
            }

            if (deleted_group_names.Count > 0)
            {
                HashStorage.Save();
            }

            return groups;
        }

        /// <summary>
        /// 특정 에셋그룹(=파일)의 데이터가 변경되었는지 확인
        /// </summary>
        /// <param name="group">확인해볼 그룹</param>
        /// <returns>변경여부</returns>
        static bool CheckForChangedAssets(AddressableAssetGroup group)
        {
            bool changed = false;
            var current_guids = new HashSet<string>(); // 현재 그룹의 GUID를 저장할 HashSet 생성

            foreach (var entry in group.entries)
            {
                string asset_path = entry.AssetPath;
                if (asset_path.Contains($"{ADDRESSABLE_GROUP}") || string.IsNullOrEmpty(entry.guid) || entry.parentGroup.Name.Equals(BUILT_IN_DATA_GROUP_NAME))
                {
                    continue;
                }
                                
                string guid = entry.guid;
                current_guids.Add(guid);
                string lastKnownHash = HashStorage.GetLastKnownHash(group.Name, guid);

                // 현재 에셋의 해시 계산
                string currentHash = AssetDatabase.GetAssetDependencyHash(asset_path).ToString();

                // 해시 값 비교
                if (lastKnownHash != currentHash)
                {
                    Debug.Log($"Asset {asset_path} has changed.");
                    changed = true;
                    // 현재 해시 값을 로컬 저장소에 저장
                    HashStorage.SetCurrentHash(group.Name, guid, currentHash);
                }
            }

            // HashStorage에서 현재 그룹에 없는 GUID 및 해시 제거
            foreach (var stored_guid in HashStorage.GetAllGuids(group.Name))
            {
                if (!current_guids.Contains(stored_guid))
                {
                    Debug.Log($"Delete {group.Name} : {stored_guid}");
                    HashStorage.RemoveHash(group.Name, stored_guid); // HashStorage에서 해당 GUID 및 해시 제거
                    changed = true;
                }
            }

            if (changed)
            {
                HashStorage.Save();
            }

            return changed;
        }

        /// <summary>
        /// Build Path 경로를 확실하게 가져오기 위한 메소드
        /// TODO: Build Path 경로가 변경되었을대 오작동할 우려가 있다
        /// </summary>
        /// <param name="patternString"></param>
        /// <param name="targetString"></param>
        /// <returns></returns>
        static string ExtractStringBeforeFirstPattern(string patternString, string targetString)
        {
            // patternString에서 첫 번째 "/[" 패턴의 위치를 찾습니다.
            int firstPatternIndex = patternString.IndexOf("/[");

            // 패턴이 없으면 전체 문자열을 반환합니다.
            if (firstPatternIndex == -1)
            {
                return targetString;
            }

            // 해당 위치까지의 targetString 내 "/"의 수를 세기 위한 카운터입니다.
            int slashCount = 0;

            // 첫 번째 "/[" 이전까지의 "/"의 수를 셉니다.
            for (int i = patternString.Length - 1; i > firstPatternIndex; i--)
            {
                if (patternString[i] == '/')
                {
                    slashCount++;
                }
            }

            // targetString에서 해당하는 위치의 "/"를 찾습니다.
            int targetSlashIndex = -1;
            for (int i = targetString.Length -1; i >= 0; i--)
            {
                if (targetString[i] == '/')
                {
                    slashCount--;
                    if (slashCount < 0)
                    {
                        targetSlashIndex = i;
                        break;
                    }
                }
            }

            // 해당 위치 이전까지의 targetString을 반환합니다.
            return targetSlashIndex != -1 ? targetString.Substring(0, targetSlashIndex) : targetString;
        }

        /*
        static async Task FetchAndroidDeviceInfos()
        {
            // ADB를 사용하여 연결된 디바이스 목록을 가져옵니다.
            string output = await ExecuteCommand(Adb_Path, "devices");

            var device_ids = ParseDeviceId(output);

            for (int i = Device_Infos.Count - 1; i >= 0; i--)
            {
                if (!device_ids.Contains(Device_Infos[i].ID))
                {
                    Device_Infos.RemoveAt(i);
                }
            }

            string result = string.Empty;

            List<DeviceInfo> devices = new List<DeviceInfo>();

            foreach (var deviceId in device_ids)
            {
                // 이미 저장중인 ID는 넘깁니다.
                if (Device_Infos.Exists(x => x.ID == deviceId))
                {
                    continue;
                }

                // 새로운 ID는 추가합니다.
                var model_datas = (await ExecuteCommand(Adb_Path, $"-s {deviceId} shell getprop ro.product.manufacturer; getprop ro.product.model")).Split('\n');
                var info = new DeviceInfo();
                info.ID = deviceId;
                info.Manufacturer = model_datas[0];
                info.Model = model_datas[1];

                Device_Infos.Add(info);
            }
        }

        static List<string> ParseDeviceId(string adbOutput)
        {
            List<string> deviceIds = new List<string>();

            // ADB 출력을 줄 단위로 분리
            string[] lines = adbOutput.Split('\n');

            foreach (var line in lines)
            {
                // 각 줄이 'device'를 포함하고 있으면, 디바이스 ID를 추출합니다.
                if (line.Contains("device") && !line.Contains("List of devices"))
                {
                    string deviceId = line.Split('\t')[0];
                    deviceIds.Add(deviceId);
                }
            }

            return deviceIds;
        }
*/

        /// <summary>
        /// PlayerPrefs를 사용하여 파일해시를 저장
        /// </summary>
        public static class HashStorage
        {
            static readonly string KEY = "Addressables_Hashes";
            static Dictionary<string, Dictionary<string, string>> _assets_hashes = new Dictionary<string, Dictionary<string, string>>();

            /// <summary>
            /// 어드레서블 빌드시에 PlayerPrefs에서 이전에 빌드했었던 에셋들의 해시를 읽어옵니다
            /// </summary>
            public static void Reload()
            {
                _assets_hashes.Clear();
                string group_names_value = PlayerPrefs.GetString(KEY, string.Empty);
                var group_names = group_names_value.Split('|');
                foreach (var group_name in group_names)
                {
                    if (group_name.Equals(BUILT_IN_DATA_GROUP_NAME))
                    {
                        continue;
                    }

                    string str = PlayerPrefs.GetString($"{KEY}/{group_name}", string.Empty);
                    if (string.IsNullOrEmpty(str))
                    {
                        return;
                    }

                    _assets_hashes.Add(group_name, new Dictionary<string, string>());

                    string[] key_value = str.Split('|');
                    int cnt = key_value.Length;

                    for (int i = 0; i < cnt; i++)
                    {
                        string[] key_and_value = key_value[i].Split(':');
                        _assets_hashes[group_name].Add(key_and_value[0], key_and_value[1]);
                    }

                }
            }

            /// <summary>
            /// PlayerPrefs에 저장된 특정 에셋의 hash를 가져옵니다
            /// </summary>
            /// <param name="group_name">에셋이 속한 그룹</param>
            /// <param name="guid">에셋의 guid</param>
            /// <returns></returns>
            public static string GetLastKnownHash(string group_name, string guid)
            {
                if (!_assets_hashes.ContainsKey(group_name) || !_assets_hashes[group_name].ContainsKey(guid))
                {
                    return string.Empty;
                }
                return _assets_hashes[group_name][guid];
            }

            /// <summary>
            /// 특정 에셋의 해시를 입력
            /// </summary>
            /// <param name="group_name">그룹 이름</param>
            /// <param name="guid">에셋의 guid</param>
            /// <param name="hash">에셋의 hash</param>
            public static void SetCurrentHash(string group_name, string guid, string hash)
            {
                if (!_assets_hashes.ContainsKey(group_name))
                {
                    _assets_hashes.Add(group_name, new Dictionary<string, string>());
                }

                _assets_hashes[group_name][guid] = hash;
            }

            /// <summary>
            /// 해시를 PlayerPrefs에 저장
            /// 그룹별로 합쳐서 저장합니다
            /// </summary>
            public static void Save()
            {
                string group_names = string.Empty;

                var list = _assets_hashes.ToList();
                int cnt = list.Count;

                for (int i = 0; i < cnt; i++)
                {
                    string value = "";
                    var l = list[i].Value.ToList();
                    for (int j = 0; j < l.Count; j++)
                    {
                        // 에셋그룹의 "키:밸류" 스트링들을 | 문자를 넣어 구분합니다
                        if (!string.IsNullOrEmpty(value))
                        {
                            value += '|';
                        }

                        // guid와 hash를 "키:밸류" 스트링으로 저장합니다
                        value += $"{l[j].Key}:{l[j].Value}";
                    }


                    // {HashStorage.KEY}/{그룹이름}, 그룹들의 에셋파일 {guid}:{hash}|{guid}:{hash}|{gui....
                    PlayerPrefs.SetString($"{KEY}/{list[i].Key}", value);

                    if (!string.IsNullOrEmpty(group_names))
                    {
                        group_names += '|';
                    }
                    group_names += list[i].Key;
                }

                PlayerPrefs.SetString(KEY, group_names);
                PlayerPrefs.Save();
            }

            public static void Delete(string group_name)
            {
                if (_assets_hashes.ContainsKey(group_name))
                {
                    _assets_hashes.Remove(group_name);
                }
                PlayerPrefs.DeleteKey($"{KEY}/{group_name}");
            }

            public static List<string> GetGroupList()
            {
                return _assets_hashes.Keys.ToList();
            }

            public static List<string> GetAllGuids(string group_name)
            {
                return _assets_hashes[group_name].Keys.ToList();
            }

            public static void RemoveHash(string group_name, string storedGuid)
            {
                if (!_assets_hashes.ContainsKey(group_name))
                {
                    Debug.Log($"없는 그룹 이름입니다 : {group_name}");
                    return;
                }

                if (!_assets_hashes[group_name].ContainsKey(storedGuid))
                {
                    Debug.Log($"없는 guid입니다 : {group_name}, {storedGuid}");
                    return;
                }

                _assets_hashes[group_name].Remove(storedGuid);
            }
        }
    }
}
#endif
