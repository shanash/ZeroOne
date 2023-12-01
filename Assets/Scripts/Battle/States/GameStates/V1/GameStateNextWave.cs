
public class GameStateNextWave : GameState<BattleManager, BattleUIManager>
{
    public GameStateNextWave()
    {
        TransID = GAME_STATES.NEXT_WAVE;
    }

    public override void EnterState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateNextWaveBegin();
    }
    public override void UpdateState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateNextWave();
    }
    public override void ExitState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateNextWaveExit();
    }
}
