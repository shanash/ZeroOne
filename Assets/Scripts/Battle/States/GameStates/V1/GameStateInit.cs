
public class GameStateInit : GameState<BattleManager, BattleUIManager>
{
    public GameStateInit()
    {
        TransID = GAME_STATES.INIT;
    }

    public override void EnterState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateInitBegin();
    }
    public override void UpdateState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateInit();
    }
    public override void ExitState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateInitExit();
    }
}
