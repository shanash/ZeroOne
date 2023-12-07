using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SkillPreview : EditorWindow
{
    [MenuItem("FluffyDuck/Skill/Preview", false, 0)]
    static void ShowWindow()
    {
        if (!IsSkillPreviewScene())
        {
            if (EditorUtility.DisplayDialog("Scene Error", "skill_preview 씬에서만 사용이 가능합니다.\n해당 씬으로 이동하시겠습니까?", "Ok", "Cancel"))
            {
                EditorSceneManager.OpenScene("Assets/Scenes/skill_preview.unity");
                GetWindow<SkillPreview>("Skill Preview").minSize = new Vector2(400, 600);
            }
            return;
        }
        GetWindow<SkillPreview>("Skill Preview").minSize = new Vector2(400, 600);
    }

    static bool IsSkillPreviewScene()
    {
        string scene_name = EditorSceneManager.GetActiveScene().name;
        return scene_name.Equals("skill_preview");
    }
}
