using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UnitBase : MonoBehaviour, IPoolableComponent
{
    /// <summary>
    /// Pause 호출 시 해당 유닛들을 일시 정지 해주기 위한 함수
    /// </summary>
    protected virtual void OnPause() { }

    /// <summary>
    /// Pause 가 풀리고 Resume이 되면 일시 정지 되어있던 모든 것들을 복원해주는 함수
    /// </summary>
    protected virtual void OnResume() { }

    /// <summary>
    /// 오브젝트 풀에서 해당 게임 오브젝트를 가져올 때 최초 호출되는 함수. 
    /// Init보다 먼저 호출됨.
    /// Awake / Start 보다는 늦게 호출됨
    /// </summary>
    public virtual void Spawned()
    {
        //  커스텀 업데이트 등록
        CustomUpdateManager.Instance.RegistCustomUpdateComponent(this);
    }
    /// <summary>
    /// 오브젝트 풀에 게임오브젝트를 반환할 때 반환되기 직전에 호출되는 함수.
    /// 이 함수 호출 이후 게임오브젝트 비활성화 됨(부모 트랜스폼이 변경되기 직전 호출)
    /// </summary>
    public virtual void Despawned()
    {
        //  커스텀 업데이트 해제
        CustomUpdateManager.Instance.DeregistCustomUpdateComponent(this);
    }
}
