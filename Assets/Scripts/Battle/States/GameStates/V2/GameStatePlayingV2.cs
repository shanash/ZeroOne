
public class GameStatePlayingV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStatePlayingV2()
    {
        TransID = GAME_STATES.PLAYING;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStatePlayingBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStatePlaying();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStatePlayingExit();
    }
}
