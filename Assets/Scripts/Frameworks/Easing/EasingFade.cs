using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace FluffyDuck.Util
{
    public abstract class EasingFade : MonoBehaviour, IPoolableComponent
    {
        enum FADE_STATES
        {
            NONE = 0,
            PRE_DELAY,
            ANIMATION,
            POST_DELAY,
            PAUSE,
            END,
        }

        protected EasingFunction.Ease EaseFade = EasingFunction.Ease.EaseInOutBack;
        
        protected float AnimationDuration = 0f;

        protected float PreDelayTime = 1f;

        protected float PostDelayTime = 0f;

        protected bool IsRepeat = false;


        System.Action Easing_End_Callback = null;


        FADE_STATES Fade_States = FADE_STATES.NONE;

        FADE_STATES Pre_Fade_States = FADE_STATES.NONE;

        float Delta_Time = 0f;

        private void Update()
        {
            switch (Fade_States)
            {
                case FADE_STATES.NONE:
                    break;
                case FADE_STATES.PRE_DELAY:
                    Delta_Time += Time.deltaTime;
                    if (Delta_Time > PreDelayTime)
                    {
                        UpdatePreDelayEnd();
                        Delta_Time = 0f;
                        ChangeStates(FADE_STATES.ANIMATION);
                    }
                    break;
                case FADE_STATES.ANIMATION:
                    Delta_Time += Time.deltaTime;
                    OnFadeUpdate(Mathf.Clamp01(Delta_Time / AnimationDuration));
                    
                    if (Delta_Time > AnimationDuration)
                    {
                        Delta_Time = 0f;
                        ChangeStates(FADE_STATES.POST_DELAY);
                    }
                    break;
                case FADE_STATES.POST_DELAY:
                    Delta_Time += Time.deltaTime;
                    if (Delta_Time > PostDelayTime)
                    {
                        UpdatePostDelayEnd();
                        Delta_Time = 0f;
                        ChangeStates(FADE_STATES.END);
                    }
                    break;
                case FADE_STATES.END:
                    Delta_Time = 0f;
                    Easing_End_Callback?.Invoke();
                    if (IsRepeat)
                    {
                        ChangeStates(FADE_STATES.PRE_DELAY);
                    }
                    else
                    {
                        ChangeStates(FADE_STATES.NONE);
                    }
                    
                    break;
            }
        }

        void ChangeStates(FADE_STATES state)
        {
            Pre_Fade_States = Fade_States;
            Fade_States = state;
        }

        void RevertState()
        {
            ChangeStates(Pre_Fade_States);
        }
        /// <summary>
        /// 최초 대기시간 종료 후 시작할때 필요한 조치를 하기위한 함수
        /// </summary>
        protected virtual void UpdatePreDelayEnd() { }
        /// <summary>
        /// Post Delay 종료 후 호출. 그때 필요한 조치를 하기 위한 함수
        /// </summary>
        protected virtual void UpdatePostDelayEnd() { }
        /// <summary>
        /// 지속시간 동안 주기적 업데이트 함수
        /// </summary>
        /// <param name="weight"></param>
        protected abstract void OnFadeUpdate(float weight);



        public EasingFade SetEasing(EasingFunction.Ease ease, float pre_delay = 0f, float anim_duration = 1f, float post_delay = 0f, bool is_repeat = false)
        {
            this.EaseFade = ease;
            PreDelayTime = pre_delay;
            AnimationDuration = anim_duration;
            PostDelayTime = post_delay;
            IsRepeat = is_repeat;
            return this;
        }

        public EasingFade SetEase(EasingFunction.Ease ease)
        {
            EaseFade = ease;
            return this;
        }

        public EasingFade SetPreDelay(float delay)
        {
            PreDelayTime = delay;
            return this;
        }
        public EasingFade SetAnimationDuration(float duration)
        {
            AnimationDuration = duration;
            return this;
        }
        public EasingFade SetPostDelay(float delay)
        {
            PostDelayTime = delay;
            return this;
        }
        public EasingFade SetRepeat(bool repeat)
        {
            IsRepeat = repeat;
            return this;
        }

        public virtual void StartEasing(object data, System.Action cb = null)
        {
            if (IsPlaying())
            {
                Debug.Log("Esle Easing Playing Now!!");
                return;
            }
            this.Easing_End_Callback = cb;
            ChangeStates(FADE_STATES.PRE_DELAY);
        }

        public virtual void StartEasing(System.Action cb = null)
        {
            StartEasing(null, cb);
        }

        public bool IsPlaying()
        {
            return Fade_States != FADE_STATES.NONE && Fade_States != FADE_STATES.END;
        }

        public void OnPause()
        {
            ChangeStates(FADE_STATES.PAUSE);
        }
        public void OnResume()
        {
            RevertState();
        }

        public void Spawned()
        {
            Easing_End_Callback = null;
            IsRepeat = false;
        }

        public void Despawned()
        {

        }
    }

    

}
