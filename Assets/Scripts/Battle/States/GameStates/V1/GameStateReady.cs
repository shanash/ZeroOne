
public class GameStateReady : GameState<BattleManager, BattleUIManager>
{
    public GameStateReady()
    {
        TransID = GAME_STATES.READY;
    }

    public override void EnterState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateReadyBegin();
    }
    public override void UpdateState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateReady();
    }
    public override void ExitState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateReadyExit();
    }
}
