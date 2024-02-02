#if UNITY_EDITOR
using DocumentFormat.OpenXml.Drawing;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace FluffyDuck.EditorUtil
{
    [InitializeOnLoad]
    public class InitializeOnLoad
    {
        const string SESSION_STATE_KEY = "InitializeOnLoad";

        static AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;

        static InitializeOnLoad()
        {
            bool is_initialized = SessionState.GetBool(SESSION_STATE_KEY, false);

            if (!is_initialized)
            {
                EditorApplication.update += SetAndroidPlatform;
                EditorApplication.update += CreateVersionText;
                SessionState.SetBool(SESSION_STATE_KEY, true);
            }
        }

        static void SetAndroidPlatform()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                Debug.Log("Setting platform to Android");
                if (!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android))
                {
                    EditorUtility.DisplayDialog("Error", $"Android Build Support 모듈이 설치되지 않았습니다.\n 모듈 설치 후에 다시 프로젝트를 열어 주세요.", "OK");
                }
            }
            else
            {
                EditorApplication.update -= SetAndroidPlatform;
            }
        }

        /// <summary>
        /// VersionText가 없으면 생성해주고 새로고침
        /// </summary>
        static void CreateVersionText()
        {
            if (!BuildLauncher.ExistVersionText())
            {
                BuildLauncher.CreateVersionText();
                _ = BuildLauncher.ModifyVersionTextMeta();

                AssetDatabase.Refresh();
            }
            else
            {
                string guid = AssetDatabase.AssetPathToGUID(BuildLauncher.ADDRESSABLE_VERSION_FILE_PATH);

                if (!BuildLauncher.VERSION_TEXT_GUID.Equals(guid))
                {
                    return;
                }

                EditorApplication.update -= CreateVersionText;
                EditorApplication.update += AddVersionTextToAddressableGroup;
            }
        }

        /// <summary>
        /// VersionText를 AddressableGroup에 포함시킵니다
        /// </summary>
        static void AddVersionTextToAddressableGroup()
        {
            AddressableAssetGroup default_group = null;

            foreach (var group in Settings.groups)
            {
                if (group.Name.Equals(BuildLauncher.DEFAULT_GROUP_NAME))
                {
                    default_group = group;
                    break;
                }
            }

            Debug.Assert(default_group != null, "Default 그룹을 찾을 수 없습니다.");

            var entry = default_group.GetAssetEntry(BuildLauncher.VERSION_TEXT_GUID);

            if (entry != null)
            {
                EditorApplication.update -= AddVersionTextToAddressableGroup;
                return;
            }

            entry = Settings.CreateOrMoveEntry(BuildLauncher.VERSION_TEXT_GUID, default_group);
            entry.address = BuildLauncher.ADDRESSABLE_VERSION_FILE_PATH.Replace(".txt", "");
            List<AddressableAssetEntry> entries = new List<AddressableAssetEntry>
            {
                entry
            };

            Settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryAdded, entries, true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif
