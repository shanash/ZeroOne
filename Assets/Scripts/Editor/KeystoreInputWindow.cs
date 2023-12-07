#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FluffyDuck.EditorUtil
{
    public class KeystoreInputWindow : EditorWindow
    {
        private string _keystore_pw;
        private string _key_alias_name;
        private string _key_alias_pw;

        // 콜백을 위한 대리자(delegate) 선언
        public delegate void ContinueBuildDelegate(string keystorePass, string keyAliasName, string keyAliasPass);
        private ContinueBuildDelegate _continueBuildCallback;

        public static void ShowWindow(string keystore_pw, string key_alias_name, string key_alias_pw, ContinueBuildDelegate continueBuildCallback)
        {
            var window = GetWindow<KeystoreInputWindow>("Keystore Settings");
            window._keystore_pw = keystore_pw;
            window._key_alias_name = key_alias_name;
            window._key_alias_pw = key_alias_pw;
            window._continueBuildCallback = continueBuildCallback;
        }

        void OnGUI()
        {
            _keystore_pw = EditorGUILayout.PasswordField("Keystore Password", _keystore_pw);
            _key_alias_name = EditorGUILayout.TextField("Key Alias Name", _key_alias_name);
            _key_alias_pw = EditorGUILayout.PasswordField("Key Alias Password", _key_alias_pw);

            if (GUILayout.Button("OK"))
            {
                if (string.IsNullOrEmpty(_keystore_pw) || string.IsNullOrEmpty(_key_alias_name) || string.IsNullOrEmpty(_key_alias_pw))
                {
                    EditorUtility.DisplayDialog("에러", "값이 입력되지 않았습니다", "확인");
                    return;
                }

                // 콜백 함수를 호출하여 빌드 프로세스를 계속 진행합니다.
                _continueBuildCallback?.Invoke(_keystore_pw, _key_alias_name, _key_alias_pw);
                Close();
            }
        }
    }
}

#endif
