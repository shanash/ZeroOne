namespace FluffyDuck.Memorial
{
    public class ActorStateReact : ActorState
    {
        public ActorStateReact() : base(ACTOR_STATES.REACT) { }

        public override void EnterState(ActorBase actor)
        {
            actor.ActorStateReactBegin();
        }

        public override void ExitState(ActorBase actor)
        {
            actor.ActorStateReactExit();
        }
    }
}
