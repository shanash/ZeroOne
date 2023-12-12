using DocumentFormat.OpenXml.Drawing.Diagrams;
using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SkillPreview : EditorWindow
{
    /// <summary>
    /// 영웅 또는 NPC 의 스킬을 선택할 것인지 여부 판단
    /// </summary>
    CHARACTER_TYPE Choice_Character_Type = CHARACTER_TYPE.NONE;
    CHARACTER_TYPE Prev_Choice_Character_Type = CHARACTER_TYPE.NONE;

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
    /// 현재 사용중인 유닛
    /// </summary>
    HeroBase_V2 Used_Unit;

    /// <summary>
    /// 유닛 썸네일
    /// </summary>
    Texture Unit_Thumbnail;


    bool Show_Skill_Info;
    SKILL_TYPE Skill_Type = SKILL_TYPE.NONE;
    SKILL_TYPE Prev_Skill_Type = SKILL_TYPE.NONE;


    
    public static void ShowWindow()
    {
        if (!IsSkillPreviewScene())
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }
            if (EditorUtility.DisplayDialog("Scene Error", "skill_preview 씬에서만 사용이 가능합니다.\n해당 씬으로 이동하시겠습니까?", "이동", "취소"))
            {
                EditorSceneManager.OpenScene("Assets/Scenes/skill_preview.unity");
                GetWindow<SkillPreview>("Skill Preview").minSize = new Vector2(400, 600);
            }
            return;
        }
        GetWindow<SkillPreview>("스킬 미리보기").minSize = new Vector2(400, 600);
    }

    void ResetData()
    {
        //  c type
        Choice_Character_Type = CHARACTER_TYPE.NONE;
        Prev_Choice_Character_Type = CHARACTER_TYPE.NONE;

        //  hero datas
        Hero_Unit_Data_List.ForEach(x => x.Dispose());
        Hero_Unit_Data_List.Clear();
        Hero_Index = 0;
        Prev_Hero_Index = 0;

        //  npc datas
        Npc_Unit_Data_List.ForEach(x => x.Dispose());
        Npc_Index = 0;
        Prev_Npc_Index = 0;

        //  Used_Unit
        ClearBattleUnit();

        //  clear thumbnail
        ClearUnitThumnail();

        Unit_Choice_Drop_Down_Menu_List.Clear();

        //  skill info
        Show_Skill_Info = false;
        Skill_Type = SKILL_TYPE.NONE;
        Prev_Skill_Type = SKILL_TYPE.NONE;
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
                    ResetData();
                    BlackBoard.Instance.SetBlackBoard(BLACK_BOARD_KEY.GAME_TYPE, GAME_TYPE.EDITOR_SKILL_PREVIEW_MODE);

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
            //CHARACTER_TYPE char_type = (CHARACTER_TYPE)Array.IndexOf(Enum.GetNames(typeof(CHARACTER_TYPE)), ((CHARACTER_TYPE)Choice_Char_Type_Index).ToString());
            if (Choice_Character_Type == CHARACTER_TYPE.PC)
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
                //Choice_Char_Type_Index = EditorGUILayout.Popup("캐릭터 타입 선택", Choice_Char_Type_Index, Enum.GetNames(typeof(CHARACTER_TYPE)));
                Choice_Character_Type = (CHARACTER_TYPE)EditorGUILayout.EnumPopup("캐릭터 타입 선택", Choice_Character_Type);
                GUILayout.Space(10);
            }
            EditorGUILayout.EndHorizontal();


            if (Choice_Character_Type == CHARACTER_TYPE.PC)
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

        ResetUnitChoiceDropDownMenuList(Choice_Character_Type);


        EditorGUILayout.BeginHorizontal("Drop Box");
        {
            Hero_Index = EditorGUILayout.Popup("영웅 선택", Hero_Index, Unit_Choice_Drop_Down_Menu_List.ToArray());
            if (GUILayout.Button("불러오기", GUILayout.Width(80), GUILayout.Height(20)))
            {
                if (Hero_Index != 0)
                {
                    var unit = Hero_Unit_Data_List[Hero_Index - 1];
                    SpawnGameObject(unit);
                }
                
            }
            GUILayout.Space(10);
        }
        EditorGUILayout.EndHorizontal();

        

        AddLayoutUnitThumbnail();

        //  skill choice
        AddSkillChoice();

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
        ResetUnitChoiceDropDownMenuList(Choice_Character_Type);

        EditorGUILayout.BeginHorizontal("Drop Box");
        {
            Npc_Index = EditorGUILayout.Popup("NPC 선택", Npc_Index, Unit_Choice_Drop_Down_Menu_List.ToArray());

            if (GUILayout.Button("불러오기", GUILayout.Width(80), GUILayout.Height(20)))
            {
                if (Npc_Index != 0)
                {
                    var unit = Npc_Unit_Data_List[Npc_Index - 1];
                    SpawnGameObject(unit);
                }
                
            }
            GUILayout.Space(10);
        }
        EditorGUILayout.EndHorizontal();

        AddLayoutUnitThumbnail();
        AddSkillChoice();
        AddLayoutStopMode();
    }

    /// <summary>
    /// 유닛 소환
    /// </summary>
    /// <param name="unit"></param>
    void SpawnGameObject(BattleUnitData unit)
    {
        
        GameObjectPoolManager.Instance.GetGameObject(unit.GetPrefabPath(), null, (obj) =>
        {
            ClearBattleUnit();
            Used_Unit = obj.GetComponent<HeroBase_V2>();
            Used_Unit.SetBattleUnitDataID(unit.GetUnitID(), unit.GetUnitNum());
            Used_Unit.GetSkeletonRenderTexture().enabled = false;
            
        });
    }

    /// <summary>
    /// 사용중인 유닛을 제거
    /// </summary>
    void ClearBattleUnit()
    {
        if (Used_Unit == null)
        {
            return;
        }
        var pool = GameObjectPoolManager.Instance;
        pool.UnusedGameObject(Used_Unit.gameObject);
        Used_Unit = null;
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
        else if (ctype == CHARACTER_TYPE.NPC)
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
    /// <summary>
    /// 썸네일 
    /// </summary>
    void AddLayoutUnitThumbnail()
    {
        if (Used_Unit == null)
        {
            return;
        }
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("썸네일", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Box(Unit_Thumbnail, GUILayout.Width(128), GUILayout.Height(128));
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 스킬 선택 UI 추가
    /// </summary>
    void AddSkillChoice()
    {
        if (Used_Unit == null)
        {
            return;
        }
        Show_Skill_Info = EditorGUILayout.Foldout(Show_Skill_Info, "스킬", true);

        if (Show_Skill_Info)
        {
            //  스킬 선택
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(10);
                Skill_Type = (SKILL_TYPE)EditorGUILayout.EnumPopup("스킬 선택", Skill_Type);
            }
            EditorGUILayout.EndHorizontal();


        }


    }


    #endregion



    
    #region Change Callback
    /// <summary>
    /// 캐릭터 타입이 변경되었을 때 호출되는 함수
    /// </summary>
    void ChangeCharacterTypeIndex()
    {
        Hero_Index = 0;
        Npc_Index = 0;

        Skill_Type = SKILL_TYPE.NONE;
        ClearBattleUnit();
        ClearUnitThumnail();
    }


    /// <summary>
    /// 영웅 또는 NPC의 선택 인덱스가 변경되었을 경우
    /// </summary>
    void ChangeUnitDataChoiceIndex()
    {
        ClearUnitThumnail();
        ClearBattleUnit();
        if (Choice_Character_Type == CHARACTER_TYPE.PC)
        {
            if (Hero_Index == 0)
            {
                //  NONE 선택
                return;
            }
        }
        else if (Choice_Character_Type == CHARACTER_TYPE.NPC)
        {
            if (Npc_Index == 0)
            {
                //  NONE 선택
                return;
            }
        }
    }

    /// <summary>
    /// 영웅 또는 NPC의 스킬 타입 값이 변경되었을 경우
    /// </summary>
    void ChangeUnitSkillType()
    {

    }
    
    #endregion


    #region Check Each Index
    /// <summary>
    /// 캐릭터 타입이 변경되었는지 여부 판단
    /// 만약 변경되었을 경우, 함수 호출 <see cref="ChangeCharacterTypeIndex"/>
    /// </summary>
    void CheckCharacterTypeIndex()
    {
        if (Prev_Choice_Character_Type != Choice_Character_Type)
        {
            Prev_Choice_Character_Type = Choice_Character_Type;
            //  todo callback
            ChangeCharacterTypeIndex();
        }
    }


    /// <summary>
    /// 영웅 또는 NPC의 선택 인덱스가 변경되었을 경우
    /// 만약 변경되었을 경우, 함수 호출 <see cref="ChangeUnitDataChoiceIndex"/>
    /// </summary>
    void CheckUnitChoiceIndex()
    {
        if (Choice_Character_Type == CHARACTER_TYPE.PC)
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

    void CheckSkillType()
    {
        if (Choice_Character_Type != CHARACTER_TYPE.NONE)
        {
            if (Prev_Skill_Type != Skill_Type)
            {
                Prev_Skill_Type = Skill_Type;
                ChangeUnitSkillType();
            }
        }
    }
    #endregion



    void UpdateUnitThumbnail()
    {
        if (!EditorApplication.isPlaying)
        {
            return;
        }

        if (Used_Unit == null)
        {
            return;
        }
       
        if (Used_Unit != null)
        {
            string path = Used_Unit.GetBattleUnitData().GetThumbnailPath();
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

        CheckSkillType();

        if (Used_Unit != null)
        {
            UpdateUnitThumbnail();
        }
        else
        {
            ClearUnitThumnail();
        }
    }
}
