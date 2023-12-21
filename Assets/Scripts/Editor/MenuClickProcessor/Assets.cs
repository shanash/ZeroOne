using System.IO;
using UnityEditor;

namespace FluffyDuck.EditorUtil.UpperMenu
{
    public static class Assets
    {
        [MenuItem("Assets/Create/C# Factory Script", priority = 19)]
        private static void Create_CsharpFactoryScript()
        {
            // 사용자 정의 스크립트 템플릿 경로
            string templatePath = "Assets/Scripts/Editor/MenuClickProcessor/FactoryInstance.txt";

            // 새 스크립트 파일 경로 및 이름 설정
            string targetPath = AssetDatabase.GetAssetPath(Selection.activeObject) + "/FactoryInstance.cs";

            // 템플릿에서 새 스크립트 파일 생성
            File.Copy(templatePath, targetPath);
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/Create/C# MonoBehaviour Factory Script", priority = 19)]
        private static void Create_CsharpMonobehaviourFactoryScript()
        {
            // 사용자 정의 스크립트 템플릿 경로
            string templatePath = "Assets/Scripts/Editor/MenuClickProcessor/MBFactoryInstance.txt";

            // 새 스크립트 파일 경로 및 이름 설정
            string targetPath = AssetDatabase.GetAssetPath(Selection.activeObject) + "/MBFactoryInstance.cs";

            // 템플릿에서 새 스크립트 파일 생성
            File.Copy(templatePath, targetPath);
            AssetDatabase.Refresh();
        }
    }
}
