
public class GameStateFinishStoryDialogueV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateFinishStoryDialogueV2()
    {
        TransID = GAME_STATES.FINISH_STORY_DIALOGUE;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateFinishStoryDialogueBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateFinishStoryDialogue();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateFinishStoryDialogueExit();
    }
}
