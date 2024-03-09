using FluffyDuck.Util;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : SceneControllerBase
{
    [SerializeField]
    ConversationControl Con_Ctrl;


    //protected override void Initialize()
    //{
    //    base.Initialize();

    //    //Con_Ctrl = GetComponent<ConversationControl>();
    //}

    /// <summary>
    /// 대사 종료 후 전투 화면으로 이동(테스트 전용)
    /// </summary>
    public void ChangeScene()
    {
        var board = BlackBoard.Instance;
        GAME_TYPE game_type = (GAME_TYPE)board.GetBlackBoardData<int>(BLACK_BOARD_KEY.GAME_TYPE, (int)GAME_TYPE.NONE);
        int dungeon_id = board.GetBlackBoardData<int>(BLACK_BOARD_KEY.DUNGEON_ID, 0);
        if (game_type == GAME_TYPE.NONE || dungeon_id == 0)
        {
            SCManager.I.ChangeScene(SceneName.home);
        }
        else
        {
            SCManager.I.ChangeScene(SceneName.battle);
        }
       
    }


    public void OnSkip()
    {
        if (Con_Ctrl != null)
        {
            Con_Ctrl.SkipAll();
        }
        ChangeScene();
    }
}
