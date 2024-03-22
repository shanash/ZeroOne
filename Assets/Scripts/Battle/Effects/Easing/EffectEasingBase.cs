using Coffee.UIParticleExtensions;
using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EffectEasingBase : EasingFade
{
    public enum EASING_MOVE_TYPE
    {
        NONE = 0,
        IN,
        OUT,
    }


    [Serializable]
    protected class EasingData
    {
        /// <summary>
        /// Move Type
        /// </summary>
        public EASING_MOVE_TYPE Move_Type = EASING_MOVE_TYPE.NONE;
        public EasingFunction.Ease Ease_Type = EasingFunction.Ease.NotUse;

        /// <summary>
        /// 초기화에 사용될 벡터
        /// </summary>
        public Vector3 Init_Vector;
        /// <summary>
        /// 목표 스케일 / 이동 거리 등으로 사용할 벡터
        /// </summary>
        public Vector3 Easing_Vector;
        /// <summary>
        /// 알파값 등으로 사용 가능한 실수
        /// </summary>
        public float Easing_Float;
        /// <summary>
        /// 지속시간(플레이 시간)
        /// </summary>
        public float Easing_Duration;
        /// <summary>
        /// 이펙트 시작전 대기 시간
        /// </summary>
        public float Pre_Delay;
        /// <summary>
        /// 이펙트 종료 후 대기시간
        /// </summary>
        public float Post_Delay;
        /// <summary>
        /// 반복 여부
        /// </summary>
        public bool Repeat;
    }

    [SerializeField, Tooltip("Easing Data List")]
    protected List<EasingData> Easing_Data_List;
    /// <summary>
    /// 기본 지속시간
    /// </summary>
    protected const float DEFAULT_DURATION = 0.2f;

    protected EASING_MOVE_TYPE Move_Type = EASING_MOVE_TYPE.NONE;

    /// <summary>
    /// Easing 데이터 찾기
    /// </summary>
    /// <param name="mtype"></param>
    /// <returns></returns>
    protected EasingData FindEasingData(EASING_MOVE_TYPE mtype)
    {
        return Easing_Data_List.Find(x => x.Move_Type == mtype);
    }

    /// <summary>
    /// 애니메이션 시작
    /// </summary>
    /// <param name="mtype"></param>
    /// <param name="cb"></param>
    public virtual void StartMove(EASING_MOVE_TYPE mtype, System.Action cb = null)
    {
        var find = FindEasingData(mtype);
        if (find != null)
        {
            Move_Type = find.Move_Type;
            float duration = find.Easing_Duration;
            if (duration == 0f)
            {
                duration = DEFAULT_DURATION;
            }
            SetEasing(find.Ease_Type, find.Pre_Delay, duration, find.Post_Delay, find.Repeat);
            StartEasing(cb);
        }
    }

    protected override void OnFadeUpdate(float weight)
    {
        if (EaseFade == EasingFunction.Ease.NotUse)
        {
            return;
        }
        UpdateEase(EasingFunction.GetEasingFunction(EaseFade), weight);
    }
    /// <summary>
    /// Ease type에 맞는 함수를 이용하여 Easing 계산
    /// </summary>
    /// <param name="func"></param>
    /// <param name="weight"></param>
    protected virtual void UpdateEase(EasingFunction.Function func, float weight) { }
    /// <summary>
    /// Easing Update 종료 후 후방 Delay 시간 종료되면 호출되는 함수.
    /// </summary>
    protected override void UpdatePostDelayEnd() { }

    /// <summary>
    /// 상태를 초기화 시키기 위한 함수<br/>
    /// 값을 받아서 각각의 상황에 맞는 데이터를 사용한다.
    /// </summary>
    /// <param name="data"></param>
    public virtual void ResetEase(params object[] data) { }

}


