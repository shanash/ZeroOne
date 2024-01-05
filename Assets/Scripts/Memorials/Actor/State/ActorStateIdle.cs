public class ActorStateIdle : ActorState
{
    public ActorStateIdle() : base(ACTOR_STATES.IDLE) { }

    public override void EnterState(ActorBase actor)
    {
        actor.ActorStateIdleBegin();
    }
}
