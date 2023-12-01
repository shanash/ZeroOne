
public class GameStatePlaying : GameState<BattleManager, BattleUIManager>
{
    public GameStatePlaying()
    {
        TransID = GAME_STATES.PLAYING;
    }

    public override void EnterState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStatePlayingBegin();
    }
    public override void UpdateState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStatePlaying();
    }
    public override void ExitState(BattleManager mng, BattleUIManager ui)
    {
        mng.GameStatePlayingExit();
    }
}
