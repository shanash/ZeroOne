
public class GameStateEnterStoryDialogueV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateEnterStoryDialogueV2()
    {
        TransID = GAME_STATES.ENTER_STORY_DIALOGUE;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateEnterStoryDialogueBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateEnterStoryDialogue();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateEnterStoryDialogueExit();
    }
}
