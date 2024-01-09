using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class BattleManager_V2 : MonoBehaviour
{
    protected GameStateSystem<BattleManager_V2, BattleUIManager_V2> FSM = null;

    float Wave_Run_Delta;

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
                    int stage_id = board.GetBlackBoardData<int>(BLACK_BOARD_KEY.DUNGEON_ID, 100001);
                    board.RemoveBlackBoardData(BLACK_BOARD_KEY.DUNGEON_ID);     //  스테이지 id를 받은 후 해당 데이터를 삭제

                    Dungeon_Data = new BattleDungeon_StoryStageData();
                    Dungeon_Data.SetDungeonID(stage_id);
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

        {
            var audio = AudioManager.Instance;

            List<string> audio_clip_list = new List<string>();
            audio_clip_list.Add("Assets/AssetResources/Audio/FX/click_01");

            audio.PreloadAudioClipsAsync(audio_clip_list, null);
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
        list.Add("Assets/AssetResources/Prefabs/UI/Battle/BattleSkillSlot");


        //  npc prefabs
        Dungeon_Data.GetMonsterPrefabsPath(ref list);

        var player_team = FindTeamManager(TEAM_TYPE.LEFT);
        if (player_team != null)
        {
            player_team.GetHeroPrefabsPath(ref list);
        }


        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(list, PreloadCallback);
    }


    protected void PreloadCallback(int load_cnt, int total_cnt)
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
    protected void InitStates()
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

    public virtual void GameStateInitBegin()
    {
        InitBattleField();
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
        ChangeState(GAME_STATES.SPAWN);
    }
    public virtual void GameStateReadyExit() { }

    public virtual void GameStateSpawnBegin()
    {
        SpawnUnits();
    }
    public virtual void GameStateSpawn()
    {
        ChangeState(GAME_STATES.WAVE_INFO);
    }
    public virtual void GameStateSpawnExit() { }


    public virtual void GameStateWaveInfoBegin()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Battle/WaveInfoUI", (popup) =>
        {
            popup.SetHideCompleteCallback(WaveInfoCloseCallback);
            Wave_Data wdata = (Wave_Data)Dungeon_Data.GetWaveData();
            popup.ShowPopup(wdata, 1f);
        });
    }
    public virtual void GameStateWaveInfo() { }
    public virtual void GameStateWaveInfoExit() { }


    public virtual void GameStatePlayingBegin()
    {
        if (IsPrevPause())
        {
            return;
        }

        UI_Mng.UpdateWaveCount();
        TeamMembersChangeState(UNIT_STATES.MOVE_IN);
    }
    public virtual void GameStatePlaying()
    {
        var all_death_team = Used_Team_List.Find(x => !x.IsAliveMembers());
        if (all_death_team != null)
        {
            var win_team = FindTeamManager(all_death_team.Team_Type == TEAM_TYPE.LEFT ? TEAM_TYPE.RIGHT : TEAM_TYPE.LEFT);
            win_team.ChangeStateTeamMembers(UNIT_STATES.WIN);

            if (win_team.Team_Type == TEAM_TYPE.LEFT)
            {
                //  다음 웨이브가 있다면
                if (Dungeon_Data.HasNextWave())
                {
                    ChangeState(GAME_STATES.WAVE_RUN);
                }
                else // 없으면 게임 종료(승리)
                {
                    ChangeState(GAME_STATES.GAME_OVER_WIN);
                }

            }
            else
            {
                ChangeState(GAME_STATES.GAME_OVER_LOSE);
            }
        }
    }
    public virtual void GameStatePlayingExit() { }

    public virtual void GameStateNextWaveBegin()
    {
        if (IsPrevPause())
        {
            return;
        }
        if (Dungeon_Data.NextWave())
        {
            var my_team = FindTeamManager(TEAM_TYPE.LEFT);
            if (my_team != null)
            {
                my_team.ChangeStateTeamMembers(UNIT_STATES.IDLE);
                my_team.LeftTeamPosition();
            }

            var enemy_team = FindTeamManager(TEAM_TYPE.RIGHT);
            if (enemy_team != null)
            {
                enemy_team.SpawnHeros();
            }

            Fade_In_Out_Layer.StartMove(UIEaseBase.MOVE_TYPE.MOVE_OUT, () =>
            {
                ChangeState(GAME_STATES.PLAYING);
            });
        }
        else
        {
            ChangeState(GAME_STATES.GAME_OVER_WIN);
        }

    }
    public virtual void GameStateNextWave() { }
    public virtual void GameStateNextWaveExit() { }

    public virtual void GameStateWaveRunBegin()
    {
        if (IsPrevPause())
        {
            return;
        }
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
    public virtual void GameStatePause() { }
    public virtual void GameStatePauseExit()
    {
        TeamMembersRevertState();
        GetEffectFactory().OnResume();
    }

    public virtual void GameStateGameOverWinBegin()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameResultWinPopup", (popup) =>
        {
            popup.ShowPopup();
        });
    }
    public virtual void GameStateGameOverWin() { }
    public virtual void GameStateGameOverWinExit() { }

    public virtual void GameStateGameOverLoseBegin()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameResultLosePopup", (popup) =>
        {
            popup.ShowPopup();
        });
    }
    public virtual void GameStateGameOverLose() { }
    public virtual void GameStateGameOverLoseExit() { }

    public virtual void GameStateEndBegin() { }
    public virtual void GameStateEnd() { }
    public virtual void GameStateEndExit() { }



    #endregion

}
