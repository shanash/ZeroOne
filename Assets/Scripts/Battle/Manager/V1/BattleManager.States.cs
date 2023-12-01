using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class BattleManager : MonoBehaviour
{
    GameStateSystem<BattleManager, BattleUIManager> FSM = null;

    private void Awake()
    {
        StartCoroutine(InitAssets());
    }

    IEnumerator InitAssets()
    {
        var handle = Addressables.InitializeAsync();
        yield return handle;

        var handle2 = Addressables.DownloadDependenciesAsync("default", true);
        yield return handle2;

        List<string> list = new List<string>();
        list.Add("Assets/AssetResources/Prefabs/Fields/Battle_Field_01");
        list.Add("Assets/AssetResources/Prefabs/Heros/Sorceress");
        list.Add("Assets/AssetResources/Prefabs/Behavior/Behavior_Hero_Icon_Node");
        list.Add("Assets/AssetResources/Prefabs/UI/LifeBarNode");
        list.Add("Assets/AssetResources/Prefabs/UI/TargetArrowNode");
        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(list, PreloadCallback);
    }

    void PreloadCallback(int load_cnt, int total_cnt)
    {
        Debug.LogFormat("Load Count [{0}], Total Count [{1}]", load_cnt, total_cnt);
        if (load_cnt == total_cnt)
        {
            InitStates();
        }
    }

    void InitStates()
    {
        FSM = new GameStateSystem<BattleManager, BattleUIManager> ();

        FSM.AddTransition(new GameStateInit());
        FSM.AddTransition(new GameStateReady());
        FSM.AddTransition(new GameStateWaveInfo());
        FSM.AddTransition(new GameStateSpawn());
        FSM.AddTransition(new GameStatePlaying());
        FSM.AddTransition(new GameStateNextWave());
        FSM.AddTransition(new GameStateWaveRun());
        FSM.AddTransition(new GameStatePause());
        FSM.AddTransition(new GameStateGameOverWin());
        FSM.AddTransition(new GameStateGameOverLose());
        FSM.AddTransition(new GameStateEnd());

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

    private void Update()
    {
        FSM?.UpdateState();
    }

    #region Game States
    public void GameStateInitBegin() 
    {
        InitBattleField();
    }
    public void GameStateInit() 
    {
        ChangeState(GAME_STATES.SPAWN);
    }
    public void GameStateInitExit() { }

    public void GameStateWaveInfoBegin() { }
    public void GameStateWaveInfo() { }
    public void GameStateWaveInfoExit() { }


    public void GameStateSpawnBegin() 
    {
        SpawnHeroes();
    }
    public void GameStateSpawn() 
    {
        ChangeState(GAME_STATES.READY);
    }
    public void GameStateSpawnExit() { }

    public void GameStateReadyBegin() { }
    public void GameStateReady()
    {
        bool is_all_ready = true;
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            if (!Used_Team_List[i].IsAllMembersState(UNIT_STATES.IDLE))
            {
                is_all_ready = false;
                break;
            }
        }
        if (is_all_ready)
        {
            Behavior_Mng.UpdateHeroIconsOrder();
            ChangeState(GAME_STATES.PLAYING);
        }
    }
    public void GameStateReadyExit() { }


    public void GameStatePlayingBegin() { }
    public void GameStatePlaying() 
    {
        bool is_all_idle = true;
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            if (!Used_Team_List[i].IsAllMembersState(UNIT_STATES.IDLE))
            {
                is_all_idle = false;
                break;
            }
        }
        if (is_all_idle)
        {
            HeroTurnOnCheck();
        }
    }
    public void GameStatePlayingExit() { }

    public void GameStateNextWaveBegin() 
    {
        FindTeamManager(TEAM_TYPE.LEFT).SetBattleBeginRapidityPoint();
    }
    public void GameStateNextWave() 
    {
        ChangeState(GAME_STATES.READY);
    }
    public void GameStateNextWaveExit() { }

    public void GameStateWaveRunBegin() { }
    public void GameStateWaveRun() { }
    public void GameStateWaveRunExit() { }


    public void GameStateGameOverWinBegin() { }
    public void GameStateGameOverWin() { }
    public void GameStateGameOverWinExit() { }


    public void GameStateGameOverLoseBegin() { }
    public void GameStateGameOverLose() { }
    public void GameStateGameOverLoseExit() { }

    public void GameStatePauseBegin() { }
    public void GameStatePause() { }
    public void GameStatePauseExit() { }

    public void GameStateEndBegin() { }
    public void GameStateEnd() { }
    public void GameStateEndExit() { }

    #endregion
}
