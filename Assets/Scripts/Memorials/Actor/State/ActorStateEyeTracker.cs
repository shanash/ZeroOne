namespace FluffyDuck.Memorial
{
    public class ActorStateEyeTracker : ActorState
    {
        public ActorStateEyeTracker() : base(ACTOR_STATES.EYE_TRACKER) { }

        public override void EnterState(ActorBase actor)
        {
            actor.ActorStateEyeTrakerBegin();
        }

        public override void UpdateState(ActorBase actor)
        {
            actor.ActorStateEyeTrackerUpdate();
        }

        public override void ExitState(ActorBase actor)
        {
            actor.ActorStateEyeTrakerEnd();
        }
    }
}
