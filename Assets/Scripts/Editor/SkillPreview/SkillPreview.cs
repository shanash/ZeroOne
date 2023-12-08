using FluffyDuck.Util;
using System;
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
    int Prev_Choice_Char_Type_Index = (int)CHARACTER_TYPE.PC;

    /// <summary>
    /// 영웅 데이터 리스트
    /// </summary>
    List<BattleUnitData> Hero_Unit_Data_List = new List<BattleUnitData>();
    int Hero_Index;
    int Prev_Hero_Index;

    /// <summary>
    /// NPC 데이터 리스트
    /// </summary>
    List<BattleUnitData> Npc_Unit_Data_List = new List<BattleUnitData>();
    int Npc_Index;
    int Prev_Npc_Index;

    /// <summary>
    /// 영웅 선택 드롭다운 메뉴 리스트
    /// </summary>
    List<string> Unit_Choice_Drop_Down_Menu_List = new List<string>();

    /// <summary>
    /// 현재 사용중인 유닛 리스트
    /// </summary>
    List<HeroBase_V2> Used_Unit_List = new List<HeroBase_V2>();

    /// <summary>
    /// 유닛 썸네일
    /// </summary>
    Texture Unit_Thumbnail;



    [MenuItem("FluffyDuck/Skill/Preview", false, 0)]
    static void ShowWindow()
    {
        if (!IsSkillPreviewScene())
        {
            if (EditorUtility.DisplayDialog("Scene Error", "skill_preview 씬에서만 사용이 가능합니다.\n해당 씬으로 이동하시겠습니까?", "이동", "취소"))
            {
                EditorSceneManager.OpenScene("Assets/Scenes/skill_preview.unity");
                GetWindow<SkillPreview>("Skill Preview").minSize = new Vector2(400, 600);
            }
            return;
        }
        GetWindow<SkillPreview>("스킬 미리보기").minSize = new Vector2(400, 600);
    }

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    static bool IsSkillPreviewScene()
    {
        string scene_name = EditorSceneManager.GetActiveScene().name;
        return scene_name.Equals("skill_preview");
    }

    
    private void OnGUI()
    {
        
        if (EditorApplication.isPlaying)
        {
            if (!IsSkillPreviewScene())
            {
                GUILayout.Space(10);
                GUILayout.Label("대기중 입니다.", EditorStyles.boldLabel);
                return;
            }
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
        {
            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal("Skill Box");
            {
                GUILayout.FlexibleSpace(); // 버튼들을 오른쪽으로 밀어내기
                if (GUILayout.Button("Play", GUILayout.Width(100), GUILayout.Height(40)))
                {
                    if (!IsSkillPreviewScene())
                    {
                        if (EditorUtility.DisplayDialog("Scene Error", "skill_preview 씬에서만 사용이 가능합니다.\n해당 씬으로 이동하시겠습니까?", "이동", "취소"))
                        {
                            EditorSceneManager.OpenScene("Assets/Scenes/skill_preview.unity");
                        }
                        return;
                    }
                    EditorApplication.EnterPlaymode();
                }
                GUILayout.FlexibleSpace(); // 버튼들을 왼쪽으로 밀어내기
            }
            
            EditorGUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
        }
        
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
        {
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

            EditorGUILayout.BeginHorizontal();
            {
                Choice_Char_Type_Index = EditorGUILayout.Popup("캐릭터 타입 선택", Choice_Char_Type_Index, Enum.GetNames(typeof(CHARACTER_TYPE)));
                GUILayout.Space(10);
            }
            EditorGUILayout.EndHorizontal();


            if (char_type == CHARACTER_TYPE.PC)
            {
                LayoutPcType();
            }
            else
            {
                LayoutNpcType();
            }
        }

            
        EditorGUILayout.EndVertical();
    }

    void LayoutPcType()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        
        //  영웅 데이터가 없을 경우 마스터에서 데이터를 가져와서 추가한다
        if (Hero_Unit_Data_List.Count == 0)
        {
            List<Player_Character_Data> pc_list = new List<Player_Character_Data>();
            MasterDataManager.Instance.Get_PlayerCharacterDataList(ref pc_list);
            int pc_cnt = pc_list.Count;
            for (int i = 0; i < pc_cnt; i++)
            {
                Player_Character_Data pc_data = pc_list[i];
                var unit = new BattlePcData();
                unit.SetUnitID(pc_data.player_character_id, 0);
                Hero_Unit_Data_List.Add(unit);
            }
        }

        ResetUnitChoiceDropDownMenuList(CHARACTER_TYPE.PC);


        EditorGUILayout.BeginHorizontal("Drop Box");
        {
            Hero_Index = EditorGUILayout.Popup("영웅 선택", Hero_Index, Unit_Choice_Drop_Down_Menu_List.ToArray());
            if (GUILayout.Button("불러오기", GUILayout.Width(80), GUILayout.Height(20)))
            {
                if (Hero_Index != 0)
                {
                    var unit = Hero_Unit_Data_List[Hero_Index - 1];
                    SpawnGameObject(unit);
                    //Debug.Log($"불러오기 PC => {unit.GetUnitID()}");

                    //var pool = GameObjectPoolManager.Instance;
                    //pool.GetGameObject(unit.GetPrefabPath(), null, (obj) =>
                    //{
                    //    var battle_unit = obj.GetComponent<HeroBase_V2>();
                    //    battle_unit.SetBattleUnitDataID(unit.GetUnitID(), unit.GetUnitNum());
                    //    Used_Unit_List.Add(battle_unit);
                    //});
                }
                
            }
            GUILayout.Space(10);
        }
        EditorGUILayout.EndHorizontal();

        AddLayoutUnitThumbnail();

        AddLayoutStopMode();
    }
    void LayoutNpcType()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        //  NPC 데이터가 없을 경우 마스터에서 데이터를 가져와서 추가한다
        if (Npc_Unit_Data_List.Count == 0)
        {
            List<Npc_Data> npc_list = new List<Npc_Data>();
            MasterDataManager.Instance.Get_NpcDataList(ref npc_list);
            int npc_cnt = npc_list.Count;
            for (int i = 0; i < npc_cnt; i++)
            {
                Npc_Data npc = npc_list[i];
                var unit = new BattleNpcData();
                unit.SetUnitID(npc.npc_data_id);
                Npc_Unit_Data_List.Add(unit);
            }
        }
        ResetUnitChoiceDropDownMenuList(CHARACTER_TYPE.NPC);

        EditorGUILayout.BeginHorizontal("Drop Box");
        {
            Npc_Index = EditorGUILayout.Popup("NPC 선택", Npc_Index, Unit_Choice_Drop_Down_Menu_List.ToArray());

            if (GUILayout.Button("불러오기", GUILayout.Width(80), GUILayout.Height(20)))
            {
                if (Npc_Index != 0)
                {
                    var unit = Npc_Unit_Data_List[Npc_Index - 1];
                    SpawnGameObject(unit);
                    //var pool = GameObjectPoolManager.Instance;
                    //pool.GetGameObject(unit.GetPrefabPath(), null, (obj) =>
                    //{
                    //    var battle_unit = obj.GetComponent<HeroBase_V2>();
                    //    battle_unit.SetBattleUnitDataID(unit.GetUnitID());
                    //    Used_Unit_List.Add(battle_unit);
                    //});
                }
                
            }
            GUILayout.Space(10);
        }
        EditorGUILayout.EndHorizontal();


        AddLayoutUnitThumbnail();

        AddLayoutStopMode();
    }

    void SpawnGameObject(BattleUnitData unit)
    {
        GameObjectPoolManager.Instance.GetGameObject(unit.GetPrefabPath(), null, (obj) =>
        {
            var battle_unit = obj.GetComponent<HeroBase_V2>();
            battle_unit.SetBattleUnitDataID(unit.GetUnitID(), unit.GetUnitNum());
            battle_unit.GetSkeletonRenderTexture().enabled = false;
            Used_Unit_List.Add(battle_unit);
        });
    }

    /// <summary>
    /// 영웅 또는 NPC의 데이터 ID를 초기화 하는 함수
    /// </summary>
    /// <param name="ctype"></param>
    void ResetUnitChoiceDropDownMenuList(CHARACTER_TYPE ctype)
    {
        Unit_Choice_Drop_Down_Menu_List.Clear();
        Unit_Choice_Drop_Down_Menu_List.Add("NONE");
        if (ctype == CHARACTER_TYPE.PC)
        {
            int cnt = Hero_Unit_Data_List.Count;
            for (int i = 0; i < cnt; i++)
            {
                var unit = Hero_Unit_Data_List[i];
                Unit_Choice_Drop_Down_Menu_List.Add(unit.GetUnitID().ToString());
            }
        }
        else
        {
            int cnt = Npc_Unit_Data_List.Count;
            for (int i = 0; i < cnt; i++)
            {
                var unit = Npc_Unit_Data_List[i];
                Unit_Choice_Drop_Down_Menu_List.Add(unit.GetUnitID().ToString());
            }
        }
    }

    #region Add Layout Funcs
    /// <summary>
    /// Stop 버튼 생성
    /// </summary>
    void AddLayoutStopMode()
    {
        //  stop button
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal("Stop Mode");
        {
            GUILayout.FlexibleSpace(); // 버튼들을 오른쪽으로 밀어내기
            if (GUILayout.Button("Stop", GUILayout.Width(100), GUILayout.Height(40)))
            {
                EditorApplication.ExitPlaymode();
            }
            GUILayout.FlexibleSpace(); // 버튼들을 오른쪽으로 밀어내기
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
    }

    void AddLayoutUnitThumbnail()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Box(Unit_Thumbnail, GUILayout.Width(128), GUILayout.Height(128));
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
    }


    #endregion



    /// <summary>
    /// 캐릭터 타입이 변경되었을 때 호출되는 함수
    /// </summary>
    void ChangeCharacterTypeIndex()
    {
        Hero_Index = 0;
        Npc_Index = 0;

    }

    /// <summary>
    /// 캐릭터 타입이 변경되었는지 여부 판단
    /// 만약 변경되었을 경우, 함수 호출 <see cref="ChangeCharacterTypeIndex"/>
    /// </summary>
    void CheckCharacterTypeIndex()
    {
        if (Prev_Choice_Char_Type_Index != Choice_Char_Type_Index)
        {
            Prev_Choice_Char_Type_Index = Choice_Char_Type_Index;
            //  todo callback
            ChangeCharacterTypeIndex();
        }
    }

    /// <summary>
    /// 영웅 또는 NPC의 선택 인덱스가 변경되었을 경우
    /// </summary>
    void ChangeUnitDataChoiceIndex()
    {
        CHARACTER_TYPE ctype = GetCharacterType();
        if (ctype == CHARACTER_TYPE.PC)
        {
            if (Hero_Index == 0)
            {
                //  NONE 선택
                return;
            }
        }
        else
        {
            if (Npc_Index == 0)
            {
                //  NONE 선택
                return;
            }
        }
        
        
    }
    /// <summary>
    /// 영웅 또는 NPC의 선택 인덱스가 변경되었을 경우
    /// 만약 변경되었을 경우, 함수 호출 <see cref="ChangeUnitDataChoiceIndex"/>
    /// </summary>
    void CheckUnitChoiceIndex()
    {
        CHARACTER_TYPE ctype = GetCharacterType();
        if (ctype == CHARACTER_TYPE.PC)
        {
            if (Prev_Hero_Index != Hero_Index)
            {
                Prev_Hero_Index = Hero_Index;
                ChangeUnitDataChoiceIndex();
            }
        }
        else
        {
            if (Prev_Npc_Index != Npc_Index)
            {
                Prev_Npc_Index = Npc_Index;
                ChangeUnitDataChoiceIndex();
            }
        }
    }

    CHARACTER_TYPE GetCharacterType()
    {
        return (CHARACTER_TYPE)Choice_Char_Type_Index;
    }

    void UpdateUnitThumbnail()
    {
        if (!EditorApplication.isPlaying)
        {
            return;
        }
        if (Selection.activeGameObject == null)
        {
            return;
        }
   

        var unit = Selection.activeGameObject.GetComponent<HeroBase_V2>();
        if (unit != null)
        {
            string path = unit.GetBattleUnitData().GetThumbnailPath();
            if (!string.IsNullOrEmpty(path))
            {
                CommonUtils.GetResourceFromAddressableAsset<Texture>(path, (obj) =>
                {
                    Unit_Thumbnail = obj;
                });

            }
            else
            {
                Unit_Thumbnail = null;
            }
        }

    }

    void ClearUnitThumnail()
    {
        Unit_Thumbnail = null;
    }
    

    private void OnInspectorUpdate()
    {
        //  캐릭터 타입 변경되었는지 체크
        CheckCharacterTypeIndex();

        CheckUnitChoiceIndex();

        if (Selection.activeGameObject != null)
        {
            UpdateUnitThumbnail();
        }
        else
        {
            ClearUnitThumnail();
        }
    }
}
