using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SkillPreview : EditorWindow
{
    /// <summary>
    /// 영웅 또는 NPC 의 스킬을 선택할 것인지 여부 판단
    /// </summary>
    int Choice_Char_Type_Index = (int)CHARACTER_TYPE.PC;

    int Hero_Index;

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
        GetWindow<SkillPreview>("스킬 미리보기").minSize = new Vector2(400, 600);
    }

    static bool IsSkillPreviewScene()
    {
        string scene_name = EditorSceneManager.GetActiveScene().name;
        return scene_name.Equals("skill_preview");
    }

    

    private void OnEnable()
    {
        
    }

    private void OnGUI()
    {
        if (EditorApplication.isPlaying)
        {
            LayoutPlayingMode();
        }
        else
        {
            LayoutNonPlayingMode();
        }
        
        
    }
    /// <summary>
    /// Play button 그리기
    /// </summary>
    void LayoutNonPlayingMode()
    {
        GUILayout.Space(10);

        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal("Skill Box");
        GUILayout.FlexibleSpace(); // 버튼들을 오른쪽으로 밀어내기
        if (GUILayout.Button("Play", GUILayout.Width(100), GUILayout.Height(40)))
        {
            EditorApplication.EnterPlaymode();
        }
        GUILayout.FlexibleSpace(); // 버튼들을 왼쪽으로 밀어내기
        EditorGUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndVertical();
    }
    /// <summary>
    /// 플레이 중
    /// 플레이어 캐릭터 / NPC의 스킬 정보를 보여준다.
    /// </summary>
    void LayoutPlayingMode()
    {
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical("Skill Box");

        CHARACTER_TYPE char_type = (CHARACTER_TYPE)Array.IndexOf(Enum.GetNames(typeof(CHARACTER_TYPE)), ((CHARACTER_TYPE)Choice_Char_Type_Index).ToString());
        if (char_type == CHARACTER_TYPE.PC)
        {
            GUILayout.Label("플레이어 캐릭터 스킬", EditorStyles.boldLabel);
        }
        else
        {
            GUILayout.Label("NPC 스킬", EditorStyles.boldLabel);
        }

        GUILayout.Space(10);


        Choice_Char_Type_Index = EditorGUILayout.Popup("캐릭터 타입 선택", Choice_Char_Type_Index, Enum.GetNames(typeof(CHARACTER_TYPE)));
        if (char_type == CHARACTER_TYPE.PC)
        {
            LayoutPcType();
        }
        else
        {
            LayoutNpcType();
        }
        EditorGUILayout.EndVertical();
    }

    void LayoutPcType()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        var m = MasterDataManager.Instance;

        EditorGUILayout.BeginVertical("Drop Box");
        string[] dropdown_labels = new string[] { "100001", "100002" };
        Hero_Index = EditorGUILayout.Popup("영웅 선택", Hero_Index, dropdown_labels);
        GUILayout.Space(10);
        EditorGUILayout.EndVertical();

        AddLayoutStopMode();
    }
    void LayoutNpcType()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        var m = MasterDataManager.Instance;

        EditorGUILayout.BeginHorizontal("Drop Box");
        string[] dropdown_labels = new string[] { "100001", "100002" };
        Hero_Index = EditorGUILayout.Popup("NPC 선택", Hero_Index, dropdown_labels);
        GUILayout.Space(10);
        EditorGUILayout.EndHorizontal();


        AddLayoutStopMode();
    }

    void AddLayoutStopMode()
    {
        //  stop button
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal("Stop Mode");
        GUILayout.FlexibleSpace(); // 버튼들을 오른쪽으로 밀어내기
        if (GUILayout.Button("Stop", GUILayout.Width(100), GUILayout.Height(40)))
        {
            EditorApplication.ExitPlaymode();
        }
        GUILayout.FlexibleSpace(); // 버튼들을 오른쪽으로 밀어내기
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
    }
}
