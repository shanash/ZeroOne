
public class GameStateWaveInfoV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateWaveInfoV2()
    {
        TransID = GAME_STATES.WAVE_INFO;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateWaveInfoBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateWaveInfo();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateWaveInfoExit();
    }
}
