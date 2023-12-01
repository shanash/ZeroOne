
public class GameStateEnd : GameState<BattleManager, BattleUIManager>
{
    public GameStateEnd()
    {
        TransID = GAME_STATES.END;
    }

    public override void EnterState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateEndBegin();
    }
    public override void UpdateState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateEnd();
    }
    public override void ExitState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateEndExit();
    }
}
