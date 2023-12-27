using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager_V2_SkillPreview : BattleManager_V2
{
    public override void GameStateSpawn()
    {
        ChangeState(GAME_STATES.PLAYING);
    }

    public override void GameStatePlayingBegin()
    {
        TeamMembersChangeState(UNIT_STATES.IDLE);
    }
    public override void GameStatePlaying()
    {
        
    }
}
