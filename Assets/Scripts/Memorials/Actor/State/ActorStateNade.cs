public class ActorStateNade : ActorState
{
    public ActorStateNade() : base(ACTOR_STATES.NADE) { }

    public override void EnterState(ActorBase actor)
    {
        actor.ActorStateNadeBegin();
    }

    public override void UpdateState(ActorBase actor)
    {
        actor.ActorStateNadeUpdate();
    }

    public override void ExitState(ActorBase actor)
    {
        actor.ActorStateNadeEnd();
    }
}
