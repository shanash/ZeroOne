
public class GameStatePause : GameState<BattleManager, BattleUIManager>
{
    public GameStatePause()
    {
        TransID = GAME_STATES.PAUSE;
    }

    public override void EnterState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStatePauseBegin();
    }
    public override void UpdateState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStatePause();
    }
    public override void ExitState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStatePauseExit();
    }
}
