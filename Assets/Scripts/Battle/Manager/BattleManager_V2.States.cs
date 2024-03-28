using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class BattleManager_V2 : SceneControllerBase
{
    protected GameStateSystem<BattleManager_V2, BattleUIManager_V2> FSM = null;

    float Game_Over_Delta;

    private void Awake()
    {
        StartCoroutine(InitAssets());
    }

    IEnumerator InitAssets()
    {
        bool is_set_scene = false;
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
                    int stage_id = board.GetBlackBoardData<int>(BLACK_BOARD_KEY.DUNGEON_ID, 1001101);
                    board.RemoveBlackBoardData(BLACK_BOARD_KEY.DUNGEON_ID);     //  스테이지 id를 받은 후 해당 데이터를 삭제

                    Dungeon_Data = new BattleDungeon_StoryStageData();
                    Dungeon_Data.SetDungeonID(stage_id);

                    var user_dungeon_data = (UserStoryStageData)Dungeon_Data.GetUserDungeonData();
                    if (user_dungeon_data != null)
                    {
                        user_dungeon_data.AddChallenageCount();
                    }
                    GameData.Instance.GetUserStoryStageDataManager().Save();
                }
                break;
            case GAME_TYPE.BOSS_DUNGEON_MODE:
                {
                    int stage_id = board.GetBlackBoardData<int>(BLACK_BOARD_KEY.DUNGEON_ID, 100101);
                    board.RemoveBlackBoardData(BLACK_BOARD_KEY.DUNGEON_ID);     //  스테이지 id를 받은 후 해당 데이터를 삭제

                    Dungeon_Data = new BattleDungeon_BossStageData();
                    Dungeon_Data.SetDungeonID(stage_id);

                    var user_dungeon_data = (UserBossStageData)Dungeon_Data.GetUserDungeonData();
                    if (user_dungeon_data != null)
                    {
                        user_dungeon_data.AddDailyChallengeCount();
                    }
                    GameData.Instance.GetUserBossStageDataManager().Save();
                }
                break;
            case GAME_TYPE.EDITOR_SKILL_PREVIEW_MODE:
                {
                    int stage_id = board.GetBlackBoardData<int>(BLACK_BOARD_KEY.EDITOR_STAGE_ID, 100001);
                    board.RemoveBlackBoardData(BLACK_BOARD_KEY.EDITOR_STAGE_ID);    //  스테이지 id를 받은 후 해당 데이터 삭제

                    Dungeon_Data = new BattleDungeon_SkillPreviewEditorData();
                    Dungeon_Data.SetDungeonID(stage_id);
                }
                break;
        }

        CreateTeamManagers();

        //  audio
        List<string> audio_clip_list = new List<string>();
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/click_01");

        List<string> list = new List<string>();
        list.Add("Assets/AssetResources/Prefabs/Fields/Battle_Field_01");

        list.Add("Assets/AssetResources/Prefabs/UI/LifeBar/LeftTeam_LifeBar_V2");
        list.Add("Assets/AssetResources/Prefabs/UI/LifeBar/RightTeam_LifeBar_V2");
        list.Add("Assets/AssetResources/Prefabs/UI/LifeBar/Boss_Life_Bar_Node");

        list.Add("Assets/AssetResources/Prefabs/UI/SkillTooltip");
        list.Add("Assets/AssetResources/Prefabs/Units/Life_Bar_Node_V2");
        list.Add("Assets/AssetResources/Prefabs/StageProceed/Team_Flag_Node");
        list.Add("Assets/AssetResources/Prefabs/StageProceed/Death_Member_Flag_Node");
        list.Add("Assets/AssetResources/Prefabs/StageProceed/Wave_Start_Point");
        list.Add("Assets/AssetResources/Prefabs/StageProceed/Wave_Mid_Point");
        list.Add("Assets/AssetResources/Prefabs/StageProceed/Wave_Boss_Point");

        //  effect
        list.Add("Assets/AssetResources/Prefabs/Effects/Common/HealText_Effect");
        list.Add("Assets/AssetResources/Prefabs/Effects/Common/TransText_Effect");

        //  damage text v2
        list.Add("Assets/AssetResources/Prefabs/Damage_Text/Recovery_Text_Node");
        list.Add("Assets/AssetResources/Prefabs/Damage_Text/Physics_Total_Damage_Text_Node");
        list.Add("Assets/AssetResources/Prefabs/Damage_Text/Physics_Damage_Text_Node");
        list.Add("Assets/AssetResources/Prefabs/Damage_Text/Magic_Total_Damage_Text_Node");
        list.Add("Assets/AssetResources/Prefabs/Damage_Text/Magic_Damage_Text_Node");
        list.Add("Assets/AssetResources/Prefabs/Damage_Text/Miss_Text_Node");

        //  skill slot
        list.Add("Assets/AssetResources/Prefabs/UI/Battle/BattleSkillSlot_V2");
        list.Add("Assets/AssetResources/Prefabs/UI/Battle/BattleDurationSkillIconNode");
        list.Add("Assets/AssetResources/Prefabs/UI/Battle/EnemyDurationSkillIconNode");


        //  npc prefabs
        Dungeon_Data.GetMonsterPrefabsPath(ref list);
        Dungeon_Data.GetMonsterSkillVoiceAndFxSoundPath(ref audio_clip_list);

        var player_team = FindTeamManager(TEAM_TYPE.LEFT);
        if (player_team != null)
        {
            player_team.GetHeroPrefabsPath(ref list);
            player_team.GetHeroVoicePath(ref audio_clip_list);
        }

        AudioManager.Instance.PreloadAudioClipsAsync(audio_clip_list, null);

        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(list, PreloadCallback);
    }


    protected void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (total_cnt == 0)
        {
            InitStates();
            return;
        }
        //float per = (float)load_cnt / (float)total_cnt;
        //Debug.LogFormat("Load Resource [{0}/{1}] <color=#ffffff>({2:P1})</color>", load_cnt, total_cnt, per);
        if (load_cnt == total_cnt)
        {
            InitStates();
        }
    }

    #region FSM
    protected void InitStates()
    {
        FSM = new GameStateSystem<BattleManager_V2, BattleUIManager_V2>();

        FSM.AddTransition(new GameStateInitV2());
        FSM.AddTransition(new GameStateReadyV2());
        FSM.AddTransition(new GameStateEnterStoryDialogueV2());
        FSM.AddTransition(new GameStateFinishStoryDialogueV2());
        FSM.AddTransition(new GameStateWaveInfoV2());
        FSM.AddTransition(new GameStateSpawnV2());
        FSM.AddTransition(new GameStateMoveInV2());
        FSM.AddTransition(new GameStatePlayReadyV2());
        FSM.AddTransition(new GameStatePlayingV2());
        FSM.AddTransition(new GameStateUltimateSkillV2());
        FSM.AddTransition(new GameStateNextWaveV2());
        FSM.AddTransition(new GameStateWaveRunReadyV2());
        FSM.AddTransition(new GameStateWaveRunV2());
        FSM.AddTransition(new GameStatePauseV2());
        FSM.AddTransition(new GameStateTimeOutV2());
        FSM.AddTransition(new GameStateGameOverWinReadyV2());
        FSM.AddTransition(new GameStateGameOverWinV2());
        FSM.AddTransition(new GameStateGameOverLoseReadyV2());
        FSM.AddTransition(new GameStateGameOverLoseV2());
        FSM.AddTransition(new GameStateEndV2());

        FSM.Lazy_Init_Setting(this, UI_Mng, GAME_STATES.INIT);

        //SCManager.Instance.SetCurrent(this);
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

    public virtual void GameStateInitBegin()
    {
        InitBattleField();
        UI_Mng.UpdateTimeLimit(Dungeon_Data.Dungeon_Limit_Time);
    }
    public virtual void GameStateInit()
    {
        ChangeState(GAME_STATES.READY);
    }
    public virtual void GameStateInitExit() { }

    public virtual void GameStateReadyBegin()
    {

    }
    public virtual void GameStateReady()
    {
        if (Game_Type == GAME_TYPE.STORY_MODE)
        {
            var story_data = (Stage_Data)Dungeon_Data.GetDungeonData();
            if (!string.IsNullOrEmpty(story_data.entrance_dialogue))
            {
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Dialogue/StoryDialogueUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
                {
                    popup.AddClosedCallbackDelegate(EnterStoryDialogueCloseCallback);
                    popup.ShowPopup(story_data.entrance_dialogue);
                    SCManager.Instance.SetCurrent(this);

                });
                ChangeState(GAME_STATES.ENTER_STORY_DIALOGUE);
            }
            else
            {
                ChangeState(GAME_STATES.SPAWN);
                SCManager.Instance.SetCurrent(this);
            }
        }
        else
        {
            ChangeState(GAME_STATES.SPAWN);
            SCManager.Instance.SetCurrent(this);
        }
    }
    public virtual void GameStateReadyExit() { }

    public virtual void GameStateEnterStoryDialogueBegin() { }
    public virtual void GameStateEnterStoryDialogue() { }
    public virtual void GameStateEnterStoryDialogueExit() { }

    public virtual void GameStateFinishStoryDialogueBegin() { }
    public virtual void GameStateFinishStoryDialogue() { }
    public virtual void GameStateFinishStoryDialogueExit() { }

    public virtual void GameStateSpawnBegin()
    {
        SpawnUnits();
        //MainThreadDispatcher.Instance.AddAction(() =>
        //{
        //    ChangeState(GAME_STATES.MOVE_IN);
        //});
    }
    public virtual void GameStateSpawn()
    {
        ChangeState(GAME_STATES.MOVE_IN);
    }
    public virtual void GameStateSpawnExit() { }


    public virtual void GameStateWaveInfoBegin()
    {
        if (Dungeon_Data.Game_Type == GAME_TYPE.STORY_MODE || Dungeon_Data.Game_Type == GAME_TYPE.BOSS_DUNGEON_MODE)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/BattleStartPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.SetHideCompleteCallback(WaveInfoCloseCallback);
                if (Dungeon_Data.Game_Type == GAME_TYPE.STORY_MODE)
                {
                    popup.ShowPopup(0);
                }
                else if (Dungeon_Data.Game_Type == GAME_TYPE.BOSS_DUNGEON_MODE)
                {
                    popup.ShowPopup(1);
                }
                else
                {
                    Debug.Assert(false);
                }
            });
        }
        else
        {
            WaveInfoCloseCallback();
        }

    }
    public virtual void GameStateWaveInfo() { }
    public virtual void GameStateWaveInfoExit() { }

    public virtual void GameStateMoveInBegin() 
    {
        TeamMembersChangeState(UNIT_STATES.MOVE_IN);
    }
    public virtual void GameStateMoveIn() 
    {
        bool is_all_idle = true;
        for (int i = 0; i < Used_Team_List.Count; i++)
        {
            if (!Used_Team_List[i].IsAllMembersState(UNIT_STATES.IDLE))
            {
                is_all_idle = false;
                break;
            }
        }

        if (is_all_idle)
        {
            //ChangeState(GAME_STATES.PLAY_READY);
            if (Dungeon_Data.GetWave() == 0)
            {
                ChangeState(GAME_STATES.WAVE_INFO);
            }
            else
            {
                ChangeState(GAME_STATES.PLAY_READY);
            }
        }
    }
    public virtual void GameStateMoveInExit() { }

    
    public virtual void GameStatePlayReadyBegin() 
    {
        if (IsPrevPause())
        {
            return;
        }
    }
    public virtual void GameStatePlayReady() 
    {
        ChangeState(GAME_STATES.PLAYING);
    }
    public virtual void GameStatePlayReadyExit() 
    {
        TeamMembersChangeState(UNIT_STATES.ATTACK_READY_1);
    }


    public virtual void GameStateUltimateSkillBegin() 
    {
        if (UI_Hide_Coroutine != null)
        {
            StopCoroutine(UI_Hide_Coroutine);
        }
        StartCoroutine(DelayHideBattleUI(0.2f));
    }
    public virtual void GameStateUltimateSkill() 
    {
    }
    public virtual void GameStateUltimateSkillExit() 
    {
        UI_Mng.ShowBattleUI(true);
    }

    public virtual void GameStatePlayingBegin()
    {
        if (IsPrevPause())
        {
            return;
        }

        UI_Mng.UpdateWaveCount();
    }
    public virtual void GameStatePlaying()
    {
        var all_death_team = Used_Team_List.Find(x => !x.IsAliveMembers());
        if (all_death_team != null)
        {
            var win_team = FindTeamManager(all_death_team.Team_Type == TEAM_TYPE.LEFT ? TEAM_TYPE.RIGHT : TEAM_TYPE.LEFT);
            win_team.HideAllUnitLifeBar();
            if (win_team.Team_Type == TEAM_TYPE.LEFT)
            {
                //  다음 웨이브가 있다면
                if (Dungeon_Data.HasNextWave())
                {
                    ChangeState(GAME_STATES.WAVE_RUN_READY);
                }
                else // 없으면 게임 종료(승리)
                {
                    if (Game_Type == GAME_TYPE.STORY_MODE)
                    {
                        var story_data = (Stage_Data)Dungeon_Data.GetDungeonData();
                        if (!string.IsNullOrEmpty(story_data.outrance_dialogue))
                        {
                            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Dialogue/StoryDialogueUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
                            {
                                popup.AddClosedCallbackDelegate(FinishStoryDialoguCloseCallback);
                                popup.ShowPopup(story_data.outrance_dialogue);

                            });
                            ChangeState(GAME_STATES.FINISH_STORY_DIALOGUE);

                        }
                        else
                        {
                            ChangeState(GAME_STATES.GAME_OVER_WIN_READY);
                        }
                    }
                    else
                    {
                        ChangeState(GAME_STATES.GAME_OVER_WIN_READY);
                    }
                }

            }
            else
            {
                ChangeState(GAME_STATES.GAME_OVER_LOSE_READY);
            }
        }
        CalcDungeonLimitTime();
    }
    public virtual void GameStatePlayingExit() { }

    public virtual void GameStateNextWaveBegin()
    {
        if (IsPrevPause())
        {
            return;
        }
        //  지속성 효과를 제외한 다른 모든 이펙트들을 제거해야 하는데...
        GetEffectFactory().ClearAllEffects();

        if (Dungeon_Data.NextWave())
        {
            var my_team = FindTeamManager(TEAM_TYPE.LEFT);
            if (my_team != null)
            {
                my_team.ChangeStateTeamMembers(UNIT_STATES.SPAWN);
                my_team.LeftTeamResetPosition();
            }

            var enemy_team = FindTeamManager(TEAM_TYPE.RIGHT);
            if (enemy_team != null)
            {
                enemy_team.SpawnHeros();
            }
            //  아군 최상위로 올려주기
            my_team.SetTeamLastSibling();
            
            Fade_In_Out_Layer.StartMove(UIEaseBase.MOVE_TYPE.MOVE_OUT, () =>
            {
                ChangeState(GAME_STATES.MOVE_IN);
            });
        }
        else
        {
            if (Game_Type == GAME_TYPE.STORY_MODE)
            {
                var story_data = (Stage_Data)Dungeon_Data.GetDungeonData();
                if (!string.IsNullOrEmpty(story_data.outrance_dialogue))
                {
                    PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Dialogue/StoryDialogueUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
                    {
                        popup.AddClosedCallbackDelegate(FinishStoryDialoguCloseCallback);
                        popup.ShowPopup(story_data.outrance_dialogue);

                    });
                    ChangeState(GAME_STATES.FINISH_STORY_DIALOGUE);

                }
                else
                {
                    ChangeState(GAME_STATES.GAME_OVER_WIN);
                }
            }
            else
            {
                ChangeState(GAME_STATES.GAME_OVER_WIN);
            }
        }

    }
    public virtual void GameStateNextWave() { }
    public virtual void GameStateNextWaveExit() { }

    public virtual void GameStateWaveRunReadyBegin() 
    {
        Game_Over_Delta = 1.5f;
    }
    public virtual void GameStateWaveRunReady() 
    {
        Game_Over_Delta -= Time.deltaTime;
        if (Game_Over_Delta < 0f)
        {
            ChangeState(GAME_STATES.WAVE_RUN);
        }
    }
    public virtual void GameStateWaveRunReadyExit() { }
    public virtual void GameStateWaveRunBegin()
    {
        if (IsPrevPause())
        {
            return;
        }
        GetEffectFactory().ClearAllEffects();
        var my_team = FindTeamManager(TEAM_TYPE.LEFT);
        if (my_team != null)
        {
            my_team.RecoveryLifeWaveEndMembers();
            my_team.ChangeStateTeamMembers(UNIT_STATES.WAVE_RUN);
        }

        Fade_In_Out_Layer.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, () =>
        {
            ChangeState(GAME_STATES.NEXT_WAVE);
        });
    }

    public virtual void GameStateWaveRun()
    {
    }
    public virtual void GameStateWaveRunExit() { }

    public virtual void GameStatePauseBegin()
    {
        TeamMembersChangeState(UNIT_STATES.PAUSE);
        GetEffectFactory().OnPause();
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/BattlePausePopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.SetHideCompleteCallback(() =>
            {
                if (IsPause())
                {
                    RevertState();
                }
            });
            popup.ShowPopup(Dungeon_Data);
        });
    }
    public virtual void GameStatePause() { }
    public virtual void GameStatePauseExit()
    {
        TeamMembersRevertState();
        GetEffectFactory().OnResume();
    }

    
    public virtual void GameStateGameOverWinReadyBegin() 
    {
        Game_Over_Delta = 1.5f;
        AudioManager.Instance.StopAllFX();
        AudioManager.Instance.FXTimeStretch = GameDefine.GAME_SPEEDS[BATTLE_SPEED_TYPE.NORMAL_TYPE];
    }
    public virtual void GameStateGameOverWinReady() 
    {
        Game_Over_Delta -= Time.deltaTime;
        if (Game_Over_Delta < 0f)
        {
            ChangeState(GAME_STATES.GAME_OVER_WIN);
        }
    }
    public virtual void GameStateGameOverWinReadyExit() { }
    public virtual void GameStateGameOverWinBegin()
    {
        TooltipManager.I.CloseAll();
        GetEffectFactory().ClearAllEffects();
        var win_team = FindTeamManager(TEAM_TYPE.LEFT);
        win_team.ChangeStateTeamMembers(UNIT_STATES.WIN);

        for (int i = 0; i < Used_Team_List.Count; i++)
        {
            Used_Team_List[i].HideAllUnits();
        }

        UI_Mng.ShowBattleUI(false);
        Game_Over_Delta = 1f;

        SetBlur(true);
        Camera.main.RemoveLayerToCullingMask(12); // Unit
        Camera.main.RemoveLayerToCullingMask(13); // UnitEffect
        var tp = Camera.main.GetComponent<TexturePhotographer>();
        tp.Capture((tex) =>
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameResultWinPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup(this, Dungeon_Data, tex);
            });
            Camera.main.AddLayerToCullingMask(12); // Unit
            Camera.main.AddLayerToCullingMask(13); // UnitEffect

            SetBlur(false);
        });
    }

    public virtual void GameStateGameOverWin() 
    {
        Game_Over_Delta -= Time.deltaTime;
        if (Game_Over_Delta < 0f)
        {
            ReleaseAllBattleObjects();
        }
    }
    public virtual void GameStateGameOverWinExit() { }


    public virtual void GameStateGameOverLoseReadyBegin() 
    {
        Game_Over_Delta = 1.5f;
        AudioManager.Instance.StopAllFX();
        AudioManager.Instance.FXTimeStretch = GameDefine.GAME_SPEEDS[BATTLE_SPEED_TYPE.NORMAL_TYPE];
    }
    public virtual void GameStateGameOverLoseReady() 
    {
        Game_Over_Delta -= Time.deltaTime;
        if (Game_Over_Delta < 0f)
        {
            ChangeState(GAME_STATES.GAME_OVER_LOSE);
        }
    }
    public virtual void GameStateGameOverLoseReadyExit() { }

    public virtual void GameStateGameOverLoseBegin()
    {
        TooltipManager.I.CloseAll();
        GetEffectFactory().ClearAllEffects();
        var win_team = FindTeamManager(TEAM_TYPE.RIGHT);
        win_team.ChangeStateTeamMembers(UNIT_STATES.WIN);
        
        for (int i = 0; i < Used_Team_List.Count; i++)
        {
            Used_Team_List[i].HideAllUnits();
        }

        Game_Over_Delta = 1f;
        UI_Mng.ShowBattleUI(false);
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameResultLosePopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup(this, Dungeon_Data);
        });
    }
    public virtual void GameStateGameOverLose() 
    {
        Game_Over_Delta -= Time.deltaTime;
        if (Game_Over_Delta < 0f)
        {
            ReleaseAllBattleObjects();
        }
    }
    public virtual void GameStateGameOverLoseExit() { }

    public virtual void GameStateTimeOutBegin() 
    {
        GetEffectFactory().ClearAllEffects();
        AudioManager.Instance.StopAllFX();
        AudioManager.Instance.FXTimeStretch = GameDefine.GAME_SPEEDS[BATTLE_SPEED_TYPE.NORMAL_TYPE];
        //  동작 정지
        TeamMembersChangeState(UNIT_STATES.TIME_OUT);
        
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/TimeOutInfoPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.SetHideCompleteCallback(TimeOutInfoCloseCallback);
            popup.ShowPopup(1f);
        });
    }
    public virtual void GameStateTimeOut() { }
    public virtual void GameStateTimeOutExit() { }

    public virtual void GameStateEndBegin() { }
    public virtual void GameStateEnd() { }
    public virtual void GameStateEndExit() { }



    #endregion

}
