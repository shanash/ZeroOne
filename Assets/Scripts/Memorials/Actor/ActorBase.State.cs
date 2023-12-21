using Spine;
using UnityEngine;

namespace FluffyDuck.Memorial
{
    public abstract partial class ActorBase : MonoBehaviour
    {
        protected StateSystemBase<ACTOR_STATES> FSM = null;

        private void Awake()
        {
            OnAwake();
            InitStates();
        }
        protected virtual void OnAwake() { }
        protected virtual void InitStates()
        {
            FSM = new StateSystemBase<ACTOR_STATES>();
            FSM.AddTransition(new ActorStateIntro());
            FSM.AddTransition(new ActorStateIdle());
            FSM.AddTransition(new ActorStateReact());
            FSM.AddTransition(new ActorStateDrag());
            FSM.AddTransition(new ActorStateNade());
        }

        public virtual void Lazy_Init(ACTOR_STATES trans)
        {
            if (FSM == null)
            {
                InitStates();
            }

            FSM.Lazy_Init_Setting(trans, this);
        }

        public void ChangeState(ACTOR_STATES trans)
        {
            FSM?.ChangeState(trans);
        }

        public void RevertState()
        {
            FSM?.RevertState();
        }
        public ACTOR_STATES GetCurrentState()
        {
            if (FSM != null)
            {
                return FSM.CurrentTransitionID;
            }
            return ACTOR_STATES.NONE;
        }
        public ACTOR_STATES GetPreviousState()
        {
            if (FSM != null)
            {
                return FSM.PreviousTransitionID;
            }
            return ACTOR_STATES.NONE;
        }

        public virtual void OnUpdate(float dt)
        {
            FSM?.UpdateState();
        }
    }
}
