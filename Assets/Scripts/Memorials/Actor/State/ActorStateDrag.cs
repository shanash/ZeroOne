public class ActorStateDrag : ActorState
{
    public ActorStateDrag() : base(ACTOR_STATES.DRAG) { }

    public override void EnterState(ActorBase actor)
    {
        actor.ActorStateDragBegin();
    }

    public override void ExitState(ActorBase actor)
    {
        actor.ActorStateDragEnd();
    }

}
