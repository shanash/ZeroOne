using System;

namespace FluffyDuck.Memorial
{
    public enum ACTOR_STATES
    {
        NONE = 0,
        INTRO,
        IDLE,
        REACT,
        DRAG,
        NADE,
        EYE_TRACKER,
    }

    public class ActorState : StateBase<ACTOR_STATES>
    {
        public ActorState(ACTOR_STATES trans)
        {
            EnterStateAction = EnterState;
            UpdateStateAction = UpdateState;
            ExitStateAction = ExitState;
            FinallyStateAction = FinallyState;

            TransID = trans;
        }

        private void EnterState(object[] obj)
        {
            EnterState(obj[0] as ActorBase);
        }

        private void ExitState(object[] obj)
        {
            ExitState(obj[0] as ActorBase);
        }

        private void UpdateState(object[] obj)
        {
            UpdateState(obj[0] as ActorBase);
        }

        private void FinallyState(object[] obj)
        {
            FinallyState(obj[0] as ActorBase);
        }

        public virtual void EnterState(ActorBase actor) { }
        public virtual void UpdateState(ActorBase actor) { }
        public virtual void ExitState(ActorBase actor) { }
        public virtual void FinallyState(ActorBase actor) { }
    }
}
