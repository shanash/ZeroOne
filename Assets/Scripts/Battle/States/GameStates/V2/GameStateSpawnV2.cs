
public class GameStateSpawnV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateSpawnV2()
    {
        TransID = GAME_STATES.SPAWN;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateSpawnBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateSpawn();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateSpawnExit();
    }
}
