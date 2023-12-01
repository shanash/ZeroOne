
public class GameStateWaveInfo : GameState<BattleManager, BattleUIManager>
{
    public GameStateWaveInfo()
    {
        TransID = GAME_STATES.WAVE_INFO;
    }

    public override void EnterState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateWaveInfoBegin();
    }
    public override void UpdateState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateWaveInfo();
    }
    public override void ExitState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateWaveInfoExit();
    }
}
