
public class GameStateSpawn : GameState<BattleManager, BattleUIManager>
{
    public GameStateSpawn()
    {
        TransID = GAME_STATES.SPAWN;
    }

    public override void EnterState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateSpawnBegin();
    }
    public override void UpdateState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateSpawn();
    }
    public override void ExitState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStateSpawnExit();
    }
}
