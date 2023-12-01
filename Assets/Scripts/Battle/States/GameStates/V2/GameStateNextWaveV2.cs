
public class GameStateNextWaveV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateNextWaveV2()
    {
        TransID = GAME_STATES.NEXT_WAVE;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateNextWaveBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateNextWave();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateNextWaveExit();
    }
}
