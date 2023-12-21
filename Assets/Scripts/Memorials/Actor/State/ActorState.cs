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
    }

    public class ActorState : StateBase<ACTOR_STATES>
    {
        public ActorState(ACTOR_STATES trans)
        {
            //EnterStateAction = new Action<ActorBase>(EnterState);
            //UpdateStateAction = new Action<ActorBase>(UpdateState);
            //ExitStateAction = new Action<ActorBase>(ExitState);
            //FinallyStateAction = new Action<ActorBase>(FinallyState);
            TransID = trans;
        }

        public virtual void EnterState(ActorBase actor) { }
        public virtual void UpdateState(ActorBase actor) { }
        public virtual void ExitState(ActorBase actor) { }
        public virtual void FinallyState(ActorBase actor) { }
    }
}
