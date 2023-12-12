using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class BattleManager_V2 : MonoBehaviour
{
    GameStateSystem<BattleManager_V2, BattleUIManager_V2> FSM = null;

    private void Awake()
    {
        StartCoroutine(InitAssets());
    }

    IEnumerator InitAssets()
    {
        var handle = Addressables.InitializeAsync();
        yield return handle;

        var board = BlackBoard.Instance;

        //  게임 타입 정보 받기
        Game_Type = board.GetBlackBoardData<GAME_TYPE>(BLACK_BOARD_KEY.GAME_TYPE, GAME_TYPE.STORY_MODE);

        //  전투용 스테이지 정보 생성 (게임 타입에 따라 각기 다른 데이터를 사용 예정)
        switch (Game_Type)
        {
            case GAME_TYPE.STORY_MODE:
                {
                    int stage_id = board.GetBlackBoardData<int>(BLACK_BOARD_KEY.STORY_STAGE_ID, 100001);
                    board.RemoveBlackBoardData(BLACK_BOARD_KEY.STORY_STAGE_ID);     //  스테이지 id를 받은 후 해당 데이터를 삭제

                    Dungeon_Data = new BattleStageData();
                    Dungeon_Data.SetDungeonID(stage_id);
                }
                break;
        }


        List<string> list = new List<string>();
        list.Add("Assets/AssetResources/Prefabs/Fields/Battle_Field_01");
        
        list.Add("Assets/AssetResources/Prefabs/UI/LifeBarNode");
        list.Add("Assets/AssetResources/Prefabs/StageProceed/Team_Flag_Node");
        list.Add("Assets/AssetResources/Prefabs/StageProceed/Death_Member_Flag_Node");
        list.Add("Assets/AssetResources/Prefabs/StageProceed/Wave_Start_Point");
        list.Add("Assets/AssetResources/Prefabs/StageProceed/Wave_Mid_Point");
        list.Add("Assets/AssetResources/Prefabs/StageProceed/Wave_Boss_Point");

        //  effect
        list.Add("Assets/AssetResources/Prefabs/Effects/UI/Damage_Normal_Effect_Text");
        list.Add("Assets/AssetResources/Prefabs/Effects/UI/Heal_Normal_Effect_Text");
        list.Add("Assets/AssetResources/Prefabs/Effects/UI/Trans_Effect_Text");


        //  npc prefabs
        Dungeon_Data.GetMonsterPrefabsPath(ref list);

        var m = MasterDataManager.Instance;

        //  player character prefabs
        var deck = GameData.Instance.GetUserHeroDeckMountDataManager().FindSelectedDeck(Game_Type);
        var deck_heroes = deck.GetDeckHeroes();
        int cnt = deck_heroes.Count;
        for (int i = 0; i < cnt; i++)
        {
            UserHeroDeckMountData hero = deck_heroes[i];
            Player_Character_Data hdata = m.Get_PlayerCharacterData(hero.Hero_Data_ID);
            if (hdata != null)
            {
                if (!list.Contains(hdata.prefab_path))
                {
                    list.Add(hdata.prefab_path);
                }
                GetPcSkillEffectPrefabPath(hdata, ref list);
            }
            
        }

        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(list, PreloadCallback);
    }

    /// <summary>
    /// 플레이어 캐릭터의 스킬 이펙트 리스트 가져오기
    /// </summary>
    /// <param name="pc_data"></param>
    /// <param name="list"></param>
    void GetPcSkillEffectPrefabPath(Player_Character_Data pc_data, ref List<string> list)
    {
        var m = MasterDataManager.Instance;
        List<Player_Character_Skill_Data> skill_list = new List<Player_Character_Skill_Data>();

        //  battle data
        var bdata = m.Get_PlayerCharacterBattleData(pc_data.battle_info_id);
        if (bdata != null)
        {
            int grp_cnt = bdata.skill_pattern.Length;

            for (int g = 0; g < grp_cnt; g++)
            {
                //  skill group
                var skill_group = m.Get_PlayerCharacterSkillGroupData(bdata.skill_pattern[g]);
                if (skill_group == null)
                {
                    Debug.Assert(false);
                    continue;
                }
                //  skill list
                m.Get_PlayerCharacterSkillDataListBySkillGroup(skill_group.pc_skill_group_id, ref skill_list);
                int skill_cnt = skill_list.Count;
                for (int s = 0; s < skill_cnt; s++)
                {
                    var pc_skill = skill_list[s];
                    //  pc skill effect
                    if (!string.IsNullOrEmpty(pc_skill.effect_path) && !list.Contains(pc_skill.effect_path))
                    {
                        list.Add(pc_skill.effect_path);
                    }
                    if (pc_skill.onetime_effect_ids != null)
                    {
                        //  onetime skill iist
                        for (int o = 0; o < pc_skill.onetime_effect_ids.Length; o++)
                        {
                            int onetime_skill_id = pc_skill.onetime_effect_ids[o];
                            if (onetime_skill_id == 0)
                            {
                                continue;
                            }
                            var onetime_data = m.Get_PlayerCharacterSkillOnetimeData(onetime_skill_id);
                            Debug.Assert(onetime_data != null);
                            if (!string.IsNullOrEmpty(onetime_data.effect_path) && !list.Contains(onetime_data.effect_path))
                            {
                                list.Add(onetime_data.effect_path);
                            }
                        }
                    }


                    //  duration skill list
                    if (pc_skill.duration_effect_ids != null)
                    {
                        for (int d = 0; d < pc_skill.duration_effect_ids.Length; d++)
                        {
                            int duration_skill_id = pc_skill.duration_effect_ids[d];
                            if (duration_skill_id == 0)
                            {
                                continue;
                            }
                            var duration_data = m.Get_PlayerCharacterSkillDurationData(duration_skill_id);
                            Debug.Assert(duration_data != null);
                            if (!string.IsNullOrEmpty(duration_data.effect_path) && !list.Contains(duration_data.effect_path))
                            {
                                list.Add(duration_data.effect_path);
                            }
                            //  반복 효과용 일회성 스킬 이펙트
                            int repeat_len = duration_data.repeat_pc_onetime_ids.Length;
                            for (int r = 0; r < repeat_len; r++)
                            {
                                int repeat_id = duration_data.repeat_pc_onetime_ids[r];
                                if (repeat_id == 0)
                                    continue;
                                var repeat_onetime = m.Get_PlayerCharacterSkillOnetimeData(repeat_id);
                                if (!string.IsNullOrEmpty(repeat_onetime.effect_path) && !list.Contains(repeat_onetime.effect_path))
                                {
                                    list.Add(repeat_onetime.effect_path);
                                }
                            }

                            //  종료 효과용 일회성 스킬 이펙트
                            int finish_len = duration_data.finish_pc_onetime_ids.Length;
                            for (int f = 0; f < finish_len; f++)
                            {
                                int finish_id = duration_data.finish_pc_onetime_ids[f];
                                if (finish_id == 0)
                                    continue;
                                var finish_onetime = m.Get_PlayerCharacterSkillOnetimeData(finish_id);
                                if (!string.IsNullOrEmpty(finish_onetime.effect_path) && !list.Contains(finish_onetime.effect_path))
                                {
                                    list.Add(finish_onetime.effect_path);
                                }
                            }
                        }
                    }
                    

                }
            }
        }
    }


    void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (total_cnt == 0)
        {
            InitStates();
            return;
        }
        float per = (float)load_cnt / (float)total_cnt;
        Debug.LogFormat("Load Resource [{0}/{1}] <color=#ffffff>({2:P1})</color>", load_cnt, total_cnt, per);
        if (load_cnt == total_cnt)
        {
            InitStates();
        }
    }

    #region FSM
    void InitStates()
    {
        FSM = new GameStateSystem<BattleManager_V2, BattleUIManager_V2>();

        FSM.AddTransition(new GameStateInitV2());
        FSM.AddTransition(new GameStateReadyV2());
        FSM.AddTransition(new GameStateWaveInfoV2());
        FSM.AddTransition(new GameStateSpawnV2());
        FSM.AddTransition(new GameStatePlayingV2());
        FSM.AddTransition(new GameStateNextWaveV2());
        FSM.AddTransition(new GameStateWaveRunV2());
        FSM.AddTransition(new GameStatePauseV2());
        FSM.AddTransition(new GameStateGameOverWinV2());
        FSM.AddTransition(new GameStateGameOverLoseV2());
        FSM.AddTransition(new GameStateEndV2());

        FSM.Lazy_Init_Setting(this, UI_Mng, GAME_STATES.INIT);
    }
    public void ChangeState(GAME_STATES trans)
    {
        FSM?.ChangeState(trans);
    }
    public void RevertState()
    {
        FSM?.RevertState();
    }
    public GAME_STATES GetCurrentState()
    {
        if (FSM != null)
        {
            return FSM.CurrentTransitionID;
        }
        return GAME_STATES.NONE;
    }
    public GAME_STATES GetPreviousState()
    {
        if (FSM != null)
        {
            return FSM.PreviousTransitionID;
        }
        return GAME_STATES.NONE;
    }
    public bool IsPrevPause()
    {
        return GetPreviousState() == GAME_STATES.PAUSE;
    }

    public bool IsPause()
    {
        return GetCurrentState() == GAME_STATES.PAUSE;
    }
    private void Update()
    {
        FSM?.UpdateState();
    }
    #endregion

    #region Game States

    public void GameStateInitBegin() 
    {
        InitBattleField();
    }
    public void GameStateInit() 
    {
        ChangeState(GAME_STATES.READY);
    }
    public void GameStateInitExit() { }

    public void GameStateReadyBegin() 
    {
        StartStageProceeding();
    }
    public void GameStateReady() 
    {
        ChangeState(GAME_STATES.SPAWN);
    }
    public void GameStateReadyExit() { }

    public void GameStateSpawnBegin()
    {
        SpawnUnits();
    }
    public void GameStateSpawn()
    {
        ChangeState(GAME_STATES.WAVE_INFO);
    }
    public void GameStateSpawnExit() { }


    public void GameStateWaveInfoBegin() 
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Battle/WaveInfoUI", (popup) =>
        {
            popup.SetHideCompleteCallback(WaveInfoCloseCallback);
            Wave_Data wdata = (Wave_Data)Dungeon_Data.GetWaveData();
            popup.ShowPopup(wdata, 1f);
        });
    }
    public void GameStateWaveInfo() { }
    public void GameStateWaveInfoExit() { }


    public void GameStatePlayingBegin() 
    {
        if (IsPrevPause())
        {
            return;
        }
        TeamMembersChangeState(UNIT_STATES.MOVE_IN);
    }
    public void GameStatePlaying() 
    {
        var all_death_team = Used_Team_List.Find(x => !x.IsAliveMembers());
        if (all_death_team != null)
        {
            var win_team = FindTeamManager(all_death_team.Team_Type == TEAM_TYPE.LEFT ? TEAM_TYPE.RIGHT : TEAM_TYPE.LEFT);
            win_team.ChangeStateTeamMembers(UNIT_STATES.WIN);

            if (win_team.Team_Type == TEAM_TYPE.LEFT)
            {
                ChangeState(GAME_STATES.GAME_OVER_WIN);
            }
            else
            {
                ChangeState(GAME_STATES.GAME_OVER_LOSE);
            }
        }
    }
    public void GameStatePlayingExit() { }

    public void GameStateNextWaveBegin() { }
    public void GameStateNextWave() { }
    public void GameStateNextWaveExit() { }

    public void GameStateWaveRunBegin() { }
    public void GameStateWaveRun() { }
    public void GameStateWaveRunExit() { }

    public void GameStatePauseBegin() 
    {
        TeamMembersChangeState((UNIT_STATES.PAUSE));
        GetEffectFactory().OnPause();
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/BattlePausePopup", (popup) =>
        {
            popup.SetHideCompleteCallback(() =>
            {
                if (IsPause())
                {
                    RevertState();
                }
            });
            popup.ShowPopup();
        });
    }
    public void GameStatePause() { }
    public void GameStatePauseExit() 
    {
        TeamMembersRevertState();
        GetEffectFactory().OnResume();
    }

    public void GameStateGameOverWinBegin() 
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameOverWinPopup", (popup) =>
        {
            popup.ShowPopup();
        });
    }
    public void GameStateGameOverWin() { }
    public void GameStateGameOverWinExit() { }

    public void GameStateGameOverLoseBegin() 
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameOverLosePopup", (popup) =>
        {
            popup.ShowPopup();
        });
    }
    public void GameStateGameOverLose() { }
    public void GameStateGameOverLoseExit() { }

    public void GameStateEndBegin() { }
    public void GameStateEnd() { }
    public void GameStateEndExit() { }



    #endregion

}
