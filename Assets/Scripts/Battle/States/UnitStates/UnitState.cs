using System;
using UnityEngine;

public enum UNIT_STATES
{
    NONE = 0,
    INIT,                   //  초기화
    READY,

    SPAWN,                  //  등장
    MOVE_IN,                //  전투 필드에 등장하는 상태

    MOVE,                   //  전투 중 적에게 접근하는 상태


    IDLE,                   //  idle
    TURN_ON,                //  사용 안함 (V2 기본 작업 완료 후 삭제 예정)

    PLAYING,

    ATTACK_READY_1,
    ATTACK_1,                 //  몬스터에만 적용. 공격 후 ATTACK_READY 상태로 변경
    ATTACK_READY_2,
    ATTACK_2,                 //  몬스터에만 적용. 공격 후 ATTACK_READY 상태로 변경

    ATTACK_END,

    SKILL_READY_1,
    SKILL_1,
    SKILL_READY_2,
    SKILL_2,
    SKILL_READY_3,
    SKILL_3,

    SKILL_END,

    STUN,                   //  기절(행동 자체가 변경되어야 하고, 다른것을 할 수 없는 상황이기 때문에 별도의 상태를 가진다.)
    SLEEP,                  //  잠들기(행동 자체가 변경되어야 하고, 다른것을 할 수 없는 상황이기 때문에 별도의 상태를 가진다.)
    FREEZE,                 //  빙결 (행동 자체가 변경되어야 함, 얼음에 감싸있기 때문에 동작이 굳어버려야 함. 아무것도 할 수 없는 상태)
    BIND,                   //  결박 (사거리 내에서만 공격/스킬 사용 가능. 결박되어 있기 때문에 이동 불가. 사거리내에 아무도 없고, 이동이 불가한 상태라면 결박이 풀릴때까지 기다려야 함)

    TURN_END,               //  사용 안함. v2 기본 작업 완료 후 삭제 예정
    WAVE_RUN,               //  전투 종료 후 나가기

    PAUSE,
    DEATH,

    WIN,
    LOSE,

    END,
}

public abstract class UnitState<N, B, U> : StateBase<UNIT_STATES>
    where N : class
    where B : class
    where U : class
{
    public UnitState(UNIT_STATES trans)
    {
        EnterStateAction = new Action<N, B, U>(EnterState);
        UpdateStateAction = new Action<N, B, U>(UpdateState);
        ExitStateAction = new Action<N, B, U>(ExitState);
        FinallyStateAction = new Action<N, B, U>(FinallyState);

        TransID = trans;
    }

    public virtual void EnterState(N unit, B mng, U ui) { }
    public virtual void UpdateState(N unit, B mng, U ui) { }
    public virtual void ExitState(N unit, B mng, U ui) { }
    public virtual void FinallyState(N unit, B mng, U ui) { }
}
