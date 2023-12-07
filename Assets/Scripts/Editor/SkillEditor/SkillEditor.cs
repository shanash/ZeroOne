using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


public class SkillEditor : EditorWindow
{

    [MenuItem("FluffyDuck/Skill/Editor", false, 1)]
    static void ShowWindow()
    {
        if (!IsSkillEditorScene())
        {
            if (EditorUtility.DisplayDialog("Scene Error", "skill_editor 씬에서만 사용이 가능합니다.\n해당 씬으로 이동하시겠습니까?", "Ok", "Cancel"))
            {
                EditorSceneManager.OpenScene("Assets/Scenes/skill_editor.unity");
                GetWindow<SkillEditor>("Skill Editor").minSize = new Vector2(400, 600);
            }
            
            return;
        }
        GetWindow<SkillEditor>("Skill Editor").minSize = new Vector2(400, 400);

    }

    static bool IsSkillEditorScene()
    {
        string scene_name = EditorSceneManager.GetActiveScene().name;
        return scene_name.Equals("skill_editor");
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void OnGUI()
    {
        
    }
}
