
public class GameStateUltimateSkillV2 : GameState<BattleManager_V2, BattleUIManager_V2>
{
    public GameStateUltimateSkillV2()
    {
        TransID = GAME_STATES.ULTIMATE_SKILL;
    }

    public override void EnterState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateUltimateSkillBegin();
    }
    public override void UpdateState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateUltimateSkill();
    }
    public override void ExitState(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        mng.GameStateUltimateSkillExit();
    }
}
